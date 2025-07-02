using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
   public class ReportHelper
    {
       public class Reports
       {
           /// <summary>
           /// Displays the report using common reportdisplaygeneral page,
           /// </summary>
           /// <param name="reportType"></param>
           /// <param name="parameters"></param>
           public static void DisplayReport(ReportType reportType, Dictionary<string, object> parameters)
           {
               string queryString = string.Format("{0}={1}", Utility.UIConstants.QueryStrings.REPORT_TYPE, (int)reportType);
               if (parameters != null)
               {
                   foreach (var key in parameters.Keys)
                   {
                       queryString = queryString + string.Format("&{0}={1}", key, parameters[key].ToString().Replace("&","__"));
                   }
               }

               Helper.PageRequestHelper.RedirectToPage(Utility.UIConstants.NamedPages.ReportDisplayGeneral, queryString);
           }
           public class ReportPages
           {
               public class MRM
               {
                   public const string Users = "ReportViewer/rptUsers.rpt"; 
               }
           }

           public enum ReportType
           {
               Unknown = 0,
               MRM_Users = 1,
              
           }
       }
    }
}
