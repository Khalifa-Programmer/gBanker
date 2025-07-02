using System;
using System.Data;

namespace Utility.Extensions
{
    public enum SQLEqualityOperator
    {
        Equal,
        Like
    }

    public static class DataExtension
    {
        /// <summary>
        /// Replaces the sql special characters, an additional parameter for equal or like operator...
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sqlEqualityOperator"></param>
        /// <returns></returns>
        public static string WithSQLInjectioChecked(this string value, SQLEqualityOperator sqlEqualityOperator)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (sqlEqualityOperator == SQLEqualityOperator.Equal)
                    return value.WithSQLInjectioChecked(); //Just change ' with ''.

                char[] specialChars = { '|', '%', '\\', '_', '+', '&', '#', '?', '[', ']' };
                foreach (char c in specialChars)
                {
                    if (value.Contains(c.ToString()))
                    {
                        string replaceString = "|" + c.ToString(); //Add with an escape character...
                        value = value.Replace(c.ToString(), replaceString);
                    }
                }

                value = value.Replace("'", "''"); //replace ' 
                if (sqlEqualityOperator == SQLEqualityOperator.Like)
                {
                    value = string.Format("'%{0}%'", value);
                    value = string.Format("{0} ESCAPE '|'", value); //ADD ESCAPE KEYWORD.... 
                }
            }
            return value;
        }
        /// <summary>
        /// Replaces the sql special characters, works for equality operators.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string WithSQLInjectioChecked(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("'", "''"); //replace '
            }
            return value;
        }
        /// <summary>
        /// Gets the value from data reader with columnname or default value if the value is null.
        /// </summary>
        /// <typeparam name="T"> Type of value to return</typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName)
        {
            object columnValue = reader[columnName];
            T returnValue = default(T);
            if (!(columnValue is DBNull))
            {
                returnValue = (T)Convert.ChangeType(columnValue, typeof(T));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to Int32 value, if the column is null, then the value is the default value for int.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int ToInt32(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            int returnValue = default(int);
            if (!(columnValue is DBNull))
            {
                returnValue = (int)Convert.ChangeType(columnValue, typeof(int));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to GUID value, if the column is null, then the value is the default value for GUID.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static Guid ToGuid(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            Guid returnValue = default(Guid);
            if (!(columnValue is DBNull))
            {
                returnValue = (Guid)Convert.ChangeType(columnValue, typeof(Guid));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to double value, if the column is null, then the value is the default value for double.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static double ToDouble(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            double returnValue = default(double);
            if (!(columnValue is DBNull))
            {
                returnValue = (double)Convert.ChangeType(columnValue, typeof(double));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to DateTime value, if the column is null, then the value is the default value for DateTime.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            DateTime returnValue = default(DateTime);
            if (!(columnValue is DBNull))
            {
                returnValue = (DateTime)Convert.ChangeType(columnValue, typeof(DateTime));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to bool value, if the column is null, then the value is the default value for bool.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ToBoolean(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            bool returnValue = default(bool);
            if (!(columnValue is DBNull))
            {
                returnValue = (bool)Convert.ChangeType(columnValue, typeof(bool));
            }
            return returnValue;
        }
        /// <summary>
        /// Converts a datarow column value to string value, if the column is null, then the value is the default value for string.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string ToString(this DataRow dataRow, string columnName)
        {
            object columnValue = dataRow[columnName];
            string returnValue = default(string);
            if (!(columnValue is DBNull))
            {
                returnValue = (string)Convert.ChangeType(columnValue, typeof(string));
            }
            return returnValue;
        }
    }
}