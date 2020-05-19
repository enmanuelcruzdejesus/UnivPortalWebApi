using System;
using System.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using PortalUnivWebApi.Services;
using PortalUnivWebApi.Utils.Database;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ApiCore
{
    public class AppConfig : IDisposable
    {
        #region FIELDS
        private static AppConfig _instance;
        private static string _connectionString;
        private static IDbConnectionFactory _dbFactory;
        //private Database _db = null;
        DatabaseContext _dbContext;



        #endregion

        #region CTOR
        private AppConfig()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
          

            var conf = builder.Build();
            _connectionString = conf.GetConnectionString("DefaultConnection");

            GDSDataHelper.ConnectionString = _connectionString;
            _instance = null;
            _dbFactory = new OrmLiteConnectionFactory(_connectionString, SqlServer2014Dialect.Provider);
        }
        #endregion


        #region GETTERS AND SETTERS
        public static AppConfig Instance()
        {
            if (_instance == null)
                _instance = new AppConfig();

            return _instance;
        }
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
            }
        }

        public DatabaseContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new DatabaseContext(ConnectionString);

                return _dbContext;
            }
        }

        //public Database Db
        //{
        //    get
        //    {
        //        if (_db == null)
        //            _db = new Database(ConnectionString);

        //        return _db;
        //    }
        //}




        public IDbConnectionFactory DbFactory { get { return _dbFactory; } }




        #endregion

        #region PUBLIC METHODS
        public void Dispose()
        {
            _instance = null;
            _connectionString = null;
            //_dbFactory = null;
        }

        #endregion



    }
}
