using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.Helpers
{
    public class ResourceHelper
    {
        private string messageResourceKey = string.Empty; 
        public static string GetMessgeString(string messageResourceKey)
        {
            string name = Messages.ResourceManager.GetString(messageResourceKey);
            return name;
        }
        public static string GetDefaultSaveMessage
        {
            get { return GetMessgeString("DefaultSave"); }
        }
    }
}