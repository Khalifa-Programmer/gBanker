using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;

namespace gBanker.Web.Controllers
{

    public class LoanCollectionController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ILoanSummaryService loansummaryService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISmsLogService smsLogService;
        private readonly ISmsConfigService smsConfigService;
        public LoanCollectionController(ILoanCollectionService loanCollectionService, ICenterService centerService, IMemberService memberService, IOfficeService officeService, IProductService productService, ILoanCollectionReportService loanCollectionReportService, IUltimateReportService ultimateReportService, ISmsLogService smsLogService, ISmsConfigService smsConfigService)
        {
            this.loanCollectionService = loanCollectionService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.productService = productService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
            this.smsLogService = smsLogService;
            this.smsConfigService = smsConfigService;
        }
        #endregion

        #region Methods


        //[HttpPost]
        //public ActionResult SaveLoanTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        //{

        //    try
        //    {
        //        loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

        //        if (!IsDayInitiated)
        //        {
        //            return Json(new { Result = "ERROR", Message = "Please run the start work process" });
        //        }
        //        var trx = allTrx;

        //        var trxId = 1;
        //        var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

        //        var loanTrxViewCollection = new List<DailyLoanTrx>();
        //        foreach (var id in loanTrxIds)
        //        {
        //            var totalId = "Total" + id;

        //            var intPaidId = "IntPaid" + id;

        //            var loanDueId = "LoanDue" + id;

        //            var loanPaidId = "LoanPaid" + id;

        //            var loanTernId = "LoanTerm" + id;

        //            var intDueId = "IntDue" + id;

        //            var princialloanId = "PrincipalLoan" + id;

        //            var loanrepaidId = "LoanRepaid" + id;

        //            decimal total = 0;
        //            decimal loanPaid = 0;
        //            decimal intPaid = 0;
        //            decimal loanDue = 0;
        //            decimal intDue = 0;
        //            decimal princialloan = 0;
        //            decimal loanrepaid = 0;
        //            if (allTrx.ContainsKey(totalId))
        //                decimal.TryParse(allTrx[totalId], out total);
        //            if (allTrx.ContainsKey(loanPaidId))
        //                decimal.TryParse(allTrx[loanPaidId], out loanPaid);
        //            if (allTrx.ContainsKey(intPaidId))
        //                decimal.TryParse(allTrx[intPaidId], out intPaid);
        //            if (allTrx.ContainsKey(loanDueId))
        //                decimal.TryParse(allTrx[loanDueId], out loanDue);
        //            if (allTrx.ContainsKey(intDueId))
        //                decimal.TryParse(allTrx[intDueId], out intDue);
        //            if (allTrx.ContainsKey(princialloanId))
        //                decimal.TryParse(allTrx[princialloanId], out princialloan);
        //            if (allTrx.ContainsKey(loanrepaidId))
        //                decimal.TryParse(allTrx[loanrepaidId], out loanrepaid);
        //            //if (loanPaid > (princialloan - loanrepaid))
        //            //{
        //            //    return Json(new { Result = "ERROR", Message = "Balance Loan Amount Cann't be greter than Principal Loan" });
        //            //}
        //            var loanTrx = new DailyLoanTrx() { DailyLoanTrxID = long.Parse(id), TotalPaid = total, LoanDue = loanDue, IntPaid = intPaid, LoanPaid = loanPaid, IntDue = intDue,LoanRepaid=loanrepaid };
        //            loanTrxViewCollection.Add(loanTrx);

        //        }

        //        loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
        //        //var center= loanTrxViewCollection.
        //        //if (LoggedInOrganizationID==99)
        //        //{
        //        //    var CheckDupli = new { @OfficeId = LoginUserOfficeID, @CenterID = 1 };
        //        //    var result = ultimateReportService.UpdateMemberProshikha(CheckDupli);
        //        //}
        //        return GetSuccessMessageResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrorMessageResult(ex);
        //    }
        //}



        [HttpPost]
        public ActionResult SaveLoanTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {

            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

                if (!IsDayInitiated)
                {
                    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
                }
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var loanTrxViewCollection = new List<DailyLoanTrx>();
                foreach (var id in loanTrxIds)
                {
                    var totalId = "Total" + id;

                    var intPaidId = "IntPaid" + id;

                    var loanDueId = "LoanDue" + id;

                    var loanPaidId = "LoanPaid" + id;

                    var loanTernId = "LoanTerm" + id;

                    var intDueId = "IntDue" + id;

                    var princialloanId = "PrincipalLoan" + id;

                    var loanrepaidId = "LoanRepaid" + id;

                    var CumIntDueId = "CumIntDue" + id;

                    decimal total = 0;
                    decimal loanPaid = 0;
                    decimal intPaid = 0;
                    decimal loanDue = 0;
                    decimal intDue = 0;
                    decimal princialloan = 0;
                    decimal loanrepaid = 0;
                    decimal CumIntDue = 0;

                    if (allTrx.ContainsKey(totalId))
                        decimal.TryParse(allTrx[totalId], out total);
                    if (allTrx.ContainsKey(loanPaidId))
                        decimal.TryParse(allTrx[loanPaidId], out loanPaid);
                    if (allTrx.ContainsKey(intPaidId))
                        decimal.TryParse(allTrx[intPaidId], out intPaid);
                    if (allTrx.ContainsKey(loanDueId))
                        decimal.TryParse(allTrx[loanDueId], out loanDue);
                    if (allTrx.ContainsKey(intDueId))
                        decimal.TryParse(allTrx[intDueId], out intDue);
                    if (allTrx.ContainsKey(princialloanId))
                        decimal.TryParse(allTrx[princialloanId], out princialloan);
                    if (allTrx.ContainsKey(loanrepaidId))
                        decimal.TryParse(allTrx[loanrepaidId], out loanrepaid);
                    if (allTrx.ContainsKey(CumIntDueId))
                        decimal.TryParse(allTrx[CumIntDueId], out  CumIntDue);

                    if (loanPaid == 0)
                    {
                        CumIntDue = 0;
                    }
                    //if (loanPaid > (princialloan - loanrepaid))
                    //{
                    //    return Json(new { Result = "ERROR", Message = "Balance Loan Amount Cann't be greter than Principal Loan" });
                    //}
                    var loanTrx = new DailyLoanTrx() { DailyLoanTrxID = long.Parse(id), TotalPaid = total, LoanDue = loanDue, IntPaid = intPaid, LoanPaid = loanPaid, IntDue = intDue, LoanRepaid = loanrepaid };
                    loanTrxViewCollection.Add(loanTrx);


                    //var loanTrxinfo = new DailyLoanTrx() { MemberName = memberNameBng,CreateUser= LoggedInEmployeeID.ToString() };
                    //loanCollectionService.Update(loanTrxinfo);


                    //using (var ctx = new gBankerDbContext())
                    //{
                    //    int noOfRowUpdated = ctx.Database.ExecuteSqlCommand("Update DailyLoanTrx set MemberName = '"+memberNameBng+"'" + "where DailyLoanTrxID ="+id);
                    //}

                }
                loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);


