using gBanker.Service.ReportExecutionService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;

namespace gBanker.Service.Helpers
{
    public static class SSRSReportProcessHelper
    {
        private static byte[] RenderReport(string ServiceURL, string ReportName, ParameterValue[] parameters, string UserId, string Password, string dataSource, string initialCatalog, ref string Status)
        {

            byte[] result = null;
            //try
            //{
            //Getting Reporting Services Proxy and do some default settings
            var rs = new ReportExecutionService.ReportExecutionService
            {
                Credentials = GetCredential(),
                Url = ServiceURL
            };
            rs.Timeout = 999900000;
            string historyID = null;
            string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
            string encoding;
            string mimeType;
            string extension;
            Warning[] warnings = null;
            string[] streamIDs = null;

            ExecutionInfo execInfo = new ExecutionInfo();
            execInfo = rs.LoadReport(ReportName, historyID);

            ExecutionHeader execHeader = new ExecutionHeader();
            execHeader.ExecutionID = execInfo.ExecutionID;
            rs.ExecutionHeaderValue = execHeader;

            DataSourceCredentials dataSourceCredentials = new DataSourceCredentials();
            if (execInfo.DataSourcePrompts.Length > 0)
            {
                dataSourceCredentials.DataSourceName = execInfo.DataSourcePrompts[0].Name;
            }
            else
                dataSourceCredentials.DataSourceName = "JCF";

            dataSourceCredentials.UserName = UserId;
            dataSourceCredentials.Password = Password;

            DataSourceCredentials[] dsCredentials = new DataSourceCredentials[] { dataSourceCredentials };
            rs.SetExecutionCredentials(dsCredentials);
            rs.SetExecutionParameters(parameters, "en-us");
            result = rs.Render("pdf", devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
            Status = "success";
            return result;
        }

        public static byte[] RenderReport(string ReportName, ParameterValue[] parameters, ref string Status, string connectionStringName = "gBankerReport")
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new Exception("Connection string cannot be null.");
            //read server and credential from web.config.
            var serverUrl = ConfigurationManager.AppSettings["ReportServerUrl"].ToString();
            var connectionSrings = ConfigurationManager.ConnectionStrings[connectionStringName];
            var connectionBuilder = new SqlConnectionStringBuilder(connectionSrings.ConnectionString);
            var user = connectionBuilder.UserID;
            var pwd = connectionBuilder.Password;
            var datasource = connectionBuilder.DataSource;
            var dbname = connectionBuilder.InitialCatalog;
            var paramList = new List<ParameterValue>() {
                new ParameterValue() { Name = "ServerName", Value = datasource } ,
                new ParameterValue() { Name = "DatabaseName", Value = dbname } };
            if (parameters != null && parameters.Length > 0)
                paramList.AddRange(parameters);
            return RenderReport(serverUrl, ReportName, paramList.ToArray(), user, pwd, datasource, dbname, ref Status);
        }

        private static NetworkCredential GetCredential()
        {            
            var useCredentail = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSRSUserCredential"].ToString());
            if (!useCredentail)
                return CredentialCache.DefaultNetworkCredentials;
            else
            {
                var ssrsUser = ConfigurationManager.AppSettings["ssrsUser"].ToString();
                var ssrsUserPwd = ConfigurationManager.AppSettings["ssrsUserPassword"].ToString();
                return new NetworkCredential(ssrsUser, ssrsUserPwd);
            }
        }
    }
}
