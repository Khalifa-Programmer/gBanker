using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoanProposalSaved : LoanTermValidationResult
    {
        public long LoanSummaryId { get; set; }
    }
}