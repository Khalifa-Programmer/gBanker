using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.Entity;
using gBanker.Web.RestApi.Models.RequestModels;
using gBanker.Web.RestApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gBanker.Web.RestApi
{
    [System.Web.Mvc.RoutePrefix("/api/loan-proposal")]
    public class LoanProposalController : ApiController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/validate-term")]
        public HttpResponseMessage validateTerm(LoanTermValidationRequest requestModel)
        {
            LoanTermValidationResponse response = new LoanTermValidationResponse();
           
            using (var db = new gBankerDbContext())
                {
                try
                {
                    if (requestModel.userId != "") {
                        string productInstallmentSql = "EXEC API_ProductInstallment " + requestModel.officeId + "," + requestModel.productId + "," + requestModel.userId;
                        var productInstallment = db.Database.SqlQuery<ProductInstallment>(productInstallmentSql).ToList();
                        response.productInstallment = productInstallment;
                    }

                    string sql = "Exec API_validate_loanProposal " + requestModel.officeId + "," + requestModel.memberId + ",'" + requestModel.mainProductCode + "'," + requestModel.productId;
                    LoanTermValidationResult ltvr = db.Database.SqlQuery<LoanTermValidationResult>(sql).FirstOrDefault();

                    
                    if (ltvr.ErrorCode == 1)
                    {
                        response.status = "true";
                        
                    }
                    else
                    {
                        response.status = "false";
                    }

                    response.result = ltvr;
                }
                catch (Exception ex)
                {
                    response.status = "false" + ex.Message;
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Route("/list")]
        public HttpResponseMessage getLoanProposals(LoanProposalListRequest requestModel)
        {
            LoanProposalListResponse lplr = new LoanProposalListResponse();
            string sql = "EXEC API_get_loanProposal " + requestModel.officeId + ",'" + requestModel.userId+"'";
            try {
                using (var db = new gBankerDbContext())
                {
                    List<LoanProposal> loanProposals = db.Database.SqlQuery<Models.Entity.LoanProposal>(sql).ToList();
                    lplr.loanProposals = loanProposals;
                    lplr.status = "true";
                    
                }
            }catch(Exception ex)
            {
                lplr.status = "false";
                lplr.message = ex.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK, lplr, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/save")]
        public HttpResponseMessage saveLoanProposal(LoanProposalRequest requestModel)
        {

            string sql = "EXEC API_Insert_loanProposal "+requestModel.officeId+","
                +requestModel.memberId+",'"+requestModel.mainProductCode+"',"+requestModel.productId+","
                +requestModel.appliedAmount+","+requestModel.transType+","+requestModel.purposeId+","
                +requestModel.investorId+",'','','"+requestModel.coApplicantName+"',"+requestModel.memberPassBookNo+","
                +"'',"+requestModel.disbursementType+",'"+requestModel.securityBankName+"','"
                +requestModel.securityBranchName+"','"+requestModel.securityChequeNo+"','"+requestModel.userId+"'";

            if (requestModel.orgId == 1)
            {
                if (requestModel.proposalNo != "" && requestModel.productInstallmentMethodId > 0)
                {
                    sql += ",'" + requestModel.proposalNo + "'," + requestModel.productInstallmentMethodId;

                }
                else
                {
                    sql += ",'',0";
                }
            }

            LoanProposalSavedResponse lpsr;
            
            using(var db = new gBankerDbContext())
            {
                lpsr = new LoanProposalSavedResponse(); 
                LoanProposalSaved executed = db.Database.SqlQuery<LoanProposalSaved>(sql).FirstOrDefault();
                if (executed.LoanSummaryId > 0)
                {
                    lpsr.inserted = executed.LoanSummaryId;
                    lpsr.status = "true";
                    string guarantorSql = "EXEC API_Insert_Guarantor @memberId,@loanProposalId,@name,"
                        + "@father,@relation,@dob,@age,@address";

                    List<object> _params = new List<object>();
                    _params.Add(new SqlParameter("@memberId", requestModel.memberId));
                    _params.Add(new SqlParameter("@loanProposalId", executed.LoanSummaryId));
                    _params.Add(new SqlParameter("@name", requestModel.guarantorName));
                    _params.Add(new SqlParameter("@father", requestModel.guarantorFather));
                    _params.Add(new SqlParameter("@relation", requestModel.guarantorRelation));
                    _params.Add(new SqlParameter("@dob", requestModel.dateOfBirth));
                    _params.Add(new SqlParameter("@age", requestModel.age));
                    _params.Add(new SqlParameter("@address",requestModel.address));
                    object[] allparams = _params.ToArray();

                    db.Database.ExecuteSqlCommand(guarantorSql, allparams);



                }
                else
                {
                    lpsr.status = "false";
                    lpsr.inserted = 0;
                }
                 
            }


            return Request.CreateResponse(HttpStatusCode.OK, lpsr, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/approve")]
        public HttpResponseMessage approveLoanProposal(LoanProposalApproveRequest reqModel)
        {
            LoanProposalApproveResponse lpar = new LoanProposalApproveResponse();
            try {
                using (var db = new gBankerDbContext())
                {

                    string sql = "EXEC API_LoanApprovalEligible "+ reqModel.officeId+","+ reqModel.memberId+","+ reqModel.productId
                                + ","+ reqModel.approvedAmount+ ",'"+ reqModel.userId + "'";

                    LoanProposalApprove approved = db.Database.SqlQuery<LoanProposalApprove>(sql).FirstOrDefault();

                    if (approved != null)
                    {
                        lpar.status = "true";
                        lpar.message = "Loan Proposal Successfully Approved";
                        lpar.loanProposalApprove = approved;

                    }
                    else
                    {
                        lpar.status = "true";
                        lpar.message = "Sorry! Loan Proposal not approved";
                    }

                }

            }catch(Exception ex)
            {
                lpar.status = "false";
                lpar.message = "Sorry! try again later";
            }

            return Request.CreateResponse(HttpStatusCode.OK, lpar, Configuration.Formatters.JsonFormatter);

        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/disburse")]
        public HttpResponseMessage disburseLoanProposal(LoanProposalDisburseRequest reqModel)
        {
            LoanProposalDisburseResponse lpdr = new LoanProposalDisburseResponse();

            try
            {
                using (var db = new gBankerDbContext())
                {

                  
                    string sql = "EXEC API_LoanDisbursement_UP @OfficeID,@MemberID,@ProductId,@BankName,@ChequeNo,@ChequeIssueDate,@UserId";

                    var bankName = (reqModel.bankName != null) ? reqModel.bankName : "";
                    var chequeNo = (reqModel.chequeNo != null) ? reqModel.chequeNo : "";
                    var chequeIssueDate = (reqModel.chequeIssueDate != null) ? reqModel.chequeIssueDate : "";

                    List<object> _params = new List<object>();
                    _params.Add(new SqlParameter("@OfficeID", reqModel.officeId));
                    _params.Add(new SqlParameter("@MemberID", reqModel.memberId));
                    _params.Add(new SqlParameter("@ProductId", reqModel.productId));
                    _params.Add(new SqlParameter("@BankName", bankName));
                    _params.Add(new SqlParameter("@ChequeNo", chequeNo));
                    _params.Add(new SqlParameter("@ChequeIssueDate", chequeIssueDate));
                    _params.Add(new SqlParameter("@UserId", reqModel.userId));
                    
                    object[] allparams = _params.ToArray();

                    int disbursed =  db.Database.ExecuteSqlCommand(sql, allparams);

                    lpdr.status = "true";
                    lpdr.message = "Disbursement Successfully Done";
                }
            }catch(Exception ex)
            {
                lpdr.status = "false";
                lpdr.message = "Sorry! Disbursement Failed "+ex.Message;

            }
            return Request.CreateResponse(HttpStatusCode.OK, lpdr, Configuration.Formatters.JsonFormatter);
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/delete")]
        public HttpResponseMessage deleteLoanProposal(LoanProposalDeleteRequest reqModel)
        {
            string sql = "EXEC API_LoanProposal_DEL "+reqModel.officeId+","+reqModel.memberId+","+reqModel.productId+","+
                reqModel.loanSummaryId+",'"+reqModel.userId+"'";

            LoanProposalDeleteResponse lpdr = new LoanProposalDeleteResponse();


            try
            {
                using(var db = new gBankerDbContext())
                {
                    LoanProposalDelete loanSummaryDelete = db.Database.SqlQuery<LoanProposalDelete>(sql).FirstOrDefault();
                    lpdr.status = "true";
                    lpdr.lpd = loanSummaryDelete;
                }
            }catch(Exception ex)
            {
                lpdr.status = "false";
                lpdr.message = ex.Message;
            }


            return Request.CreateResponse(HttpStatusCode.OK, lpdr, Configuration.Formatters.JsonFormatter);
        }
    }
}