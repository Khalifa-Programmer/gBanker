using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;
using System.Web.Profile;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using Utility.Enums;

namespace Utility
{
    public class Helper
    {

        #region LocalizationHelper
        public class LocalizationHelper
        {
            enum DateFormatSequenceEnum
            {
                DayMonthYear = 0,
                MonthDayYear = 1,
                YearMonthDay = 2,
                YearDayMonth = 3,
                DayYearMonth = 4,
                MonthYearDay = 5
            }
            private static DateFormatSequenceEnum dateSquence;

            /// <summary>
            /// Gets the short date format for current culture.. Used in calender control to set the dateFormat.
            /// </summary>
            /// <returns></returns>
            public static string GetDateFormat()
            {
                string dateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string separator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
                string calenderPattern = string.Empty;
                string[] formats = dateFormat.Split(separator.ToCharArray());

                if (formats.Length == 3)
                {
                    string first = formats[0].Substring(0, formats[0].Length <= 2 ? formats[0].Length : 2).ToLower();
                    string second = formats[1].Substring(0, formats[1].Length <= 2 ? formats[1].Length : 2).ToLower();
                    string third = formats[2].Substring(0, formats[2].Length <= 2 ? formats[2].Length : 2).ToLower();

                    SetDateFormatSequnceEnum(first, second, third);

                    calenderPattern = string.Format("{0}{1}{2}{1}{3}", first, separator, second, third);
                }
                return calenderPattern;
            }
            /// <summary>
            /// Gets the  date format for current culture.. Used in calender control to set the display text for the calender.
            /// </summary>
            /// <returns></returns>
            public static string GetCalenderDateFormatText()
            {
                string dateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string separator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
                string calenderPattern = string.Empty;
                string[] formats = dateFormat.Split(separator.ToCharArray());

                if (formats.Length == 3)
                {
                    string first = formats[0].Length == 1 ? formats[0] + formats[0] : formats[0];
                    string second = formats[1].Length == 1 ? formats[1] + formats[1] : formats[1];
                    string third = formats[2].Length == 1 ? formats[2] + formats[2] : formats[2];

                    calenderPattern = string.Format("{0}{1}{2}{1}{3}", first.ToLower(), separator, second.ToLower(), third.ToLower());
                }
                return calenderPattern;
            }
            private static void SetDateFormatSequnceEnum(string first, string second, string third)
            {
                if (first.ToLower().Contains("d") && second.ToLower().Contains("m") && third.ToLower().Contains("y"))
                    dateSquence = DateFormatSequenceEnum.DayMonthYear;
                if (first.ToLower().Contains("d") && second.ToLower().Contains("y") && third.ToLower().Contains("m"))
                    dateSquence = DateFormatSequenceEnum.DayYearMonth;
                if (first.ToLower().Contains("m") && second.ToLower().Contains("d") && third.ToLower().Contains("y"))
                    dateSquence = DateFormatSequenceEnum.MonthDayYear;
                if (first.ToLower().Contains("m") && second.ToLower().Contains("y") && third.ToLower().Contains("d"))
                    dateSquence = DateFormatSequenceEnum.MonthYearDay;
                if (first.ToLower().Contains("y") && second.ToLower().Contains("m") && third.ToLower().Contains("d"))
                    dateSquence = DateFormatSequenceEnum.YearMonthDay;
                if (first.ToLower().Contains("y") && second.ToLower().Contains("d") && third.ToLower().Contains("m"))
                    dateSquence = DateFormatSequenceEnum.YearDayMonth;
            }
            /// <summary>
            /// Gets the current culture short date pattern.
            /// </summary>
            /// <returns></returns>
            public static string GetShortDatePattern()
            {
                return Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
            /// <summary>
            /// Gets the current culture date separator.
            /// </summary>
            /// <returns></returns>
            public static string GetDateSeparator()
            {
                return Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
            }
            /// <summary>
            /// Gets the currency decimal separator.
            /// </summary>
            /// <returns></returns>
            public static string GetCurrencyDecimalSeparator()
            {
                return Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            }
            /// <summary>
            /// Gets the current culture currency group separator.
            /// </summary>
            /// <returns></returns>
            public static string GetCurrencyGroupSeparator()
            {
                return Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            }
            /// <summary>
            /// Gets the currency group sizes for the current culture\
            /// </summary>
            /// <returns></returns>
            public static int[] GetCurrencyGroupSizes()
            {
                return Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes;
            }
            /// <summary>
            /// Gets the regular expression for the current culture date format.
            /// </summary>
            /// <returns></returns>
            public static string GetDateFormatRegExp()
            {
                //var pattern = /(0?[1-9]|1\d|2\d|3[0-1])[\.](0?[1-9]|1[0-2])[\.](19\d{2}|20\d{2})/;
                string dayRE = string.Empty, monthRE = string.Empty, yearRE = string.Empty, dateSeperator = string.Empty, regularExpression = string.Empty;
                string dateFormat = GetDateFormat();

                dayRE = "(0?[1-9]|[12][0-9]|3[01])";
                monthRE = "(0?[1-9]|1[012])";
                yearRE = "((19|20)\\d\\d)";
                dateSeperator = GetDateSeparator();

                if(dateSeperator.Contains("."))
                    dateSeperator = @"\.";
                if (dateSeperator.Contains("/"))
                    dateSeperator = @"\/";

                if (dateSquence == DateFormatSequenceEnum.DayMonthYear)
                    regularExpression = dayRE + dateSeperator + monthRE + dateSeperator + yearRE;
                if (dateSquence == DateFormatSequenceEnum.DayYearMonth)
                    regularExpression = dayRE + dateSeperator + yearRE + dateSeperator + monthRE;
                if (dateSquence == DateFormatSequenceEnum.MonthDayYear)
                    regularExpression = monthRE + dateSeperator + dayRE + dateSeperator + yearRE;
                if (dateSquence == DateFormatSequenceEnum.MonthYearDay)
                    regularExpression = monthRE + dateSeperator + yearRE + dateSeperator + dayRE;
                if (dateSquence == DateFormatSequenceEnum.YearDayMonth)
                    regularExpression = yearRE + dateSeperator + dayRE + dateSeperator + monthRE;
                if (dateSquence == DateFormatSequenceEnum.YearMonthDay)
                    regularExpression = yearRE + dateSeperator + monthRE + dateSeperator + dayRE;

                return regularExpression;
            }
            /// <summary>
            /// Gets the regular expression for the current culture number format.
            /// </summary>
            /// <returns></returns>
            public static string GetCurrencyFormatRegExp()
            {
                //Sample regular expression is below.
                //(\d{1,3},?(\d{3},?)*\d{3}(\.\d{1,3})?|\d{1,3}(\.\d{1,2})?) for us currency..
                // @"(\d{1,3}\s?(\d{3}\s?)*\d{3}((,|\.)\d{1,2})?|\d{1,3}((,|\.)\d{1,2})?)"; for finland

                int[] groupSizes = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes;
                string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                string groupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;

                string regDecimal = string.Empty, regGroupSep = string.Empty, regularExp = string.Empty;
                int decimalPoint = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits;

                if (decimalSeparator.Contains(".")) //. is a special character in regular expression, so change this with \. character.
                    regDecimal = decimalSeparator.Replace(".", @"\.");
                else
                    regDecimal = decimalSeparator;

                if (groupSeparator.Contains("."))
                    regGroupSep = groupSeparator.Replace(".", @"\.");
                else if (string.IsNullOrEmpty(groupSeparator))
                    regGroupSep = @"\s";
                else
                    regGroupSep = groupSeparator;

                string sepSizeRange1 = string.Empty, sepSizeFixed1 = string.Empty, decimalPointRange = string.Empty, sepSizeRange2 = string.Empty, sepSizeFixed2 = string.Empty;
                decimalPointRange = "{1," + decimalPoint + "}";

                string numberWithoutSeparatorRangeAndDecimalPoint = string.Empty; //for 1.00 or 12.00 or 123.00
                string numberWithoutSeparatorFixedAndDecimalPoint = string.Empty; //for 123.00 if it has like 12,123.00
                string numberAndSeparator = string.Empty; //for 12, that is used in 12,123.00

                //Group Sizes can be upto 2 lengths, eg: 11,00,00,215.00 Bangladesh style and 111,222,222,222.00 US Style or Fin Style.
                switch (groupSizes.Length)
                {
                    case 1:
                        sepSizeRange1 = "{1," + groupSizes[0] + "}";//Can be upto the group size number of characters for first group.
                        sepSizeFixed1 = "{" + groupSizes[0] + "}"; //Fixed size characters.
                        numberWithoutSeparatorRangeAndDecimalPoint = string.Format(@"\d{0}({1}\d{2})?", sepSizeRange1, regDecimal, decimalPointRange);
                        numberWithoutSeparatorFixedAndDecimalPoint = string.Format(@"\d{0}({1}\d{2})?", sepSizeFixed1, regDecimal, decimalPointRange);
                        numberAndSeparator = string.Format(@"\d{0}{1}?(\d{2}{3}?)*", sepSizeRange1, regGroupSep, sepSizeFixed1, regGroupSep);
                        break;
                    case 2:
                        sepSizeRange1 = "{1," + groupSizes[0] + "}";//Can be upto the group size number of characters for first group.
                        sepSizeFixed1 = "{" + groupSizes[0] + "}";
                        sepSizeRange2 = "{1," + groupSizes[1] + "}"; //Can be upto the group size number of characters for second group.
                        sepSizeFixed2 = "{" + groupSizes[1] + "}";

                        numberWithoutSeparatorRangeAndDecimalPoint = string.Format(@"\d{0}({1}\d{2})?", sepSizeRange1, regDecimal, decimalPointRange);
                        numberWithoutSeparatorFixedAndDecimalPoint = string.Format(@"\d{0}({1}\d{2})?", sepSizeFixed1, regDecimal, decimalPointRange);
                        numberAndSeparator = string.Format(@"\d{0}{1}?(\d{2}{3}?)*", sepSizeRange2, regGroupSep, sepSizeFixed2, regGroupSep);
                        break;
                    default:
                        break;
                }
                regularExp = "(" + numberAndSeparator + numberWithoutSeparatorFixedAndDecimalPoint + "|" + numberWithoutSeparatorRangeAndDecimalPoint + ")";
                return regularExp;    
            }
        }

