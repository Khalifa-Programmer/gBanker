using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Xml.Linq;
using Utility.Exceptions;


namespace BasicDataAccess
{
    public static partial class DataWrapper
    {
        public static TTarget Map<TTarget>(IDataReader reader) where TTarget : new()
        {
            IList<TTarget> list = MapReader<TTarget>(reader);
            if (list.Count > 0)
            {
                return list[0];
            }
            return new TTarget();
        }

        public static List<TTarget> MapReader<TTarget>(IDataReader reader) where TTarget : new()
        {
            List<TTarget> list = new List<TTarget>();
            if (ValidateMappings<TTarget>(reader))
            {
                while (reader.Read())
                {
                    TTarget obj = new TTarget();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i) != DBNull.Value)
                        {
                           
                            SetPropertyValue(obj, reader.GetName(i), reader.GetValue(i));
                        }
                        else if (reader.GetValue(i) == DBNull.Value)
                        {
                            SetDefaultPropertyValue(obj, reader, reader.GetName(i));
                        }
                    }
                    list.Add(obj);
                }
            }
            return list;
        }

        private static bool ValidateMappings<TTarget>(IDataRecord reader)
        {
            List<PropertyInfo> props = new List<PropertyInfo>(GetSourceProperties(typeof(TTarget)));
            for (int i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo propinfo = props.Find(pi => pi.Name.ToLower() == reader.GetName(i).ToLower());
                if (propinfo != null) continue;
                string err = string.Format("Property '{0}' of type '{1}' is missing from the type '{2}'",
                                           reader.GetName(i), reader.GetFieldType(i), typeof(TTarget).FullName);
                return false;
            }
            return true;
        }

        public static void SetDefaultPropertyValue(object target, IDataReader reader, string propertyName)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName);
            Type pType = GetPropertyType(propertyInfo.PropertyType);

            if (pType == typeof(int) || pType == typeof(Int16) || pType == typeof(Int32) || pType == typeof(Int64))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<int>(reader, propertyName));
            }
            else if (pType == typeof(string))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<string>(reader, propertyName));
            }
            else if (pType == typeof(DateTime))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<DateTime>(reader, propertyName));
            }
            else if (pType == typeof(decimal))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<decimal>(reader, propertyName));
            }
            else if (pType == typeof(long))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<long>(reader, propertyName));
            }
            else if (pType == typeof(bool))
            {
                SetPropertyValue(target, propertyName, Extensions.DataAccessExtension.GetValueOrDefault<bool>(reader, propertyName));
            }
        }
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName);
            if (value == null)
                propertyInfo.SetValue(target, value, null);
            else
            {
                Type pType = GetPropertyType(propertyInfo.PropertyType);
                Type vType = GetPropertyType(value.GetType());
                if (pType.Equals(vType))
                {
                    // types match, just copy value
                    propertyInfo.SetValue(target, value, null);
                }
                else
                {
                    // types don't match, try to coerce
                    if (pType.Equals(typeof(Guid)))
                        propertyInfo.SetValue(target, new Guid(value.ToString()), null);
                    else if (pType.IsEnum && vType.Equals(typeof(string)))
                        propertyInfo.SetValue(target, Enum.Parse(pType, value.ToString()), null);
                    else
                        propertyInfo.SetValue(target, Convert.ChangeType(value, pType), null);
                }
            }
        }

        public static PropertyInfo[] GetSourceProperties(Type sourceType)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(sourceType);
            foreach (PropertyDescriptor item in props)
                if (item.IsBrowsable)
                    result.Add(sourceType.GetProperty(item.Name));
            return result.ToArray();
        }
        /// <summary>
        /// Gets a type 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private static Type GetPropertyType(Type propertyType)
        {
            Type type = propertyType;
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type);
            return type;
        }
    }
}