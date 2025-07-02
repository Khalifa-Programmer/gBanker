using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BasicDataAccess.Data;
using System.Data.Common;
using System.Data;

namespace BasicDataAccess
{
    public class gBankerDataAccess : DataAccessBase
    {

        public gBankerDataAccess()
        {

        }
        public string GetConnectionString()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerDbContext"];

            return connectionSrings.ConnectionString.ToString();

        }
        public string GetReportConnectionString()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerDbContext"];

            return connectionSrings.ConnectionString.ToString();

        }
        protected override ConnectionStringSettings LoadConnectionStringSetting()
        {
            
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerDbContext"];
            //TO DO: Load connection string values from web.config file, any other source.
            ConnectionStringSettings _dbConnectionStringSetting = connectionSrings;// new ConnectionStringSettings("DCURDB", @"Data Source=10.10.10.40;Initial Catalog=DCURDB;User ID=sa;Password=sa1234;Connect Timeout=45;Pooling=false", "System.Data.SqlClient");

            return _dbConnectionStringSetting;
        }
      
    }
}
