using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class ApiSetting
    {
        public int ID { get; set; }
        public int VersionNo
        {
            get; set;
        }
        public int LivePort
        {
            get; set;
        }
        public int SandboxPort
        {
            get; set;
        }
        public string Consumer
        {
            get; set;
        }
    }
}