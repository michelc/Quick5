using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
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

        public int ExecuteSql(string sql)
        {
            int data = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                data = connexion.Execute(sql);
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

        public int ExecuteSql(string sql, object param)
        {
            int data = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                data = connexion.Execute(sql, param);
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

        public int ExecuteScalar(string sql)
        {
            int data = 0;
            bool is_open_before = (connexion.State == ConnectionState.Open);
            try
            {
                if (!is_open_before) connexion.Open();
                data = connexion.Query<int>(sql).FirstOrDefault();
            }
            catch (InvalidCastException)
            {
                // Avec Oracle, COUNT(*) renvoie un decimal !
                data = (int)connexion.Query<decimal>(sql).FirstOrDefault();
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
}