        #endregion

        #region ValidationHelper

        /// <summary>
        /// This class provides all Validation function.
        /// </summary>
        public class ValidationHelper
        {
            public const string VALIDATION_ATTRIBUTE_KEY = "Attributes";
            public const string VALIDATION_COMPARE_ATTRIBUTE_KEY = "CompareAttributes";
            public const string VALIDATION_SELECT_ATTRIBUTE_KEY = "SelectAttributes";
            // 
            /// <summary>
            /// Get the Validation String. ex "IsRequired:1,FixedLength:5,MaxLength:20,MinLength:5,IsEmail:1,IsNumeric:1,IsDecimal=1"
            /// </summary>
            /// <param name="IsRequired"></param>
            /// <param name="FixedLength"></param>
            /// <param name="MinLength"></param>
            /// <param name="MaxLength"></param>
            /// <param name="IsEmail"></param>
            /// <param name="IsNumeric"></param>
            /// <param name="IsDecimal"></param>
            /// <returns></returns>
            public static string GetValidationString(bool? IsRequired, int? FixedLength, int? MinLength, int? MaxLength, bool? IsAlphanumeric, bool? IsIntger, bool? IsDecimal)
            {
                StringBuilder sb = new StringBuilder();

                if (IsRequired.HasValue && IsRequired != null)
                {
                    if (IsRequired == true)
                        sb.Append("IsRequired:1,");
                    else
                        sb.Append("IsRequired:0,");
                }
                if (FixedLength.HasValue && FixedLength != null)
                    sb.Append(string.Format("FixedLength:{0},", FixedLength));
                if (MinLength.HasValue && MinLength != null)
                    sb.Append(string.Format("MinLength:{0},", MinLength));

                if (MaxLength.HasValue && MaxLength != null)
                    sb.Append(string.Format("MaxLength:{0},", MaxLength));
                if (IsAlphanumeric.HasValue && IsAlphanumeric != null)
                    sb.Append("IsAlphanumeric:1,");
                if (IsIntger.HasValue && IsIntger != null && IsIntger == true)
                    sb.Append("IsIntger:1,");
                if (IsDecimal.HasValue && IsDecimal != null && IsDecimal == true)
                    sb.Append("IsDecimal:1,");

                string validationString = sb.ToString();

                if (validationString.Contains(","))
                    validationString = validationString.Remove(validationString.Length - 1, 1);

                return validationString;
            }

           
            public static string GetCompareValidationString(string comparedControlID, string valueToCompare, UIEnums.CompareOperator compareOperator, UIEnums.DataType dataType)
            {
                // Text, Date, number, Equal, less, greater

                StringBuilder sb = new StringBuilder();
                string op;

                switch (compareOperator)
                {
                    case UIEnums.CompareOperator.Equal: op = "=="; break;
                    case UIEnums.CompareOperator.NotEqual: op = "!="; break;
                    case UIEnums.CompareOperator.GreaterThan: op = ">"; break;
                    case UIEnums.CompareOperator.GreaterThanOrEqual: op = ">="; break;
                    case UIEnums.CompareOperator.LessThan: op = "<"; break;
                    case UIEnums.CompareOperator.LessThanOrEqual: op = "<="; break;
                    default: op = "=="; break;
                }

                sb.Append(string.Format("CompareOperator:{0},", op));
                sb.Append(string.Format("DataType:{0},", dataType.ToString()));

                if (!string.IsNullOrEmpty(comparedControlID))
                    sb.Append(string.Format("ComparedControlID:{0},", comparedControlID));
                if (!string.IsNullOrEmpty(valueToCompare))
                    sb.Append(string.Format("ValueToCompare:{0},", valueToCompare));

                string validationString = sb.ToString();

                if (validationString.Contains(","))
                    validationString = validationString.Remove(validationString.Length - 1, 1);

                return validationString;
            }

