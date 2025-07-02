using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class OrganizationViewModel : BaseModel
    {
        [Key]
        public int OrgID { get; set; }

        [StringLength(20)]
        public string OrganizationCode { get; set; }

        [StringLength(50)]
        public string OrganizationName { get; set; }

       

        [StringLength(150)]
        public string OrgAddress { get; set; }

        public byte[] OrgLOGO { get; set; }
       
        [Display(Name = "Orgnazation Image")]
        public HttpPostedFileBase ImgFile { get; set; }
        public string OrgLogoImage64String { get; set; }
    }
}