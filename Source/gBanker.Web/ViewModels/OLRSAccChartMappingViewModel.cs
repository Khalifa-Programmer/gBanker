using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{ 
    public class AccChartXOlrsAccCodeMappingViewModel
    {
        public IEnumerable<OLRSAccChartMapping> OLRSAccChartMappings { get; set; }
        public IEnumerable<AccChart> AccCharts { get; set; }
        public IEnumerable<PO_A_ACC_HEADModel> POAccCharts { get; set; }

    }
    public class OLRSAccChartMappingViewModel
    {
        public int Id { get; set; }

        [Display(Name = "OLRS ACC Chart")]
        [Required(ErrorMessage = "{0} is Required")]
        public string AccCodeOLRS { get; set; }       

        [Display(Name ="Level")]
        [Required( ErrorMessage ="{0} is Required")]
        public string AccChartLevel { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string AccChartCode { get; set; }

        [Display(Name = "Acc Chart")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ConfigAccChartCode { get; set; }        

        public IEnumerable<SelectListItem> AccCodeOLRSList { get; set; }
    }    
}