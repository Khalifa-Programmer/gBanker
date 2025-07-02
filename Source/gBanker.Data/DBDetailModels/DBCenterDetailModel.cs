using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBCenterDetailModel
    {
        public int CenterID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string OfficeFullName { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string CenterFullName { get; set; }
        public string CenterAddress { get; set; }
        public string CenterNameBng { get; set; }
        public short EmployeeId { get; set; }
        public string Organizer { get; set; }
        public string EmployeeFullName { get; set; }
        public string CollectionDay { get; set; }
        public byte CenterTypeID { get; set; }
        public System.DateTime CollectionDate { get; set; }
        public Nullable<int> GeoLocationID { get; set; }
        public string LocationName { get; set; }
        public string CenterDistance { get; set; }
        public System.DateTime OperationStartDate { get; set; }
        public byte CenterStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }


        public long? CenterChief { get; set; }
        public long? AssoCenterChief { get; set; }
        public long? PanelMember { get; set; }

        public string CenterChiefName { get; set; }
        public string AssoCenterChiefName { get; set; }
        public string PanelMemberName { get; set; }

        public DateTime? CenterTime { get; set; }
        public string CenterTimeOnly
        {
            get
            {
                if (CenterTime == null)
                    return "";

                return ((DateTime)CenterTime).ToString("HH:mm");
            }
        }

        public string CenterCategoryName { get; set; } //_25
    }
}
