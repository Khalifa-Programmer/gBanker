using System.Configuration;
using System.Web.Configuration;

namespace Utility
{
    public partial class Section : ConfigurationSection
    {
        [ConfigurationProperty("defaultConnectionStringName", DefaultValue = "LocalSqlServer")]
        public string DefaultConnectionStringName
        {
            get { return (string)base["defaultConnectionStringName"]; }
            set { base["defaultConnectionStringName"] = value; }
        }

        [ConfigurationProperty("defaultCacheDuration", DefaultValue = "600")]
        public int DefaultCacheDuration
        {
            get { return (int)base["defaultCacheDuration"]; }
            set { base["defaultCacheDuration"] = value; }
        }

        [ConfigurationProperty("website", IsRequired = true)]
        public WebsiteElement WebsiteConfiguration
        {
            get { return (WebsiteElement)base["website"]; }
        }
        [ConfigurationProperty("settings", IsRequired = false)]
        public SpecificSettingsElement SpecificSettings
        {
            get { return (SpecificSettingsElement)base["settings"]; }
        }
        [ConfigurationProperty("mailSettings", IsRequired = false)]
        public MailSettingsElement MailSettings
        {
            get { return (MailSettingsElement)base["mailSettings"]; }
        }
        [ConfigurationProperty("logSettings", IsRequired = false)]
        public LogSettingsElement LogSettings
        {
            get { return (LogSettingsElement)base["logSettings"]; }
        }
    }

    public class SpecificSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("encryption-decryption-key", IsRequired = false)]
        public EncryptionDecryptionElement EncryptionDecryptionKey
        {
            get { return (EncryptionDecryptionElement)base["encryption-decryption-key"]; }
        }

        [ConfigurationProperty("smtp-config-preparation", IsRequired = false)]
        public MailSettingsElement MailSettings
        {
            get { return (MailSettingsElement)base["smtp-config-preparation"]; }
        }
    }

    public class EncryptionDecryptionElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }
    }

    public class WebsiteElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                                                                                             Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        public string ProviderName
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.ConnectionStringName) ?
                                                                                             Globals.Settings.DefaultConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ProviderName;
            }
        }

        [ConfigurationProperty("pageSize", DefaultValue = "10")]
        public int PageSize
        {
            get { return (int)base["pageSize"]; }
            set { base["pageSize"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }

        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : Globals.Settings.DefaultCacheDuration);
            }
            set { base["cacheDuration"] = value; }
        }
    }

    public class MailSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("host")]
        public string Host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("username")]
        public string UserName
        {
            get { return (string)base["username"]; }
            set { base["username"] = value; }
        }

        [ConfigurationProperty("displayName")]
        public string DisplayName
        {
            get { return (string)base["displayName"]; }
            set { base["displayName"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get {  return (string)base["password"]; }
            set { base["password"] = value; }
        }

        [ConfigurationProperty("from")]
        public string FromAddress
        {
            get { return (string)base["from"]; }
            set { base["from"] = value; }
        }
        [ConfigurationProperty("to")]
        public string ToAddress
        {
            get { return (string)base["to"]; }
            set { base["to"] = value; }
        }
        [ConfigurationProperty("cc")]
        public string CCAddress
        {
            get { return (string)base["cc"]; }
            set { base["cc"] = value; }
        }
        [ConfigurationProperty("bcc")]
        public string BCCAddress
        {
            get { return (string)base["bcc"]; }
            set { base["bcc"] = value; }
        }
    }

    public class LogSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("isLogFileEnable")]
        public bool IsLogFileEnable
        {
            get { return (bool)base["isLogFileEnable"]; }
            set { base["isLogFileEnable"] = value; }
        }

        [ConfigurationProperty("isEmailEnable")]
        public bool IsEmailEnable
        {
            get { return (bool)base["isEmailEnable"]; }
            set { base["isEmailEnable"] = value; }
        }
    }
}