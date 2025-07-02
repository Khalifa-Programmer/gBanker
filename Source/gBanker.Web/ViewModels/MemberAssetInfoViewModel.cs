using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberAssetInfoViewModel : BaseModel
    {
        public long MemberAssetID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }
        
        public decimal? Cow { get; set; }
        
        public decimal? Goat { get; set; }
        
        public decimal? Sheep { get; set; }
        
        public decimal? Duck { get; set; }
        
        public decimal? Hen { get; set; }
        
        public string Others { get; set; }
        
        public decimal? Bed { get; set; }
        
        public decimal? Chair { get; set; }
        
        public decimal? AssetTable { get; set; }
        
        public decimal? Cycle { get; set; }
        
        public decimal? Radio { get; set; }
        
        public decimal? Ornament { get; set; }

        public string OtherAsset { get; set; }
        

    }
}