            public static string GetSelectValidationString(bool? isSelect)
            {
                StringBuilder sb = new StringBuilder();

                if (isSelect.HasValue && isSelect != null && isSelect == true)
                    sb.Append("IsSelect:1,");

                string validationString = sb.ToString();

                if (validationString.Contains(","))
                    validationString = validationString.Remove(validationString.Length - 1, 1);

                return validationString;
            }

            /// <summary>
            /// Check if the parameter is valid Guid or not.
            /// </summary>
            /// <param name="guidID">
            /// A String formatted in this pattern: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            /// where the value of the GUID is represented as a series of lower-case hexadecimal
            /// digits in groups of 8, 4, 4, 4, and 12 digits and separated by hyphens. An
            /// example of a return value is "382c74c3-721d-4f34-80e5-57657b6cbc27".
            /// </param>
            /// <returns></returns>
            public static bool IsValidGuid(string guidID)
            {
                if (guidID.Length != 36)
                    return false;
                else
                {
                    string[] arrGuid = guidID.Split('-');

                    if (arrGuid.Length != 5)
                        return false;
                    else
                    {
                        if (arrGuid[0].Length != 8)
                            return false;
                        if (arrGuid[1].Length != 4)
                            return false;
                        if (arrGuid[2].Length != 4)
                            return false;
                        if (arrGuid[3].Length != 4)
                            return false;
                        if (arrGuid[4].Length != 12)
                            return false;
                    }
                }
                return true;
            }
        }

        #endregion

        #region PageRequestHelper
        /// <summary>
        /// This class provides all page request function.
        /// </summary>
        public class PageRequestHelper
        {

            /// <summary>
            /// Try to get querysring parameter value. 
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            public static bool TryQueryStringParameter(string parameter)
            {
                object value = HttpContext.Current.Request.QueryString[parameter];
                return value != null;
            }
            /// <summary>
            /// Gets a query string parameter value.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="parameter"></param>
            /// <param name="throughIfNotFoundOrException"></param>
            /// <returns></returns>
            public static string GetQueryStringParameterValue(string parameter, bool throughIfNotFoundOrException)
            {
                object value = HttpContext.Current.Request.QueryString[parameter];

                if (throughIfNotFoundOrException)
                {
                    if (TryQueryStringParameter(parameter))
                    {
                        if (value == null)
                            throw new Exception("Query string Parameter not found");
                        else
                            return value.ToString();
                    }
                    else
                        throw new Exception("Query string Parameter not found");
                }
                else
                    return value.ToString();
            }
            /// <summary>
            /// Try to get session parameter value. 
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            public static bool TrySessionParameter(string parameter)
            {
                object value = HttpContext.Current.Session[parameter];
                return value != null;
            }
            /// <summary>
            /// Gets a session parameter value. If the parameter not found in the session, returns default value of that type or throw exception
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="parameter"></param>
            /// <param name="throughExceptionIfNotFound"></param>
            /// <returns></returns>
            public static T GetSessionParameterValue<T>(string parameter, bool throwExceptionIfNotFound)
            {
                object value = HttpContext.Current.Session[parameter] as object;

                if (value == null)
                {
                    if (throwExceptionIfNotFound)
                        throw new Exception("Session data not found");
                    else
                        return default(T);
                }
                else
                    return (T)value;
            }

            public static Control FindControlRecursive(Control root, string id)
            {
                if (root.ID == id)
                    return root;
                foreach (Control ctl in root.Controls)
                {
                    Control foundCtl = FindControlRecursive(ctl, id);

                    if (foundCtl != null)
                        return foundCtl;
                }
                return null;
            }


            /// <summary>
            /// Returns an array with the names of all local Themes
            /// </summary>
            public static string[] GetThemes()
            {
                if (HttpContext.Current.Cache["SiteThemes"] != null)
                {
                    return (string[])HttpContext.Current.Cache["SiteThemes"];
                }
                else
                {
                    string themesDirPath = HttpContext.Current.Server.MapPath("~/App_Themes");
                    // get the array of themes folders under /app_themes
                    string[] themes = Directory.GetDirectories(themesDirPath);
                    for (int i = 0; i <= themes.Length - 1; i++)
                        themes[i] = Path.GetFileName(themes[i]);
                    // cache the array with a dependency to the folder
                    CacheDependency dep = new CacheDependency(themesDirPath);
                    HttpContext.Current.Cache.Insert("SiteThemes", themes, dep);
                    return themes;
                }
            }

            /// <summary>
            /// Adds the onfocus and onblur attributes to all input controls found in the specified parent,
            /// to change their apperance with the control has the focus
            /// </summary>
            public static void SetInputControlsHighlight(Control container, string className, bool onlyTextBoxes)
            {
                foreach (Control ctl in container.Controls)
                {
                    if ((onlyTextBoxes && ctl is TextBox) ||
                        (!onlyTextBoxes && (ctl is TextBox || ctl is DropDownList ||
                                            ctl is ListBox || ctl is CheckBox || ctl is RadioButton ||
                                            ctl is RadioButtonList || ctl is CheckBoxList)))
                    {
                        WebControl wctl = ctl as WebControl;
                        if (wctl != null)
                        {
                            wctl.Attributes.Add("onfocus", string.Format("this.className = '{0}';", className));
                            wctl.Attributes.Add("onblur", "this.className = '';");
                        }
                    }
                    else
                    {
                        if (ctl.Controls.Count > 0)
                            SetInputControlsHighlight(ctl, className, onlyTextBoxes);
                    }
                }
            }


