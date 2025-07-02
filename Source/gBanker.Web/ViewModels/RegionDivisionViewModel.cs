using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class RegionDivisionViewModel : BaseModel
    {
        public int RDID { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public int ParentID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PostCode { get; set; }
        public string RDAddress { get; set; }

    }
    public class RegionXMappingViewModel
    {
        public int RDID { get; set; }
        public string RDName { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }

    }
    public class RDMappingCreateViewModel
    {
        public List<RDOfficeViewModel> rdView { get; set; }
        public int RDID { get; set; }
    }
    public class RDOfficeViewModel
    {
        public int RDID { get; set; }
        public int OfficeID { get; set; }
    }
}