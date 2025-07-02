using System;
using System.Data;

namespace BasicDataAccess.Extensions
{
    public static class DataAccessExtension
    {
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
    }
}