            /// <summary>
            /// Converts the input plain-text to HTML version, replacing carriage returns
            /// and spaces with <br /> and &nbsp;
            /// </summary>
            public static string ConvertToHtml(string content)
            {
                content = HttpUtility.HtmlEncode(content);
                content = content.Replace("  ", "&nbsp;&nbsp;").Replace(
                    "\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br>");
                return content;
            }

            public static string DecryptToReturnString(string stringToDecrypt, string encryptionKey)
            {
                //get the length of byte array from string to Decrypted.
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                string returnString = string.Empty;
                try
                {
                    //get byte array for 8 bit UTF8 encryption key.
                    byte[] key = Encoding.UTF8.GetBytes(encryptionKey.ToCharArray(), 0, 8);

                    //get random hex code of byte for the description.
                    byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
                    stringToDecrypt = stringToDecrypt.Replace(" ", "+");

                    //Convert descrypted strong to base 64 character.
                    //http://www.garykessler.net/library/base64.html
                    //http://www.faqs.org/rfcs/rfc3548.html

                    inputByteArray = Convert.FromBase64String(stringToDecrypt);

                    //new memory stream for writing byte of input stream.
                    MemoryStream ms = new MemoryStream();

                    //new DESCryptoServiceProvider from System.Security.Cryptography;
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                    //new CryptoStream from System.Security.Cryptography and write the decrypted string to memory 
                    //stream using same hex and excrypted key that used when encrypted string.
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                    //write decrypted string to cryptor stream which contain memory stream.
                    cs.Write(inputByteArray, 0, inputByteArray.Length);

                    //Flush immediately decrypted string to cryptor stream..
                    cs.FlushFinalBlock();

                    Encoding encoding = Encoding.UTF8;

                    //get the depcrypted string from memory stream and return the string.
                    returnString = encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return returnString;
            }

            public static NameValueCollection DecryptToReturnNameValueCollection(string stringToDecrypt, string encryptionKey)
            {
                //declare namevaluecollection for holding return key value pair.
                NameValueCollection queryString = new NameValueCollection();

                //get the length of byte array from string to Decrypted.
                byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
                try
                {
                    //get byte array for 8 bit UTF8 encryption key.
                    byte[] key = Encoding.UTF8.GetBytes(encryptionKey.ToCharArray(), 0, 8);
                    //get random hex code of byte for the description.
                    byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
                    stringToDecrypt = stringToDecrypt.Replace(" ", "+");

                    //Convert descrypted strong to base 64 character.
                    //http://www.garykessler.net/library/base64.html
                    //http://www.faqs.org/rfcs/rfc3548.html
                    inputByteArray = Convert.FromBase64String(stringToDecrypt);

                    //new memory stream for writing byte of input stream for holding binary bits.
                    MemoryStream ms = new MemoryStream();
                    //new DESCryptoServiceProvider from System.Security.Cryptography;
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                    //new CryptoStream from System.Security.Cryptography and write the decrypted string to memory 
                    //stream using same hex and excrypted key that used when encrypted string.
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

                    //write decrypted string to cryptor stream which contain memory stream.
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    //Flush immediately decrypted string to cryptor stream..
                    cs.FlushFinalBlock();

                    Encoding encoding = Encoding.UTF8;
                    //get the depcrypted string from memory stream and return the string.
                    string decryptedString = encoding.GetString(ms.ToArray());

                    //split string with & for name value pair.
                    string[] nameVals = decryptedString.Split('&');
                    queryString = new NameValueCollection(nameVals.Length);

                    //adding name value pair of the decrypted string.
                    foreach (string nameValPair in nameVals)
                    {
                        string[] pair = nameValPair.Split('=');
                        queryString.Add(pair[0], pair[1]);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return queryString;
            }
            public static string Encrypt(string stringToEncrypt, string encryptionKey)
            {
                string returnString = string.Empty;
                try
                {
                    //get byte array for 8 bit UTF8 encryption key.
                    byte[] key = Encoding.UTF8.GetBytes(encryptionKey.ToCharArray(), 0, 8);
                    //get random hex code of byte for the description.
                    byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };

                    //new DESCryptoServiceProvider from System.Security.Cryptography;
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                    //get the length of byte array from string to encrypted.
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);

                    //new CryptoStream from System.Security.Cryptography and write the decrypted string to memory 
                    //stream using same hex and encrypted key.
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

                    //write encrypted string to cryptor stream which contain memory stream.
                    cs.Write(inputByteArray, 0, inputByteArray.Length);

                    //Flush immediately encrypted string to cryptor stream.
                    cs.FlushFinalBlock();

                    //Converts the value of an array of 8-bit unsigned integers to its equivalent String representation encoded 
                    //with base 64 digits.
                    //retun encrypted string.

                    returnString = Convert.ToBase64String(ms.ToArray());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return returnString;

            }
            public static int GetRandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }


            public static int MaxValue = int.MaxValue;

            #region Private Methods

            private static string GetCurrentUserName()
            {
                return HttpContext.Current.User.Identity.Name;
            }
            #endregion

            #region Fields and Properties

            public static string CurrentApplicationPath { get; set; }

            protected static IPrincipal CurrentUser
            {
                get { return HttpContext.Current.User; }
            }

            public static string CurrentUserName
            {
                get
                {
                    string userName = "";
                    if (IsCurrentUserAuthenticated())
                        userName = GetCurrentUserName();

                    return userName;
                }
            }
            public static string LoggedInUserName
            {
                get
                {
                    //TODO: Uncomment the following line and comment the next line in production.
                    //string userName = "";
                    string userName = "DEVELOPER";
                    if (IsCurrentUserAuthenticated())
                        userName = GetCurrentUserName();

                    return userName;
                }
            }
            public static ProfileBase Profile
            {
                get { return HttpContext.Current.Profile; }
            }
            public static Cache Cache
            {
                get { return HttpContext.Current.Cache; }
            }

            public static WebsiteElement Settings
            {
                get { return Globals.Settings.WebsiteConfiguration; }
            }

            public static SpecificSettingsElement SpecificSettings
            {
                get { return Globals.Settings.SpecificSettings; }
            }

            public static EncryptionDecryptionElement EncryptionDecryptionSettings
            {
                get { return SpecificSettings.EncryptionDecryptionKey; }
            }

            public static MailSettingsElement MailSettings
            {
                get { return SpecificSettings.MailSettings; }
            }

            public static bool IsCurrentUserAuthenticated()
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }

