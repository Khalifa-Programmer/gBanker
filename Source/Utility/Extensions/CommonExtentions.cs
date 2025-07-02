using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility.Extensions
{

    public static class CommonExtentions
    {
        public static byte[] StringToByteArray(this String str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static DateTime? ToDateTimeOrNull(this string str)
        {
            DateTime value;
            bool isParsed = DateTime.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                return null;
        }

        public static DateTime ToDateTime(this string str)
        {
            DateTime value;
            bool isParsed = DateTime.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                throw new Exception("Data is not in correct format.");
        }

        public static string ToStringOrEmptyIfDefault(this DateTime dt)
        {
            string value = dt == default(DateTime) ? "" : dt.ToString();
            return value;
        }

        public static string ToStringOrEmptyIfDefault(this DateTime dt, string format)
        {
            if (dt != null && dt == default(DateTime))
            {
                return dt.ToString(format);
            }
            else
                return string.Empty;
        }

        public static string ToStringOrEmptyIfDefault(this DateTime? dt, string format)
        {
            if (dt.HasValue && dt != null && dt == default(DateTime))
            {
                DateTime date = (DateTime)dt;
                return date.ToString(format);
            }
            else
                return string.Empty;
        }

        public static string ToStringOrEmptyIfDefault(this DateTime? dt)
        {
            string value = dt == default(DateTime) ? "" : dt.ToString();
            return value;
        }

        public static string ToShortDateStringOrEmptyIfDefault(this DateTime? dt)
        {
            string value = dt == default(DateTime) ? "" : dt.ToShortDateString();
            return value;
        }

        public static Int32? ToInt32OrNull(this string str)
        {
            Int32 value;
            bool isParsed = Int32.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                return null;
        }

        public static double? ToDoubleOrNull(this string str)
        {
            double value;
            bool isParsed = double.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                return null;
        }

        public static Int64? ToInt64OrNull(this string str)
        {
            Int64 value;
            bool isParsed = Int64.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                return null;
        }

        public static string ToShortDateString(this DateTime? dt)
        {
            if (dt.HasValue == false || dt == null || dt == default(DateTime))
                return string.Empty;
            else
            {
                return ((DateTime)dt).ToShortDateString();
            }
        }
        /// <summary>
        /// An extension method for string to get the int value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int32 ToInt(this string str)
        {
            Int32 value;
            bool isParsed = Int32.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                throw new Exception("Data type mitchmatch. The string is not a valid integer value.");
        }
        /// <summary>
        /// An extension method for string to get the long value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int64 ToLong(this string str)
        {
            Int64 value;
            bool isParsed = Int64.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                throw new Exception("Data type mitchmatch. The string is not a valid Int64 value.");
        }
        /// <summary>
        /// An extension method for string to get the double value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double value;
            bool isParsed = double.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                throw new Exception("Data type mitchmatch. The string is not a valid double value.");
        }
        /// <summary>
        /// An extension method for string to get the double value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this string str)
        {
            Int16 value;
            bool isParsed = Int16.TryParse(str, out value);
            if (isParsed)
                return value;
            else
                throw new Exception("Data type mitchmatch. The string is not a valid Int16 value.");
        }
    }
}
