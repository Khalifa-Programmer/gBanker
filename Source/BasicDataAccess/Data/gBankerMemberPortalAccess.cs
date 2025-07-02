using System.Configuration;
using BasicDataAccess.Data;

namespace BasicDataAccess.Data
{
    public class gBankerMemberPortalAccess: DataAccessBase
    {

        public gBankerMemberPortalAccess()
        {

        }
        public string GetConnectionString()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerMemberPortal"];
            return connectionSrings.ConnectionString.ToString();
        }
        public string GetReportConnectionString()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerMemberPortal"];
            return connectionSrings.ConnectionString.ToString();

        }
        protected override ConnectionStringSettings LoadConnectionStringSetting()
        {
            var connectionSrings = ConfigurationManager.ConnectionStrings["gBankerMemberPortal"];
            ConnectionStringSettings _dbConnectionStringSetting = connectionSrings;// new ConnectionStringSettings("DCURDB", @"Data Source=10.10.10.40;Initial Catalog=DCURDB;User ID=sa;Password=sa1234;Connect Timeout=45;Pooling=false", "System.Data.SqlClient");
            return _dbConnectionStringSetting;
        }

        //public ConnectionStringSettings DynamicConnectionString(string ServerIP, string User, string Password, string DatabaseName)
        //{
        //    ConnectionStringSettings _dbConnectionStringSetting = new ConnectionStringSettings("DCURDB", @"Data Source=" + ServerIP + ";Initial Catalog=" + DatabaseName + ";User ID=" + User + ";Password=" + Password + ";Connect Timeout=45;Pooling=false", "System.Data.SqlClient");
        //    return _dbConnectionStringSetting;

        //}



    }// End Class
}// End Namespace