            public static string ConvertNullToEmptyString(string input)
            {
                return (input ?? "");
            }


            public static void CacheData(string key, object data)
            {
                if (data != null)
                {
                    Cache.Insert(key, data, null, DateTime.Now.AddSeconds(86400), TimeSpan.Zero);
                }
            }
            public static void PurgeCacheItems(string prefix)
            {
                prefix = prefix.ToLower();
                List<string> itemsToRemove = new List<string>();
                IDictionaryEnumerator enumerator = Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.ToString().ToLower().Contains(prefix.ToLower()))
                        itemsToRemove.Add(enumerator.Key.ToString());
                }

                foreach (string itemToRemove in itemsToRemove)
                    Cache.Remove(itemToRemove);
            }
            public static object GetCacheData(string key)
            {
                return Cache[key];
            }
            public static string GetDuration(DateTime msgDate)
            {
                TimeSpan timeDuration = DateTime.UtcNow - msgDate;
                string returnSpanStr;

                if (timeDuration.TotalSeconds < 60)
                {
                    returnSpanStr = "a moment";
                }
                else if (timeDuration.TotalMinutes < 60)
                {
                    double minute = Math.Floor(timeDuration.TotalMinutes);
                    returnSpanStr = minute.ToString() + " minute" + (minute > 1 ? "s" : "");
                }
                else if (timeDuration.TotalHours < 24)
                {
                    double hour = Math.Floor(timeDuration.TotalHours);
                    returnSpanStr = hour.ToString() + " hour" + (hour > 1 ? "s" : "");
                }
                else if (timeDuration.TotalDays < 30)
                {
                    double day = Math.Floor(timeDuration.TotalDays);
                    returnSpanStr = day.ToString() + " day" + (day > 1 ? "s" : "");
                }
                else
                {
                    double months = (timeDuration.TotalDays - 1) / 30;
                    if (months < 12)
                    {
                        double month = Math.Floor(months);
                        double monthFract = months - month;
                        string monthStr = string.Empty;
                        if (monthFract >= 0.7)
                        {
                            month = Math.Ceiling(months);
                            monthStr = month.ToString();
                        }
                        else if (monthFract >= 0.4)
                        {
                            monthStr = month.ToString() + " & half";
                        }
                        else
                        {
                            monthStr = month.ToString();
                        }

                        returnSpanStr = monthStr + " month" + (month > 1 ? "s" : "");
                    }
                    else
                    {
                        double years = months / 12;
                        double year = Math.Floor(years);
                        double yearFract = years - year;
                        string yearStr = string.Empty;
                        if (yearFract >= 0.7)
                        {
                            year = Math.Ceiling(years);
                            yearStr = year.ToString();
                        }
                        else if (yearFract >= 0.4)
                        {
                            yearStr = year.ToString() + " & half";
                        }
                        else
                        {
                            yearStr = year.ToString();
                        }
                        returnSpanStr = yearStr + " year" + (year > 1 ? "s" : "");
                    }
                }

                return returnSpanStr;
            }

            public static bool IsInCache(string key)
            {
                return Settings.EnableCaching && Cache[key] != null;
            }

            #endregion

   

            #region Session objects
            public static object GetSessionData(string key)
            {
                return HttpContext.Current.Session[key];
            }
            public static void RemoveSessionData(string key)
            {
                HttpContext.Current.Session.Remove(key);
            }
            public static void SetSessionData(string key, object value)
            {
                HttpContext.Current.Session[key] = value;
            }
            #endregion


            #region Page Request Handler
            /// <summary>
            /// Not implemented yet...
            /// </summary>
            /// <param name="PageName"></param>
            /// <param name="QueryStringParams"></param>
            public static void RedirectToPage(string PageName, Dictionary<string, string> QueryStringParams)
            {
            }
            /// <summary>
            /// Builds a URL from PageName and a specific QuerySting. Then redirect to specified URL.
            /// </summary>
            /// <param name="PageName"></param>
            /// <param name="QueryString"></param>
            public static void RedirectToPage(string PageName, string QueryString)
            {
                string pageURL;
                if (PageName != null && PageName.Length > 0)
                {
                    if (QueryString != null && QueryString.Length > 0)
                    {
                        if (QueryString.Contains("?"))
                            pageURL = string.Format("{0}{1}", PageName, QueryString);
                        else
                            pageURL = string.Format("{0}?{1}", PageName, QueryString);
                    }
                    else
                        pageURL = PageName;

                    HttpContext.Current.Response.Redirect(pageURL);
                }
                else
                    throw new Exception("Page Name cannot be null or zero length");
            }
            #endregion

        }

        #endregion

        #region LogHelper

        /// <summary>
        /// This class provides all Log function.
        /// </summary>
        //public class LogHelper : log4net.ILog
        //{
        //    private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //    private LogSettingsElement LogSettings = Globals.URMSettings.LogSettings;
        //    //private static EmailHelper email = new EmailHelper();

        //    public static LogHelper Log
        //    {
        //        get { return new LogHelper(); }
        //    }

        //    #region Debug

        //    public void Debug(object message, Exception exception)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Debug(message, exception);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.DEBUG, message.ToString() + "<br /><br />EXCEPTION: " + exception.Message);
        //    }

        //    public void Debug(object message)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Debug(message);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.DEBUG, message.ToString());
        //    }

        //    public void Debug(object message, Exception exception, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Debug(message, exception);
        //    }

        //    public void Debug(object message, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Debug(message);
        //    }

        //    public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        //    {
        //        log.DebugFormat(provider, format, args);
        //    }

        //    public void DebugFormat(string format, object arg0, object arg1, object arg2)
        //    {
        //        log.DebugFormat(format, arg0, arg1, arg2);
        //    }

        //    public void DebugFormat(string format, object arg0, object arg1)
        //    {
        //        log.DebugFormat(format, arg0, arg1);
        //    }

        //    public void DebugFormat(string format, object arg0)
        //    {
        //        log.DebugFormat(format, arg0);
        //    }

