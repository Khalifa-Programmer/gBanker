using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Web.ViewModels
{
    [Table("att.View_TimeKeepingDetail")]
    public class FeedBackViewModel
    {
        [Key]
        public long rowSl { get; set; }
        public string StatusType { get; set; }
        public int StatusId { get; set; }
        public string NecessityType { get; set; }
        public int NecessityId { get; set; }
        public int ProblemTypeId { get; set; }
        public int ProblemId { get; set; }
        public int CorrectionStatusId { get; set; }
        public string ProblemType { get; set; }
        public string User { get; set; }
        public string Comment { get; set; }
        public Int64 NoteId { get; set; }
        public string Date { get; set; }
        public Int64 CorrectionId { get; set; }
        public string Zone { get; set; }
        public string Branch { get; set; }
        public string Status { get; set; }
        public string Necessity { get; set; }    
        public string Problem { get; set; }
        public string ProblemDetail { get; set; }
        public string SolvedDetail { get; set; }
        public string SolvedBy { get; set; }
        public string ProblemDate { get; set; }
        public string EntryDate { get; set; }
        public string OfficeName { get; set; }
        public string SolvedDate { get; set; }
        public int OfficeId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CreateUser { get; set; }

    }//END Class
}// END Namespace