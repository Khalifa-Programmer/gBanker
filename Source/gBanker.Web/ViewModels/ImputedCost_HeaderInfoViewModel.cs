using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ImputedCost_HeaderInfoViewModel : BaseModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "MNYR")]
        public string MNYR { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Market Rate")]
        public decimal MarketRate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Inflation Rate")]
        public decimal InflationRate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "FY Month")]
        public string FY_Month { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Imputed Cost non-pksf (fund + savings)")]
        public decimal imp_fs_npk { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Imputed Cost pksf (fund + savings)")]
        public decimal imp_fs_pk { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Imputed Cost non-pksf (fund + savings + inflation adjustment)")]
        public decimal imp_fsi_npk { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Imputed Cost pksf (fund + savings + inflation adjustment)")]
        public decimal imp_fsi_pk { get; set; }
        public string SYNCED_STATUS { get; set; }
        

        public IEnumerable<SelectListItem> MonthList { get; set; }
    }

    public class ImputedCost_INFEquityInfoViewModel : BaseModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Month/Year(mm/yyyy)")]
        public string MNYR { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "NPK Last Year")]
        public decimal NPK_LAST_YR { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "PK Last Year")]
        public decimal PK_LAST_YR { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "NPK This Month")]
        public decimal NPK_THIS_MONTH { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "PK This Month")]
        public decimal PK_THIS_MONTH { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "NPK Avg")]
        public decimal NPK_AVG { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "PK Avg")]
        public decimal PK_AVG { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "NPK Inf Adj")]
        public decimal NPK_INF_ADJ { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "PK Inf Adj")]
        public decimal PK_INF_ADJ { get; set; }

        [Display(Name = "SYNCED STATUS")]
        public string SYNCED_STATUS { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }
    }
}