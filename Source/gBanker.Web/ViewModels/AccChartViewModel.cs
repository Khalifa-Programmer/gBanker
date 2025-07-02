using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AccChartViewModel:BaseModel
    {
        public int Sl { get; set; }
        public int AccID { get; set; }

        [Display(Name = "New Code")]
        public string AccCode { get; set; }

        [Display(Name = "Parent Code")]
        public int ParentCode { get; set; }
        public int ParentID { get; set; }

        [Display(Name = "Account Head")]
        public string AccName { get; set; }

        [Display(Name = "Level")]
        public int AccLevel { get; set; }
        public string FirstLevel { get; set; }
        public string SecondLevel { get; set; }
        public string ThirdLevel { get; set; }
        public string FourthLevel { get; set; }
        public string FifthLevel { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Office Level")]
        public int? OfficeLevel { get; set; }

        [Display(Name = "Is Transaction")]
        public bool IsTransaction { get; set; }
        public string Nature { get; set; }

        [Display(Name = "Module")]
        public int ModuleID { get; set; }

        [Display(Name = "Note")]
        public int NoteID { get; set; }
        public string NoteName { get; set; }
        public int SlNo { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> ModuleList { get; set; }
        public IEnumerable<SelectListItem> AccNoteList { get; set; }

    }
}