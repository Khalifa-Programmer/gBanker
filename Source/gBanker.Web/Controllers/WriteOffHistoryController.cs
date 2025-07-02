using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.ReportExecutionService;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gBanker.Data.CodeFirstMigration;
using System.Data.Entity.Validation;

namespace gBanker.Web.Controllers
{
    public class WriteOffHistoryController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IWriteOffHistoryService WriteOffHistoryService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public WriteOffHistoryController(
            ICenterService centerService,
            IOfficeService officeService,
            IMemberService memberService,
            IWriteOffHistoryService WriteOffHistoryService,
            IUltimateReportService unlimitedReportService,
            IUltimateReportService ultimateReportService,
            IGroupwiseReportService groupwiseReportService,
            ILoanSummaryService loanSummaryService
        )
        {
            this.centerService = centerService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.WriteOffHistoryService = WriteOffHistoryService;
            this.unlimitedReportService = unlimitedReportService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;
        }
        #endregion


        public ActionResult Index()
        {
            var model = new WriteOffHistoryViewModel();
            mapDropdownForWriteOffHistory(model);
            return View(model);
        }

        public void mapDropdownForWriteOffHistory(WriteOffHistoryViewModel model)
        {
            //var CenterID = 973;
            var CenterID = LoginUserOfficeID;
            var pram = new { CenterID = CenterID };
            var centerList = groupwiseReportService.GetActiveCenter(pram, "SP_GET_GetActiveCenter");
            var viewList = centerList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("CenterCode") + " " + row.Field<string>("CenterName"),
                     Value = row.Field<int>("CenterID").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.CenterList = list;
        }

