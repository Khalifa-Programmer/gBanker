using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBAccChartDetailModel
    {
        public int AccID { get; set; }

        public string AccCode { get; set; }

        public string AccName { get; set; }

        public int? AccLevel { get; set; }

        public string FirstLevel { get; set; }

        public string SecondLevel { get; set; }

        public string ThirdLevel { get; set; }

        public string FourthLevel { get; set; }

        public string FifthLevel { get; set; }

        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? OfficeLevel { get; set; }
        public bool? IsTransaction { get; set; }
        public bool? IsActive { get; set; }        
        public string Nature { get; set; }
        public int? ModuleID { get; set; }
        public int? NoteID { get; set; }
        public string NoteName { get; set; }
        
    }
}
