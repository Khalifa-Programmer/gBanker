using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class PurposeViewModel : BaseModel
    {
        [Required(ErrorMessage = "Purpose ID is required")]
        [Display(Name = "Purpose ID")]
        public int PurposeID { get; set; }

        [Required(ErrorMessage = "Purpose Code is required")]        
        [Display(Name = "Purpose Code")]
        public string PurposeCode { get; set; }

        [Required(ErrorMessage = "Purpose Name is required")]
 
        [Display(Name = "Purpose Name")]
        public string PurposeName { get; set; }

        [Required(ErrorMessage = "Main Sector is required")]
        [Display(Name = "Main Sector")]
        public string MainSector { get; set; }

        [Required(ErrorMessage = "Main Sector Name is required")]
        [Display(Name = "Main Sector Name")]
        public string MainSectorName { get; set; }

        [Required(ErrorMessage = "Sub Sector is required")]
        [Display(Name = "Sub Sector")]
        public string SubSector { get; set; }

        [Required(ErrorMessage = "Sub Sector Name is required")]
        [Display(Name = "Sub Sector Name")]
        public string SubSectorName { get; set; }
    }
}