                if (LoggedInOrganizationID == 99)
                {
                    var trxIds = 1;
                    var loanTrxIdlists = allLoanTrxId.Where(w => int.TryParse(w, out trxIds));
                    var off = LoginUserOfficeID;
                    foreach (var id in loanTrxIdlists)
                    {
                        var Memberid = "MemberID" + id;
                        long memid = 0;
                        if (allTrx.ContainsKey(Memberid))
                            long.TryParse(allTrx[Memberid], out memid);
                        long CenterID = memberService.GetByMemberId(memid).CenterID;

                        var updateMemberName = new { @OfficeID= off, @CenterID = CenterID };
                        var result = ultimateReportService.UpdateMemberProshikhaMemberName(updateMemberName);
                        if (CenterID != 0)
                            break;
                    }
                    //var Memberid = "MemberID" + id;
                    //long memid = 0;
                    //if (allTrx.ContainsKey(Memberid))
                    //    long.TryParse(allTrx[Memberid], out memid);
                    //var CenterID = "CenterID" + id;
                    //long CenID = 0;
                    //if (allTrx.ContainsKey(CenterID))
                    //    long.TryParse(allTrx[CenterID], out CenID);
                    //var memberNameBng = memberService.GetByMemberId(memid).FirstName;

                    //var updateMemberName = new { @memberNameBng = memberNameBng, @id = id };
                    //var result = ultimateReportService.UpdateMemberProshikhaMemberName(updateMemberName);
                
                }





                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }




        #region Previous

        /*
       private string TwilioSms(string msgBody, string receiver, string memId, string collDt)
        {
            var result = "";
            try
            {
                var sms_config = smsConfigService.GetByOrgID(1);
                //string AccSid = "AC4b512e190c24c98270bdf2bc6d5c53b6";
                //string AuthToken = "750eca8f7ebdf6cc2713c490ab7d08b6";
                //string PhoneNo = "+16467592441";
                int msgLength = msgBody.Length;
                string AccSid = smsConfigService.Decrypt(sms_config.AccSID);
                string AuthToken = smsConfigService.Decrypt(sms_config.AuthToken);
                //string PhoneNo = smsConfigService.Decrypt(sms_config.PhoneNo);
                string PhoneNo = "+1777742237";
                var twilio = new TwilioRestClient(AccSid, AuthToken);

                ////var message = twilio.SendMessage("+16467592441", "[To]", null, null, null);
                var messsage = twilio.SendSmsMessage(PhoneNo, receiver, msgBody);

                if (messsage.Sid != null)
                {
                    SmsLogViewModel log = new SmsLogViewModel();
                    log.OrgID = 1;
                    log.MemberID = Convert.ToInt64(memId);
                    log.SmsType = "C";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(collDt);
                    log.SmsStatus = "Sent";
                    log.IsActive = true;
                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                    smsLogService.Create(entity);
                    result = "1";
                }
                else
                {
                    SmsLogViewModel log = new SmsLogViewModel();
                    log.OrgID = 1;
                    log.MemberID = Convert.ToInt64(memId);
                    log.SmsType = "C";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(collDt);
                    log.SmsStatus = "Fail";
                    log.IsActive = true;
                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                    smsLogService.Create(entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result = string.Empty;
            }
        }
         */

        #endregion
        //private void calculate(decimal total, int Duration, decimal intdue, decimal loandue,
        //    decimal principalLoan, decimal loanRepaid, decimal DurationOverLoanDue, decimal DurationOverIntDue,
        //    int InstallmentNo, decimal CumInterestPaid, decimal CumIntCharge, string calcMethod
        //    ,int vDOC
        //    )
        //{
        //    decimal vcumInrerestCharge;
        //     decimal vcumInrerestPaid;
        //     vcumInrerestCharge = CumIntCharge;
        //    vcumInrerestPaid = CumInterestPaid;
        //    decimal vInterestBalance = (vcumInrerestCharge - vcumInrerestPaid);

        //    decimal vLoanInstallment = 0;
        //    decimal vInterestInstallment = 0;
        //    decimal vPrincipalLOan = principalLoan;
        //    decimal vloanRepaid = loanRepaid;
        //    decimal vLoan = DurationOverLoanDue;
        //    decimal vInt = DurationOverIntDue;


        //    decimal vLoanDueSCase = DurationOverLoanDue;
        //    decimal vIntDueSCase = DurationOverIntDue;
        //    decimal vTotalInstall = (vLoan + vInt);
        //    if (InstallmentNo>Duration)
        //    {

        //        if (total == 0)
        //        {
        //            vLoanInstallment = 0;
        //            vInterestInstallment = 0;
        //        }
        //        else {

        //            if (calcMethod == "D")
        //            {
        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {
        //                    vLoanInstallment = total;
        //                }
        //               if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)
        //                {
        //                    vLoanInstallment = 0;
        //                }
        //                if (vDOC == 0)
        //                {
        //                    if (vLoan > 0)
        //                    {

        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;

        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vLoanInstallment = total;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vLoanInstallment = 0;
        //                        }
        //                        if (vLoan == 0 && vInt > 0)
        //                        {
        //                            if (total >= vInterestBalance)
        //                            {
        //                                vLoanInstallment = (total - vInterestBalance);
        //                            }

        //                        }
        //                    }

        //                }


        //            }
        //           else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
        //            {
        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {
        //                    vLoanInstallment = total;
        //                }
        //               if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)
        //                { vLoanInstallment = 0; }

        //                if (vDOC == 0)
        //                {
        //                    if (vLoan > 0)
        //                    {

        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;

        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vLoanInstallment = total;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vLoanInstallment = 0;
        //                        }
        //                    }

        //                }

        //            }
        //            else if (calcMethod == "E")
        //            {

        //                if (total > vInt)
        //                {
        //                    vLoanInstallment = (total - vInt);
        //                }
        //                if (total <= vInt)
        //                {
        //                    vLoanInstallment = 0;
        //                }
        //            }
        //            else {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoanDueSCase == 0 && vIntDueSCase > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vIntDueSCase)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {

        //                    vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                }
        //                if (vDOC == 0)
        //                {
        //                    if (vTotalInstall > 0)
        //                    {

        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;

        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vLoanInstallment = total;
        //                        }
        //                        // if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoan) + parseFloat(vInt)))
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vLoanInstallment = 0;
        //                        }
        //                    }

        //                }
        //            }

        //        }
        //        // loanPaidId = vLoanInstallment;

        //        if (total == 0)
        //        {

        //            vInterestInstallment = 0;
        //        }
        //        else {
        //            if (calcMethod == "D")
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {

        //                    vInterestInstallment = 0;
        //                }
        //                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)
        //                {
        //                    vInterestInstallment = total;
        //                }

        //                if (vDOC == 0)
        //                {
        //                    if (vInt > 0)
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;
        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vInterestInstallment = 0;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vInterestInstallment = total;
        //                        }
        //                        if (vLoan == 0 && vInt > 0)
        //                        {
        //                            if (total >= vInterestBalance)
        //                            {
        //                                vInterestInstallment = (total - vLoanInstallment);
        //                            }

        //                        }
        //                    }

        //                }

        //            }
        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
        //            {


        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {

        //                    vInterestInstallment = 0;
        //                }
        //                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)
        //                {
        //                    vInterestInstallment = total;
        //                }

        //                if (vDOC == 0)
        //                {
        //                    if (vInt > 0)
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;
        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vInterestInstallment = 0;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vInterestInstallment = total;
        //                        }
        //                    }
        //                }
        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vInterestInstallment = vInt;
        //                }
        //                if (total <= vInt)
        //                {
        //                    vInterestInstallment = total;
        //                }
        //            }
        //            else {
        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if (vLoanDueSCase == 0 && vIntDueSCase > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vIntDueSCase)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {
        //                    vInterestInstallment = (vInt * total) / vTotalInstall;
        //                }
        //                if (vDOC == 0)
        //                {
        //                    if (vInt > 0)
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;
        //                    }
        //                    else {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vInterestInstallment = 0;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                        {
        //                            vInterestInstallment = total;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        decimal vLoanTotal = total;
        //        decimal vLoanBal;
        //        if (calcMethod != "A")
        //        {

        //            decimal vCheck = (vloanRepaid + vLoanInstallment);
        //            if (vCheck > vPrincipalLOan)
        //            {
        //                vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //            }

        //            vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
        //            decimal calIns = (vloanRepaid + vLoan);
        //            if (vLoan >= vLoanBal)
        //            {
        //                vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //            }

        //        }
        //        else if (calcMethod == "A" || calcMethod == "R")
        //        {

        //            vLoanBal = vPrincipalLOan - vloanRepaid;
        //            var calIns = vInterestBalance + vLoanBal;
        //           if (vLoan >= calIns)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


        //            }
        //        }


        //        var vLoanBalance = vPrincipalLOan - vloanRepaid;
        //        var vBal = vLoanBalance + vInterestBalance;
        //        if (vBal <= total)
        //        {

        //            if (vInterestBalance > 0)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);



        //            }
        //            else

        //                if (total > vLoanBalance)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //            }
        //            else {
        //                vInterestInstallment = 0;
        //                vLoanInstallment = total;
        //                }


        //        }
        //        if ((vLoanInstallment + vInterestInstallment) > total)
        //        {
        //            vLoanInstallment = total - vInterestInstallment;

        //        }


        //        if (calcMethod == "F")
        //        {
        //            if (total> vLoanInstallment + vInterestInstallment)
        //            {
        //                vLoanInstallment = 0;
        //                vInterestInstallment = 0;
        //                total = 0;

        //            }
        //        }
        //    }

        //    else
        //    {
        //        if (total == 0)
        //        {
        //            vLoanInstallment = 0;
        //            vInterestInstallment = 0;
        //        }
        //        else {

        //            if (calcMethod == "D")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {

        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }

        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }

        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {

        //                    if (total < vLoan)
        //                    {
        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                        // vLoanInstallment = 0;
        //                    }
        //                    else {
        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                    }
        //                }
        //            }

        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total > vInt)
        //                    {
        //                        vLoanInstallment = (total - vInt);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {
        //                    if (calcMethod == "A")
        //                    {
        //                        if (vInterestBalance > vInt)
        //                        {
        //                            if (total > vInterestBalance)
        //                            {
        //                                vLoanInstallment = (total - vInterestBalance);
        //                            }
        //                            if (total <= vInterestBalance)
        //                            {
        //                                vLoanInstallment = 0;
        //                            }
        //                            // vLoanInstallment = (parseFloat(total) - parseFloat(vInterestBalance))
        //                        }

        //                        else {
        //                            if (total > vInt)
        //                            {
        //                                vLoanInstallment = (total - vInt);
        //                            }
        //                            if (total <= vInt)
        //                            {
        //                                vLoanInstallment = 0;
        //                            }
        //                        }
        //                    }
        //                    else if (calcMethod == "R" || calcMethod == "V")
        //                    {
        //                        if (total > (vLoan + vInt) && total - (vLoan + vInt) > vInterestBalance - vInt)
        //                        {
        //                          if (vInterestBalance > 0)
        //                            {
        //                                vLoanInstallment = (total - vInterestBalance);

        //                            }
        //                            else {
        //                                vLoanInstallment = total;
        //                            }


        //                        }
        //                        else {
        //                            if (total < (vLoan + vInt))
        //                            {

        //                                if (total < vInt)
        //                                {
        //                                    vLoanInstallment = 0;
        //                                }
        //                                else {
        //                                    vLoanInstallment = (total - vInt);
        //                                }

        //                            }
        //                            else
        //                                vLoanInstallment = vLoan;

        //                        }


        //                    }

        //                }



        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vLoanInstallment = (total - vInt);
        //                }
        //                if (total <= vInt)
        //                {
        //                    vLoanInstallment = 0;
        //                }
        //            }
        //            else {

        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {

        //                    vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                }



        //            }
        //        }

        //        if (total == 0)
        //        {

        //            vInterestInstallment = 0;
        //        }
        //        else {
        //            if (calcMethod == "D")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {


        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {


        //                    if (total < vInt)
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;

        //                    }
        //                    else {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;
        //                    }
        //                }
        //            }

        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total > vInt)
        //                    {
        //                        vInterestInstallment = vInt;
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {

        //                    if (calcMethod == "A")
        //                    {
        //                        if (vInterestBalance > vInt)
        //                        {
        //                            if (total> vInterestBalance)
        //                            {
        //                                vInterestInstallment = vInterestBalance;
        //                            }
        //                            if (total <= vInterestBalance)
        //                            {
        //                                vInterestInstallment = total;
        //                            }
        //                            //vInterestInstallment = parseFloat(vInterestBalance)
        //                        }
        //                        else {
        //                            if (total > vInt)
        //                            {
        //                                vInterestInstallment = vInt;
        //                            }
        //                            if (total <= vInt)
        //                            {
        //                                vInterestInstallment = total;
        //                            }
        //                        }
        //                    }
        //                    else if (calcMethod == "R" || calcMethod == "V")
        //                    {
        //                        if (total > (vLoan + vInt) && total - (vLoan + vInt) > (vInterestBalance - vInt))
        //                        {

        //                            if (vInterestBalance > 0)
        //                            {
        //                                vInterestInstallment = vInterestBalance;
        //                            }
        //                            else {
        //                                vInterestInstallment = 0;
        //                            }


        //                        }
        //                        else {
        //                            if (total < (vLoan + vInt))
        //                            {
        //                                if (total < vInt)
        //                                { vInterestInstallment = total; }
        //                                else
        //                                {
        //                                    vInterestInstallment = vInt;
        //                                }

        //                            }
        //                            else {
        //                                vInterestInstallment = (total - vLoan);
        //                            }

        //                        }
        //                    }

        //                }

        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vInterestInstallment = vInt;
        //                }
        //                if (total <= vInt)
        //                {
        //                    vInterestInstallment = total;
        //                }
        //            }
        //            else {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }

        //                    if (total < vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else {

        //                    vInterestInstallment = (vInt * total) / vTotalInstall;
        //                }
        //            }
        //        }

        //        if (calcMethod != "A")
        //        {
        //            decimal vCheck = (vloanRepaid + vLoanInstallment);
        //            if (vCheck > vPrincipalLOan)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }

        //            }

        //            decimal vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
        //            decimal calIns = (vloanRepaid + vLoan);
        //            if (vLoan >= vLoanBal)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                    vLoanInstallment = (total - vInterestBalance);

        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }

        //            }
        //        }
        //        else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //        {
        //            decimal vLoan1 = total;
        //            decimal vLoanBal = vPrincipalLOan - vloanRepaid;
        //            decimal calIns = (vloanRepaid + vLoan);
        //            if (calIns >= vPrincipalLOan)
        //            {
        //                vLoanInstallment = (total - vInterestInstallment);

        //            }
        //        }

        //        decimal vLoanBalance = vPrincipalLOan - vloanRepaid;
        //        decimal vBal = (vLoanBalance + vInterestBalance);
        //        if (vBal <= total)
        //        {

        //            if (vInterestBalance > 0)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                    if ((total - vInterestBalance) >= (vPrincipalLOan - vloanRepaid))
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }


        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }


        //            }
        //            else
        //                if (total> vLoanBalance)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }
        //            }
        //            else {
        //                vInterestInstallment = 0;
        //                vLoanInstallment = total;
        //                }


        //        }

        //        if ((vLoanInstallment + vInterestInstallment) > total)
        //        {
        //            vLoanInstallment = total - vInterestInstallment;

        //        }
        //        if (calcMethod == "A" || calcMethod == "H")
        //        {


        //            decimal vLoanPayable = (total - vInterestInstallment);
        //            if (vLoanPayable > vPrincipalLOan)
        //            {
        //                decimal NeatLoanPay = total - vInterestBalance;
        //                vLoanInstallment= NeatLoanPay;
        //                decimal intPay = total - NeatLoanPay;
        //                vInterestInstallment=intPay;
        //            }
        //            else {
        //                vLoanInstallment = vLoanPayable;

        //            }

        //        }

        //        if (calcMethod == "F")
        //        {
        //            if (vInterestInstallment > vInterestBalance)
        //            {
        //               vInterestInstallment= vInterestBalance;
        //                vLoanInstallment = total - vInterestBalance;

        //            }

        //        }




        //        if (calcMethod == "F")
        //        {
        //            if (total > (vInterestBalance + vPrincipalLOan - vloanRepaid))
        //            {
        //                total = 0;
        //                vLoanInstallment = 0;
        //                vInterestInstallment = 0;


        //            }
        //        }
        //    }
        //}

        public static void CalculateSavings(double SavingInstallment, double DueSavingInstallment,

             out double vSavingsInstallment,
             int OrgID, int productid
              )
        {
            vSavingsInstallment = 0;

            if (OrgID == 54)
            {
                if (productid == 21)
                {
                    if (SavingInstallment < 20)
                    {
                        vSavingsInstallment = 0;
                    }
                }
                else
                {
                    var instMod = (SavingInstallment % DueSavingInstallment);
                    if (instMod != 0)
                    {
                        vSavingsInstallment = 0;

                    }

                }

            }
        }

        public static void Calculate(double total, int Duration, double intdue, double loandue,
        double principalLoan, double loanRepaid, double DurationOverLoanDue, double DurationOverIntDue,
        int InstallmentNo, double CumInterestPaid, double CumIntCharge, string calcMethod,
        double vLoanInstallment, double vInterestInstallment, int vDOC,
        int OrgID, Int64 summaryid, double vcumLoanDue, double vcumIntDue, double cumInterestPaid,
        double cumIntCharge, double durationOverLoanDue,
        double durationOverIntDue, int installmentNo, int duration, int doc,
        int orgId, double intDue,
        double loanDue
       )
        {

            double vCumInterestCharge;
            double vCumInterestPaid;
            vCumInterestCharge = cumIntCharge;
            vCumInterestPaid = cumInterestPaid;

            double vInterestBalance = (vCumInterestCharge - vCumInterestPaid);

            vLoanInstallment = 0;
            vInterestInstallment = 0;

            double vPrincipalLoan = principalLoan;
            double vLoanRepaid = loanRepaid;
            double vLoan = durationOverLoanDue;
            double vInt = durationOverIntDue;
            double vLoanDueSCase = durationOverLoanDue;
            double vIntDueSCase = durationOverIntDue;
            double vTotalInstall = (vLoan + vInt);

            if (installmentNo > duration)
            {

                vLoan = durationOverLoanDue;
                vInt = durationOverIntDue;

                vLoanDueSCase = durationOverLoanDue;
                vIntDueSCase = durationOverIntDue;
                vTotalInstall = vLoan + vInt;

                if (total == 0)
                {
                    vLoanInstallment = 0;
                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {
                        if ((vPrincipalLoan - vLoanRepaid) >= total)
                        {
                            vLoanInstallment = total;
                        }
                        if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                        {
                            vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                        }
                        if ((vPrincipalLoan - vLoanRepaid) == 0)
                        {
                            vLoanInstallment = 0;
                        }

                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }

                        if (doc == 0)
                        {
                            if (vLoan > 0)
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                            }
                            else
                            {
                                if ((vPrincipalLoan - vLoanRepaid) >= total)
                                {
                                    vLoanInstallment = total;
                                }
                                if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                                {
                                    vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                                }
                                if ((vPrincipalLoan - vLoanRepaid) == 0)
                                {
                                    vLoanInstallment = 0;
                                }
                            }
                        }
                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D" || calcMethod == "F" || calcMethod == "H" || calcMethod == "S")
                    {
                        if ((vPrincipalLoan - vLoanRepaid) >= total)
                        {
                            vLoanInstallment = total;
                        }
                        if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                        {
                            vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                        }
                        if ((vPrincipalLoan - vLoanRepaid) == 0)
                        {
                            vLoanInstallment = 0;
                        }

                        if (doc == 0)
                        {
                            if (vLoan > 0)
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                            }
                            else
                            {
                                if ((vPrincipalLoan - vLoanRepaid) >= total)
                                {
                                    vLoanInstallment = total;
                                }
                                if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                                {
                                    vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                                }
                                if ((vPrincipalLoan - vLoanRepaid) == 0)
                                {
                                    vLoanInstallment = 0;
                                }
                                if (vLoan == 0 && vInt > 0)
                                {
                                    if (total >= vInterestBalance)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);
                                    }
                                    if (total <= vInt)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                }
                                if (vLoan == 0 && vInt == 0)
                                {
                                    if (total > vInterestBalance)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);
                                    }
                                    if (total <= vInterestBalance)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                }

                            }
                        }

                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vLoanInstallment = (total - vInt);
                        }
                        if (total <= vInt)
                        {
                            vLoanInstallment = 0;
                        }
                    }
                    else
                    {

                        ////////// For Loan And Int equal 0 ///////////////////////
                        if (vLoanDueSCase == 0 && vIntDueSCase > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vIntDueSCase)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoan > 0 && vInt > 0)
                        { ///// for General Calculation /////
                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }
                        else
                        {
                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }

                    }
                }

                if (total == 0)
                {
                    vInterestInstallment = 0;
                }
                else
                {
                    // start from 190 line
                    if (calcMethod == "D")
                    {
                        ///////////// For Loan And Int equal 0 //////////////////
                        if ((vPrincipalLoan - vLoanRepaid) >= total)
                        {
                            vInterestInstallment = 0;
                        }
                        if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                        }
                        if ((vPrincipalLoan - vLoanRepaid) == 0)
                        {
                            vInterestInstallment = total;
                        }

                        if (vLoan == 0 && vInt > 0)
                        { // line 210
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vInterestInstallment = 0;
                            }
                        }

                        if (doc == 0)
                        {
                            if (vTotalInstall > 0)
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;
                            }
                            else
                            {
                                if ((vPrincipalLoan - vLoanRepaid) >= total)
                                {
                                    vInterestInstallment = 0;
                                }

                                if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                                {
                                    vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                                }

                                if ((vPrincipalLoan - vLoanRepaid) == 0)
                                {
                                    vInterestInstallment = total;
                                }

                            }
                        }
                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D" || calcMethod == "F" || calcMethod == "H" || calcMethod == "S")
                    {
                        if (calcMethod == "S")
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = total - vLoanInstallment;
                            }
                            else
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else
                        {

                            if ((vPrincipalLoan - vLoanRepaid) >= total)
                            {
                                vInterestInstallment = 0;
                            }

                            if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                            {
                                vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                            }
                            if ((vPrincipalLoan - vLoanRepaid) == 0)
                            {
                                vInterestInstallment = total;
                            }
                            if (doc == 0)
                            {
                                if (vTotalInstall > 0)
                                {
                                    vInterestInstallment = (vInt * total) / vTotalInstall;
                                }
                                else
                                {
                                    if ((vPrincipalLoan - vLoanRepaid) >= total)
                                    {
                                        vInterestInstallment = 0;
                                    }

                                    if ((vPrincipalLoan - vLoanRepaid) > 0 && (vPrincipalLoan - vLoanRepaid) < total)
                                    {
                                        vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                                    }

                                    if ((vPrincipalLoan - vLoanRepaid) == 0)
                                    {
                                        vInterestInstallment = total;
                                    }

                                    if (vLoan == 0 && vInt > 0)
                                    {
                                        if (total >= vInterestBalance)
                                        {
                                            vInterestInstallment = vInterestBalance;
                                        }
                                        if (total < vInterestBalance)
                                        {
                                            vInterestInstallment = total;
                                        }
                                        if (total <= vInt)
                                        {
                                            vInterestInstallment = total;
                                        }
                                    }
                                    if (vLoan == 0 && vInt == 0)
                                    {
                                        if (total > vInterestBalance)
                                        {
                                            if (vInterestBalance <= 0)
                                            {
                                                vInterestInstallment = 0;
                                            }
                                            else
                                                vInterestInstallment = vInterestBalance;
                                        }
                                        if (total <= vInterestBalance)
                                        {
                                            vInterestInstallment = total;
                                        }
                                    }
                                    if (vInterestBalance >= vInt)
                                    {
                                        if (total > vInterestBalance)
                                        {
                                            vInterestInstallment = vInterestBalance;
                                        }
                                        if (total <= vInterestBalance)
                                        {
                                            vInterestInstallment = total;
                                        }
                                    }
                                    if (vInterestBalance < vInt)
                                    {
                                        if (total <= vInterestBalance)
                                        {
                                            vInterestInstallment = total;
                                        }
                                        else
                                            vInterestInstallment = vInterestBalance;
                                    }
                                }
                            }
                        }

                    }
                    else if (calcMethod == "E")
                    {  // line 296
                        if (total > vInt)
                        {
                            vInterestInstallment = vInt;
                        }
                        if (total <= vInt)
                        {
                            vInterestInstallment = total;
                        }
                    }
                    else
                    {
                        /////////// For Loan And Int equal 0 ///////////////
                        if (vLoanDueSCase == 0 && vIntDueSCase > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vIntDueSCase)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan > 0 && vInt > 0)
                        {
                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                        ///////////////// For General Calculation ///////////////////
                        else
                        {
                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                        if (doc == 0)
                        {
                            if (vTotalInstall > 0)
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;
                            }
                        }

                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                    }
                }

                double vLoanTotal = total;
                double vLoanBal;
                if (calcMethod != "A")
                { // line 345

                    double vCheck = (vLoanRepaid + vLoanInstallment);
                    if (vCheck > vPrincipalLoan)
                    {
                        vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                        vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                    }

                    vLoanBal = (vPrincipalLoan + vInterestBalance - vLoanRepaid);
                    double calIns = (vLoanRepaid + vLoan);

                    if (vLoan >= vLoanBal)
                    {
                        vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                        vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                    }

                }
                else if (calcMethod == "A" || calcMethod == "R")
                {
                    vLoanBal = vPrincipalLoan - vLoanRepaid;
                    var calIns = vInterestBalance + vLoanBal;

                    if (vLoan >= calIns)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                        }
                    }
                }

                var vLoanBalance = vPrincipalLoan - vLoanRepaid;
                var vBal = vLoanBalance + vInterestBalance;

                if (vBal <= total)
                {

                    if (vInterestBalance > 0)
                    {

                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                        }

                    }
                    else
                    {
                        if (total > vLoanBalance)
                        {
                            if (calcMethod == "F")
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            else
                            {
                                vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                            }
                        }
                        else
                        {
                            vInterestInstallment = 0;
                            vLoanInstallment = total;
                        }
                    }
                }

                if ((vLoanInstallment + vInterestInstallment) > total)
                {
                    vLoanInstallment = total - vInterestInstallment;
                }

                if (calcMethod == "F")
                { // line 428
                    if (total > vLoanInstallment + vInterestInstallment)
                    {
                        vLoanInstallment = 0;
                        vInterestInstallment = 0;
                        total = 0;
                    }
                }

            }
            else
            {
                vLoan = loanDue;
                vInt = intDue;
                vTotalInstall = vLoan + vInt;
                if (total == 0)
                {
                    vLoanInstallment = 0;
                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {

                        ////////////// For Loan And Int equal 0 ////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total > vInt)
                            {
                                vLoanInstallment = (total - vInt);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else
                        { ////////////// For General Calculation ////////////////////////
                            if (total < vLoan)
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                            }
                            else
                            {
                                vLoanInstallment = (vLoan * total) / vTotalInstall;
                            }
                        }

                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V" || calcMethod == "S")
                    {
                        //////////// For Loan And Int equal 0 //////////////////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (calcMethod == "S")
                            {
                                if (total > vInterestBalance)
                                {
                                    vLoanInstallment = total - vInterestBalance;
                                }
                                else
                                {
                                    vLoanInstallment = 0;
                                }
                            }
                            else
                            {
                                if (total > vInt)
                                {
                                    vLoanInstallment = (total - vInt);
                                }
                                if (total <= vInt)
                                {
                                    vLoanInstallment = 0;
                                }
                            }

                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else
                        { ///////////// For General Calculation /////////////////////
                            if (calcMethod == "A" || calcMethod == "S")
                            {
                                if (vInterestBalance > vInt)
                                {
                                    if (total > vInterestBalance)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);
                                    }
                                    if (total <= vInterestBalance)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                }
                                else
                                {
                                    if (total > vInterestBalance)
                                    {
                                        if (vInterestBalance <= 0)
                                        {
                                            vLoanInstallment = total - vInterestBalance;

                                        }
                                        else
                                            vLoanInstallment = (total - vInterestBalance);
                                    }
                                    if (total <= vInt)
                                    {
                                        vLoanInstallment = 0;
                                    }
                                }
                                //if (total > vInterestBalance)
                                //{
                                //    vLoanInstallment = (total - vInt);
                                //}
                                //if (total <= vInt)
                                //{
                                //    vLoanInstallment = 0;
                                //}

                            }
                            else if (calcMethod == "R" || calcMethod == "V")
                            {
                                if (total > (vLoan + vInt) && total - (vLoan + vInt) > vInterestBalance - vInt)
                                {

                                    if (vInterestBalance > 0)
                                    {
                                        vLoanInstallment = (total - vInterestBalance);
                                    }
                                    else
                                    {
                                        vLoanInstallment = total;
                                    }

                                }
                                else
                                {

                                    if (total < (vLoan + vInt))
                                    {
                                        if (total < vInt)
                                        {
                                            vLoanInstallment = 0;
                                        }
                                        else
                                        {
                                            vLoanInstallment = (total - vInt);
                                        }
                                    }
                                    else
                                    {
                                        vLoanInstallment = vLoan;
                                    }
                                }
                            }
                        }
                    }
                    else if (calcMethod == "H")
                    {
                        if (vInterestBalance > vInt)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                            // vLoanInstallment = (parseFloat(total) - parseFloat(vInterestBalance))
                        }

                        else
                        {
                            if (total > vInt)
                            {
                                if (vInterestBalance <= 0)
                                {
                                    vLoanInstallment = total - vInterestBalance;

                                }
                                else
                                    vLoanInstallment = (total - vInt);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vLoanInstallment = (total - vInt);
                        }
                        if (total <= vInt)
                        {
                            vLoanInstallment = 0;
                        }
                    }
                    else
                    {
                        /////////////// For Loan and Int equal 0 /////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total >= vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInt)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                            if (total <= vInterestBalance)
                            {
                                vLoanInstallment = 0;
                            }
                        }
                        else
                        { ///////////////// For General Calculation /////////////////
                            vLoanInstallment = (vLoan * total) / vTotalInstall;
                        }
                    }
                }

                if (total == 0)
                {
                    vInterestInstallment = 0;
                }
                else
                {
                    if (calcMethod == "D")
                    {
                        //////////////// For Loan And Int equal 0 //////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (total > vInt)
                            {
                                vInterestInstallment = vInt;
                            }
                            if (total <= vInt)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }
                        else
                        { ///////////////////// For General Calculation ///////////////////
                            if (total < vInt)
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;
                            }
                            else
                            {
                                vInterestInstallment = (vInt * total) / vTotalInstall;
                            }

                        }
                    }
                    else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V" || calcMethod == "S")
                    {
                        ///////////////////// For Loan And Int equal 0 ////////////////////////
                        if (vLoan == 0 && vInt > 0)
                        {
                            if (calcMethod == "S")
                            {
                                if (total > vInterestBalance)
                                {
                                    vInterestInstallment = total - vLoanInstallment;
                                }
                                else
                                {
                                    vInterestInstallment = total;
                                }

                            }
                            else
                            {
                                if (total > vInt)
                                {
                                    vInterestInstallment = vInt;
                                }
                                if (total <= vInt)
                                {
                                    vInterestInstallment = total;
                                }
                            }
                            //if (total > vInt)
                            //{
                            //    vInterestInstallment = vInt;
                            //}
                            //if (total <= vInt)
                            //{
                            //    vInterestInstallment = total;
                            //}
                        }
                        else if (vLoan == 0 && vInt == 0)
                        {
                            if (total > vInterestBalance)
                            {
                                if (vInterestBalance <= 0)
                                {
                                    vInterestInstallment = 0;
                                }
                                else
                                    vInterestInstallment = vInterestBalance;

                                //vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }
                        }

                        else
                        { //////////////// For General Calculation //////////////////////////
                            if (calcMethod == "A")
                            {

                                if (vInterestBalance > vInt)
                                {

                                    if (total > vInterestBalance)
                                    {
                                        vInterestInstallment = vInterestBalance;
                                    }
                                    if (total <= vInterestBalance)
                                    {
                                        vInterestInstallment = total;
                                    }

                                }
                                else
                                {

                                    if (total > vInt)
                                    {
                                        vInterestInstallment = vInt;
                                    }
                                    if (total <= vInt)
                                    {
                                        vInterestInstallment = total;
                                    }

                                }

                            }
                            else if (calcMethod == "R" || calcMethod == "V")
                            {

                                if (total > (vLoan + vInt) && total - (vLoan + vInt) > (vInterestBalance - vInt))
                                {

                                    if (vInterestBalance > 0)
                                    {
                                        vInterestInstallment = vInterestBalance;
                                    }
                                    else
                                    {
                                        vInterestInstallment = 0;
                                    }

                                }
                                else
                                {

                                    if (total < (vLoan + vInt))
                                    {

                                        if (total < vInt)
                                        {
                                            vInterestInstallment = total;
                                        }
                                        else
                                        {
                                            vInterestInstallment = vInt;
                                        }
                                    }
                                    else
                                    {
                                        vInterestInstallment = (total - vLoan);

                                    }

                                }
                            }

                        }
                    }
                    else if (calcMethod == "E")
                    {
                        if (total > vInt)
                        {
                            vInterestInstallment = vInt;
                        }
                        if (total <= vInt)
                        {
                            vInterestInstallment = total;
                        }
                    }
                    else
                    {

                        ///////////// For Loan And Int equal 0 ////////////////////////

                        if (vLoan == 0 && vInt > 0)
                        {

                            if (total >= vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }

                            if (total < vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }

                            if (total <= vInt)
                            {
                                vInterestInstallment = total;
                            }

                        }
                        else if (vLoan == 0 && vInt == 0)
                        {

                            if (total > vInterestBalance)
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            if (total <= vInterestBalance)
                            {
                                vInterestInstallment = total;
                            }

                        }
                        else
                        {
                            vInterestInstallment = (vInt * total) / vTotalInstall;
                        }
                    }
                }

                if (calcMethod != "A")
                {
                    double vCheck = (vLoanRepaid + vLoanInstallment);
                    if (vCheck > vPrincipalLoan)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                            vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                        }
                    }

                    double vLoanBal = (vPrincipalLoan + vInterestBalance - vLoanRepaid);
                    double calIns = (vLoanRepaid + vLoan);
                    if (vLoan >= vLoanBal)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                            vLoanInstallment = (total - vInterestBalance);
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                            vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                        }
                    }
                }
                else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
                {
                    double vLoan1 = total;
                    double vLoanBal = vPrincipalLoan - vLoanRepaid;
                    double calIns = (vLoanRepaid + vLoan);
                    if (calIns >= vPrincipalLoan)
                    {
                        vLoanInstallment = (total - vInterestInstallment);
                    }
                }

                double vLoanBalance = vPrincipalLoan - vLoanRepaid;
                double vBal = (vLoanBalance + vInterestBalance);

                if (vBal <= total)
                {

                    if (vInterestBalance > 0)
                    {
                        if (calcMethod == "F")
                        {
                            vInterestInstallment = vInterestBalance;
                            if ((total - vInterestBalance) >= (vPrincipalLoan - vLoanRepaid))
                            {
                                vLoanInstallment = (total - vInterestBalance);
                            }
                        }
                        else
                        {
                            vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                            vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                        }
                    }
                    else
                    {
                        if (total > vLoanBalance)
                        {
                            if (calcMethod == "F")
                            {
                                vInterestInstallment = vInterestBalance;
                            }
                            else
                            {
                                vInterestInstallment = total - (vPrincipalLoan - vLoanRepaid);
                                vLoanInstallment = (vPrincipalLoan - vLoanRepaid);
                            }
                        }
                        else
                        {
                            vInterestInstallment = 0;
                            vLoanInstallment = 0;
                        }
                    }
                }

                if ((vLoanInstallment + vInterestInstallment) > total)
                {
                    vLoanInstallment = total - vInterestInstallment;
                }

                if (calcMethod == "A" || calcMethod == "H")
                {
                    double vLoanPayable = (total - vInterestInstallment);
                    if (vLoanPayable > vPrincipalLoan)
                    {
                        double netLoanPay = total - vInterestBalance;
                        vLoanInstallment = netLoanPay;
                        double intPay = total - netLoanPay;
                        vInterestInstallment = intPay;
                    }
                    else
                    {
                        vLoanInstallment = vLoanPayable;
                    }
                }

                if (calcMethod == "F")
                {
                    if (vInterestInstallment > vInterestBalance)
                    {
                        vInterestInstallment = vInterestBalance;
                        vLoanInstallment = total - vInterestBalance;
                    }
                }

                if (calcMethod == "F")
                {
                    if (total > (vInterestBalance + vPrincipalLoan - vLoanRepaid))
                    {
                        total = 0;
                        vLoanInstallment = 0;
                        vInterestInstallment = 0;
                    }
                }

                if (orgId == 54)
                {
                    double vBuroLoanBal = (vPrincipalLoan - vLoanRepaid);
                    double vBuroIntBal = (vCumInterestCharge - vCumInterestPaid);
                    double vBuroActualBal = (vBuroLoanBal + vBuroIntBal);

                    double vTotalInstallBuro = 0;
                    double exceptIstIns = 0;
                    double instMod;
                    double vCumLoanDueF;
                    double vCumIntDueF;
                    double vTotalCumDue;
                    double vCumLoanDue = 0;
                    double vCumIntDue = 0;

                    if (vBuroActualBal == total)
                    {
                        vInterestInstallment = vBuroIntBal;
                        vLoanInstallment = vBuroLoanBal;
                    }
                    else
                    {
                        if (installmentNo == 1 && vLoanRepaid == 0)
                        {
                            if (total < (vLoan + vInt))
                            {
                                vInterestInstallment = 0;
                                vLoanInstallment = 0;
                                //              print(723);
                            }
                            else
                            {
                                //              print(726);
                                vTotalInstallBuro = durationOverLoanDue + durationOverIntDue;
                                exceptIstIns = total - (vLoan + vInt);
                                if (exceptIstIns == 0)
                                {
                                    vInterestInstallment = vInt;
                                    vLoanInstallment = vLoan;
                                }
                                else
                                {
                                    //                print(733);
                                    instMod = (exceptIstIns % vTotalInstallBuro);
                                    if (instMod == 0)
                                    {
                                        vLoanInstallment = (durationOverLoanDue * exceptIstIns) / vTotalInstallBuro + vLoan;
                                        vInterestInstallment = (durationOverIntDue * exceptIstIns) / vTotalInstallBuro + vInt;
                                    }
                                    else
                                    {
                                        vLoanInstallment = 0;
                                        vInterestInstallment = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (total == vLoan + vInt)
                            {
                                vLoanInstallment = vLoan;
                                vInterestInstallment = vInt;
                                //              print(745);
                            }
                            else
                            {
                                //              print(747);
                                vCumLoanDueF = vCumLoanDue - vLoanRepaid;
                                vCumIntDueF = vCumIntDue - vCumInterestPaid;
                                vTotalCumDue = vCumLoanDueF + vCumIntDueF;

                                if (vTotalCumDue > 0 && vLoanRepaid == 0)
                                {
                                    total = total - (vCumLoanDueF + vCumIntDueF);
                                }


                                vTotalInstallBuro = (durationOverLoanDue + durationOverIntDue);
                                //  print("durOverLoanDue ${durationOverLoanDue} durIntDue  ${durationOverIntDue}  total ${total} vTotalInstallBuro ${vTotalInstallBuro}");
                                instMod = (total % vTotalInstallBuro);


                                if (instMod == 0)
                                {
                                    vLoanInstallment = (durationOverLoanDue * total) / vTotalInstallBuro;
                                    vInterestInstallment = (durationOverIntDue * total) / vTotalInstallBuro;
                                    int noOfinst = Convert.ToInt16((total / vTotalInstallBuro));
                                    int buroTotal = Convert.ToInt16((noOfinst * vTotalInstallBuro));
                                    if (vTotalCumDue > 0 && vLoanRepaid == 0)
                                    {
                                        vLoanInstallment = vLoan + vCumLoanDueF;
                                        vInterestInstallment = vInt + vCumIntDueF;
                                    }
                                    else
                                    {
                                        vLoanInstallment = (durationOverLoanDue * total) / vTotalInstallBuro;
                                        vInterestInstallment = (durationOverIntDue * total) / vTotalInstallBuro;
                                    }
                                }
                                else
                                {
                                    vLoanInstallment = 0;
                                    vInterestInstallment = 0;
                                }
                            }
                        }
                    }
                }

            }
        }
        //{
        //    double vcumInrerestCharge;
        //    double vcumInrerestPaid;
        //    vcumInrerestCharge = CumIntCharge;
        //    vcumInrerestPaid = CumInterestPaid;
        //    double vInterestBalance = (vcumInrerestCharge - vcumInrerestPaid);

        //    vLoanInstallment = 0;
        //    vInterestInstallment = 0;
        //    double vPrincipalLOan = principalLoan;
        //    double vloanRepaid = loanRepaid;
        //    double vLoan = DurationOverLoanDue;
        //    double vInt = DurationOverIntDue;
        //    double vLoanDueSCase = DurationOverLoanDue;
        //    double vIntDueSCase = DurationOverIntDue;
        //    double vTotalInstall = (vLoan + vInt);
        //    if (InstallmentNo > Duration)
        //    {
        //        vLoan = DurationOverLoanDue;
        //        vInt = DurationOverIntDue;
        //        vLoanDueSCase = DurationOverLoanDue;
        //        vIntDueSCase = DurationOverIntDue;
        //        vTotalInstall = vLoan + vInt;
        //        if (total == 0)
        //        {
        //            vLoanInstallment = 0;
        //            vInterestInstallment = 0;
        //        }
        //        else
        //        {
        //            if (calcMethod == "D")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                    vLoanInstallment = total;
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                if ((vPrincipalLOan - vloanRepaid) == 0)
        //                    vLoanInstallment = 0;


        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                if (vDOC == 0)
        //                {
        //                    if (vLoan > 0)
        //                    {

        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;

        //                    }
        //                    else
        //                    {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                            vLoanInstallment = total;
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)
        //                            vLoanInstallment = 0;
        //                    }
        //                }

        //            }
        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
        //            {


        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {

        //                    vLoanInstallment = total;
        //                }
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)

        //                    vLoanInstallment = 0;
        //                if (vDOC == 0)
        //                {
        //                    if (vLoan > 0)
        //                    {

        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;

        //                    }
        //                    else
        //                    {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vLoanInstallment = total;
        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)

        //                            vLoanInstallment = 0;
        //                    }
        //                }


        //            }
        //            else if (calcMethod == "E")
        //            {

        //                if (total > vInt)
        //                {
        //                    vLoanInstallment = (total - vInt);
        //                }
        //                if (total <= vInt)
        //                {
        //                    vLoanInstallment = 0;
        //                }
        //            }
        //            else
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoanDueSCase == 0 && vIntDueSCase > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vIntDueSCase)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else if (vLoan > 0 && vInt > 0)
        //                {
        //                    vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                }
        //                else
        //                {

        //                    vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                }
        //            }

        //        }
        //        // loanPaidId = vLoanInstallment;

        //        if (total == 0)
        //        {

        //            vInterestInstallment = 0;
        //        }
        //        else
        //        {
        //            if (calcMethod == "D")
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {

        //                    vInterestInstallment = 0;
        //                }
        //                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)

        //                    vInterestInstallment = total;


        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vInterestInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = 0;
        //                    }
        //                }
        //                if (vDOC == 0)
        //                {
        //                    if (vTotalInstall > 0)
        //                    {

        //                        vInterestInstallment = (vInt * total) / vTotalInstall;

        //                    }
        //                    else
        //                    {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vInterestInstallment = 0;
        //                        }
        //                        //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)

        //                            vInterestInstallment = total;
        //                    }
        //                }

        //            }
        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "D")
        //            {


        //                if ((vPrincipalLOan - vloanRepaid) >= total)
        //                {

        //                    vInterestInstallment = 0;
        //                }
        //                //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                }
        //                if ((vPrincipalLOan - vloanRepaid) == 0)

        //                    vInterestInstallment = total;

        //                if (vDOC == 0)
        //                {
        //                    if (vTotalInstall > 0)
        //                    {

        //                        vInterestInstallment = (vInt * total) / vTotalInstall;

        //                    }
        //                    else
        //                    {
        //                        if ((vPrincipalLOan - vloanRepaid) >= total)
        //                        {

        //                            vInterestInstallment = 0;
        //                        }
        //                        //if ((parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) > 0 && (parseFloat(vPrincipalLOan) - parseFloat(vloanRepaid)) < (parseFloat(vLoanDueSCase) + parseFloat(vIntDueSCase)))
        //                        if ((vPrincipalLOan - vloanRepaid) > 0 && (vPrincipalLOan - vloanRepaid) < total)
        //                        {
        //                            vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);

        //                        }
        //                        if ((vPrincipalLOan - vloanRepaid) == 0)

        //                            vInterestInstallment = total;
        //                    }
        //                }

        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vInterestInstallment = vInt;
        //                }
        //                if (total <= vInt)
        //                {
        //                    vInterestInstallment = total;
        //                }
        //            }
        //            else
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if (vLoanDueSCase == 0 && vIntDueSCase > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vIntDueSCase)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoanDueSCase == 0 && vIntDueSCase == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan > 0 && vInt > 0)
        //                {
        //                    vInterestInstallment = (vInt * total) / vTotalInstall;
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {
        //                    vInterestInstallment = (vInt * total) / vTotalInstall;
        //                }
        //            }
        //        }
        //        double vLoanTotal = total;
        //        double vLoanBal;
        //        if (calcMethod != "A")
        //        {

        //            double vCheck = (vloanRepaid + vLoanInstallment);
        //            if (vCheck > vPrincipalLOan)
        //            {
        //                vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //            }

        //            vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
        //            double calIns = (vloanRepaid + vLoan);
        //            if (vLoan >= vLoanBal)
        //            {
        //                vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                vLoanInstallment = (vPrincipalLOan - vloanRepaid);

        //            }

        //        }
        //        else if (calcMethod == "A" || calcMethod == "R")
        //        {

        //            vLoanBal = vPrincipalLOan - vloanRepaid;
        //            var calIns = vInterestBalance + vLoanBal;
        //            if (vLoan >= calIns)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


        //            }
        //        }


        //        var vLoanBalance = vPrincipalLOan - vloanRepaid;
        //        var vBal = vLoanBalance + vInterestBalance;
        //        if (vBal <= total)
        //        {

        //            if (vInterestBalance > 0)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);



        //            }
        //            else

        //                if (total > vLoanBalance)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //            }
        //            else
        //            {
        //                vInterestInstallment = 0;
        //                vLoanInstallment = total;
        //            }


        //        }
        //        if ((vLoanInstallment + vInterestInstallment) > total)
        //        {
        //            vLoanInstallment = total - vInterestInstallment;

        //        }


        //        if (calcMethod == "F")
        //        {
        //            if (total > vLoanInstallment + vInterestInstallment)
        //            {
        //                vLoanInstallment = 0;
        //                vInterestInstallment = 0;
        //                total = 0;

        //            }
        //        }
        //    }

        //    else
        //    {
        //        vLoan = loandue;
        //        vInt = intdue;
        //        vTotalInstall = vLoan + vInt;
        //        if (total == 0)
        //        {
        //            vLoanInstallment = 0;
        //            vInterestInstallment = 0;
        //        }
        //        else
        //        {

        //            if (calcMethod == "D")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {

        //                    if (total > vInt)
        //                    {
        //                        vLoanInstallment = (total - vInt);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }

        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }

        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {

        //                    if (total < vLoan)
        //                    {
        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                        // vLoanInstallment = 0;
        //                    }
        //                    else
        //                    {
        //                        vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                    }
        //                }
        //            }

        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total > vInt)
        //                    {
        //                        vLoanInstallment = (total - vInt);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {
        //                    if (calcMethod == "A")
        //                    {
        //                        if (vInterestBalance > vInt)
        //                        {
        //                            if (total > vInterestBalance)
        //                            {
        //                                vLoanInstallment = (total - vInterestBalance);
        //                            }
        //                            if (total <= vInterestBalance)
        //                            {
        //                                vLoanInstallment = 0;
        //                            }
        //                            // vLoanInstallment = (parseFloat(total) - parseFloat(vInterestBalance))
        //                        }

        //                        else
        //                        {
        //                            if (total > vInt)
        //                            {
        //                                vLoanInstallment = (total - vInt);
        //                            }
        //                            if (total <= vInt)
        //                            {
        //                                vLoanInstallment = 0;
        //                            }
        //                        }
        //                    }
        //                    else if (calcMethod == "R" || calcMethod == "V")
        //                    {
        //                        if (total > (vLoan + vInt) && total - (vLoan + vInt) > vInterestBalance - vInt)
        //                        {
        //                            if (vInterestBalance > 0)
        //                            {
        //                                vLoanInstallment = (total - vInterestBalance);

        //                            }
        //                            else
        //                            {
        //                                vLoanInstallment = total;
        //                            }


        //                        }
        //                        else
        //                        {
        //                            if (total < (vLoan + vInt))
        //                            {

        //                                if (total < vInt)
        //                                {
        //                                    vLoanInstallment = 0;
        //                                }
        //                                else
        //                                {
        //                                    vLoanInstallment = (total - vInt);
        //                                }

        //                            }
        //                            else
        //                                vLoanInstallment = vLoan;

        //                        }


        //                    }

        //                }



        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vLoanInstallment = (total - vInt);
        //                }
        //                if (total <= vInt)
        //                {
        //                    vLoanInstallment = 0;
        //                }
        //            }
        //            else
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////
        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vLoanInstallment = 0;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {

        //                    vLoanInstallment = (vLoan * total) / vTotalInstall;
        //                }



        //            }
        //        }

        //        if (total == 0)
        //        {

        //            vInterestInstallment = 0;
        //        }
        //        else
        //        {
        //            if (calcMethod == "D")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {


        //                    if (total > vInt)
        //                    {
        //                        vInterestInstallment = vInt;
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {


        //                    if (total < vInt)
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;

        //                    }
        //                    else
        //                    {
        //                        vInterestInstallment = (vInt * total) / vTotalInstall;
        //                    }
        //                }
        //            }

        //            else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //            {
        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total > vInt)
        //                    {
        //                        vInterestInstallment = vInt;
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {

        //                    if (calcMethod == "A")
        //                    {
        //                        if (vInterestBalance > vInt)
        //                        {
        //                            if (total > vInterestBalance)
        //                            {
        //                                vInterestInstallment = vInterestBalance;
        //                            }
        //                            if (total <= vInterestBalance)
        //                            {
        //                                vInterestInstallment = total;
        //                            }
        //                            //vInterestInstallment = parseFloat(vInterestBalance)
        //                        }
        //                        else
        //                        {
        //                            if (total > vInt)
        //                            {
        //                                vInterestInstallment = vInt;
        //                            }
        //                            if (total <= vInt)
        //                            {
        //                                vInterestInstallment = total;
        //                            }
        //                        }
        //                    }
        //                    else if (calcMethod == "R" || calcMethod == "V")
        //                    {
        //                        if (total > (vLoan + vInt) && total - (vLoan + vInt) > (vInterestBalance - vInt))
        //                        {

        //                            if (vInterestBalance > 0)
        //                            {
        //                                vInterestInstallment = vInterestBalance;
        //                            }
        //                            else
        //                            {
        //                                vInterestInstallment = 0;
        //                            }


        //                        }
        //                        else
        //                        {
        //                            if (total < (vLoan + vInt))
        //                            {
        //                                if (total < vInt)
        //                                { vInterestInstallment = total; }
        //                                else
        //                                {
        //                                    vInterestInstallment = vInt;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                vInterestInstallment = (total - vLoan);
        //                            }

        //                        }
        //                    }

        //                }

        //            }
        //            else if (calcMethod == "E")
        //            {
        //                if (total > vInt)
        //                {
        //                    vInterestInstallment = vInt;
        //                }
        //                if (total <= vInt)
        //                {
        //                    vInterestInstallment = total;
        //                }
        //            }
        //            else
        //            {

        //                /////////////////////For LOan And Int equal  0////////////////////////

        //                if (vLoan == 0 && vInt > 0)
        //                {
        //                    if (total >= vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }

        //                    if (total < vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                    if (total <= vInt)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                else if (vLoan == 0 && vInt == 0)
        //                {
        //                    if (total > vInterestBalance)
        //                    {
        //                        vInterestInstallment = vInterestBalance;
        //                    }
        //                    if (total <= vInterestBalance)
        //                    {
        //                        vInterestInstallment = total;
        //                    }
        //                }
        //                /////////////////////for General Calculation///////////////////////////////////////
        //                else
        //                {

        //                    vInterestInstallment = (vInt * total) / vTotalInstall;
        //                }
        //            }
        //        }

        //        if (calcMethod != "A")
        //        {
        //            double vCheck = (vloanRepaid + vLoanInstallment);
        //            if (vCheck > vPrincipalLOan)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);


        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }

        //            }

        //            double vLoanBal = (vPrincipalLOan + vInterestBalance - vloanRepaid);
        //            double calIns = (vloanRepaid + vLoan);
        //            if (vLoan >= vLoanBal)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                    vLoanInstallment = (total - vInterestBalance);

        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }

        //            }
        //        }
        //        else if (calcMethod == "A" || calcMethod == "R" || calcMethod == "V")
        //        {
        //            double vLoan1 = total;
        //            double vLoanBal = vPrincipalLOan - vloanRepaid;
        //            double calIns = (vloanRepaid + vLoan);
        //            if (calIns >= vPrincipalLOan)
        //            {
        //                vLoanInstallment = (total - vInterestInstallment);

        //            }
        //        }

        //        double vLoanBalance = vPrincipalLOan - vloanRepaid;
        //        double vBal = (vLoanBalance + vInterestBalance);
        //        if (vBal <= total)
        //        {

        //            if (vInterestBalance > 0)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                    if ((total - vInterestBalance) >= (vPrincipalLOan - vloanRepaid))
        //                    {
        //                        vLoanInstallment = (total - vInterestBalance);
        //                    }


        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }


        //            }
        //            else
        //                if (total > vLoanBalance)
        //            {
        //                if (calcMethod == "F")
        //                {
        //                    vInterestInstallment = vInterestBalance;
        //                }
        //                else
        //                {
        //                    vInterestInstallment = total - (vPrincipalLOan - vloanRepaid);
        //                    vLoanInstallment = (vPrincipalLOan - vloanRepaid);
        //                }
        //            }
        //            else
        //            {
        //                vInterestInstallment = 0;
        //                vLoanInstallment = total;
        //            }


        //        }

        //        if ((vLoanInstallment + vInterestInstallment) > total)
        //        {
        //            vLoanInstallment = total - vInterestInstallment;

        //        }
        //        if (calcMethod == "A" || calcMethod == "H")
        //        {


        //            double vLoanPayable = (total - vInterestInstallment);
        //            if (vLoanPayable > vPrincipalLOan)
        //            {
        //                double NeatLoanPay = total - vInterestBalance;
        //                vLoanInstallment = NeatLoanPay;
        //                double intPay = total - NeatLoanPay;
        //                vInterestInstallment = intPay;
        //            }
        //            else
        //            {
        //                vLoanInstallment = vLoanPayable;

        //            }

        //        }

        //        if (calcMethod == "F")
        //        {
        //            if (vInterestInstallment > vInterestBalance)
        //            {
        //                vInterestInstallment = vInterestBalance;
        //                vLoanInstallment = total - vInterestBalance;

        //            }

        //        }




        //        if (calcMethod == "F")
        //        {
        //            if (total > (vInterestBalance + vPrincipalLOan - vloanRepaid))
        //            {
        //                total = 0;
        //                vLoanInstallment = 0;
        //                vInterestInstallment = 0;


        //            }
        //        }
        //        ///For Buro//////////////Check Mulitiple Installment////////
        //        if (OrgID == 54)
        //        {

        //            double vBuroLoanBal = (vPrincipalLOan - vloanRepaid);
        //            double vBuroIntBal = (vcumInrerestCharge - vcumInrerestPaid);
        //            double vBuroActualBal = (vBuroLoanBal + vBuroIntBal);
        //            double vTotalInstallBuro = 0;
        //            double exceptIstIns = 0;
        //            int instMod;
        //            double vCumLoanDueF;
        //            double vCumIntDueF;
        //            double vTotalCumDue;
        //            if (vBuroActualBal == total)
        //            {
        //                vInterestInstallment = vBuroIntBal;
        //                vLoanInstallment = vBuroLoanBal;

        //            }
        //            else {
        //                if (InstallmentNo == 1 && vloanRepaid == 0)
        //                {
        //                    if (total < (vLoan + vInt))
        //                    {
        //                        vInterestInstallment = 0;
        //                        vLoanInstallment = 0;
        //                    }
        //                    else
        //                    {
        //                        vTotalInstallBuro = DurationOverLoanDue + DurationOverIntDue;
        //                        exceptIstIns = total - (vLoan + vInt);
        //                        if(exceptIstIns==0)
        //                        {
        //                            vInterestInstallment = vInt;
        //                            vLoanInstallment = vLoan;
        //                        }
        //                        else
        //                        {
        //                            instMod = Convert.ToInt16(exceptIstIns % vTotalInstallBuro);
        //                            if(instMod==0)
        //                            {
        //                                vLoanInstallment = (DurationOverLoanDue * exceptIstIns) / vTotalInstallBuro + vLoan;
        //                                vInterestInstallment = (DurationOverIntDue * exceptIstIns) / vTotalInstallBuro + vInt;
        //                            }
        //                            else
        //                            {
        //                                vLoanInstallment = 0;
        //                                vInterestInstallment = 0;
        //                            }
        //                        }
        //                    }
        //                }


        //                else
        //                {
        //                    if (total == vLoan + vInt)
        //                    {

        //                        vLoanInstallment = vLoan;
        //                        vInterestInstallment = vInt;

        //                    }
        //                    else
        //                    {
        //                         vCumLoanDueF = vcumLoanDue - vloanRepaid;
        //                         vCumIntDueF = vcumIntDue - vcumInrerestPaid;
        //                        vTotalCumDue = vCumLoanDueF + vCumIntDueF;
        //                        if (vTotalCumDue > 0 && vloanRepaid == 0)
        //                        {
        //                            total = total - (vCumLoanDueF + vCumIntDueF);
        //                        }
        //                        vTotalInstallBuro = (DurationOverLoanDue + DurationOverIntDue);
        //                        instMod = Convert.ToInt16(total % vTotalInstallBuro);
        //                        if (instMod == 0)
        //                        {
        //                            vLoanInstallment = (DurationOverLoanDue * total) / vTotalInstallBuro;
        //                            vInterestInstallment = (DurationOverIntDue * total) / vTotalInstallBuro;
        //                            int NoOfinst = Convert.ToInt16(total / vTotalInstallBuro);
        //                            int buroTotal = Convert.ToInt16(NoOfinst * vTotalInstallBuro);
        //                            if (vTotalCumDue > 0 && vloanRepaid == 0)
        //                            {
        //                                vLoanInstallment = vLoan + vCumLoanDueF;
        //                                vInterestInstallment = vInt + vCumIntDueF;

        //                            }
        //                            else {
        //                                vLoanInstallment = vLoan;
        //                                vInterestInstallment = vInt;

        //                            }
        //                        }
        //                        else {
        //                            vLoanInstallment = 0;
        //                            vInterestInstallment = 0;
        //                        }
        //                    }
        //                }
        //            }

        //        }


        //    }
        //}

        #region Send Sms
        private string TwilioSms(string msgBody, string receiver, string memId, string collDt)
        {
            var result = "";
            try
            {
                SerialPort port = new SerialPort();

                clsSMS objclsSMS = new clsSMS();
                ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
                //      port = objclsSMS.OpenPort("COM3", 9600, 8, 300, 300);
                // if(port.PortName == null)
                port = objclsSMS.OpenPort(getModemPortNumber(), 9600, 8, 300, 300);

                if (port != null)
                {
                    if (objclsSMS.sendMsg(port, receiver, msgBody))
                    {
                        SmsLogViewModel log = new SmsLogViewModel();
                        log.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        log.MemberID = Convert.ToInt64(memId);
                        log.SmsType = "C";
                        log.SmsFrom = "gBanker Plus";
                        log.SmsTo = receiver;
                        log.SmsBody = msgBody;
                        log.DateSent = Convert.ToDateTime(collDt);
                        log.SmsStatus = "Sent";
                        log.IsActive = true;
                        var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                        smsLogService.Create(entity);
                        result = "1";
                    }
                    else
                    {
                        SmsLogViewModel log = new SmsLogViewModel();
                        log.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        log.MemberID = Convert.ToInt64(memId);
                        log.SmsType = "C";
                        log.SmsFrom = "gBanker Plus";
                        log.SmsTo = receiver;
                        log.SmsBody = msgBody;
                        log.DateSent = Convert.ToDateTime(collDt);
                        log.SmsStatus = "Fail";
                        log.IsActive = true;
                        var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                        smsLogService.Create(entity);
                    }
                }
                objclsSMS.ClosePort(port);
                return result;
            }
            catch (Exception ex)
            {
                return result = string.Empty;
            }
        }
        #endregion
        public ActionResult SendSMS(string msgBody, string receiver, string memId, string collDt)
        {
            var result = "";
            try
            {
                clsSMS objclsSMS = new clsSMS();
                ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
                var portname = ConfigurationManager.AppSettings["PortNumber"];
                HttpClient Client = new HttpClient();
                Client.BaseAddress = new Uri("http://192.192.192.233:8091/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Client.DefaultRequestHeaders.Add("Accept", "application/json");
                //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                HttpResponseMessage message = Client.GetAsync($"api/nonmaskingsms/easysend?sender={receiver}&message={msgBody}").Result;
                if (message.IsSuccessStatusCode)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);

                //using (SerialPort port = objclsSMS.OpenPort(portname, 9600, 8, 300, 300))
                //{

                //    if (port != null)
                //    {
                //        if (objclsSMS.sendMsg(port, receiver, msgBody))
                //        {
                //            SmsLogViewModel log = new SmsLogViewModel();
                //            log.OrgID = 1;
                //            log.MemberID = Convert.ToInt64(memId);
                //            log.SmsType = "D";
                //            log.SmsFrom = "Server";
                //            log.SmsTo = receiver;
                //            log.SmsBody = msgBody;
                //            log.DateSent = Convert.ToDateTime(collDt);
                //            log.SmsStatus = "Sent";
                //            log.IsActive = true;
                //            var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                //            smsLogService.Create(entity);
                //            result = "1";
                //        }
                //        else
                //        {
                //            SmsLogViewModel log = new SmsLogViewModel();
                //            log.OrgID = 1;
                //            log.MemberID = Convert.ToInt64(memId);
                //            log.SmsType = "D";
                //            log.SmsFrom = "Server";
                //            log.SmsTo = receiver;
                //            log.SmsBody = msgBody;
                //            log.DateSent = Convert.ToDateTime(collDt);
                //            log.SmsStatus = "Fail";
                //            log.IsActive = true;
                //            var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                //            smsLogService.Create(entity);
                //        }
                //    }

                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenerateRepaymentReport(string MemberID)
        {
            var param = new { MemberID = MemberID, OfficeId = LoginUserOfficeID, };
            var allproducts = loanCollectionReportService.GenerateReceipt(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
            if (allproducts.Tables[0].Rows.Count > 0)
            {
                ReportHelper.PrintReport("PrintRecipt.rpt", allproducts.Tables[0], reportParam);
            }
            else
                return GetErrorMessageResult("Record not found");

            return Content(string.Empty);
        }
        public ActionResult GenerateReport(string fromDate, string toDate, string CenterID)
        {
            var param = new { OfficeID = LoginUserOfficeID, CenterId = CenterID };
            var allproducts = loanCollectionReportService.GetDataCollectionInfo(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
            ReportHelper.PrintReport("rptDailyLoanCollection.rpt", allproducts.Tables[0], reportParam);
            return Content(string.Empty);
        }
        public ActionResult UpdateDataLessFiftyPercent(string officeId, string CenterID)
        {
            loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
            var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(CenterID), 1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public Product GetProduct(int productid)
        {
            var mbr = productService.GetById(productid);
            return mbr;
        }
        private void MapDropDownList(DailyLoanTrxViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var EmployeeType = "";
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
            if (LoanInstallMent.Tables[0].Rows.Count > 0)
            {
                EmployeeType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
            }
            else
            {
                EmployeeType = "";
            }
            var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, EmpId = Convert.ToInt16(LoggedInEmployeeID), EmpType = EmployeeType.ToString(), ColDay = TransactionDay };

            IEnumerable<Center> allcenter;
            List<CenterViewModel> List_ProductViewModel = new List<CenterViewModel>();
            DataSet div_items;
            if (LoggedInOrganizationID == 54)
            {                
                div_items = ultimateReportService.GetDailyCenterList(param);
            }
            else
            {                
                div_items = ultimateReportService.GetDailyCenterList(param);                
            }

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<Int32>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " - " + x.CenterName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.centerListItems = d_items;
            
            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "11", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Bad Debt", Value = "104" });
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var alloffice = officeService.GetAll().Where(l => l.OfficeID == LoginUserOfficeID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID), "L");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;


        }
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            string vInterestCalcMethod = string.Empty, vPaymentFreq = string.Empty;
            decimal vLoanDue = 0;
            decimal vInterestDue = 0;
            decimal vPrincipalLoan = 0;
            decimal vLoanRepaid = 0;
            Int64 vDailyLoanTrxID = 0;

            DataSet LoanInstallMent;

            // LoanInstallMent = loanCollectionService.GetAll().Where(l => l.OfficeID == Convert.ToInt16(officeId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm && l.IsActive == true).FirstOrDefault(); ;
            var paramSLC = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(centerId), @ProductID = productid, @MemberID = Convert.ToInt64(MemId), @LoanTerm = loanTerm };

            LoanInstallMent = ultimateReportService.GetDataDailyLoantrx(paramSLC);

            if (LoanInstallMent != null)
            {
                vLoanInstallment = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanDue"].ToString());
                vInterestInstallment = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntDue"].ToString());
                vTotalIns = vLoanInstallment + vInterestInstallment;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
                vLoanDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanDue"].ToString());
                vInterestDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntDue"].ToString());
                vPrincipalLoan = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["PrincipalLoan"].ToString());
                vLoanRepaid = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanRepaid"].ToString());
                vDailyLoanTrxID = Convert.ToInt64(LoanInstallMent.Tables[0].Rows[0]["DailyLoanTrxID"]);

                //vLoanInstallment = LoanInstallMent.LoanDue;
                //vInterestInstallment = LoanInstallMent.IntDue;
                //vTotalIns = vLoanInstallment + vInterestInstallment;
                //var prod = productService.GetById(productid);
                //vInterestCalcMethod = prod.InterestCalculationMethod;
                //vPaymentFreq = prod.PaymentFrequency;
                //vLoanDue = LoanInstallMent.LoanDue;
                //vInterestDue = LoanInstallMent.IntDue;
                //vPrincipalLoan = LoanInstallMent.PrincipalLoan;
                //vLoanRepaid = LoanInstallMent.LoanRepaid;
                //vDailyLoanTrxID = LoanInstallMent.DailyLoanTrxID;

            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
            }
            //  var pbr = productService.GetById(productid);


            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanDue, InterestDue = vInterestDue, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, total = vTotalIns.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new DailyLoanTrxViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
            var mlt = loanCollectionService.getMaxLoanterm(entity);
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Convert.ToInt16(mlt);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName + " " + m.MiddleName + " " + m.LastName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName + " " + m1.MiddleName + " " + m1.LastName) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDailyLoanCollectionSheet(int centerId, int productId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            try
            {


                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenterQueryable(centerId, filterColumn, filterValue, sortColumn, sortOrder);
                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID).OrderBy(c => c.MemberCode.Substring(c.MemberCode.Length - 5));

                var memberModels = Mapper.Map<IEnumerable<DailyLoanTrx>, IEnumerable<DailyLoanTrxViewModel>>(members);

                List<DailyLoanTrxViewModel> detail = new List<DailyLoanTrxViewModel>();
                int rowSl = 0;

                if (LoggedInOrganizationID == 54)
                {
                    foreach (var vd in memberModels)
                    {
                        var prod = GetProduct(vd.ProductID);
                        var loans = new DailyLoanTrxViewModel()
                        {
                            rowSl = rowSl,
                            DailyLoanTrxID = vd.DailyLoanTrxID,
                            TrxDate = vd.TrxDate,
                            TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"),
                            LoanSummaryID = vd.LoanSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            InterestCalculationMethod = vd.InterestCalculationMethod,
                            CenterID = vd.CenterID,
                            MemberCategoryID = vd.MemberCategoryID,
                            LoanTerm = vd.LoanTerm,
                            PurposeID = vd.PurposeID,
                            InstallmentDate = vd.InstallmentDate,
                            PrincipalLoan = vd.PrincipalLoan,
                            LoanRepaid = vd.LoanRepaid,
                            LoanDue = vd.LoanDue,
                            LoanPaid = vd.LoanPaid,
                            IntPaid = vd.IntPaid,
                            CumIntCharge = vd.CumIntCharge,
                            IntCharge = vd.IntCharge,
                            IntDue = vd.IntDue,
                            Advance = vd.Advance,
                            DueRecovery = vd.DueRecovery,
                            TrxType = vd.TrxType,
                            InstallmentNo = vd.InstallmentNo,
                            EmployeeID = vd.EmployeeID,
                            TotalPaid = vd.TotalPaid,
                            InvestorID = vd.InvestorID,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            DueLoanSummary = vd.DueLoanSummary,
                            LoanCollectionSummary = vd.LoanCollectionSummary,
                            DueInterestSummary = vd.DueInterestSummary,
                            InterestCollectionSummary = vd.InterestCollectionSummary,
                            TotalDueSummary = vd.TotalDueSummary,
                            TotalCollectionSummary = vd.TotalCollectionSummary,
                            Duration = vd.Duration,
                            DurationOverLoanDue = vd.DurationOverLoanDue,
                            DurationOverIntDue = vd.DurationOverIntDue,
                            PhoneNo = vd.PhoneNo,
                            DurationOverCollection = prod.DurationOverCollection,
                            OrgID = vd.OrgID,
                            LoanAccountNo = vd.LoanAccountNo.Substring(4, (vd.LoanAccountNo.Length - 4)),
                            CumLoanDue = vd.CumLoanDue,
                            CumIntDue = vd.CumIntDue
                        };
                        detail.Add(loans);
                        rowSl++;
                    }
                }
                else
                {
                    foreach (var vd in memberModels)
                    {
                        var prod = GetProduct(vd.ProductID);
                        var loans = new DailyLoanTrxViewModel()
                        {
                            rowSl = rowSl,
                            DailyLoanTrxID = vd.DailyLoanTrxID,
                            TrxDate = vd.TrxDate,
                            TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"),
                            LoanSummaryID = vd.LoanSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            InterestCalculationMethod = vd.InterestCalculationMethod,
                            CenterID = vd.CenterID,
                            MemberCategoryID = vd.MemberCategoryID,
                            LoanTerm = vd.LoanTerm,
                            PurposeID = vd.PurposeID,
                            InstallmentDate = vd.InstallmentDate,
                            PrincipalLoan = vd.PrincipalLoan,
                            LoanRepaid = vd.LoanRepaid,
                            LoanDue = vd.LoanDue,
                            LoanPaid = vd.LoanPaid,
                            IntPaid = vd.IntPaid,
                            CumIntCharge = vd.CumIntCharge,
                            IntCharge = vd.IntCharge,
                            IntDue = vd.IntDue,
                            Advance = vd.Advance,
                            DueRecovery = vd.DueRecovery,
                            TrxType = vd.TrxType,
                            InstallmentNo = vd.InstallmentNo,
                            EmployeeID = vd.EmployeeID,
                            TotalPaid = vd.TotalPaid,
                            InvestorID = vd.InvestorID,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            DueLoanSummary = vd.DueLoanSummary,
                            LoanCollectionSummary = vd.LoanCollectionSummary,
                            DueInterestSummary = vd.DueInterestSummary,
                            InterestCollectionSummary = vd.InterestCollectionSummary,
                            TotalDueSummary = vd.TotalDueSummary,
                            TotalCollectionSummary = vd.TotalCollectionSummary,
                            Duration = vd.Duration,
                            DurationOverLoanDue = vd.DurationOverLoanDue,
                            DurationOverIntDue = vd.DurationOverIntDue,
                            PhoneNo = vd.PhoneNo,
                            DurationOverCollection = prod.DurationOverCollection,
                            OrgID = vd.OrgID,
                            LoanAccountNo = vd.LoanAccountNo,
                            CumIntDue=vd.CumIntDue
                        };
                        detail.Add(loans);
                        rowSl++;
                    }

                }

                return Json(new { Result = "OK", Records = detail });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult GetDailyLoanCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue)
        {
            try
            {
                // long totalCount;

                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenter(centerId, filterColumn, filterValue, "", "").ToList();
                // var collectionList = loanCollectionService.GetLoanCollectionDetailPaged(centerId, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount).ToList();
                var products = new List<DailyLoanTrxViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId).OrderBy(p => p.ProductCode).FirstOrDefault() == null)
                    {
                        var prodViewModel = Mapper.Map<DailyLoanTrx, DailyLoanTrxViewModel>(tr);
                        // var v = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanDue);
                        prodViewModel.DueLoanSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanDue);
                        prodViewModel.LoanCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanPaid);
                        prodViewModel.DueInterestSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.IntDue);
                        prodViewModel.InterestCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.IntPaid);
                        prodViewModel.TotalDueSummary = prodViewModel.DueLoanSummary + prodViewModel.DueInterestSummary;
                        prodViewModel.TotalCollectionSummary = prodViewModel.LoanCollectionSummary + prodViewModel.InterestCollectionSummary;
                        products.Add(prodViewModel);
                    }

                }

                var currentPageProducts = products.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageProducts, TotalRecordCount = products.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult GetDailyProshikaLoanCollectionSheet(int centerId, int productId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            try
            {


                var collectionList = loanCollectionService.GetDailyProshikaLoanCollectionByCenterQueryable(centerId, filterColumn, filterValue, sortColumn, sortOrder);
                var members = collectionList.Where(c => c.MainProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID).OrderBy(c => c.MemberCode.Substring(c.MemberCode.Length - 5));

                var memberModels = Mapper.Map<IEnumerable<DailyLoanTrx>, IEnumerable<DailyLoanTrxViewModel>>(members);

                List<DailyLoanTrxViewModel> detail = new List<DailyLoanTrxViewModel>();
                int rowSl = 0;

                if (LoggedInOrganizationID == 54)
                {
                    foreach (var vd in memberModels)
                    {
                        var prod = GetProduct(vd.ProductID);
                        var loans = new DailyLoanTrxViewModel()
                        {
                            rowSl = rowSl,
                            DailyLoanTrxID = vd.DailyLoanTrxID,
                            TrxDate = vd.TrxDate,
                            TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"),
                            LoanSummaryID = vd.LoanSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            InterestCalculationMethod = vd.InterestCalculationMethod,
                            CenterID = vd.CenterID,
                            MemberCategoryID = vd.MemberCategoryID,
                            LoanTerm = vd.LoanTerm,
                            PurposeID = vd.PurposeID,
                            InstallmentDate = vd.InstallmentDate,
                            PrincipalLoan = vd.PrincipalLoan,
                            LoanRepaid = vd.LoanRepaid,
                            LoanDue = vd.LoanDue,
                            LoanPaid = vd.LoanPaid,
                            IntPaid = vd.IntPaid,
                            CumIntCharge = vd.CumIntCharge,
                            IntCharge = vd.IntCharge,
                            IntDue = vd.IntDue,
                            Advance = vd.Advance,
                            DueRecovery = vd.DueRecovery,
                            TrxType = vd.TrxType,
                            InstallmentNo = vd.InstallmentNo,
                            EmployeeID = vd.EmployeeID,
                            TotalPaid = vd.TotalPaid,
                            InvestorID = vd.InvestorID,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            DueLoanSummary = vd.DueLoanSummary,
                            LoanCollectionSummary = vd.LoanCollectionSummary,
                            DueInterestSummary = vd.DueInterestSummary,
                            InterestCollectionSummary = vd.InterestCollectionSummary,
                            TotalDueSummary = vd.TotalDueSummary,
                            TotalCollectionSummary = vd.TotalCollectionSummary,
                            Duration = vd.Duration,
                            DurationOverLoanDue = vd.DurationOverLoanDue,
                            DurationOverIntDue = vd.DurationOverIntDue,
                            PhoneNo = vd.PhoneNo,
                            DurationOverCollection = prod.DurationOverCollection,
                            OrgID = vd.OrgID,
                            LoanAccountNo = vd.LoanAccountNo.Substring(4, (vd.LoanAccountNo.Length - 4)),
                            CumLoanDue = vd.CumLoanDue,
                            CumIntDue = vd.CumIntDue
                        };
                        detail.Add(loans);
                        rowSl++;
                    }
                }
                else
                {
                    foreach (var vd in memberModels)
                    {
                        var prod = GetProduct(vd.ProductID);
                        var loans = new DailyLoanTrxViewModel()
                        {
                            rowSl = rowSl,
                            DailyLoanTrxID = vd.DailyLoanTrxID,
                            TrxDate = vd.TrxDate,
                            TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"),
                            LoanSummaryID = vd.LoanSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            InterestCalculationMethod = vd.InterestCalculationMethod,
                            CenterID = vd.CenterID,
                            MemberCategoryID = vd.MemberCategoryID,
                            LoanTerm = vd.LoanTerm,
                            PurposeID = vd.PurposeID,
                            InstallmentDate = vd.InstallmentDate,
                            PrincipalLoan = vd.PrincipalLoan,
                            LoanRepaid = vd.LoanRepaid,
                            LoanDue = vd.LoanDue,
                            LoanPaid = vd.LoanPaid,
                            IntPaid = vd.IntPaid,
                            CumIntCharge = vd.CumIntCharge,
                            IntCharge = vd.IntCharge,
                            IntDue = vd.IntDue,
                            Advance = vd.Advance,
                            DueRecovery = vd.DueRecovery,
                            TrxType = vd.TrxType,
                            InstallmentNo = vd.InstallmentNo,
                            EmployeeID = vd.EmployeeID,
                            TotalPaid = vd.TotalPaid,
                            InvestorID = vd.InvestorID,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            DueLoanSummary = vd.DueLoanSummary,
                            LoanCollectionSummary = vd.LoanCollectionSummary,
                            DueInterestSummary = vd.DueInterestSummary,
                            InterestCollectionSummary = vd.InterestCollectionSummary,
                            TotalDueSummary = vd.TotalDueSummary,
                            TotalCollectionSummary = vd.TotalCollectionSummary,
                            Duration = vd.Duration,
                            DurationOverLoanDue = vd.DurationOverLoanDue,
                            DurationOverIntDue = vd.DurationOverIntDue,
                            PhoneNo = vd.PhoneNo,
                            DurationOverCollection = prod.DurationOverCollection,
                            OrgID = vd.OrgID,
                            LoanAccountNo = vd.LoanAccountNo,
                            MainProductID=vd.MainProductID
                        };
                        detail.Add(loans);
                        rowSl++;
                    }

                }

                return Json(new { Result = "OK", Records = detail });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult GetDailyLoanCollectiongProshikaProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue)
        {
            try
            {
                // long totalCount;

                var collectionList = loanCollectionService.GetDailyProshikaLoanCollectionByCenter(centerId, filterColumn, filterValue, "", "").ToList();
                // var collectionList = loanCollectionService.GetLoanCollectionDetailPaged(centerId, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount).ToList();
                var products = new List<DailyLoanTrxViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.MainProductID == tr.MainProductID && tr.CenterID == centerId).OrderBy(p => p.MainProductCode).FirstOrDefault() == null)
                    {
                        var prodViewModel = Mapper.Map<DailyLoanTrx, DailyLoanTrxViewModel>(tr);
                        // var v = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanDue);
                        prodViewModel.DueLoanSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.MainProductID == tr.MainProductID).Sum(s => s.LoanDue);
                        prodViewModel.LoanCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.MainProductID == tr.MainProductID).Sum(s => s.LoanPaid);
                        prodViewModel.DueInterestSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.MainProductID == tr.MainProductID).Sum(s => s.IntDue);
                        prodViewModel.InterestCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.MainProductID == tr.MainProductID).Sum(s => s.IntPaid);
                        prodViewModel.TotalDueSummary = prodViewModel.DueLoanSummary + prodViewModel.DueInterestSummary;
                        prodViewModel.TotalCollectionSummary = prodViewModel.LoanCollectionSummary + prodViewModel.InterestCollectionSummary;
                        products.Add(prodViewModel);
                    }

                }

                var currentPageProducts = products.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageProducts, TotalRecordCount = products.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private List<DailyLoanTrxViewModel> GetProductList()
        {
            var collectionList = new List<DailyLoanTrxViewModel>();
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 225, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 2, CenterID = 1, MemberID = 10000, ProductID = 333, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 3, CenterID = 1, MemberID = 10002, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 4, CenterID = 1, MemberID = 10003, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 5, CenterID = 1, MemberID = 10003, ProductID = 333, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 6, CenterID = 1, MemberID = 10004, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 7, CenterID = 1, MemberID = 10004, ProductID = 555, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 8, CenterID = 1, MemberID = 10006, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 9, CenterID = 1, MemberID = 10007, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });

            return collectionList.Select(s => new DailyLoanTrxViewModel() { ProductID = s.ProductID }).Distinct().ToList();
        }
        #endregion

        #region Events
        // GET: LoanCollection
        public ActionResult Index()
        {

            var model = new DailyLoanTrxViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Dictionary<string, string> allTrx)
        {
            var trx = allTrx;
            //var formvalues = form;
            //return View();
            var model = new DailyLoanTrxViewModel();


            MapDropDownList(model);

            return View(model);
        }
        public ActionResult BuroIndex()
        {

            var model = new DailyLoanTrxViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
            }

            return View(model);
        }
        public ActionResult ProshikhaIndex()
        {

            var model = new DailyLoanTrxViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ProshikhaIndex(Dictionary<string, string> allTrx)
        {
            var trx = allTrx;
            //var formvalues = form;
            //return View();
            var model = new DailyLoanTrxViewModel();


            MapDropDownList(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult BuroIndex(Dictionary<string, string> allTrx)
        {
            var trx = allTrx;
            var model = new DailyLoanTrxViewModel();
            MapDropDownList(model);
            return View(model);
        }
        // GET: LoanCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: LoanCollection/Create
        public ActionResult Create()
        {

            var model = new DailyLoanTrxViewModel();

            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
            }

            MapDropDownList(model);

            return View(model);
        }
        // POST: LoanCollection/Create
        [HttpPost]
        public ActionResult Create(DailyLoanTrxViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {

                    loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                    var summary = loanCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.CenterID == entity.CenterID && s.ProductID == entity.ProductID && s.LoanTerm == entity.LoanTerm && s.IsActive == true).FirstOrDefault();
                    // var Gsummary = loanCollectionService.GetById(SessionHelper.LoginUserOfficeID.Value);


                    if (summary != null)
                    {

                        var getLoanCol = loanCollectionService.GetByIdLong(Convert.ToInt16(summary.DailyLoanTrxID));
                        getLoanCol.LoanPaid = entity.LoanPaid;
                        getLoanCol.IntPaid = entity.IntPaid;
                        getLoanCol.TotalPaid = entity.TotalPaid;
                        getLoanCol.InstallmentDate = model.TrxDate;
                        getLoanCol.TrxType = 0;
                        getLoanCol.Advance = 0;
                        getLoanCol.CumIntCharge = 0;
                        getLoanCol.DueRecovery = 0;
                        getLoanCol.IntCharge = 0;
                        getLoanCol.IntDue = 0;
                        getLoanCol.LoanDue = 0;
                        getLoanCol.LoanRepaid = 0;
                        getLoanCol.PrincipalLoan = 0;
                        getLoanCol.TotalPaid = 0;
                        getLoanCol.TrxType = 11;
                        var errors = loanCollectionService.IsValidLoan(getLoanCol);

                        if (errors.ToList().Count == 0)
                        {

                            loanCollectionService.Create(getLoanCol);
                            return GetSuccessMessageResult();

                        }
                        else
                            return GetErrorMessageResult();

                    }

                }

                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: LoanCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: LoanCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: LoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: LoanCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, DailyLoanTrxViewModel model)
        {

            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var sp = loanCollectionService.GetById(DailyLoanTrxID);
                var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);
                if (sp.TrxType == 11)
                {
                    loanCollectionService.Delete(DailyLoanTrxID);
                }

                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


        }
        #endregion
    }
}