        //    public void DebugFormat(string format, params object[] args)
        //    {
        //        log.DebugFormat(format, args);
        //    }
        //    #endregion

        //    #region Error
        //    public void Error(object message, Exception exception)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Error(message, exception);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.ERROR, message.ToString() + "<br /><br />EXCEPTION: " + exception.Message);
        //    }

        //    public void Error(object message)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Error(message);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.ERROR, message.ToString());
        //    }

        //    public void Error(object message, Exception exception, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Error(message, exception);
        //    }

        //    public void Error(object message, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Error(message);
        //    }
        //    public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        //    {
        //        log.ErrorFormat(provider, format, args);
        //    }

        //    public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        //    {
        //        log.ErrorFormat(format, arg0, arg1, arg2);
        //    }

        //    public void ErrorFormat(string format, object arg0, object arg1)
        //    {
        //        log.ErrorFormat(format, arg0, arg1);
        //    }

        //    public void ErrorFormat(string format, object arg0)
        //    {
        //        log.ErrorFormat(format, arg0);
        //    }

        //    public void ErrorFormat(string format, params object[] args)
        //    {
        //        log.ErrorFormat(format, args);
        //    }
        //    #endregion

        //    #region Fatal
        //    public void Fatal(object message, Exception exception)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Fatal(message, exception);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.FATAL, message.ToString() + "<br /><br />EXCEPTION: " + exception.Message);
        //    }

        //    public void Fatal(object message)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Fatal(message);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.FATAL, message.ToString());
        //    }

        //    public void Fatal(object message, Exception exception, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Fatal(message, exception);
        //    }

        //    public void Fatal(object message, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Fatal(message);
        //    }

        //    public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        //    {
        //        log.FatalFormat(provider, format, args);
        //    }

        //    public void FatalFormat(string format, object arg0, object arg1, object arg2)
        //    {
        //        log.FatalFormat(format, arg0, arg1, arg2);
        //    }

        //    public void FatalFormat(string format, object arg0, object arg1)
        //    {
        //        log.FatalFormat(format, arg0, arg1);
        //    }

        //    public void FatalFormat(string format, object arg0)
        //    {
        //        log.FatalFormat(format, arg0);
        //    }

        //    public void FatalFormat(string format, params object[] args)
        //    {
        //        log.FatalFormat(format, args);
        //    }
        //    #endregion

        //    #region Info
        //    public void Info(object message, Exception exception)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Info(message, exception);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.INFO, message.ToString() + "<br /><br />EXCEPTION: " + exception.Message);
        //    }

        //    public void Info(object message)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Info(message);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.INFO, message.ToString());
        //    }

        //    public void Info(object message, Exception exception, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Info(message, exception);
        //    }

        //    public void Info(object message, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Info(message);
        //    }

        //    public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        //    {
        //        log.InfoFormat(provider, format, args);
        //    }

        //    public void InfoFormat(string format, object arg0, object arg1, object arg2)
        //    {
        //        log.InfoFormat(format, arg0, arg1, arg2);
        //    }

        //    public void InfoFormat(string format, object arg0, object arg1)
        //    {
        //        log.InfoFormat(format, arg0, arg1);
        //    }

        //    public void InfoFormat(string format, object arg0)
        //    {
        //        log.InfoFormat(format, arg0);
        //    }

        //    public void InfoFormat(string format, params object[] args)
        //    {
        //        log.InfoFormat(format, args);
        //    }

        //    #endregion

        //    #region Boolean Property
        //    public bool IsDebugEnabled
        //    {
        //        get { return log.IsDebugEnabled; }
        //    }

        //    public bool IsErrorEnabled
        //    {
        //        get { return log.IsErrorEnabled; }
        //    }

        //    public bool IsFatalEnabled
        //    {
        //        get { return log.IsFatalEnabled; }
        //    }

        //    public bool IsInfoEnabled
        //    {
        //        get { return IsInfoEnabled; }
        //    }

        //    public bool IsWarnEnabled
        //    {
        //        get { return log.IsWarnEnabled; }
        //    }

        //    #endregion

        //    #region Warn
        //    public void Warn(object message, Exception exception)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Warn(message, exception);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.WARN, message.ToString() + "<br /><br />EXCEPTION: " + exception.Message);
        //    }

        //    public void Warn(object message)
        //    {
        //        if (LogSettings.IsLogFileEnable)
        //            log.Warn(message);
        //        if (LogSettings.IsEmailEnable)
        //            EmailHelper.Email.SendMessage(UIConstants.LogLevel.WARN, message.ToString());
        //    }

        //    public void Warn(object message, Exception exception, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Warn(message, exception);
        //    }

        //    public void Warn(object message, Type type)
        //    {
        //        log = LogManager.GetLogger(type);
        //        Warn(message);
        //    }

        //    public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        //    {
        //        log.WarnFormat(provider, format, args);
        //    }

        //    public void WarnFormat(string format, object arg0, object arg1, object arg2)
        //    {
        //        log.WarnFormat(format, arg0, arg1, arg2);
        //    }

        //    public void WarnFormat(string format, object arg0, object arg1)
        //    {
        //        log.WarnFormat(format, arg0, arg1);
        //    }

        //    public void WarnFormat(string format, object arg0)
        //    {
        //        log.WarnFormat(format, arg0);
        //    }

        //    public void WarnFormat(string format, params object[] args)
        //    {
        //        log.WarnFormat(format, args);
        //    }

        //    #endregion

        //    public log4net.Core.ILogger Logger
        //    {
        //        get { return log.Logger; }
        //    }
        //}

        #endregion

        #region EmailHelper

        /// <summary>
        /// This class provides all Email function.
        /// </summary>
        public class EmailHelper
        {
            private static SmtpClient client = null;
            public static string Host { get; set; }
            public static string UserName { get; set; }
            public static string DisplayName { get; set; }
            public static string Password { get; set; }
            public static string FromAddress { get; set; }
            public static string ToAddress { get; set; }
            public static string CCAddress { get; set; }
            public static string BCCAddress { get; set; }

