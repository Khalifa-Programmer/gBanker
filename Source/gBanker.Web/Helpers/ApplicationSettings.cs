using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace gBanker.Web.Helpers
{
    public class ApplicationSettings
    {
        public static string OrganiztionName { get { return SessionHelper.OrganizationName; } }
        public static string ColDay { get { return ConfigurationManager.AppSettings["ColDay"]; } }
        public static string SmsOption { get { return ConfigurationManager.AppSettings["SmsOption"]; } }

    }
}