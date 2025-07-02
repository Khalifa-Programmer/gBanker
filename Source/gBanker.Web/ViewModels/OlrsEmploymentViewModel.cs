using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class OlrsEmploymentViewModel : BaseModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Loan Date")]
        public string MNYR { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Loan Code")]
        public string LOAN_CODE { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Self Full (Male)")]
        public int SELF_FULL_M { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Self Full (Female)")]
        public int SELF_FULL_F { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Self Part (Male)")]
        public int SELF_PART_M { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Self Part (Female)")]
        public int SELF_PART_F { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Wage Full (Male)")]
        public decimal WAGE_FULL_M { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Wage Full (Female)")]
        public decimal WAGE_FULL_F { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Wage Part (Male)")]
        public decimal WAGE_PART_M { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Wage Part (Female)")]
        public decimal WAGE_PART_F { get; set; }
    }
}