            #region Constructor
            public EmailHelper()
            {
                MailSettingsElement mailSettings = Globals.MRMSettings.MailSettings;
                UserName = mailSettings.UserName;
                DisplayName = mailSettings.DisplayName;
                Password = mailSettings.Password;
                FromAddress = mailSettings.FromAddress;
                ToAddress = mailSettings.ToAddress;
                CCAddress = mailSettings.CCAddress;
                BCCAddress = mailSettings.BCCAddress;

                // create smtp client at mail server location
                client = new SmtpClient(mailSettings.Host);
                // add credentials
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(UserName, Password);
                client.Port = 587;
                client.EnableSsl = true; //Gmail works on Server Secured Layer

            }
            public EmailHelper(string userName, string displayName, string password, string fromAddress, string toAddress)
            {
                MailSettingsElement mailSettings = Globals.MRMSettings.MailSettings;
                UserName = userName;
                DisplayName = displayName;
                Password = password;
                FromAddress = fromAddress;
                ToAddress = toAddress;

                // create smtp client at mail server location
                client = new SmtpClient(mailSettings.Host);
                // add credentials
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(UserName, Password);
                client.Port = 587;
                client.EnableSsl = true; //Gmail works on Server Secured Layer
            }
            public EmailHelper(string userName, string displayName, string password, string fromAddress, string toAddress, string cCAddress, string bCCAddress)
            {
                MailSettingsElement mailSettings = Globals.MRMSettings.MailSettings;
                UserName = userName;
                DisplayName = displayName;
                Password = password;
                FromAddress = fromAddress;
                ToAddress = toAddress;
                CCAddress = cCAddress;
                BCCAddress = bCCAddress;

                // create smtp client at mail server location
                client = new SmtpClient(mailSettings.Host);
                // add credentials
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(UserName, Password);
                client.Port = 587;
                client.EnableSsl = true; //Gmail works on Server Secured Layer
            }
            #endregion

            public static EmailHelper Email { get { return new EmailHelper(); } }