        public JsonResult SaveWriteOffHistory(WriteOffHistoryViewModel WriteOffHistory)
        {
            var result = string.Empty;
            try
            {


                var paramCheck = new
                {
                    IsActive = true,
                    PhoneNo = WriteOffHistory.PhoneNo,
                    WriteOffHistoryID = WriteOffHistory.WriteOffHistoryID
                };
                var isDuplicate = groupwiseReportService.GetWriteOffHistoryCheck(paramCheck, "GetWriteOffHistoryCheck");  
                if (isDuplicate.Tables[0].Rows.Count > 0)
                {
                    result = "Duplicate PhoneNo found, Save denied";
                }
                
                //var isDuplicate =
                //    WriteOffHistoryService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.PhoneNo.ToUpper().Trim() == WriteOffHistory.PhoneNo.ToUpper().Trim())
                //        .ToList();
                //if (isDuplicate.Any())
                //{
                //    result = "Duplicate PhoneNo Name found, Save denied";
                //}
                else
                {
                    //var entity = new WriteOffHistory();
                    //entity.OldMemberName = WriteOffHistory.OldMemberName;
                    //entity.OfficeID = WriteOffHistory.OfficeID;
                    //entity.CenterID = WriteOffHistory.CenterID;
                    //entity.FatherName = WriteOffHistory.FatherName;
                    //entity.MotherName = WriteOffHistory.MotherName;
                    //entity.SpouseName = WriteOffHistory.SpouseName;
                    //entity.PhoneNo = WriteOffHistory.PhoneNo;
                    //entity.NationalID = WriteOffHistory.NationalID;
                    //entity.Address = WriteOffHistory.Address;
                    //entity.DisburseDate = WriteOffHistory.DisburseDate;
                    //entity.DisburseAmount = WriteOffHistory.DisburseAmount;
                    //entity.WriteOffDate = WriteOffHistory.WriteOffDate;
                    //entity.WriteOffAmount = WriteOffHistory.WriteOffAmount;
                    //entity.WriteOffReceovery = WriteOffHistory.WriteOffReceovery;
                    //entity.OpeningDate = DateTime.Now;
                    //entity.IsActive = true;
                    //entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    //entity.CreateDate = DateTime.UtcNow;
                    //WriteOffHistoryService.Create(entity);

                    var mem = memberService.CheckMemberNationalId(WriteOffHistory.NationalID);
                    if (mem.ToList().Count > 0)
                    {
                        return GetErrorMessageResult("NationalID Already Exists");
                    }

                    var memPhone = memberService.CheckMemberPhoneNo(WriteOffHistory.PhoneNo);
                    if (memPhone.ToList().Count > 0)
                    {
                        return GetErrorMessageResult("PhoneNo Already Exists");
                    }

                    var param1 = new { @OfficeID = LoginUserOfficeID };
                    var LoanInstallMent = ultimateReportService.GenerateMemberLastCodeWriteOff(param1);
                    var MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();


                    var paramInsert = new
                    {
                        WriteOffHistoryID = WriteOffHistory.WriteOffHistoryID,
                        OldMemberName = WriteOffHistory.OldMemberName,
                        OldMemberCodeOld = WriteOffHistory.OldMemberCodeOld,
                        OfficeID = LoginUserOfficeID,
                        CenterID = WriteOffHistory.CenterID,
                        FatherName = WriteOffHistory.FatherName,
                        MotherName = WriteOffHistory.MotherName,
                        SpouseName = WriteOffHistory.SpouseName,
                        PhoneNo = WriteOffHistory.PhoneNo,
                        NationalID = WriteOffHistory.NationalID,
                        Address = WriteOffHistory.Address,
                        DisburseDate = WriteOffHistory.DisburseDate,
                        DisburseAmount = WriteOffHistory.DisburseAmount,
                        WriteOffDate = WriteOffHistory.WriteOffDate,
                        WriteOffAmount = WriteOffHistory.WriteOffAmount,
                        WriteOffReceovery = WriteOffHistory.WriteOffReceovery,
                        OpeningDate = WriteOffHistory.OpeningDate,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserEmployeeID,
                        CreateDate = DateTime.Now,
                        MemberCode = MemberCode
                    };
                    

                    var targetAchievementInsert = groupwiseReportService.GetWriteOffHistoryInsert(paramInsert, "GetWriteOffHistoryInsert"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";

                    var ent = new { MemberID = WriteOffHistory.MemberID, MemberCode = MemberCode, result = result };
                    return Json(new { data = ent }, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            //return Json(result, JsonRequestBehavior.AllowGet);
            var ente = new {result = result };
            return Json(new { data = ente }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateWriteOffHistory(WriteOffHistoryViewModel WriteOffHistorydd)
        {
            var result = string.Empty;
            try
            {


                var paramCheck = new
                {
                    IsActive = true,
                    PhoneNo = WriteOffHistorydd.PhoneNo,
                    WriteOffHistoryID = WriteOffHistorydd.WriteOffHistoryID
                };
                var isDuplicate = groupwiseReportService.GetWriteOffHistoryCheck(paramCheck, "GetWriteOffHistoryCheck");
                if (isDuplicate.Tables[0].Rows.Count > 0)
                {
                    result = "Duplicate PhoneNo found, Update denied";
                }
                
                //var isDuplicate =
                //   WriteOffHistoryService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.WriteOffHistoryID != WriteOffHistory.WriteOffHistoryID &&
                //               p.PhoneNo.ToUpper().Trim() == WriteOffHistory.PhoneNo.ToUpper().Trim()).ToList();
                //if (isDuplicate.Any())
                //{
                //    result = "Duplicate PhoneNo found, Update denied";
                //}
                else
                {
                    var entity = new gBankerDbContext().Database.SqlQuery<WriteOffHistoryViewModel>("SELECT WriteOffHistoryID,OldMemberCode FROM WriteOffHistory WHERE WriteOffHistoryID=" + WriteOffHistorydd.WriteOffHistoryID).FirstOrDefault();
                    //var entity1 = WriteOffHistoryService.GetById(Convert.ToInt16(WriteOffHistorydd.WriteOffHistoryID));
                    //entity.WriteOffHistoryID = WriteOffHistory.WriteOffHistoryID;
                    //entity.OldMemberName = WriteOffHistory.OldMemberName;
                    //entity.OfficeID = WriteOffHistory.OfficeID;
                    //entity.CenterID = WriteOffHistory.CenterID;
                    //entity.FatherName = WriteOffHistory.FatherName;
                    //entity.MotherName = WriteOffHistory.MotherName;
                    //entity.SpouseName = WriteOffHistory.SpouseName;
                    //entity.PhoneNo = WriteOffHistory.PhoneNo;
                    //entity.NationalID = WriteOffHistory.NationalID;
                    //entity.Address = WriteOffHistory.Address;
                    //entity.DisburseDate = WriteOffHistory.DisburseDate;
                    //entity.DisburseAmount = WriteOffHistory.DisburseAmount;
                    //entity.WriteOffDate = WriteOffHistory.WriteOffDate;
                    //entity.WriteOffAmount = WriteOffHistory.WriteOffAmount;
                    //entity.WriteOffReceovery = WriteOffHistory.WriteOffReceovery;
                    //entity.OpeningDate = DateTime.Now;
                    //entity.IsActive = true;
                    //entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    //entity.CreateDate = DateTime.UtcNow;
                    //WriteOffHistoryService.Update(entity);


                    var paramInsert = new
                    {
                        WriteOffHistoryID = WriteOffHistorydd.WriteOffHistoryID,
                        OldMemberName = WriteOffHistorydd.OldMemberName,
                        OldMemberCodeOld = WriteOffHistorydd.OldMemberCodeOld,
                        OfficeID = SessionHelper.LoginUserOfficeID,
                        CenterID = WriteOffHistorydd.CenterID,
                        FatherName = WriteOffHistorydd.FatherName,
                        MotherName = WriteOffHistorydd.MotherName,
                        SpouseName = WriteOffHistorydd.SpouseName,
                        PhoneNo = WriteOffHistorydd.PhoneNo,
                        NationalID = WriteOffHistorydd.NationalID,
                        Address = WriteOffHistorydd.Address,
                        DisburseDate = WriteOffHistorydd.DisburseDate,
                        DisburseAmount = WriteOffHistorydd.DisburseAmount,
                        WriteOffDate = WriteOffHistorydd.WriteOffDate,
                        WriteOffAmount = WriteOffHistorydd.WriteOffAmount,
                        WriteOffReceovery = WriteOffHistorydd.WriteOffReceovery,
                        OpeningDate = WriteOffHistorydd.OpeningDate,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserEmployeeID,
                        CreateDate = DateTime.Now,
                        MemberCode = entity.OldMemberCode
                    };
                    var targetAchievementInsert = groupwiseReportService.GetWriteOffHistoryInsert(paramInsert, "GetWriteOffHistoryInsert"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";

                    var ent = new { result = result };
                    return Json(new { data = ent }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                //return GetErrorMessageResult(ex);
            }
            //catch (Exception ex)
            //{

            //    result = ex.InnerException.Message.ToString();
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            var ente = new { result = result };
            return Json(new { data = ente }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListWriteOffHistory([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string OpeningDate)
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive , OpeningDate = OpeningDate, OfficeId = LoginUserOfficeID };
                var officewiseDivisionList = groupwiseReportService.GetWriteOffHistory(pram, "SP_GET_GetWriteOffHistory"); //SP_GET_GetActiveCenter
                var officewiseDivisionListViewModel = officewiseDivisionList.Tables[0].AsEnumerable()
                   .Select(row => new WriteOffHistoryViewModel
                   {
                       WriteOffHistoryID = row.Field<long>("WriteOffHistoryID"),
                       OldMemberName = row.Field<string>("OldMemberName"),
                       OldMemberCode = row.Field<string>("OldMemberCode"),
                       OldMemberCodeOld = row.Field<string>("OldMemberCodeOld"),
                       OfficeID = row.Field<int?>("OfficeID"),
                       CenterID = row.Field<int>("CenterID"),
                       Address = row.Field<string>("Address"),
                       FatherName = row.Field<string>("FatherName"),
                       SpouseName = row.Field<string>("SpouseName"),
                       MotherName = row.Field<string>("MotherName"),
                       PhoneNo = row.Field<string>("PhoneNo"),
                       DisburseAmount = row.Field<decimal?>("DisburseAmount"),
                       WriteOffAmount = row.Field<decimal?>("WriteOffAmount"),
                       WriteOffReceovery = row.Field<decimal?>("WriteOffReceovery"),
                       DisburseDate = row.Field<DateTime>("DisburseDate"),
                       WriteOffDate = row.Field<DateTime>("WriteOffDate"),
                       NationalID = row.Field<string>("NationalID"),
                       WD = row.Field<string>("WD"),
                       DD = row.Field<string>("DD"),
                       OD = row.Field<string>("OD"),
                       IsActive = row.Field<bool?>("IsActive"),
                       CenterCode = row.Field<string>("CenterCode"),
                       CenterName = row.Field<string>("CenterName"),
                       centercodename = row.Field<string>("centercodename"),
                       WriteOffBalance = row.Field<decimal>("WriteOffBalance")
                       
                   }).ToList();

                DataSourceResult result = officewiseDivisionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteWriteOffHistory(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var param = new
                {
                    Id = Id
                };

                var targetAchievementInsert = groupwiseReportService.GetWriteOffHistoryDelete(param, "GetWriteOffHistoryDelete");
                result = 1;
                message = "Deleted Successfully";
                


                //var model = WriteOffHistoryService.GetById(Id);
                //var DD = Convert.ToDateTime(model.WriteOffDate);
                //var PL = Convert.ToDecimal(model.WriteOffAmount);
                //model.IsActive = false;
                //model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                //model.CreateDate = DateTime.UtcNow;
                //WriteOffHistoryService.Update(model);
                //var OldMemberCode = model.OldMemberCode;

                //var membermodel = memberService.GetByMemeberCode(OldMemberCode);
                //var memberID = membermodel.MemberID;
                //membermodel.IsActive = false;
                //membermodel.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                //membermodel.CreateDate = DateTime.UtcNow;
                //memberService.Update(membermodel);

                //long loansummaryid = loanSummaryService.GetByWriteoffinfowithmemberinfo(memberID,DD,PL).LoanSummaryID;
                //if (loansummaryid != 0)
                //{
                //    var loansummarymodel = loanSummaryService.GetByIdLong(loansummaryid);
                //    loansummarymodel.IsActive = false;
                //    loanSummaryService.Update(loansummarymodel);
                //}

                //result = 1;
                //message = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }



        //public ActionResult GetMemberList(string memberid)
        //{
        //    var MemberID = memberid;
        //    var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", LoginUserOfficeID);
        //    var memberList = new List<Member>();
        //    var mbr = new gBankerDbContext().Database.SqlQuery<Member>("select *  FROM Member WHERE OfficeID=" + LoginUserOfficeID + "and MemberStatus!=5 and MemberCode LIKE" + "'" + MemberID + "%" + "'").ToList();
        //    //var mbr = memberService.GetByOfficeIdAll(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
        //    //    Session[MemberByCenterSessionKey] = mbr;
        //    //    memberList = mbr;
        //    var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

        //    return Json(members, JsonRequestBehavior.AllowGet);
        //}


        ////public ActionResult GetMemberList(string memberid, string centerId)
        ////{
        ////    var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
        ////    var memberList = new List<Member>();
        ////    if (Session[MemberByCenterSessionKey] != null)
        ////        memberList = Session[MemberByCenterSessionKey] as List<Member>;
        ////    else
        ////    {
        ////        var mbr = memberService.GetByCenterIdAll(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
        ////        Session[MemberByCenterSessionKey] = mbr;
        ////        memberList = mbr;
        ////    }
        ////    var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

        ////    return Json(members, JsonRequestBehavior.AllowGet);
        ////}


        public ActionResult GetMemberList(string memberid)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", LoginUserOfficeID);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByOfficeAll(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WriteOffOldMemberCodeSearch(string memberid)
        {
            var param = new {memberid = memberid, OfficeID = LoginUserOfficeID };
            var result = groupwiseReportService.writeOffOldMemberCode(param, "WriteOffOldMemberCodeSearch");  //employeeSPService.GetDataWithParameter(param, "dbo.getcollection_Loansavings");
            int OfficeID;
            int CenterID;
            int MemberID;
            string MotherName;
            string FatherName;
            string MemberName;
            string MemberCode;
            string SpouseName;
            string PhoneNo;
            string NationalID;
            string Address;
            string DisburseDate;
            int DisburseAmount;
            string WriteOffDate;
            int WriteOffAmount;
            int WriteOffReceovery;
            if (result.Tables[0].Rows.Count > 0)
            {
                //resul = Convert.ToInt16(result.Tables[0].Rows[0]["OldMemberCode"].ToString());
                MemberCode = result.Tables[0].Rows[0]["OldMemberCodeOld"].ToString();
                OfficeID = Convert.ToInt16(result.Tables[0].Rows[0]["OfficeID"].ToString());
                CenterID = Convert.ToInt16(result.Tables[0].Rows[0]["CenterID"].ToString());
                FatherName = result.Tables[0].Rows[0]["FatherName"].ToString();
                SpouseName = result.Tables[0].Rows[0]["SpouseName"].ToString();
                MemberName = result.Tables[0].Rows[0]["OldMemberName"].ToString();
                PhoneNo = result.Tables[0].Rows[0]["PhoneNo"].ToString();
                NationalID = result.Tables[0].Rows[0]["NationalID"].ToString();
                Address = result.Tables[0].Rows[0]["Address"].ToString();
                DisburseDate = result.Tables[0].Rows[0]["DisburseDate"].ToString();
                DisburseAmount = Convert.ToInt16(result.Tables[0].Rows[0]["DisburseAmount"].ToString());
                WriteOffDate = result.Tables[0].Rows[0]["WriteOffDate"].ToString();
                WriteOffAmount = Convert.ToInt16(result.Tables[0].Rows[0]["WriteOffAmount"].ToString());
                WriteOffReceovery = Convert.ToInt16(result.Tables[0].Rows[0]["WriteOffReceovery"].ToString());
                MemberID = Convert.ToInt32(result.Tables[0].Rows[0]["MemberID"].ToString());
                MotherName = result.Tables[0].Rows[0]["MotherName"].ToString();
            }
            else
            {
                OfficeID = 0;
                CenterID = 0;
                MemberID = 0;
                MotherName = "";
                FatherName = "";
                MemberName = "";
                MemberCode = "";
                SpouseName = "";
                PhoneNo = "";
                NationalID = "";
                Address = "";
                DisburseDate = "";
                DisburseAmount = 0;
                WriteOffDate = "";
                WriteOffAmount = 0;
                WriteOffReceovery = 0;
            }



            return Json(new {
                OfficeID = OfficeID,
                CenterID = CenterID,
                MemberID = MemberID,
                MotherName = MotherName,
                FatherName = FatherName,
                MemberName = MemberName,
                MemberCode = MemberCode,
                SpouseName = SpouseName,
                PhoneNo = PhoneNo,
                NationalID = NationalID,
                Address = Address,
                DisburseDate = DisburseDate,
                DisburseAmount = DisburseAmount,
                WriteOffDate = WriteOffDate,
                WriteOffAmount = WriteOffAmount,
                WriteOffReceovery = WriteOffReceovery
            }, JsonRequestBehavior.AllowGet);
            //return Json(OfficeID, CenterID, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LedgerPostWriteOff(SavingSummaryViewModel model)
        {
            var members = "Success";
            var val = WriteOffHistoryService.SetOpeningSavingEntry(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value);
            return Json(members, JsonRequestBehavior.AllowGet);

        }



    }
}
