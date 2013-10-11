using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Dapper;
using StackExchange.Profiling;

namespace Quick5.Models
{
    public class SqlBase
    {
        public IDbConnection connexion;

        public SqlBase(string config_key)
        {
            connexion = GetConnection(config_key);
            MvcApplication.IsDbProduction = connexion.ConnectionString.ToLower().Contains("=extracv;");
            MvcApplication.IsDbTests = connexion.ConnectionString.ToLower().Contains("db_tests.sdf");
        }

        private IDbConnection GetConnection(string config_key)
        {
            var settings = ConfigurationManager.ConnectionStrings[config_key];

            var factory = DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return new StackExchange.Profiling.Data.ProfiledDbConnection(conn, MiniProfiler.Current);
        }

        public void Open()
        {
            connexion.Open();
        }

        public void Close()
        {
            connexion.Close();
        }

        public int ExecuteSql(string sql, object param = null)
        {
            int data = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                data = connexion.Execute(sql, param);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!is_open_before) connexion.Close();
            }

            return data;
        }

        public int ExecuteScalar(string sql, object param = null)
        {
            int data = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                data = connexion.Query<int>(sql, param).FirstOrDefault();
            }
            catch (InvalidCastException)
            {
                // Avec Oracle, COUNT(*) renvoie un decimal !
                data = (int)connexion.Query<decimal>(sql, param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!is_open_before) connexion.Close();
            }

            return data;
        }
    }

    public static class SqlMapperExtensions
    {
        public static T Get<T>(this IDbConnection cnx, int id) where T : class
        {
            var prefix = cnx.GetPrefix();
            T data = null;

            var is_open_before = (cnx.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) cnx.Open();
                var sql = GetSelect(typeof(T), prefix);
                data = cnx.Query<T>(sql, new { id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!is_open_before) cnx.Close();
            }

            return data;
        }

        public static IEnumerable<T> List<T>(this IDbConnection cnx, string where, object param) where T : class
        {
            IEnumerable<T> data = null;
            if (param == null) param = new { };
            var where_params = param.GetType().GetProperties().ToArray().Select(p => p.Name);
            var prefix = cnx.GetPrefix();
            var bad_prefix = prefix == ":" ? "@" : ":";

            var is_open_before = (cnx.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) cnx.Open();
                var sql = GetSelect(typeof(T)) + where;
                foreach (var p in where_params)
                {
                    sql = sql.Replace(bad_prefix + p, prefix + p); // ne gère pas majuscule / minuscule !!! 
                }
                data = cnx.Query<T>(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!is_open_before) cnx.Close();
            }

            return data;
        }

        public static int Insert<T>(this IDbConnection connexion, T data) where T : class
        {
            var type = typeof(T);
            var columns = GetColumns(type).Skip(1);
            var table_name = GetTableName(type);
            var prefix = connexion.GetPrefix();

            var sql = new StringBuilder();
            sql.Append("INSERT INTO ");
            sql.Append(table_name);
            sql.Append(" (");
            sql.Append(string.Join(", ", columns));
            sql.Append(") VALUES (" + prefix);
            sql.Append(string.Join(", " + prefix, columns));
            sql.Append(")");

            int result = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                result = connexion.Execute(sql.ToString(), data);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!is_open_before) connexion.Close();
            }

            return result;
        }

        public static int Update<T>(this IDbConnection connexion, T data) where T : class
        {
            var type = typeof(T);
            var columns = GetColumns(type);
            var table_name = GetTableName(type);
            var prefix = connexion.GetPrefix();

            var sql = new StringBuilder();
            sql.Append("UPDATE ");
            sql.Append(table_name);
            sql.Append(" SET ");
            var cols = columns.Skip(1).Select(c => c + " = " + prefix + c);
            sql.Append(string.Join(", ", cols));
            sql.Append(" WHERE (");
            sql.Append(columns.Take(1).Select(c => c + " = " + prefix + c).First());
            sql.Append(")");

            int result = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                result = connexion.Execute(sql.ToString(), data);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!is_open_before) connexion.Close();
            }

            return result;
        }

        public static int Delete<T>(this IDbConnection cnx, int id) where T : class
        {
            var type = typeof(T);
            var columns = GetColumns(type);
            var table_name = GetTableName(type);
            var prefix = cnx.GetPrefix();

            var sql = new StringBuilder();
            sql.Append("DELETE FROM ");
            sql.Append(table_name);
            sql.Append(" WHERE (");
            sql.Append(columns.First());
            sql.Append(" = " + prefix + "Id)");

            int result = 0;
            var is_open_before = (cnx.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) cnx.Open();
                result = cnx.Execute(sql.ToString(), new { id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!is_open_before) cnx.Close();
            }

            return result;
        }

        private static string GetSelect(Type type, string prefix = null)
        {
            var columns = GetColumns(type);
            var table_name = GetTableName(type);

            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(string.Join(", ", columns));
            sql.Append(" FROM ");
            sql.Append(table_name);

            if (prefix != null)
            {
                sql.Append(" WHERE (");
                sql.Append(columns.First());
                sql.Append(" = " + prefix + "Id)");
            }

            return sql.ToString();
        }

        private static IEnumerable<string> GetColumns(Type type)
        {
            var columns = type.GetProperties().ToArray().Select(p => p.Name);

            return columns;
        }

        private static string GetTableName(Type type)
        {
            var attribute = type.GetCustomAttributes(false).SingleOrDefault(a => a.GetType().Name == "TableAttribute") as dynamic;
            if (attribute != null) return attribute.Name;

            return type.Name + "s";
        }

        private static string GetPrefix(this IDbConnection cnx)
        {
            if (cnx.ConnectionString.ToLower().Contains("db_tests.sdf")) return "@";

            return ":";
        }
    }
}
