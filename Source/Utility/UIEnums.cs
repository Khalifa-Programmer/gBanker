using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utility.Enums
{
    public enum LogAction
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3,
        SELECT = 4,
        ERROR = 5,
        SECURITY_EXCEPTION = 6,      
        REQUEST = 7,
        INFORMATION = 8,
        LOGIN = 9,
        LOGOUT = 10,
        UNKNOWN = 11
    }
    public class UIEnums
    {
        /// <summary>
        /// This enum is used for checking transaction type whether the transaction is for insert or update or delete data.
        /// </summary>
        public enum UIOperationMode
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
            Select = 4,
            UnKnown=5,
        }

        /// <summary>
        /// This enum is used for showing Message type whether the transaction is success or failure.
        /// </summary>
        public enum MessageIconType
        {
            Success = 1,
            Error = 2,
            Warning = 3,
            Information = 4,
            Confirmation = 5,
            AjaxLoading = 6,
        }
        /// <summary>
        /// This enum is used for compare validation. this enum is also available in Validation.js file
        /// </summary>
        public enum CompareOperator
        {
            Equal = 1,
            NotEqual = 2,
            GreaterThan = 3,
            GreaterThanOrEqual = 4,
            LessThan = 5,
            LessThanOrEqual = 6
        }
        /// <summary>
        /// This enum is used for compare validation. this enum is also available in Validation.js file
        /// </summary>
        public enum DataType
        {
            Integer = 1,
            Float = 2,
            Double = 3,
            String = 4,
            Date = 5
        }
    }
}