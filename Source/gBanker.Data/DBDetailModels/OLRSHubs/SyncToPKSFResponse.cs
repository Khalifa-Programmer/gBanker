using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs
{
    public class SyncToPKSFResponse
    {
        public string Type { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class SynchizedResonseFromPKSF
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
