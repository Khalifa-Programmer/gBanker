using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("FileUpload")]
    public partial class FileUpload
    {
        public int FileUploadId { get; set; }

        [StringLength(50)]
        public string UploadType { get; set; }

        [StringLength(50)]
        public string FileType { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        [StringLength(250)]
        public string FileLocation { get; set; }

        public bool? IsDownloadable { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UploadBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UploadDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}