            /// <summary>
            /// Transmit an email message to a recipient without
            /// any attachments
            /// </summary>
            /// <param name="subject">Subject Line Describing Message</param>
            /// <param name="body">The Email Message Body</param>
            /// <returns>Status Message as String</returns>
            public string SendMessage(string subject, string body)
            {
                try
                {
                    // validate the email address
                    bool bTest = ValidateEmailAddress(ToAddress);

                    // if the email address is bad, return message
                    if (bTest == false)
                        return "Invalid recipient email address: " + ToAddress;


                    // create the email message 
                    MailMessage message = new MailMessage(new MailAddress(FromAddress, DisplayName, System.Text.Encoding.UTF8), new MailAddress(ToAddress));
                    message.Subject = subject;
                    message.Body = getHtmlBody("User Right Management", "Copyright © 2010 Nordicsoft.com.bd. All rights reserved.", subject, body);
                    message.IsBodyHtml = true;
                    addCC(message, CCAddress);
                    addBCC(message, BCCAddress);

                    client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                    object userstate = message;
                    // send message
                    //client.SendAsync(message, userstate);
                    client.Send(message);
                    return "Message sent to " + ToAddress + " at " + DateTime.Now.ToString() + ".";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }

            private string getHtmlBody(string header, string footer, string subject, string body)
            {
                return string.Format("<table cellpadding='0' cellspacing='0' style='font-family: calibri; width: 800px; border-color:#003366'>" +
                                "<tr>" +
                                    "<td style='background-color: #003366; font-size: xx-large; font-variant: small-caps; color: #FFFFFF;'>" +
                                        "{0}</td>" + // Header
                                "</tr>" +                                
                                "<tr>" +
                                    "<td style='background-color: #336699; font-size: large; font-variant: small-caps; color: #FFFFFF;'>" +
                                        "{1}</td>" + // Title
                                "</tr>" +
                                "<tr>" +
                                    "<td style='background-color: #F4F4F4; color: #000080'>" +
                                        "&nbsp;</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='background-color: #F4F4F4; color: #000080'>" +
                                        "{2}</td>" + // Here is the body
                                "</tr>" +
                                "<tr>" +
                                    "<td style='background-color: #F4F4F4; color: #000080'>" +
                                        "&nbsp;</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style='background-color: #99BBDD; color: #000000; '>" +
                                        "{3}</td>" + // Footer
                                "</tr>" +
                            "</table>", header, subject, body, footer);
            }

            private void addCC(MailMessage message, string CCAddress)
            {
                if (CCAddress.Length > 0)
                {
                    string[] array1 = CCAddress.Split(',');
                    for (int i = 0; i < array1.Length; i++)
                    {
                        message.CC.Add(array1[i].ToString());
                    }
                }
            }

            private void addBCC(MailMessage message, string BCCAddress)
            {
                if (BCCAddress.Length > 0)
                {
                    string[] array2 = BCCAddress.Split(',');
                    for (int i = 0; i < array2.Length; i++)
                    {
                        message.Bcc.Add(array2[i].ToString());
                    }
                }
            }


            /// <summary>
            /// Transmit an email message with
            /// attachments
            /// </summary>
            /// <param name="subject">Subject Line Describing Message</param>
            /// <param name="body">The Email Message Body</param>
            /// <param name="attachments">A string array pointing to the location of each attachment</param>
            /// <returns>Status Message as String</returns>
            public string SendMessageWithAttachment(string subject, string body, ArrayList attachments)
            {
                try
                {
                    // validate email address
                    bool bTest = ValidateEmailAddress(ToAddress);

                    if (bTest == false)
                        return "Invalid recipient email address: " + ToAddress;

                    // Create the basic message
                    MailMessage message = new MailMessage(new MailAddress(FromAddress, DisplayName, System.Text.Encoding.UTF8), new MailAddress(ToAddress));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    addCC(message, CCAddress);
                    addBCC(message, BCCAddress);

                    // The attachments arraylist should point to a file location where
                    // the attachment resides - add the attachments to the message
                    foreach (string attach in attachments)
                    {
                        Attachment attached = new Attachment(attach, MediaTypeNames.Application.Octet);
                        message.Attachments.Add(attached);
                    }

                    // Add credentials
                    client.UseDefaultCredentials = true;
                    client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

                    // send message
                    client.Send(message);

                    return "Message sent to " + ToAddress + " at " + DateTime.Now.ToString() + ".";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }

            void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                MailMessage completedmessage = (MailMessage)e.UserState;
                string emailfrom = completedmessage.From.Address;
                string subject = completedmessage.Subject;
                
                if (e.Cancelled)
                {
                    //"user Send canceled.";
                }
                if (e.Error != null)
                {
                    //"Send error.";

                }
                else
                {
                    //"Send success.";
                }
                if (sender.GetType() == typeof(MailMessage))
                {
                    MailMessage message = (MailMessage)sender;
                    message.Dispose();
                }
            }


            /// <summary>
            /// Confirm that an email address is valid
            /// in format
            /// </summary>
            /// <param name="emailAddress">Full email address to validate</param>
            /// <returns>True if email address is valid</returns>
            public bool ValidateEmailAddress(string emailAddress)
            {
                try
                {
                    string TextToValidate = emailAddress;
                    Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                    // test email address with expression
                    if (expression.IsMatch(TextToValidate))
                    {
                        // is valid email address
                        return true;
                    }
                    else
                    {
                        // is not valid email address
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

        #region UtilityHelper

        /// <summary>
        /// This class provides all utility function.
        /// </summary>
        public class UtilityHelper
        {
            #region Contries Array
            /// <summary>
            /// private static array for all country.
            /// </summary>
            private static string[] _countries = new string[] { 
                                                              "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", 
                                                              "Angola", "Anguilla", "Antarctica", "Antigua And Barbuda", "Argentina", 
                                                              "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan",
                                                              "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus",
                                                              "Belgium", "Belize", "Benin", "Bermuda", "Bhutan",
                                                              "Bolivia", "Bosnia Hercegovina", "Botswana", "Bouvet Island", "Brazil",
                                                              "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Byelorussian SSR",
                                                              "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands",
                                                              "Central African Republic", "Chad", "Chile", "China", "Christmas Island",
                                                              "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands",
                                                              "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Cyprus",
                                                              "Czech Republic", "Czechoslovakia", "Denmark", "Djibouti", "Dominica",
                                                              "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador",
                                                              "England", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia",
                                                              "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France",
                                                              "Gabon", "Gambia", "Georgia", "Germany", "Ghana",
                                                              "Gibraltar", "Great Britain", "Greece", "Greenland", "Grenada",
                                                              "Guadeloupe", "Guam", "Guatemela", "Guernsey", "Guiana",
                                                              "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Islands",
                                                              "Honduras", "Hong Kong", "Hungary", "Iceland", "India",
                                                              "Indonesia", "Iran", "Iraq", "Ireland", "Isle Of Man",
                                                              "Israel", "Italy", "Jamaica", "Japan", "Jersey",
                                                              "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, South",
                                                              "Korea, North", "Kuwait", "Kyrgyzstan", "Lao People's Dem. Rep.", "Latvia",
                                                              "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein",
                                                              "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar",
                                                              "Malawi", "Malaysia", "Maldives", "Mali", "Malta",
                                                              "Mariana Islands", "Marshall Islands", "Martinique", "Mauritania", "Mauritius",
                                                              "Mayotte", "Mexico", "Micronesia", "Moldova", "Monaco",
                                                              "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar",
                                                              "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles",
                                                              "Neutral Zone", "New Caledonia", "New Zealand", "Nicaragua", "Niger",
                                                              "Nigeria", "Niue", "Norfolk Island", "Northern Ireland", "Norway",
                                                              "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea",
                                                              "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland",
                                                              "Polynesia", "Portugal", "Puerto Rico", "Qatar", "Reunion",
                                                              "Romania", "Russian Federation", "Rwanda", "Saint Helena", "Saint Kitts",
                                                              "Saint Lucia", "Saint Pierre", "Saint Vincent", "Samoa", "San Marino",
                                                              "Sao Tome and Principe", "Saudi Arabia", "Scotland", "Senegal", "Seychelles",
                                                              "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands",
                                                              "Somalia", "South Africa", "South Georgia", "Spain", "Sri Lanka",
                                                              "Sudan", "Suriname", "Svalbard", "Swaziland", "Sweden",
                                                              "Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikista", "Tanzania",
                                                              "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago",
                                                              "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu",
                                                              "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States",
                                                              "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City State", "Venezuela",
                                                              "Vietnam", "Virgin Islands", "Wales", "Western Sahara", "Yemen",
                                                              "Yugoslavia", "Zaire", "Zambia", "Zimbabwe"};

            #endregion

            /// <summary>
            /// Returns an array with all countries
            /// </summary>     
            public static StringCollection GetCountries()
            {
                StringCollection countries = new StringCollection();
                countries.AddRange(_countries);
                return countries;
            }
            public static SortedList GetCountries(bool insertEmpty)
            {
                SortedList countries = new SortedList();
                if (insertEmpty)
                    countries.Add("", "Please select one...");
                foreach (String country in _countries)
                    countries.Add(country, country);
                return countries;
            }
            /// <summary>
            /// Gets all controls information from  System.Web.UI.WebControls namespace that implements Control type.
            /// </summary>
            /// <returns></returns>
            public static List<ControlType> GetUIControlTypes()
            {
                List<ControlType> lst = new List<ControlType>();

                Type type = (new System.Web.UI.WebControls.Button()).GetType();
                Control ctl = new Control();
                Assembly asm = Assembly.GetAssembly(type);
                if (asm != null)
                {
                    var types = asm.GetTypes().Where(t => t.Namespace == "System.Web.UI.WebControls" && (t.IsSubclassOf(ctl.GetType()) || t.IsSubclassOf(typeof(DataControlField))));
                    foreach (Type t in types)
                        lst.Add(new ControlType(t.Name, t.FullName));
                }
                lst.Sort();
                return lst;
            }
            /// <summary>
            /// Gets all properties of a web control.
            /// </summary>
            /// <param name="controlFullName"></param>
            /// <returns></returns>
            public static List<string> GetUIControlTypeProperties(string controlFullName)
            {
                List<string> lst = new List<string>();

                Type virtualType = (new System.Web.UI.WebControls.Button()).GetType();
                Control ctl = new Control();
                Assembly asm = Assembly.GetAssembly(virtualType);
                if (asm != null)
                {
                    var type = asm.GetType(controlFullName);
                    if (type == null)
                        return null;

                    var properties = asm.GetType(controlFullName).GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string));
                    foreach (PropertyInfo pi in properties)
                        lst.Add(pi.Name);
                }
                lst.Sort();
                return lst;
            }

        }

        #endregion
    }

    #region ControlType
    public class ControlType : IComparable
    {
        public ControlType(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
        }
        public string Name { get; set; }
        public string FullName { get; set; }

        public int CompareTo(object obj)
        {
            ControlType ctype = obj as ControlType;
            if (ctype != null)
                return this.Name.CompareTo(ctype.Name);
            else
                throw new ArgumentException("Object is not a ControlType");

        }
    }
    #endregion
}