
using gBanker.Web.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace gBanker.Web.ViewModels
{
    public class MemberCategoryViewModel : BaseModel
    {
        [Required(ErrorMessage = "Category ID is required")]
        //[GlobalizedDisplayName("CategoryID")]
        [Display(Name = "Category ID")]
        public byte MemberCategoryID { get; set; }

        [Required(ErrorMessage = "Category Code is required")]
        //[GlobalizedDisplayName("CategoryCode")]
        [Display(Name = "Category Code")]
        public string MemberCategoryCode { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100)]
        //[GlobalizedDisplayName("MemberCategoryName")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Category Short Name is required")]
        [Display(Name = "Short Name")]
        public string CategoryShortName { get; set; }

        [Display(Name = "Category Name(Unicode)")]
        public string CategoryNameBng { get; set; }

        [Display(Name = "Savings Code")]
        public string CategoryShortNameBng { get; set; }
        [NotMapped]
        public int ProductCategoryID { get; set; }
        public bool IsActive { get; set; }

        public System.DateTime? InActiveDate { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Admission Fee")]
        public decimal? AdmissionFee { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Pass Book Fee")]
        public decimal? PassBookFee { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Loan Form Fee")]
        public decimal? LoanFormFee { get; set; }        
    }
}