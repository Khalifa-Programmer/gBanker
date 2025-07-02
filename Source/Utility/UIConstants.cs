using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Utility
{

    public class UIConstants
    {       

        public class NamedPages
        {
            public const string DefaultPage = "~/Default.aspx";
            public const string LoginPage = "~/Login.aspx";
            public const string LogoutPage = "~/Logout.aspx";
            public const string RegisterPage = "~/Pages/UserRight/Register.aspx";
            public const string ChangePasswordPage = "~/Pages/UserRight/ChangePassword.aspx";
            public const string PasswordRetrievePage = "~/Pages/UserRight/RetrievePassword.aspx";

            public const string ReportDisplayGeneral = "~/Pages/Reports/ReportDisplayGeneral.aspx";
        }

        public class CommandName
        {
            public const string INSERT = "Insert";
            public const string EDIT = "Edit";
            public const string DELETEITEM = "DeleteItem";
            public const string SORT = "Sort";
            public const string SORT_DIRECTION = "SortDirection";
            public const string SORT_COLUMN = "SortColumn";

            public const string FIRST = "First";
            public const string PREVIOUS = "Previous";
            public const string LAST = "Last";
            public const string NEXT = "Next";
        }

        public class SessionKeys
        {
            public const string USER_RIGHT_ID = "UserRightID";
            public const string USER_RIGHTS = "UserRights";
            public const string FILTER_EXPRESSION = "FilterExpression";
            public const string SORT_EXPRESSION = "SortExpression";
            public const string CURRENT_PAGE_NO = "CurrentPageNo";
            public const string START_ROW_INDEX = "StartRowIndex";

        }

        public class QueryStrings
        {
            public const string ID = "ID";
            public const string ENTITY_ID = "EntityID";
            public const string Page_ID = "PAGE_ID";
            public const string REQUEST_ID = "REQUEST_ID";
            public const string REPORT_TYPE = "REPORT_TYPE";

        }
        public class UIMessages
        {
            public const string FIELD_EMPTY = "Field is empty.";
            public const string PLEASE_SELECT = "Please Select.";
            public const string DUPLICATE_ENTRY = "Duplicate entry found.";
            public const string AUTHENTICATION_FAILED = "Authentication failed.";
            public const string SESSION_TIMEOUT = "Session time out. Please login again.";
            public const string USER_NOT_LOGGEDIN = "User is not logged in.";
            public const string QUERYSTRING_INCORRECT = "Query string is not in correct format.";
            public const string NO_DATA_FOUND = "No data found.";
            public const string PASSWORD_NOT_MATCHED = "New Password and Confirm New Password are not same";
            public const string CURRENT_PASSWORD_AND_NEW_PASSWORD_SAME = "Current Password and New Password are same";
            public const string INVALID_ID = "Invalid ID supplied.";
            public const string SELECT_DROP_DOWN_ITEM = "Please select item from drop down list";
            public const string FROM_DATE_IS_GREATER_THAN_TO_DATE = "From Date is greater than To Date.";
            public const string TO_DATE_IS_LESS_THAN_TODAY = "To Date is less than Today.";
        }
        public class UIMessageBox
        {
            public const string SHOWMSG_INVALID_DATE = "ShowMsg('Warning','Input is not a valid datetime.', '../Resources/Images/warning.jpg');";
            public const string SHOWMSG_INVALID_DATA = "ShowMsg('Warning','Invalid data. Should be an integer.', '../Resources/Images/warning.jpg');";
        }
        public class Settings
        {
            public const string PASSWORD_EXPIRATION_DAYS = "PASSWORD_EXPIRATION_DAYS";
        }
        public class Common
        {
            public const int MAX_COLUMN_CHAR_LENGTH = 20;
            public const int ZERO = 0;
            public const string DATE_FORMAT = "yyyy-MM-dd";
            public const string AUTO_GENERATED = "[Auto Generated]";
            public const Int16 Account_Type_ID_For_Asset = 3;
        }
        public class LogLevel
        {
            public const string DEBUG = "Debug";
            public const string ERROR = "Error";
            public const string FATAL = "Fatal";
            public const string INFO = "Info";
            public const string WARN = "Warn";
        }
    }
}