using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class StatisticsReportDetailsViewModel : BaseModel
    {
        public long StatisticsReportDetailsID { get; set; }

        public int? StatisticsReportId { get; set; }

        //public long? StatisticsDescriptionID { get; set; }
        

        [Column(TypeName = "numeric")]
        public decimal? AmountM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountF { get; set; }

        public DateTime? StatisticsReportDetailsDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [StringLength(50)]
        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }



        
        public string StatisticsReportName { get; set; }
        public IEnumerable<SelectListItem> StatisticsReportList { get; set; }



        //public string StatisticsDescriptionName { get; set; }
        public string StatisticsReportDetailsDateStr { get; set; }
        public IEnumerable<SelectListItem> StatisticsDescriptionList { get; set; }

        public int RowSl { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? OfficeID { get; set; }
        public int? ColumnShow { get; set; }


        public int? ReportType { get; set; }
        public int? ItemSubID { get; set; }
        public string ItemHeadName { get; set; }
        public string statisticsDesprition { get; set; }
        public long? statisticsDespritionID { get; set; }
        



    }
}