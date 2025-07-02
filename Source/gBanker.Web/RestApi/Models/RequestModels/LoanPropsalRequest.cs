using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanProposalRequest
    {
        public int officeId { get; set; }
        public int centerId { get; set; }
        public long memberId { get; set; }
        public string frequency { get; set; }
        public string mainProductCode { get; set; }
        public string subMainProductCode { get; set; }
        public int productId { get; set; }
        public int investorId { get; set; }
        public int loanTerm { get; set; }
        public int purposeId { get; set; }
        public int disbursementType { get; set; }
        public int appliedAmount { get; set; }
        public int duration { get; set; }
        public string coApplicantName { get; set; }
        public int memberPassBookNo { get; set; }
        public int loanInstallment { get; set; }
        public int scInstallment { get; set; }
        public int transType { get; set; }
        public string guarantorName { get; set; }
        public string guarantorFather { get; set; }
        public string guarantorRelation { get; set; }
        public string dateOfBirth { get; set; }
        public string age { get; set; }
        public string address { get; set; }
        public string securityBankName { get; set; }
        public string securityBranchName { get; set; }
        public string securityChequeNo { get; set; }
        public string userId { get; set; }
        public string proposalNo { get; set; }
        public int productInstallmentMethodId { get; set; }
        public int orgId { get; set; }
    }
}