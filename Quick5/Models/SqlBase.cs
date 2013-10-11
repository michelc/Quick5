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
        protected IDbConnection connexion;

        public SqlBase(string config_key)
        {
            connexion = GetConnection(config_key);
            MvcApplication.IsDbProduction = connexion.ConnectionString.ToUpper().Contains("=EXTRACV;");
        }

        private IDbConnection GetConnection(string config_key)
        {
            var settings = ConfigurationManager.ConnectionStrings[config_key];

            var factory = DbProviderFactories.GetFactory(settings.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = settings.ConnectionString;

            return new StackExchange.Profiling.Data.ProfiledDbConnection(conn, MiniProfiler.Current);
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

        public string GetTableName(Type type)
        {
            var attribute = type.GetCustomAttributes(false).SingleOrDefault(a => a.GetType().Name == "TableAttribute") as dynamic;
            if (attribute != null) return attribute.Name;

            return type.Name + "s";
        }
    }

    public static class SqlMapperExtensions
    {
        public static T Get<T>(this IDbConnection cnx, int id) where T : class
        {
            T data = null;

            var is_open_before = (cnx.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) cnx.Open();
                var sql = GetSelect(typeof(T), true);
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

            var is_open_before = (cnx.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) cnx.Open();
                var sql = GetSelect(typeof(T)) + where;
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

            var sql = new StringBuilder();
            sql.Append("INSERT INTO ");
            sql.Append(table_name);
            sql.Append(" (");
            sql.Append(string.Join(", ", columns));
            sql.Append(") VALUES (:");
            sql.Append(string.Join(", :", columns));
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

            var sql = new StringBuilder();
            sql.Append("UPDATE ");
            sql.Append(table_name);
            sql.Append(" SET ");
            var cols = columns.Skip(1).Select(c => c + " = :" + c);
            sql.Append(string.Join(", ", cols));
            sql.Append(" WHERE (");
            sql.Append(columns.Take(1).Select(c => c + " = :" + c).First());
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

            var sql = new StringBuilder();
            sql.Append("DELETE FROM ");
            sql.Append(table_name);
            sql.Append(" WHERE (");
            sql.Append(columns.First());
            sql.Append(" = :Id)");

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

        private static string GetSelect(Type type, bool where_id = false)
        {
            var columns = GetColumns(type);
            var table_name = GetTableName(type);

            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(string.Join(", ", columns));
            sql.Append(" FROM ");
            sql.Append(table_name);

            if (where_id)
            {
                sql.Append(" WHERE (");
                sql.Append(columns.First());
                sql.Append(" = :Id)");
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
    }
}
