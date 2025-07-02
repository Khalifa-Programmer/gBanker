using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gBanker.Data.DBDetailModels
{
    public class DBholidayDetailModel
    {
        public int HolidayID { get; set; }
        public DateTime BusinessDate { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public int CenterID { get; set; }
        public string CenterName { get; set; }
        public string Description { get; set; }
        public string HolidayType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        //public string CreateUser { get; set; }
        //public DateTime CreateDate { get; set; }
    }
}
