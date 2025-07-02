using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ImputedCostSavingInterestViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Date")]
        public string MNYR { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Interest Rate")]
        public decimal INT_RATE { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Non PKSF Fund")]
        public decimal NPK { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Regular Fund")]
        public decimal Regular { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Voluntary Fund")]
        public decimal VOL { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Other Fund")]
        public decimal Other { get; set; }
        public string SYNCED_STATUS { get; set; }

    }
}