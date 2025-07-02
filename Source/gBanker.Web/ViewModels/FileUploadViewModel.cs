using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class FileUploadViewModel : BaseModel
    {
        public int FileUploadId { get; set; }
        public string UploadType { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public bool? IsDownloadable { get; set; }
        public decimal? UploadBy { get; set; }
        public DateTime? UploadDate { get; set; }
       
        //public string UploadDateMsg { get; set; }
        //public string OfficeName { get; set; }
        //public int rowSl { get; set; }
    }
}