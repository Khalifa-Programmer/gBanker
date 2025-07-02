using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AccNoteViewModel:BaseModel
    {
        public int NoteID { get; set; }

        [Display(Name = "Note No")]
        public int NoteNo { get; set; }

        [Display(Name = "Note Name")]
        public string NoteName { get; set; }
        public int SlNo { get; set; }

    }
}