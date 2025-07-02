using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetClientInfo")]
    public class AssetClientInfo
    {
        [Key]
        public long AssetClientInfoID { get; set; }
        public string AssetClientCode { get; set; }
        public string AssetClientName { get; set; }
        public string ClientType { get; set; }
        public string AssetClientAddress { get; set; }
        public string BusLicNo { get; set; }
        public string VATRegistrationNo { get; set; }
        public string CorporateStatus { get; set; }
        public string BusinessExperience { get; set; }
        public string TIN { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public int? OrgID { get; set; }
        //public int? OfficeID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
