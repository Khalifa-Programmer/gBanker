using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DistrictThanaMappingViewModel : BaseModel
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Id { get; set; }

        [Display(Name = "District")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Required(ErrorMessage = "{0} is Required")]
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }

        [Display(Name = "Olrs District")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Required(ErrorMessage = "{0} is Required")]
        public string OlrsDistrictCode { get; set; }

        [Display(Name = "Thana")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ThanaCode { get; set; }
        public string ThanaName { get; set; }

        [Display(Name = "Olrs Thana")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Required(ErrorMessage = "{0} is Required")]
        public string OlrsThanaCode { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> ThanaList { get; set; }
        public IEnumerable<SelectListItem> OlrsDistrictList { get; set; }
        public IEnumerable<SelectListItem> OlrsThanaList { get; set; }
    }
}