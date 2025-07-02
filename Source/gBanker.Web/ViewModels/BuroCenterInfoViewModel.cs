using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class BuroCenterInfoViewModel
    {
        public int Sl { get; set; }
        public string BranchCode { get; set; }
        public string Centertype { get; set; }
        public string CenterID { get; set; }
        public string CenterName { get; set; }
        public DateTime? CenteropeningDate { get; set; }
        public string CenterVillage { get; set; }
        public string CenterPO { get; set; }
        public string CenterUnion { get; set; }
        public string CenterThana { get; set; }
        public string CenterDistrict { get; set; }
        public int? CenterTime { get; set; }
        public string CenterDay { get; set; }
        public string PO_APO { get; set; }
        public string BA_ABA { get; set; }
        public string BM_ABM { get; set; }
        public string CenterChief { get; set; }
        public string AssCenterChief { get; set; }
        public string CenterLocation { get; set; }
    }
}