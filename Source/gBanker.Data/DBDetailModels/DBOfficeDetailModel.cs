using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBOfficeDetailModel
    {
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public byte OfficeLevel { get; set; }
        public string FirstLevel { get; set; }
        public string SecondLevel { get; set; }
        public string ThirdLevel { get; set; }
        public string FourthLevel { get; set; }
        public System.DateTime OperationStartDate { get; set; }
        public string OfficeAddress { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> GeoLocationID { get; set; }
        public string LocationName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int? UnionID { get; set; }
        public string UnionName { get; set; }
        public int? InvestorID { get; set; } //  public byte? InvestorID { get; set; }
        public string InvestorName { get; set; }
    }
}
