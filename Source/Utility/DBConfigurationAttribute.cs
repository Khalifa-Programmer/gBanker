using System;
using System.Data;

namespace Utility.Attributes
{
    /// <summary>
    /// An attribute class used in entity class to map an entity class with stored procedure parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DBConfigurationAttribute : Attribute
    {
        private bool _generateUIControl = true;
        /// <summary>
        /// Primary key field.
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// If this field is specified to true, then this property will not be used as parameter.
        /// </summary>
        public bool ExcludeFromInputParam { get; set; }
        public bool ExcludeFromSelectionList { get; set; }
        public bool IncludeInDeleteParameter { get; set; }

        public bool IsIdentity { get; set; }
        public bool IsForeignKey { get; set; }

        public string ForeignKeyColumnName { get; set; }
        public string ForeignKeyTableName { get; set; }

        public bool IsRequired { get; set; }
        public string MappingColumnName { get; set; }
        public string MappingTableName { get; set; }        
        public ParameterDirection Direction { get; set; }
        public int Size { get; set; }
        /// <summary>
        /// If specified to true, this property will not be used as parameter. This object contains the child table data. This is normally used in list. 
        /// </summary>
        public bool IsChild { get; set; }
        public string DisplayableColumnName { get; set; }
        public bool GenerateUIControl { get { return this._generateUIControl; } set { this._generateUIControl = value; } }
    }
}
