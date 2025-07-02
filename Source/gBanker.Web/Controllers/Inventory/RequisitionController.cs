using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace gBanker.Web.Controllers.Inventory
{
    public class RequisitionController : BaseController
    {
        #region Variables
        private readonly IInvStoreService iInvStoreService;
        private readonly IOfficeService iOfficeService;
        private readonly IInv_RequsitionMasterService iInv_RequsitionMasterService;
        private readonly IInv_RequsitionDetailsService iInv_RequsitionDetailsService;
        private readonly IInv_RequisitionConsulateMasterService iInv_RequsitionConsulateMasterService;
        private readonly IInv_RequisitionConsulateDetailsService iInv_RequsitionConsulateDetailsService;
        //private readonly IAccTrxMasterService iAccTrxMasterService;
        //private readonly IAccTrxDetailService iAccTrxDetailService;
        private readonly IInvTrxMasterService iInvTrxMasterService;
        private readonly IInvTrxDetailService iInvTrxDetailService;
        private readonly IInv_TempStoreService iInvTempStoreService;

        public RequisitionController
            (IInvStoreService iInvStoreService
            , IOfficeService iOfficeService
            , IInv_RequsitionMasterService iInv_RequsitionMasterService
            , IInv_RequsitionDetailsService iInv_RequsitionDetailsService
            , IInv_RequisitionConsulateMasterService iInv_RequsitionConsulateMasterService
            , IInv_RequisitionConsulateDetailsService iInv_RequsitionConsulateDetailsService
            , IInvTrxMasterService iInvTrxMasterService
            , IInvTrxDetailService iInvTrxDetailService
            , IInv_TempStoreService iInvTempStoreService
            )
        {
            this.iInvStoreService = iInvStoreService;
            this.iOfficeService = iOfficeService;
            this.iInv_RequsitionMasterService = iInv_RequsitionMasterService;
            this.iInv_RequsitionDetailsService = iInv_RequsitionDetailsService;
            this.iInv_RequsitionConsulateMasterService = iInv_RequsitionConsulateMasterService;
            this.iInv_RequsitionConsulateDetailsService = iInv_RequsitionConsulateDetailsService;
            this.iInvTrxMasterService = iInvTrxMasterService;
            this.iInvTrxDetailService = iInvTrxDetailService;
            this.iInvTempStoreService = iInvTempStoreService;
        }
        #endregion Variables
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }
        #region Requisition Process
        [HttpGet]
        public ActionResult Create()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult RequisitionDetails()
        {
            if (LoginUserOfficeID.HasValue)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Approve()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ApproveedList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        [HttpGet]
        public ActionResult ConsulateRequisitionDetails()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                return View();
            }

            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult ConsulateRequisitionApprove()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }

            else
                return RedirectToAction("Index", "Home");
        }
        public ActionResult ConsulateRequisitionForArea()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public ActionResult ConsulateRequisitionForZone()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        #endregion Requisition Process

        #region Web Service
        public JsonResult GetRequsitionNo()
        {
            if (LoginUserOfficeID.HasValue)
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var req = db.Database.SqlQuery<string>("exec spRequsitionNo {0}", (LoginUserOfficeID ?? 0)).ToList();
                    return Json(new { status = 1, Result = req.First() }, JsonRequestBehavior.AllowGet);
                }
            }
            else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllRequsition(string reqType, bool isSelfStore, int jtStartIndex, int jtPageSize, string jtSorting = null)
        {
            int totalCount = 0;
            var currentPageRecords = Mapper.Map<IEnumerable<Inv_RequsitionMasterViewModel>, IEnumerable<Inv_RequsitionMasterViewModel>>(new List<Inv_RequsitionMasterViewModel>());
            try
            {
                SqlParameter[] arr = new SqlParameter[] {
                            new SqlParameter("@reqType", reqType),
                            new SqlParameter("@fromStoreID", (LoginUserOfficeID ?? 0)),
                            new SqlParameter("@isSelfStore", isSelfStore)
                };

                List<Inv_RequsitionMasterViewModel> objList = new gBankerDbContext().Database.SqlQuery<Inv_RequsitionMasterViewModel>("sp_RequsitionHistory @reqType,@fromStoreID,@isSelfStore", arr).ToList();
                totalCount = objList.Count();
                if (objList.Any())
                    objList = objList.Skip(jtStartIndex).Take(jtPageSize).ToList();
                currentPageRecords = Mapper.Map<IEnumerable<Inv_RequsitionMasterViewModel>, IEnumerable<Inv_RequsitionMasterViewModel>>(objList);
            }
            catch (Exception ex)
            {
                currentPageRecords = Mapper.Map<IEnumerable<Inv_RequsitionMasterViewModel>, IEnumerable<Inv_RequsitionMasterViewModel>>(new List<Inv_RequsitionMasterViewModel>());
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
        }
        public JsonResult GetAllRequsitionDetails(int requsitionMasterID, string status)
        {
            int totalCount = 0;
            #region Analysis
            if (status == "an")
            {
                var currentPageRecords = Mapper.Map<IEnumerable<RequsitionAnalysisViewModel>, IEnumerable<RequsitionAnalysisViewModel>>(new List<RequsitionAnalysisViewModel>());
                try
                {
                    var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                    List<RequsitionAnalysisViewModel> objList = new gBankerDbContext().Database.SqlQuery<RequsitionAnalysisViewModel>("sp_Inv_RequsitionAnalysis " + requsitionMasterID + "," + off.OfficeLevel + "").ToList();
                    totalCount = objList.Count();

                    currentPageRecords = Mapper.Map<IEnumerable<RequsitionAnalysisViewModel>, IEnumerable<RequsitionAnalysisViewModel>>(objList);
                }
                catch (Exception ex)
                {
                    currentPageRecords = Mapper.Map<IEnumerable<RequsitionAnalysisViewModel>, IEnumerable<RequsitionAnalysisViewModel>>(new List<RequsitionAnalysisViewModel>());
                }
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            #endregion Analysis
            #region Others
            else
            {
                var currentPageRecords = Mapper.Map<IEnumerable<RequsitionDetailViewmodel>, IEnumerable<RequsitionDetailViewmodel>>(new List<RequsitionDetailViewmodel>());
                try
                {

                    SqlParameter[] arr = new SqlParameter[] {
                            new SqlParameter("@reqMasterID", requsitionMasterID),
                            new SqlParameter("@dStatus", status)};

                    List<RequsitionDetailViewmodel> objList = new gBankerDbContext().Database.SqlQuery<RequsitionDetailViewmodel>("sp_RequsitionDetails @reqMasterID,@dStatus", arr).ToList();
                    totalCount = objList.Count();

                    currentPageRecords = Mapper.Map<IEnumerable<RequsitionDetailViewmodel>, IEnumerable<RequsitionDetailViewmodel>>(objList);
                }
                catch (Exception ex)
                {
                    currentPageRecords = Mapper.Map<IEnumerable<RequsitionDetailViewmodel>, IEnumerable<RequsitionDetailViewmodel>>(new List<RequsitionDetailViewmodel>());
                }
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            #endregion Others
        }

        public JsonResult GetRequisitionWiseTotalQtyInItem(int? storeID)
        {
            int totalCount = 0;
            var currentPageRecords = Mapper.Map<IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>, IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>>(new List<Inv_RequisitionWiseTotalQtyInItemViewModel>());
            if (LoginUserOfficeID.HasValue)
            {
                try
                {
                    var office = iOfficeService.GetById(LoginUserOfficeID.Value);
                    if (office.OfficeLevel != 4)
                    {
                        if (!storeID.HasValue)
                        {
                            List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                            storeID = wObj.First().WarehouseID;
                        }
                        List<Inv_RequisitionWiseTotalQtyInItemViewModel> objList = new gBankerDbContext().Database.SqlQuery<Inv_RequisitionWiseTotalQtyInItemViewModel>("Inv_sp_RequisitionWiseTotalQtyInItem " + storeID + "," + office.OfficeLevel + "").ToList();
                        totalCount = objList.Count();
                        currentPageRecords = Mapper.Map<IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>, IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>>(objList);
                    }
                }
                catch (Exception ex)
                {
                    currentPageRecords = Mapper.Map<IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>, IEnumerable<Inv_RequisitionWiseTotalQtyInItemViewModel>>(new List<Inv_RequisitionWiseTotalQtyInItemViewModel>());
                }
            }

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
        }
        public JsonResult GetConsulateRequsitionAfterApproved()
        {
            int totalCount = 0;
            var currentPageRecords = Mapper.Map<IEnumerable<ConsulateRequsitionAfterApprovedViewModel>, IEnumerable<ConsulateRequsitionAfterApprovedViewModel>>(new List<ConsulateRequsitionAfterApprovedViewModel>());
            if (LoginUserOfficeID.HasValue)
            {
                try
                {
                    var office = iOfficeService.GetById(LoginUserOfficeID.Value);
                    if (office.OfficeLevel != 4)
                    {
                        List<ConsulateRequsitionAfterApprovedViewModel> objList = new gBankerDbContext().Database.SqlQuery<ConsulateRequsitionAfterApprovedViewModel>("sp_Inv_ConsulateRequsitionAfterApproved " + LoginUserOfficeID.Value + "").ToList();
                        totalCount = objList.Count();
                        currentPageRecords = Mapper.Map<IEnumerable<ConsulateRequsitionAfterApprovedViewModel>, IEnumerable<ConsulateRequsitionAfterApprovedViewModel>>(objList);
                    }
                }
                catch (Exception ex)
                {
                    currentPageRecords = Mapper.Map<IEnumerable<ConsulateRequsitionAfterApprovedViewModel>, IEnumerable<ConsulateRequsitionAfterApprovedViewModel>>(new List<ConsulateRequsitionAfterApprovedViewModel>());
                }
            }

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
        }
        public JsonResult CreateRequsition(Inv_RequsitionMaster mObj, List<Inv_RequsitionDetails> dObj)
        {
            string result = "", msg = "";
            if (LoggedInEmployeeID.HasValue && LoginUserOfficeID.HasValue)
            {
                if (dObj != null)
                {
                    if (dObj.Any())
                    {
                        mObj.CreatedBy = LoginUserOfficeID;
                        mObj.CreatedDate = DateTime.Now;
                        //mObj.FromStoreID = (LoginUserOfficeID ?? 0);
                        mObj.IsActive = true;
                        iInv_RequsitionMasterService.Create(mObj);
                        foreach (var v in dObj)
                        {
                            v.AprovedStatus = "p";
                            v.CreatedBy = LoginUserOfficeID;
                            v.CreatedDate = mObj.CreatedDate;
                            v.RequsitionMasterID = mObj.RequsitionID;
                            iInv_RequsitionDetailsService.Create(v);
                        }
                        result = "Success";
                        msg = "Requsition create successfully";
                    }
                    else
                    {
                        result = "Fail";
                        msg = "Requsition Fail";
                    }
                }
                else
                {
                    result = "Fail";
                    msg = "Requsition fail";
                }
            }
            else
            {
                result = "Login Fail";
                msg = "Please re-login";
            }

            return Json(new { Result = result, message = msg, RmID = mObj.RequsitionID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateConsulateRequsitionForArea(Inv_RequisitionConsulateMaster mObj, List<RequisitionConsulateDetailsViewModel> dObj)
        {
            string result = "", msg = "";
            if (LoggedInEmployeeID.HasValue && LoginUserOfficeID.HasValue)
            {
                if (dObj != null)
                {
                    if (dObj.Any())
                    {
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                mObj.SenderBy = LoginUserOfficeID.Value;
                                iInv_RequsitionConsulateMasterService.Create(mObj);
                                if (mObj.ConsulateRequisitionID > 0)
                                {
                                    foreach (var d in dObj)
                                    {
                                        var detailsIDArr = d.DetailsIDs.Split(',');
                                        int itemID = 0;
                                        foreach (string ra in detailsIDArr)
                                        {
                                            var Requisitiondetails = iInv_RequsitionDetailsService.GetById(int.Parse(ra));
                                            itemID = Requisitiondetails.ItemID;
                                            Requisitiondetails.ConsulateRequisitionID = mObj.ConsulateRequisitionID;
                                            Requisitiondetails.SendingQty = Requisitiondetails.SendingQty > 0 ? Requisitiondetails.SendingQty : Requisitiondetails.RequestQty;
                                            iInv_RequsitionDetailsService.Update(Requisitiondetails);
                                        }
                                        //var md = Mapper.Map<RequisitionConsulateDetailsViewModel, Inv_RequisitionConsulateDetails>(d);
                                        var md = new Inv_RequisitionConsulateDetails()
                                        {
                                            ConsulateRequisitionMasterID = mObj.ConsulateRequisitionID,
                                            Qty = d.SendingQty,
                                            ItemID = itemID,
                                            AprovedStatus = "Pending"
                                        };
                                        iInv_RequsitionConsulateDetailsService.Create(md);
                                    }
                                    transaction.Complete();
                                    result = "Success";
                                    msg = "Requsition Sending successfully";
                                }
                                transaction.Dispose();
                            }
                            catch (Exception ex)
                            {
                                transaction.Dispose();
                                result = "Fail";
                                msg = "Requsition Sending Fail";
                            }
                        }
                    }
                    else
                    {
                        result = "Fail";
                        msg = "Requsition Sending Fail";
                    }
                }
                else
                {
                    result = "Fail";
                    msg = "Requsition fail";
                }
            }
            else
            {
                result = "Login Fail";
                msg = "Please re-login";
            }

            return Json(new { Result = result, message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateConsulateRequsitionForZone(Inv_RequisitionConsulateMaster mObj, List<RequisitionConsulateDetailsViewModel> dObj)
        {
            string result = "", msg = "";
            if (LoggedInEmployeeID.HasValue && LoginUserOfficeID.HasValue)
            {
                if (dObj != null)
                {
                    if (dObj.Any())
                    {
                        using (var transaction = new TransactionScope())
                        {
                            try
                            {
                                mObj.SenderBy = LoginUserOfficeID.Value;
                                iInv_RequsitionConsulateMasterService.Create(mObj);
                                if (mObj.ConsulateRequisitionID > 0)
                                {
                                    int itemID = 0;
                                    if (new gBankerDbContext().inv_Settings.Where(x => x.Purpose == "Distribution"
                                    && x.IsActive == true && x.PurposeKey == "Zone" && x.PurposeValue == 1).Any())
                                    {
                                        foreach (var d in dObj)
                                        {
                                            itemID = 0;
                                            var detailsIDArr = d.DetailsIDs.Split(',');
                                            foreach (string ra in detailsIDArr)
                                            {
                                                var Requisitiondetails = iInv_RequsitionDetailsService.GetById(int.Parse(ra));
                                                itemID = Requisitiondetails.ItemID;
                                                Requisitiondetails.ConsulateRequisitionID = mObj.ConsulateRequisitionID;
                                                Requisitiondetails.SendingQty = Requisitiondetails.SendingQty > 0 ? Requisitiondetails.SendingQty : Requisitiondetails.RequestQty;
                                                iInv_RequsitionDetailsService.Update(Requisitiondetails);
                                            }
                                            var md = new Inv_RequisitionConsulateDetails()
                                            {
                                                ConsulateRequisitionMasterID = mObj.ConsulateRequisitionID,
                                                Qty = d.SendingQty,
                                                ItemID = itemID,
                                                AprovedStatus = "Pending"
                                            };
                                            iInv_RequsitionConsulateDetailsService.Create(md);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var d in dObj)
                                        {
                                            var detailsIDArr = d.DetailsIDs.Split(',');
                                            itemID = 0;
                                            foreach (string ra in detailsIDArr)
                                            {
                                                var Requisitiondetails = iInv_RequsitionConsulateDetailsService.GetById(int.Parse(ra));
                                                itemID = Requisitiondetails.ItemID;
                                                Requisitiondetails.ReConsulateRequisitionID = mObj.ConsulateRequisitionID;
                                                iInv_RequsitionConsulateDetailsService.Update(Requisitiondetails);
                                            }
                                            var md = new Inv_RequisitionConsulateDetails()
                                            {
                                                ConsulateRequisitionMasterID = mObj.ConsulateRequisitionID,
                                                Qty = d.SendingQty,
                                                ItemID = itemID,
                                                AprovedStatus = "Pending"
                                            };
                                            iInv_RequsitionConsulateDetailsService.Create(md);
                                        }
                                    }

                                    //var md = Mapper.Map<RequisitionConsulateDetailsViewModel, Inv_RequisitionConsulateDetails>(d);


                                    transaction.Complete();
                                    result = "Success";
                                    msg = "Requsition Sending successfully";
                                }
                                transaction.Dispose();
                            }
                            catch (Exception ex)
                            {
                                transaction.Dispose();
                                result = "Fail";
                                msg = "Requsition Sending Fail";
                            }
                        }
                    }
                    else
                    {
                        result = "Fail";
                        msg = "Requsition Sending Fail";
                    }
                }
                else
                {
                    result = "Fail";
                    msg = "Requsition fail";
                }
            }
            else
            {
                result = "Login Fail";
                msg = "Please re-login";
            }

            return Json(new { Result = result, message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRequsitionMaster(int ids, string status)
        {
            int result = 0; string msg = ""; long vID = 0; ;
            if (LoginUserOfficeID.HasValue)
            {
                #region s c
                if (status == "s" || status == "c")
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        int s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionMaster SET RequsitionStatus='" + status /*(status == "p" ? "s" : "c")*/ + "'" +
                            " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionID=" + ids + " AND RequsitionStatus='p'");
                        if (s > 0)
                        {
                            s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionDetails SET AprovedStatus='" + status /*(status == "p" ? "s" : "c")*/ + "'" +
                            " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='p'");
                            result = 1; msg = "Requsition " + (status == "s" ? "posted" : status == "c" ? "canceled" : "");
                        }
                        else
                        {
                            result = 0; msg = "Fail to Update";
                        }

                    }
                }
                #endregion s c
                #region r
                else if (status == "r")
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        int s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionMaster SET RequsitionStatus='" + status + "'" +
                            " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionID=" + ids + " AND RequsitionStatus='s'");
                        if (s > 0)
                        {
                            s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionDetails SET AprovedStatus='" + status + "'" +
                            " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='s'");
                            result = 1; msg = "Requsition Rerjected";
                        }
                        else
                        {
                            result = 0; msg = "Fail to Update";
                        }
                    }
                }
                #endregion r
                #region a 
                else if (status == "a")
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        List<Inv_RequsitionDetails> detailsList = db.Database.SqlQuery<Inv_RequsitionDetails>("SELECT * FROM Inv_RequsitionDetails WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='s'").ToList();
                        Inv_RequsitionMaster m = db.Database.SqlQuery<Inv_RequsitionMaster>("SELECT * FROM Inv_RequsitionMaster WHERE RequsitionID=" + ids + " AND RequsitionStatus='s'").First();
                        if (detailsList.Any() && m != null)
                        {
                            List<int> itemList = detailsList.Select(x => x.ItemID).ToList();
                            var sqty = iInvStoreService.GetMany(x => x.WarehouseID == m.ToStoreID && x.StockType == "I" && itemList.Contains(x.ItemID) && x.StockBalance > 0).OrderBy(x => x.ID);
                            if (sqty.Any())
                            {
                                var itmXBalance = (from t in sqty
                                                   group t by new { t.ItemID }
                                     into grp
                                                   select new
                                                   {
                                                       grp.Key.ItemID,
                                                       StockBalance = grp.Sum(t => t.StockBalance)
                                                   }
                                     ).ToList();
                                foreach (var b in itmXBalance)
                                {
                                    if (detailsList.Where(x => x.ItemID == b.ItemID).Any())
                                    {
                                        //if(detailsList.Where(x => x.ItemID == b.ItemID).Sum(x => x.Qty) > b.StockBalance)
                                        if (detailsList.Where(x => x.ItemID == b.ItemID).Sum(x => (x.ApprovedQty > 0 ? x.ApprovedQty : x.RequestQty)) > b.StockBalance)
                                            return Json(new { Result = 0, message = "Stock not sufficient" }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                        return Json(new { Result = 0, message = "Stock not sufficient" }, JsonRequestBehavior.AllowGet);
                                }
                                foreach (var b in detailsList)
                                {
                                    var invStoreListXItem = sqty.Where(x => x.ItemID == b.ItemID);
                                    int qty = (b.ApprovedQty > 0 ? b.ApprovedQty : b.RequestQty)/* b.Qty*/;
                                    foreach (var i in invStoreListXItem)
                                    {
                                        if (qty > 0)
                                        {
                                            if (i.StockBalance >= qty)
                                            {
                                                i.StockBalance = i.StockBalance - qty;
                                                qty = 0;
                                                iInvStoreService.Update(i);
                                            }
                                            else
                                            {
                                                qty = qty - i.StockBalance;
                                                i.StockBalance = 0;
                                                iInvStoreService.Update(i);
                                            }
                                        }
                                        else
                                            break;
                                    }
                                    Inv_Store ist = new Inv_Store()
                                    {
                                        CreateBy = LoginUserOfficeID,
                                        CreateDate = DateTime.UtcNow,
                                        ID = 0,
                                        IsActive = true,
                                        ItemID = b.ItemID,
                                        Qty = b.RequestQty,
                                        Remarks = "",
                                        RequisitionID = ids,
                                        EmployeeID = 0,
                                        StockBalance = b.RequestQty,
                                        StockInOrOutDate = DateTime.UtcNow.Date,
                                        StockType = "O",
                                        UnitPrice = 0,
                                        VendorID = 0,
                                        WarehouseID = m.ToStoreID
                                    };
                                    iInvStoreService.Create(ist);
                                }
                            }
                            else return Json(new { Result = 0, message = "Stock not sufficient" }, JsonRequestBehavior.AllowGet);


                            int s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionMaster SET RequsitionStatus='" + status /*(status == "p" ? "s" : "c")*/ + "'" +
                       " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionID=" + ids + " AND RequsitionStatus='s'");
                            if (s > 0)
                            {
                                s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionDetails SET AprovedStatus='" + status + "',ApprovedQty= (CASE WHEN ApprovedQty>0 THEN ApprovedQty ELSE Qty END)" +
                                " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='s'");
                                result = 1; msg = "Requsition approved";
                            }
                            else return Json(new { Result = 0, message = "Fail to Update" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new { Result = 0, message = "Fail to Update" }, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion a
                #region i
                else if (status == "i")
                {
                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            using (gBankerDbContext db = new gBankerDbContext())
                            {
                                int s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionMaster SET RequsitionStatus='" + status + "'" +
                                    " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionID=" + ids + " AND RequsitionStatus='a'");
                                if (s > 0)
                                {
                                    decimal totalPrice = 0;
                                    s = db.Database.ExecuteSqlCommand("UPDATE Inv_RequsitionDetails SET AprovedStatus='" + status + "'" +
                                    " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='a'");
                                    if (s > 0)
                                    {
                                        List<Inv_RequsitionDetails> detailsList = db.Database.SqlQuery<Inv_RequsitionDetails>("SELECT * FROM Inv_RequsitionDetails WHERE RequsitionMasterID=" + ids + "AND AprovedStatus='i'").ToList();
                                        Inv_RequsitionMaster m = db.Database.SqlQuery<Inv_RequsitionMaster>("SELECT * FROM Inv_RequsitionMaster WHERE RequsitionID=" + ids + " AND RequsitionStatus='i'").First();


                                        var storIn = iInvTempStoreService.GetMany(x => x.RequisitionID == ids).ToList();
                                        #region  Temp Store
                                        if (storIn.Any())
                                        {
                                            totalPrice = storIn.Sum(x => x.Qty * x.UnitPrice);

                                            // Acccount Voucher

                                            var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','4535')").ToList();
                                            Inv_TrxMaster am = new Inv_TrxMaster()
                                            {
                                                CreateDate = DateTime.Now,
                                                CreateUser = LoginUserOfficeID.ToString(),
                                                //InActiveDate = DateTime.Now.Date,
                                                //IsActive = true,
                                                //IsAutoVoucher = true,
                                                Reference = "storein",
                                                IsPosted = false,
                                                OfficeID = LoginUserOfficeID.Value,
                                                //OrgID = 1,
                                                TrxDate = SessionHelper.TransactionDate,//DateTime.Now.Date,
                                                VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                                                VoucherType = "JR"
                                            };
                                            var a = iInvTrxMasterService.Create(am);
                                            foreach (var acc in accList)
                                            {
                                                Inv_TrxDetail d = new Inv_TrxDetail()
                                                {
                                                    AccID = acc.AccID,
                                                    CreateDate = DateTime.Now,
                                                    CreateUser = LoginUserOfficeID.Value.ToString(),
                                                    Credit = (acc.AccCode == "2001" ? 0 : totalPrice),
                                                    Debit = (acc.AccCode == "2001" ? totalPrice : 0),
                                                    IsActive = true,
                                                    Narration = acc.AccCode == "2001" ? "" : "Store In",
                                                    TrxMasterID = a.TrxMasterID
                                                };
                                                iInvTrxDetailService.Create(d);
                                            }
                                            vID = am.TrxMasterID;
                                            foreach (var t in storIn)
                                            {
                                                Inv_Store sIn = new Inv_Store()
                                                {
                                                    CreateBy = LoginUserOfficeID.Value,
                                                    CreateDate = DateTime.Now,
                                                    EmployeeID = 0,
                                                    IsActive = true,
                                                    ItemID = t.ItemID,
                                                    Qty = t.Qty,
                                                    RequisitionID = ids,
                                                    StoreNo = m.RequsitionNo,
                                                    RequestPage = "Requisition",
                                                    Remarks = "",
                                                    StockBalance = t.Qty,
                                                    StockInOrOutDate = DateTime.Now.Date,
                                                    StockType = "I",
                                                    UnitPrice = t.UnitPrice,
                                                    VendorID = 0,
                                                    WarehouseID = t.StoreID,
                                                    TrxMasterID = vID
                                                };
                                                iInvStoreService.Create(sIn);
                                            }
                                        }
                                        #endregion Temp Store
                                        else
                                        {
                                            var invStore = new List<Inv_Store>();
                                            foreach (var d in detailsList)
                                            {
                                                invStore.AddRange(StoreOut(itemid: d.ItemID, SendstoreID: m.FromStoreID
                                                    , OutStoreID: m.ToStoreID, qty: d.ApprovedQty
                                                    , ConsulateRequisitionID: 0, RequisitionID: m.RequsitionID
                                                    , StockoutDate: SessionHelper.TransactionDate));
                                            }
                                            var storeIn = new List<Inv_Store>();
                                            foreach (var i in invStore)
                                            {
                                                Inv_Store S = new Inv_Store()
                                                {
                                                    CreateBy = i.CreateBy,
                                                    CreateDate = i.CreateDate,
                                                    IsActive = i.IsActive,
                                                    ItemID = i.ItemID,
                                                    Qty = i.Qty,
                                                    RequestPage = "Transfer",
                                                    RequisitionID = i.RequisitionID,
                                                    StoreNo = "",
                                                    StockBalance = i.Qty,
                                                    StockInOrOutDate = i.StockInOrOutDate,
                                                    StockType = "I",
                                                    UnitPrice = i.UnitPrice,
                                                    WarehouseID = m.FromStoreID
                                                };
                                                storeIn.Add(S);
                                            }
                                            invStore.AddRange(storeIn);
                                            totalPrice = storeIn.Sum(x => x.Qty * x.UnitPrice);
                                            var office = db.InvWarehouses.Where(w => w.WarehouseID == m.FromStoreID || w.WarehouseID == m.ToStoreID).ToList();

                                            // Acccount Voucher
                                            var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','4535')").ToList();
                                            Inv_TrxMaster trxM = new Inv_TrxMaster()
                                            {
                                                CreateDate = DateTime.Now,
                                                CreateUser = LoginUserOfficeID.ToString(),
                                                IsPosted = false,
                                                OfficeID = office.First(x => x.WarehouseID == m.ToStoreID).OfficeID,
                                                Reference = "storeout",
                                                TrxDate = SessionHelper.TransactionDate,
                                                VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                                                VoucherType = "JR"
                                            };
                                            var a = iInvTrxMasterService.Create(trxM);
                                            foreach (var acc in accList)
                                            {
                                                Inv_TrxDetail d = new Inv_TrxDetail()
                                                {
                                                    AccID = acc.AccID,
                                                    CreateDate = DateTime.Now,
                                                    CreateUser = LoginUserOfficeID.Value.ToString(),
                                                    Credit = (acc.AccCode == "2001" ? totalPrice : 0),
                                                    Debit = (acc.AccCode == "2001" ? 0 : totalPrice),
                                                    IsActive = true,
                                                    Narration = "",
                                                    TrxMasterID = a.TrxMasterID
                                                };
                                                iInvTrxDetailService.Create(d);
                                            }
                                            vID = a.TrxMasterID;
                                            invStore.Where(x => x.StockType == "O").ToList().ForEach(x => { x.TrxMasterID = vID; x.RequestPage = "Transfer"; });
                                            /// storein
                                            trxM = new Inv_TrxMaster()
                                            {
                                                CreateDate = DateTime.Now,
                                                CreateUser = LoginUserOfficeID.ToString(),
                                                Reference = "storein",
                                                IsPosted = false,
                                                OfficeID = office.First(x => x.WarehouseID == m.FromStoreID).OfficeID,
                                                TrxDate = SessionHelper.TransactionDate,//DateTime.Now.Date,
                                                VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                                                VoucherType = "JR"
                                            };
                                            a = iInvTrxMasterService.Create(trxM);
                                            foreach (var acc in accList)
                                            {
                                                Inv_TrxDetail d = new Inv_TrxDetail()
                                                {
                                                    AccID = acc.AccID,
                                                    CreateDate = DateTime.Now,
                                                    CreateUser = LoginUserOfficeID.Value.ToString(),
                                                    Credit = (acc.AccCode == "2001" ? 0 : totalPrice),
                                                    Debit = (acc.AccCode == "2001" ? totalPrice : 0),
                                                    IsActive = true,
                                                    Narration = acc.AccCode == "2001" ? "" : "Store In",
                                                    TrxMasterID = a.TrxMasterID
                                                };
                                                iInvTrxDetailService.Create(d);
                                            }
                                            vID = a.TrxMasterID;
                                            invStore.Where(x => x.StockType == "I").ToList().ForEach(x => x.TrxMasterID = vID);
                                            db.Inv_Stores.AddRange(invStore);
                                            db.SaveChanges();
                                        }
                                        result = 1; msg = "Store in Successfully";
                                        transaction.Complete();
                                    }
                                    else
                                    {
                                        result = 0; msg = "Fail to Update";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = 0; msg = "Fail to Update";
                        }
                        transaction.Dispose();
                    }
                }
                #endregion i
                #region f
                else if (status == "f")
                {

                }
                #endregion f
                else
                    return Json(new { Result = 0, message = "Fail to Update" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Result = 2, message = "Re-Login" }, JsonRequestBehavior.AllowGet);
            //result = 2; msg = "Re-Login";

            return Json(new { Result = result, message = msg, VID = vID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateRequsitionDetails
            (int reDId, string status, string rowStatus, int apvQty, int ItemID)
        {
            if (LoginUserOfficeID.HasValue)
            {
                if (rowStatus == "s")
                {
                    if (status == "In" && apvQty <= 0)
                        return Json(new { Result = 0, message = "Approved Quantity Required" }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        if (status == "In")
                        {
                            List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                            int wID = wObj.First().WarehouseID;
                            var lst = iInvStoreService.GetMany(x => x.WarehouseID == wID && x.StockType == "I" && x.ItemID == ItemID && x.StockBalance > 0).ToList();
                            var sqty = 0;
                            if (lst.Any())
                                sqty = lst.Sum(x => x.StockBalance);
                            if (sqty < apvQty)
                                return Json(new { Result = 0, message = "Approve Quantity is large than Stock Balance" }, JsonRequestBehavior.AllowGet);
                        }


                        string qry = "UPDATE Inv_RequsitionDetails SET " +
                                                    status == "In" ? ("ApprovedQty=" + apvQty + "") :
                                                    status == "rin" ? ("AprovedStatus='r'") : ""
                                                + " ,UpdateBy=" + LoginUserOfficeID + " ,UpdateDate='" + DateTime.Now + "'  WHERE ID=" + reDId + "AND AprovedStatus='" + status + "'";
                        using (gBankerDbContext db = new gBankerDbContext())
                        {
                            db.Database.ExecuteSqlCommand(qry);
                            return Json(new { Result = 1, message = "Success" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                    return Json(new { Result = 0, message = "Wrong information post" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Result = 2, message = "Re-Login" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ForwardRequsition(int mid, int forwordStoreID)
        {
            if (LoginUserOfficeID.HasValue)
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    List<Inv_RequsitionDetails> detailsList = db.Database.SqlQuery<Inv_RequsitionDetails>("SELECT * FROM Inv_RequsitionDetails WHERE RequsitionMasterID=" + mid + "AND AprovedStatus='s'").ToList();
                    Inv_RequsitionMaster m = db.Database.SqlQuery<Inv_RequsitionMaster>("SELECT * FROM Inv_RequsitionMaster WHERE RequsitionID=" + mid + " AND RequsitionStatus='s'").First();
                    var req = db.Database.SqlQuery<string>("exec spRequsitionNo {0}", (LoginUserOfficeID ?? 0)).ToList();
                    Inv_RequsitionMaster obMaster = new Inv_RequsitionMaster()
                    {
                        CreatedBy = LoginUserOfficeID.Value,
                        CreatedDate = DateTime.UtcNow,
                        ForwardRequsitionID = m.RequsitionID,
                        FromStoreID = m.ToStoreID,
                        IsActive = true,
                        RequsitionDate = DateTime.Now.Date,
                        RequsitionNo = req.First(),
                        RequsitionStatus = "s",
                        ToStoreID = forwordStoreID,
                    };
                    iInv_RequsitionMasterService.Create(obMaster);
                    foreach (var v in detailsList)
                    {
                        Inv_RequsitionDetails d = new Inv_RequsitionDetails()
                        {
                            AprovedStatus = "s",
                            CreatedBy = LoginUserOfficeID,
                            CreatedDate = obMaster.CreatedDate,
                            ApprovedQty = 0,
                            ItemID = v.ItemID,
                            RequestQty = v.RequestQty,
                            RequsitionMasterID = obMaster.RequsitionID
                        };
                        iInv_RequsitionDetailsService.Create(d);
                    }
                }
                return Json(new { Result = 1, message = "Forward Success" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { Result = 0, message = "Re-Login" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConsulateRequisitionDetail(string DetailsIDs, int itemID)
        {
            try
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                    var lst = db.Database.SqlQuery<Inv_ConsulateRequisitionDetailsViewModel>("sp_Inv_ConsulateRequisitionDetails '" + DetailsIDs + "'," + itemID + "," + off.OfficeLevel + "").ToList();
                    return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult RequisitionDetailSendingQtyUpdate(long dID, int sendingQty)
        {
            int result = 0;
            try
            {
                string sql = "";
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                if (off.OfficeLevel == 2)
                    sql = "UPDATE Inv_RequisitionConsulateDetails SET SendingQty=" + sendingQty + " WHERE ConsulateDetailID=" + dID + "";
                else if (off.OfficeLevel == 3)
                    sql = "UPDATE Inv_RequsitionDetails SET SendingQty=" + sendingQty + " WHERE ID=" + dID + "";
                new gBankerDbContext().Database.ExecuteSqlCommand(sql);
                result = 1;
            }
            catch (Exception)
            {

            }
            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConsulateRequisitionApprove(int requestStorID)
        {
            if (LoginUserOfficeID.HasValue)
            {
                try
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                        var lst = db.Database.SqlQuery<InvRequisitionConsulateViewModel>("sp_Inv_GetConsulateRequisitionApprove {0},{1}", requestStorID, wObj.First().WarehouseID).ToList();
                        return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.Count() });
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
                return Json(new { Result = "Relogin" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsulateRequisitionApproved
            (ConsulateRequisitionMasterApproveViewModel vmObj
            , List<ConsulateRequisitionDetailsApproveViewModel> vdObj)
        {
            string msg = ""; int result = 0; long vID = 0;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                    int senderStoreID = wObj.First().WarehouseID;
                    decimal totalPrice = 0;
                    string naration = "Store out";
                    List<Inv_Store> objInvStoreLst = new List<Inv_Store>();
                    foreach (var d in vdObj)
                    {
                        var detailObj = iInv_RequsitionConsulateDetailsService.GetById(d.ConsulateDetailID.Value);
                        detailObj.ApprovedQty = d.ModifyQty.Value;
                        detailObj.AprovedStatus = "Approved";
                        detailObj.StatusChangeBy = LoginUserOfficeID.Value;
                        detailObj.StatusChangeDate = vmObj.ApproveDate;
                        iInv_RequsitionConsulateDetailsService.Update(detailObj);
                        objInvStoreLst.AddRange(StoreOut(d.ItemID.Value, vmObj.ApproveStoreID, senderStoreID, d.ModifyQty.Value, detailObj.ConsulateRequisitionMasterID, 0, vmObj.ApproveDate));
                    }
                    // Acccount Voucher

                    var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','4535')").ToList();
                    Inv_TrxMaster m = new Inv_TrxMaster()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = LoginUserOfficeID.ToString(),
                        //InActiveDate = vmObj.ApproveDate,
                        //IsActive = true,
                        //IsAutoVoucher = true,
                        IsPosted = false,
                        OfficeID = LoginUserOfficeID.Value,
                        Reference = "storeout",
                        //OrgID = 1,
                        TrxDate = vmObj.ApproveDate,
                        VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                        VoucherType = "JR"
                    };
                    var a = iInvTrxMasterService.Create(m);
                    totalPrice = objInvStoreLst.Sum(s => s.Qty * s.UnitPrice);
                    foreach (var acc in accList)
                    {
                        Inv_TrxDetail d = new Inv_TrxDetail()
                        {
                            AccID = acc.AccID,
                            CreateDate = DateTime.Now,
                            CreateUser = LoginUserOfficeID.Value.ToString(),
                            Credit = (acc.AccCode == "2001" ? totalPrice : 0),
                            Debit = (acc.AccCode == "2001" ? 0 : totalPrice),
                            IsActive = true,
                            Narration = acc.AccCode == "2001" ? naration : "",
                            TrxMasterID = a.TrxMasterID
                        };
                        iInvTrxDetailService.Create(d);
                    }
                    vID = a.TrxMasterID;
                    var itmList = objInvStoreLst.GroupBy(g => new { g.ItemID, g.UnitPrice, g.StockType, g.WarehouseID, g.RequisitionID, g.RefStoreID })
                            .Select(s => new
                            {
                                s.Key.ItemID
                                ,
                                s.Key.UnitPrice
                                ,
                                s.Key.StockType
                                ,
                                s.Key.WarehouseID
                                ,
                                s.Key.RequisitionID
                                ,
                                s.Key.RefStoreID
                                ,
                                Qty = s.Sum(t => t.Qty)
                            }).ToList();
                    foreach (var r in itmList)
                    {
                        Inv_Store sObj = new Inv_Store()
                        {
                            EmployeeID = 0,
                            IsActive = true,
                            Qty = r.Qty,
                            RequestPage = "ConOut",
                            StockBalance = r.Qty,
                            StockInOrOutDate = vmObj.ApproveDate,
                            CreateBy = LoginUserOfficeID.Value,
                            CreateDate = DateTime.Now,
                            ItemID = r.ItemID,
                            StockType = r.StockType,
                            UnitPrice = r.UnitPrice,
                            WarehouseID = r.WarehouseID,
                            RequisitionID = r.RequisitionID,
                            StoreNo = vmObj.RequsitionNo,
                            TrxMasterID = vID,
                            RefStoreID = r.RefStoreID
                        };
                        iInvStoreService.Create(sObj);
                        Inv_TempStore t = new Inv_TempStore()
                        {
                            ConsulateRequisitionID = r.RequisitionID,
                            ItemID = r.ItemID,
                            StoreID = vmObj.ApproveStoreID,
                            Qty = r.Qty,
                            RequisitionID = 0/*r.RequisitionID*/,
                            UnitPrice = r.UnitPrice
                        };
                        iInvTempStoreService.Create(t);
                    }
                    transaction.Complete();


                    result = 1;
                    msg = "Success";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    result = 0;
                }
                transaction.Dispose();
            }
            return Json(new { Message = msg, Result = result, VID = vID }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RequisitionApproved(int requisitionID, List<RequsitionAnalysisViewModel> objlst)
        {
            string msg = ""; int result = 0; long vID = 0;
            if (LoginUserOfficeID.HasValue)
            {
                if (objlst.Where(x => (x.StockBalance - x.ModifyQty) < x.MinStockLevel).Any())
                    msg = "Stock Balance Check";
                else
                {
                    using (var transaction = new TransactionScope())
                    {
                        try
                        {
                            decimal totalPrice = 0;
                            var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                            #region Area
                            if (off.OfficeLevel == 3)
                            {
                                var rm = iInv_RequsitionMasterService.GetById(requisitionID);
                                rm.RequsitionStatus = "a";
                                iInv_RequsitionMasterService.Update(rm);
                                var rd = iInv_RequsitionDetailsService.GetMany(x => x.RequsitionMasterID == requisitionID);

                                foreach (var d in rd)
                                {
                                    d.ApprovedQty = objlst.Where(x => x.ItemID == d.ItemID).Sum(x => x.ModifyQty);
                                    d.AprovedStatus = "a";
                                    d.UpdateBy = LoginUserOfficeID;
                                    d.UpdateDate = DateTime.Now;
                                    // temp store out with store in
                                    var temp = iInvTempStoreService.GetMany(x => x.ItemID == d.ItemID && x.Qty > 0 && x.StoreID == rm.ToStoreID);
                                    int qty = d.ApprovedQty;

                                    foreach (var s in temp)
                                    {
                                        if (qty > 0)
                                        {
                                            if (s.Qty >= qty)
                                            {
                                                Inv_TempStore t = new Inv_TempStore()
                                                {
                                                    ConsulateRequisitionID = 0,
                                                    ItemID = d.ItemID,
                                                    StoreID = rm.FromStoreID,
                                                    Qty = qty,
                                                    RequisitionID = requisitionID,
                                                    UnitPrice = s.UnitPrice
                                                };
                                                iInvTempStoreService.Create(t);
                                                totalPrice += s.UnitPrice * qty;
                                                s.Qty = s.Qty - qty;
                                                qty = 0;

                                                iInvTempStoreService.Update(s);

                                            }
                                            else
                                            {
                                                Inv_TempStore t = new Inv_TempStore()
                                                {
                                                    ConsulateRequisitionID = 0,
                                                    ItemID = d.ItemID,
                                                    StoreID = rm.FromStoreID,
                                                    Qty = s.Qty,
                                                    RequisitionID = requisitionID,
                                                    UnitPrice = s.UnitPrice
                                                };
                                                iInvTempStoreService.Create(t);
                                                totalPrice += s.UnitPrice * s.Qty;
                                                qty = qty - s.Qty;
                                                s.Qty = 0;
                                                iInvTempStoreService.Update(s);
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                            #endregion Area
                            else
                            {
                                var rm = iInv_RequsitionMasterService.GetById(requisitionID);
                                rm.RequsitionStatus = "a";
                                iInv_RequsitionMasterService.Update(rm);
                                var rd = iInv_RequsitionDetailsService.GetMany(x => x.RequsitionMasterID == requisitionID);
                                List<Inv_Store> objInvStoreLst = new List<Inv_Store>();

                                foreach (var d in rd)
                                {
                                    d.ApprovedQty = objlst.Where(x => x.ItemID == d.ItemID).Sum(x => x.ModifyQty);
                                    d.AprovedStatus = "a";
                                    d.UpdateBy = LoginUserOfficeID;
                                    d.UpdateDate = DateTime.Now;
                                    // temp store out with store in
                                    objInvStoreLst.AddRange(StoreOut(d.ItemID, rm.FromStoreID, rm.ToStoreID, d.ApprovedQty, 0, rm.RequsitionID, SessionHelper.TransactionDate));
                                }
                                //if(objlst.Sum(x=>x.ApprovedQty)!= objInvStoreLst.Sum(x => x.Qty))
                                //{
                                //    transaction.Dispose();
                                //    return Json(new { Message = "Quantity not match", Result = 0, VID = 0 }, JsonRequestBehavior.AllowGet);
                                //}


                                var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','4535')").ToList();
                                Inv_TrxMaster m = new Inv_TrxMaster()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = LoginUserOfficeID.ToString(),
                                    //InActiveDate = SessionHelper.TransactionDate,
                                    //IsActive = true,
                                    //IsAutoVoucher = true,
                                    IsPosted = false,
                                    OfficeID = LoginUserOfficeID.Value,
                                    Reference = "storeout",
                                    //OrgID = 1,
                                    TrxDate = SessionHelper.TransactionDate,
                                    VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                                    VoucherType = "JR"
                                };
                                var a = iInvTrxMasterService.Create(m);
                                totalPrice = objInvStoreLst.Sum(s => s.Qty * s.UnitPrice);
                                foreach (var acc in accList)
                                {
                                    Inv_TrxDetail d = new Inv_TrxDetail()
                                    {
                                        AccID = acc.AccID,
                                        CreateDate = DateTime.Now,
                                        CreateUser = LoginUserOfficeID.Value.ToString(),
                                        Credit = (acc.AccCode == "2001" ? totalPrice : 0),
                                        Debit = (acc.AccCode == "2001" ? 0 : totalPrice),
                                        IsActive = true,
                                        Narration = acc.AccCode == "2001" ? "Store Out" : "",
                                        TrxMasterID = a.TrxMasterID
                                    };
                                    iInvTrxDetailService.Create(d);
                                }
                                vID = a.TrxMasterID;
                                var itmList = objInvStoreLst.GroupBy(g => new { g.ItemID, g.UnitPrice, g.StockType, g.WarehouseID, g.RequisitionID, g.RefStoreID })
                                        .Select(s => new
                                        {
                                            s.Key.ItemID
                                            ,
                                            s.Key.UnitPrice
                                            ,
                                            s.Key.StockType
                                            ,
                                            s.Key.WarehouseID
                                            ,
                                            s.Key.RequisitionID
                                            ,
                                            s.Key.RefStoreID
                                            ,
                                            Qty = s.Sum(t => t.Qty)
                                        }).ToList();
                                foreach (var r in itmList)
                                {
                                    Inv_Store sObj = new Inv_Store()
                                    {
                                        EmployeeID = 0,
                                        IsActive = true,
                                        Qty = r.Qty,
                                        RequestPage = "rOut",
                                        StockBalance = r.Qty,
                                        StockInOrOutDate = SessionHelper.TransactionDate,
                                        CreateBy = LoginUserOfficeID.Value,
                                        CreateDate = DateTime.Now,
                                        ItemID = r.ItemID,
                                        StockType = r.StockType,
                                        UnitPrice = r.UnitPrice,
                                        WarehouseID = r.WarehouseID,
                                        RequisitionID = r.RequisitionID,
                                        StoreNo = rm.RequsitionNo,
                                        TrxMasterID = vID,
                                        RefStoreID = r.RefStoreID
                                    };
                                    iInvStoreService.Create(sObj);
                                    Inv_TempStore t = new Inv_TempStore()
                                    {
                                        ConsulateRequisitionID = 0/*Convert.ToInt32(r.RequisitionID)*/,
                                        ItemID = r.ItemID,
                                        StoreID = rm.FromStoreID,
                                        Qty = r.Qty,
                                        RequisitionID = r.RequisitionID,
                                        UnitPrice = r.UnitPrice
                                    };
                                    iInvTempStoreService.Create(t);
                                }
                            }
                            transaction.Complete();
                            result = 1;
                            msg = "Approved Done";
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message;
                        }
                        transaction.Dispose();
                    }
                }

            }
            else
                msg = "Re-Login";
            return Json(new { Message = msg, Result = result, VID = vID }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsulateRequisitionStoreIn(int masterID)
        {
            string msg = ""; int result = 0; long vID = 0;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var cm = iInv_RequsitionConsulateMasterService.GetById(masterID);
                    decimal totalPrice = 0;
                    var detailObj = iInv_RequsitionConsulateDetailsService.GetMany(x => x.ConsulateRequisitionMasterID == masterID);
                    detailObj.ToList().ForEach(x => x.AprovedStatus = "StoreIn");
                    var temp = iInvTempStoreService.GetMany(x => x.ConsulateRequisitionID == masterID).ToList();
                    totalPrice = temp.Sum(x => x.Qty * x.UnitPrice);


                    // Acccount Voucher

                    var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','4535')").ToList();
                    Inv_TrxMaster m = new Inv_TrxMaster()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = LoginUserOfficeID.ToString(),
                        //InActiveDate = DateTime.Now.Date,
                        //IsActive = true,
                        //IsAutoVoucher = true,
                        IsPosted = false,
                        OfficeID = LoginUserOfficeID.Value,
                        Reference = "storein",
                        //OrgID = 1,
                        TrxDate = DateTime.Now.Date,
                        VoucherNo = LoginUserOfficeID.Value.ToString() + DateTime.Now.ToString("ddMMhmm") + "-" + DateTime.Now.ToString("yyyy"),
                        VoucherType = "JR"
                    };
                    var a = iInvTrxMasterService.Create(m);
                    foreach (var acc in accList)
                    {
                        Inv_TrxDetail d = new Inv_TrxDetail()
                        {
                            AccID = acc.AccID,
                            CreateDate = DateTime.Now,
                            CreateUser = LoginUserOfficeID.Value.ToString(),
                            Credit = (acc.AccCode == "2001" ? 0 : totalPrice),
                            Debit = (acc.AccCode == "2001" ? totalPrice : 0),
                            IsActive = true,
                            Narration = acc.AccCode == "2001" ? "" : "Store In",
                            TrxMasterID = a.TrxMasterID
                        };
                        iInvTrxDetailService.Create(d);
                    }
                    vID = a.TrxMasterID;
                    foreach (var t in temp)
                    {
                        Inv_Store s = new Inv_Store()
                        {
                            CreateBy = LoginUserOfficeID.Value,
                            CreateDate = DateTime.Now,
                            EmployeeID = 0,
                            IsActive = true,
                            ItemID = t.ItemID,
                            Qty = t.Qty,
                            RequisitionID = masterID,
                            StoreNo = cm.RequisitionNo,
                            RequestPage = "ConIn",
                            Remarks = "",
                            StockBalance = t.Qty,
                            StockInOrOutDate = DateTime.Now.Date,
                            StockType = "I",
                            UnitPrice = t.UnitPrice,
                            VendorID = 0,
                            WarehouseID = t.StoreID,
                            TrxMasterID = vID
                        };
                        iInvStoreService.Create(s);
                        //iInvTempStoreService.Delete(t.ID);
                    }
                    transaction.Complete();
                    result = 1;
                    msg = "Store in Completed";
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    msg = ex.Message;
                }
                return Json(new { Message = msg, Result = result, VID = vID }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ConsulateRequisitionAreaView(int masterID)
        {
            var detailObj = iInv_RequsitionConsulateDetailsService.GetMany(x => x.ConsulateRequisitionMasterID == masterID);
            foreach (var t in detailObj)
            {
                t.AprovedStatus = "View";
                t.StatusChangeBy = LoginUserOfficeID.Value;
                t.StatusChangeDate = DateTime.Now;
                iInv_RequsitionConsulateDetailsService.Update(t);
            }

            //detailObj.ToList().ForEach(x => x.AprovedStatus = "StoreIn");
            return Json(new { Message = "Success", Result = 1, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConRequisitionWiseOffice()
        {
            if (LoginUserOfficeID.HasValue)
            {
                try
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        //var w = db.Database.SqlQuery<InvWarehouse>("SELECT DISTINCT w.* FROM " +
                        //    "Inv_RequisitionConsulateMaster m " +
                        //    "INNER JOIN dbo.Inv_RequisitionConsulateDetails d ON m.ConsulateRequisitionID=d.ConsulateRequisitionMasterID " +
                        //    "INNER JOIN Inv_Warehouse w ON m.SenderStoreID = w.WarehouseID " +
                        //    "INNER JOIN Office o ON w.OfficeID = o.OfficeID " +
                        //    "WHERE m.ReceiverStoreID = {0} AND d.AprovedStatus='Pending'", LoginUserOfficeID).ToList();
                        // For Head Office  sp_Inv_RequisitionWaitingForApprove
                        var store = db.InvWarehouses.Where(x => x.OfficeID == LoginUserOfficeID).First();
                        var w = db.Database.SqlQuery<InvWarehouse>("sp_Inv_GetConRequisitionWiseOffice " + store.WarehouseID + "").ToList();
                        return Json(new { Records = w, Result = "OK" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = ex.Message, Result = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { Result = "Relogin" }, JsonRequestBehavior.AllowGet);
        }


        #endregion Web Service

        #region Dispose Requisition 
        public ActionResult DisposeRequisitionList()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public ActionResult DisposeRequisitionCreate()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public JsonResult CreateDisposeRequest(List<Inv_Store> obj)
        {
            string msg = "", DisposeNo = ""; int result = 0;
            if (obj != null)
            {
                if (obj.Any())
                {
                    try
                    {
                        using (gBankerDbContext db = new gBankerDbContext())
                        {
                            foreach (var o in obj)
                            {
                                Inv_RequsitionDispose d = new Inv_RequsitionDispose()
                                {
                                    DisposeStatus = 1,
                                    DisposeRequestNo = o.StoreNo,
                                    ItemID = o.ItemID,
                                    Qty = o.Qty,
                                    UnitPrice = o.UnitPrice,
                                    RequestBy = SessionHelper.LoginUserEmployeeID,
                                    RequestDate = o.StockInOrOutDate,
                                    RequestRemark = o.Remarks,
                                    DisposeRequestOfficeID = o.WarehouseID,
                                };
                                db.inv_RequsitionDisposes.Add(d);
                            }
                            db.SaveChanges();
                            msg = "Request Add Successfully";
                            result = 1;
                            DisposeNo = obj.First().StoreNo;
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }
            return Json(new { message = msg, Result = result, DisposeNo = DisposeNo }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DisposeApproveRequisition()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public JsonResult GetDisposeRequest(string status, string from, string to)
        {
            var dispObj = new List<RequsitionDisposeViewModel>();
            if (!string.IsNullOrEmpty(status) && !DateTime.MinValue.Equals(from) && !DateTime.MinValue.Equals(to))
            {
                try
                {
                    dispObj = new gBankerDbContext().Database.SqlQuery<RequsitionDisposeViewModel>("sp_DisposeRequest " +
                    SessionHelper.LoginUserOfficeID + "," + SessionHelper.LoggedInOfficeDetail.OfficeLevel + ", '" + status + "','" +
                    from + "','" + to + "'").ToList();
                }
                catch (Exception ex)
                {
                    dispObj = new List<RequsitionDisposeViewModel>();
                }
            }
            return Json(new { Result = "OK", Records = dispObj, TotalRecordCount = dispObj.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendDisposeRequest(long DisposeRequestID, string status)
        {
            string msg = "";
            try
            {
                var warehouseObj = new gBankerDbContext().InvWarehouses.First(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
                if (status == "r")
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        var obj = db.inv_RequsitionDisposes.First(x => x.DisposeRequestID == DisposeRequestID);
                        obj.RejectBy = SessionHelper.LoginUserEmployeeID;
                        obj.RejectDate = DateTime.Now;
                        obj.RejectOfficeID = warehouseObj.WarehouseID;
                        obj.DisposeStatus = 0;
                        db.SaveChanges();
                        msg = "Reject Completed";
                    }
                }
                else if (status == "A" && SessionHelper.LoggedInOfficeDetail.OfficeLevel == 1)
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        var obj = db.inv_RequsitionDisposes.First(x => x.DisposeRequestID == DisposeRequestID);
                        obj.RequestApprovedBy = SessionHelper.LoginUserEmployeeID;
                        obj.ApprovedDate = DateTime.Now;
                        obj.DisposeRequestApproveOfficeID = warehouseObj.WarehouseID;
                        obj.DisposeStatus = 2;
                        db.SaveChanges();
                        msg = "Approved Success";
                    }
                }
                #region Dispose
                else if (status == "D" && SessionHelper.LoggedInOfficeDetail.OfficeLevel == 2)
                {
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        var obj = db.inv_RequsitionDisposes.First(x => x.DisposeRequestID == DisposeRequestID);
                        var storeObj = db.Inv_Stores.Where(x => x.ItemID == obj.ItemID && x.WarehouseID == obj.DisposeRequestOfficeID
                          && x.UnitPrice == obj.UnitPrice && x.StockType == "I" && x.StockBalance > 0).ToList();
                        if (storeObj.Any())
                        {
                            int qty = obj.ApprovedQty.Value;
                            if (storeObj.Sum(x => x.StockBalance) >= qty)
                            {
                                obj.DisposeBy = SessionHelper.LoginUserEmployeeID;
                                obj.DisposeDate = DateTime.Now;
                                obj.DisposeOfficeID = warehouseObj.WarehouseID;
                                obj.DisposeStatus = 3;
                                db.SaveChanges();
                                foreach (var s in storeObj)
                                {
                                    if (qty > 0)
                                    {
                                        if (s.StockBalance >= qty)
                                        {
                                            Inv_Store sObj = new Inv_Store()
                                            {
                                                EmployeeID = 0,
                                                IsActive = true,
                                                Qty = qty,
                                                StockBalance = qty,
                                                StoreNo = obj.DisposeRequestNo,
                                                StockInOrOutDate = DateTime.Now.Date,
                                                CreateBy = LoginUserOfficeID.Value,
                                                CreateDate = DateTime.Now,
                                                ItemID = obj.ItemID,
                                                StockType = "D",
                                                RequestPage = "D",
                                                UnitPrice = s.UnitPrice,
                                                WarehouseID = obj.DisposeRequestOfficeID.Value,
                                                RequisitionID = 0,
                                                RefStoreID = s.ID
                                            };
                                            iInvStoreService.Create(sObj);
                                            s.StockBalance = s.StockBalance - qty;
                                            qty = 0;
                                            iInvStoreService.Update(s);

                                        }
                                        else
                                        {
                                            Inv_Store sObj = new Inv_Store()
                                            {
                                                EmployeeID = 0,
                                                IsActive = true,
                                                Qty = s.StockBalance,
                                                StockBalance = s.StockBalance,
                                                StockInOrOutDate = DateTime.Now.Date,
                                                CreateBy = LoginUserOfficeID.Value,
                                                CreateDate = DateTime.Now,
                                                ItemID = obj.ItemID,
                                                StockType = "D",
                                                RequestPage = "D",
                                                StoreNo = obj.DisposeRequestNo,
                                                UnitPrice = s.UnitPrice,
                                                WarehouseID = obj.DisposeRequestOfficeID.Value,
                                                RequisitionID = 0,
                                                RefStoreID = s.ID
                                            };
                                            iInvStoreService.Create(sObj);
                                            qty = qty - s.StockBalance;
                                            s.StockBalance = 0;
                                            iInvStoreService.Update(s);
                                        }
                                    }
                                    else
                                        break;
                                }
                                msg = "Dispose Successfully";
                            }
                            else msg = "price wise Stock not match";
                        }
                        else msg = "Stock not found";
                    }
                }
                #endregion Dispose
            }
            catch (Exception ex)
            {
                msg = "Error found";
            }

            return Json(new { message = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisposeRequisitionSend()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public JsonResult ConsulateDisposeRequest(List<Inv_RequsitionDispose> obj)
        {
            string msg = "";
            try
            {
                if (obj.Where(w => w.ApprovedQty > w.Qty).Any())
                    msg = "Approved Qty Check";
                else
                {
                    var ids = obj.Select(x => x.DisposeRequestID);
                    using (gBankerDbContext db = new gBankerDbContext())
                    {
                        var warehouseObj = db.InvWarehouses.First(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
                        var disObj = db.inv_RequsitionDisposes.Where(x => ids.Contains(x.DisposeRequestID)).ToList();
                        foreach (var o in obj)
                            disObj.Where(x => x.DisposeRequestID == o.DisposeRequestID).ToList().ForEach(x => { x.ApprovedQty = o.ApprovedQty; x.IsConsulateApproved = false; });
                        var cObj = disObj.GroupBy(g => new { g.ItemID/*, g.UnitPrice */})
                            .Select(s => new { ItemID = s.Key.ItemID/*, UnitPrice = s.Key.UnitPrice*/, Qty = s.Sum(g => g.ApprovedQty) }).ToList();
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {

                                foreach (var f in cObj)
                                {
                                    Inv_ConsolidateDisposeRequest c = new Inv_ConsolidateDisposeRequest()
                                    {
                                        ConsolidateOfficeID = warehouseObj.WarehouseID,
                                        ConsolidateDate = DateTime.Now,
                                        ConsolidateBy = SessionHelper.LoginUserEmployeeID,
                                        ItemID = f.ItemID,
                                        Qty = f.Qty ?? 0,
                                        IsActive=true
                                    };
                                    if (c.Qty > 0)
                                    {
                                        db.Inv_ConsolidateDisposeRequests.Add(c);
                                        db.SaveChanges();
                                        disObj.Where(x => x.ItemID == f.ItemID && x.ApprovedQty > 0).ToList().ForEach(s => s.ConsolidateDisposeID = c.ConsolidateDisposeID);
                                    }
                                }
                                foreach (var c in disObj)
                                {
                                    if (c.ApprovedQty == 0)
                                    {
                                        c.ConsolidateDisposeID = null;
                                        c.IsConsulateApproved = null;
                                        c.RejectBy = SessionHelper.LoginUserEmployeeID;
                                        c.RejectDate = DateTime.Now;
                                        c.RejectOfficeID = warehouseObj.WarehouseID;
                                        c.DisposeStatus = 0;
                                    }
                                    db.Entry(c).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                scope.Complete();
                                msg = "Send Complete";
                            }
                            catch (Exception)
                            {
                                msg = "Send fail";
                            }
                            scope.Dispose();
                        }
                    }
                }
            }
            catch (Exception)
            {
                msg = "Error Found";
            }
            return Json(new { Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsolidateDisposeData(string from, string to)
        {
            var dispObj = new gBankerDbContext().Database.SqlQuery<Inv_ConsolidateDisposeRequestViewModel>("inv_sp_ConsolidateDispose '" + from + "','" + to + "'").ToList();
            return Json(new { Result = "OK", Records = dispObj, TotalRecordCount = dispObj.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsolidateDisposeDataNew(string from, string to, int? officeID, int? itemID, string reportType)
        {
            var dispObj = new gBankerDbContext().Database.SqlQuery<Inv_ConsolidateDisposeRequestViewModel>("inv_sp_ConsolidateDispose2 '" + from + "','" + to + "','HO','" + (reportType == null ? "" : reportType) + "'," + (officeID ?? 0) + "," + (itemID ?? 0) + "").ToList();
            return Json(new { Result = "OK", Records = dispObj, TotalRecordCount = dispObj.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApprovedDisposeRequest(int ConsolidateDisposeID, int ItemID)
        {
            string msg = "";

            using (gBankerDbContext db = new gBankerDbContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        var warehouseObj = db.InvWarehouses.First(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
                        var cObj = db.Inv_ConsolidateDisposeRequests.FirstOrDefault(x => x.ConsolidateDisposeID == ConsolidateDisposeID);
                        cObj.ApprovedBy = SessionHelper.LoginUserEmployeeID;
                        cObj.ApprovedDate = DateTime.Now;
                        cObj.ApprovedOfficeID = warehouseObj.WarehouseID;
                        db.SaveChanges();
                        var disObj = db.inv_RequsitionDisposes.Where(x => x.ConsolidateDisposeID == ConsolidateDisposeID && x.ItemID == ItemID);
                        foreach (var o in disObj)
                        {
                            db.inv_RequsitionDisposes.Attach(o);
                            o.ApprovedDate = DateTime.Now;
                            o.RequestApprovedBy = SessionHelper.LoginUserEmployeeID;
                            o.DisposeRequestApproveOfficeID = warehouseObj.WarehouseID;
                            o.DisposeStatus = 2;
                            o.IsConsulateApproved = true;
                            db.SaveChanges();
                        }
                        scope.Complete();
                        msg = "Approved Completed";
                    }
                    catch (Exception)
                    {
                        msg = "Error found";
                    }
                    scope.Dispose();
                }

            }
            return Json(new { message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovedDisposeRequestNew(int WarehouseID, int Qty,string from,string to)
        {
            string msg = "";
            try
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var warehouseObj = db.InvWarehouses.First(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
                    var result = db.Database.SqlQuery<string>("inv_sp_ConsolidateDisposeApproved " + WarehouseID + ","+ warehouseObj.WarehouseID +
                        ","+Qty+",'"+ from + "','"+to+"',"+ SessionHelper.LoginUserEmployeeID + "").ToList();
                    msg = result.First();
                }
            }
            catch (Exception)
            {
                msg = "Error Found";
            }
            return Json(new { message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RejectOfficeWiseItem(int WarehouseID,int itemID, int Qty, string from, string to)
        {
            string msg = "";
            try
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var warehouseObj = db.InvWarehouses.First(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
                    var result = db.Database.SqlQuery<string>("inv_sp_ConsolidateDisposeReject " + itemID+"," + WarehouseID + "," + warehouseObj.WarehouseID +
                        "," + Qty + ",'" + from + "','" + to + "'," + SessionHelper.LoginUserEmployeeID + "").ToList();
                    msg = result.First();
                }
            }
            catch (Exception)
            {
                msg = "Error Found";
            }
            return Json(new { message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion Dispose Requisition 
        public List<Inv_Store> StoreOut(int itemid, int SendstoreID, int OutStoreID
            , int qty, int ConsulateRequisitionID
            , long RequisitionID, DateTime StockoutDate)
        {
            //DateTime vdt = Convert.ToDateTime("01 Nov 2020");
            List<Inv_Store> objInvStoreLst = new List<Inv_Store>();
            decimal totalPrice = 0;
            var sqty = iInvStoreService.GetMany(x => x.WarehouseID == OutStoreID && x.StockType == "I"
            && x.ItemID == itemid && x.StockBalance > 0 /*&& x.StockInOrOutDate >= vdt*/)
            .OrderBy(x => x.ID);
            if (sqty.Any())
            {
                foreach (var s in sqty)
                {
                    if (qty > 0)
                    {
                        if (s.StockBalance >= qty)
                        {
                            Inv_Store sObj = new Inv_Store()
                            {
                                EmployeeID = 0,
                                IsActive = true,
                                Qty = qty,
                                StockBalance = qty,
                                StoreNo = "",
                                StockInOrOutDate = StockoutDate,
                                CreateBy = LoginUserOfficeID.Value,
                                CreateDate = DateTime.Now,
                                ItemID = itemid,
                                StockType = "O",
                                UnitPrice = s.UnitPrice,
                                WarehouseID = OutStoreID,
                                RequisitionID = (ConsulateRequisitionID > 0 ? ConsulateRequisitionID : RequisitionID),
                                RefStoreID = s.ID
                            };
                            objInvStoreLst.Add(sObj);
                            //iInvStoreService.Create(sObj);
                            totalPrice += s.UnitPrice * qty;

                            s.StockBalance = s.StockBalance - qty;
                            qty = 0;

                            iInvStoreService.Update(s);

                        }
                        else
                        {
                            Inv_TempStore t = new Inv_TempStore()
                            {
                                ConsulateRequisitionID = ConsulateRequisitionID,
                                ItemID = itemid,
                                StoreID = SendstoreID,
                                Qty = s.StockBalance,
                                RequisitionID = RequisitionID,
                                UnitPrice = s.UnitPrice
                            };
                            //iInvTempStoreService.Create(t);
                            Inv_Store sObj = new Inv_Store()
                            {
                                EmployeeID = 0,
                                IsActive = true,
                                Qty = s.StockBalance,
                                StockBalance = s.StockBalance,
                                StockInOrOutDate = StockoutDate,
                                CreateBy = LoginUserOfficeID.Value,
                                CreateDate = DateTime.Now,
                                ItemID = itemid,
                                StockType = "O",
                                StoreNo = "",
                                UnitPrice = s.UnitPrice,
                                WarehouseID = OutStoreID,
                                RequisitionID = ConsulateRequisitionID,
                                RefStoreID = s.ID
                            };
                            objInvStoreLst.Add(sObj);
                            //iInvStoreService.Create(sObj);
                            totalPrice += s.UnitPrice * s.StockBalance;
                            qty = qty - s.StockBalance;
                            s.StockBalance = 0;
                            iInvStoreService.Update(s);
                        }
                    }
                    else
                        break;
                }
            }
            return objInvStoreLst;
        }
    }
}