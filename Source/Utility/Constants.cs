using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
   public partial class Constants
    {
       public class MessageTexts
       {
           public const string Insert_Success = "Data inserted successfully.";
           public const string Insert_Failed = "Insertion failed";
           public const string Update_Success = "Data updated successfully";
           public const string Update_Failed = "Update failed";
           public const string Delete_Success = "Data deleted successfully";
           public const string Delete_Failed = "Delete failed";
           public const string Duplicate_Entry = "Duplicate entry. This item is already exists";
           public const string Change_Password_Success = "Password changed successfully";
           public const string Change_Password_Failed = "Password change failed.";
           public const string Reset_Password_Success = "Password Reset successfully";
           public const string Reset_Password_Failed = "Password Reset failed.";
       }
      
       public class MessageKey
       {
           public const string ReturnValue = "ExecuteReturnValue";

       }
       public class CommonDbParameterNames
       {         

           public const string OPTION = "Option";
           public const string RETURN_KEY = "ReturnKey";
           public const string TOTAL_ROW_COUNT = "TotalRowCount";

           public const string START_ROW_INDEX = "StartRowIndex";
           public const string PAGE_NO = "PageNo";
           public const string PAGE_SIZE = "PageSize";
           public const string TOTAL_COUNT = "TotalCount";

           public const string SORT_EXPRESSION = "SortExpression";
           public const string FILTER_EXPRESSION = "FilterExpression";

       }
       public class OperationStatus
       {
           public const int SUCCESS = 1;
           public const int FAILED = -99;
           public const int ACCESS_DENIED = -100;
       }
       public class StatusType
       {
           public const int Active = 1;
           public const int Inactive = 0;
       }
    }
}
