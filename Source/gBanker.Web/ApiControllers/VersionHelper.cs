using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace gBanker.Web.ApiControllers
{
    public class VersionHelper
    {
        public static int GetMobileAPIVersion()
        {
            var version = 0;
            try
            {
                version = int.Parse(ConfigurationManager.AppSettings["MobileAPPVersion"].ToString());
            }
            catch (Exception ex)
            {

            }
            return version;
        }
        //public static string GetMobileAPIVersion()
        //{
        //    var version = 0;
        //    var strUrl = "";
        //    try
        //    {
        //        version = int.Parse(ConfigurationManager.AppSettings["MobileAPPVersion"].ToString());
        //        strUrl = ConfigurationManager.AppSettings["MobileUrl"].ToString() + "#" + version;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return strUrl;
        //}
    }
}