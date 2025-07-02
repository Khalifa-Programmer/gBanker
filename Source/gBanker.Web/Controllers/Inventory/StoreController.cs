using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers.Inventory
{
    public class StoreController : BaseController
    {
        #region Variables
        private readonly IInvWarehouseService iInvWarehouseService;
        private readonly IInvStoreItemService iInvStoreItemService;
        private readonly IOrganizationService iOrganizationService;
        private readonly IOfficeService iIOfficeService;
        private readonly IInvStoreService iInvStoreService;
        private readonly IEmployeeService iEmployeeService;
        //private readonly IAccTrxMasterService iAccTrxMasterService;
        //private readonly IAccTrxDetailService iAccTrxDetailService;
        private readonly IInvTrxMasterService iInvTrxMasterService;
        private readonly IInvTrxDetailService iInvTrxDetailService;
        public StoreController(IInvWarehouseService iInvWarehouseService,
        IInvStoreItemService iInvStoreItemService,
        IOrganizationService iOrganizationService,
        IOfficeService iIOfficeService,
        IInvStoreService iInvStoreService,
        IEmployeeService iEmployeeService,
        IInvTrxMasterService iInvTrxMasterService,
        IInvTrxDetailService iInvTrxDetailService
        )
        {
            this.iInvWarehouseService = iInvWarehouseService;
            this.iInvStoreItemService = iInvStoreItemService;
            this.iOrganizationService = iOrganizationService;
            this.iIOfficeService = iIOfficeService;
            this.iInvStoreService = iInvStoreService;
            this.iEmployeeService = iEmployeeService;
            this.iInvTrxMasterService = iInvTrxMasterService;
            this.iInvTrxDetailService = iInvTrxDetailService;
        }
        #endregion Variables
        // GET: Store
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        #region Warehouse


        [HttpGet]
        public ActionResult WarehouseOpening()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UnitAdd()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StoreList()
        {
            if (LoginUserOfficeID.HasValue)
            {
                int allowMultipleoffice = 0;
                if (SessionHelper.LoginUserRoleId == 12 | SessionHelper.LoginUserRoleId == 6
                    | SessionHelper.LoginUserRoleId == 1) allowMultipleoffice = 1;
                ViewBag.allowMultipleoffice = allowMultipleoffice;
                Office obj = iIOfficeService.GetById(LoginUserOfficeID ?? 0);
                if (obj != null)
                {
                    ViewBag.OfficeLevel = obj.OfficeLevel;
                    if (obj.OfficeLevel == 2)
                        ViewBag.isZone = 1;
                    else
                        ViewBag.isZone = 0;
                }

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UpdateStore()
        {
            if (LoginUserOfficeID.HasValue)
            {
                int allowMultipleoffice = 0;
                if (SessionHelper.LoginUserRoleId == 12 | SessionHelper.LoginUserRoleId == 6
                    | SessionHelper.LoginUserRoleId == 1) allowMultipleoffice = 1;
                ViewBag.allowMultipleoffice = allowMultipleoffice;
                Office obj = iIOfficeService.GetById(LoginUserOfficeID ?? 0);
                if (obj != null)
                {
                    ViewBag.OfficeLevel = obj.OfficeLevel;
                    if (obj.OfficeLevel == 2)
                        ViewBag.isZone = 1;
                    else
                        ViewBag.isZone = 0;
                }
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult StoreIn()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iIOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult StoreOut()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iIOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult StoreDispose()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult ItemDispose()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iIOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        #endregion Warehouse

        #region Web Service
        #region    store
        public JsonResult GetStoreInfo()
        {
            var wList = iInvWarehouseService.GetAll();
            wList = iInvWarehouseService.GetOfficeIDWise(LoginUserOfficeID ?? 0);
            return Json(new { Records = wList, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStoreInfoXOffice()
        {
            // var wList = iInvWarehouseService.GetMany(x=>x.OfficeID==(LoginUserOfficeID ?? 0));
            var wList = iInvWarehouseService.GetOfficeIDWise(LoginUserOfficeID ?? 0);
            return Json(new { Records = wList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStoreOfficeXMapping()
        {
            List<InvWarehouse> listObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("exec sp_InvOfficeMapping {0}", (LoginUserOfficeID ?? 0)).ToList();
            return Json(new { Records = listObj }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStoreDetailXGrid(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue)
        {
            try
            {
                List<Inv_StoreViewModel> objList = new List<Inv_StoreViewModel>();
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    objList = db.Database.SqlQuery<Inv_StoreViewModel>("exec sp_StoreDetails " + (LoginUserOfficeID ?? 0) + "," + 54 + "").ToList();
                }
                var totalCount = objList.Count();
                var entities = objList.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Inv_StoreViewModel>, IEnumerable<Inv_StoreViewModel>>(entities);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetWarehouseItems()
        {
            List<Inv_StoreItemViewModel> objList = new List<Inv_StoreItemViewModel>();
            using (gBankerDbContext db = new gBankerDbContext())
            {
                objList = db.Database.SqlQuery<Inv_StoreItemViewModel>("exec sp_StoreDetailsItem " + (LoginUserOfficeID ?? 0) + "").ToList();
                if (objList.Any())
                    objList = objList.Where(x => x.Qty > 0).ToList();
            }
            return Json(new { Records = objList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWarehouseItemsXCategory(int categoryID)
        {
            List<Inv_StoreItemViewModel> objList = new List<Inv_StoreItemViewModel>();
            using (gBankerDbContext db = new gBankerDbContext())
            {
                objList = db.Database.SqlQuery<Inv_StoreItemViewModel>("exec sp_StoreDetailsItem2 " + (LoginUserOfficeID ?? 0) + "," + categoryID + "").ToList();
                if (objList.Any())
                    objList = objList.Where(x => x.Qty > 0).ToList();
            }
            return Json(new { Records = objList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveStoreItem(InvStoreItem obj)
        {
            var result = 0;
            var message = "";
            try
            {
                bool status = false;
                var innObj = new InvStoreItem();
                var dup = iInvStoreItemService.GetCheck(obj.WarehouseID, obj.ItemID);
                if (dup.Any())
                {
                    if (obj.StoreItemID > 0)
                    {
                        var v = iInvStoreItemService.GetById(obj.StoreItemID);
                        if (v.ItemID == obj.ItemID)
                            status = true;
                        else
                            status = false;
                    }
                    else
                        status = false;
                }
                else status = true;
                // Update
                if (status)
                {
                    if (obj.StoreItemID > 0)
                    {
                        innObj = iInvStoreItemService.GetById(obj.StoreItemID);
                        innObj.ItemID = obj.ItemID;
                        innObj.Price = obj.Price;
                        innObj.Qty = obj.Qty;
                        innObj.UpdateBy = LoginUserOfficeID;
                        innObj.UpdateDate = DateTime.UtcNow;
                        iInvStoreItemService.Update(innObj);
                        result = 1;
                        message = "Update Success";

                    }
                    // Insert
                    else
                    {
                        obj.IsActive = true;
                        obj.CreateBy = LoginUserOfficeID;
                        obj.CreateDate = DateTime.UtcNow;
                        iInvStoreItemService.Create(obj);
                        result = 1;
                        message = "Save Success";
                    }
                }
                else
                    message = "Duplicate Item not allow";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreGrid()
        {
            var param = new { @officeID = (LoginUserOfficeID ?? 0), @warehouseID = 0 };
            var objList = iInvStoreItemService.GetAllStoreItem(param).ToList();

            var currentPageRecords = Mapper.Map<IEnumerable<Inv_WarehouseViewModel>, IEnumerable<Inv_WarehouseViewModel>>(objList);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objList.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByID(int id, int sts)
        {
            var obj = iInvStoreItemService.GetById(id);
            //edit
            if (sts == 0)
            {
                return Json(new { Result = "OK", Records = obj }, JsonRequestBehavior.AllowGet);
            }
            //delete
            else if (sts == 1)
            {
                obj.IsActive = false;
                iInvStoreItemService.Update(obj);
                return Json(new { message = "Delete Successfull" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockXItem(int itemID)
        {
            try
            {
                List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                int sqty = 0;
                int wID = wObj.First().WarehouseID;
                var lst = iInvStoreService.GetMany(x => x.WarehouseID == wID && x.StockType == "I" && x.ItemID == itemID && x.StockBalance > 0).ToList();
                if (lst.Any())
                    sqty = lst.Sum(x => x.StockBalance);
                return Json(sqty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult StoreOutInfo(List<Inv_Store> obj)
        {
            string msg = ""; int result = 0; long vID = 0;
            if (obj != null)
            {
                if (obj.Any())
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            decimal totalPrice = 0;
                            int vendorID = obj.First().VendorID ?? 0;
                            var itemList = obj.GroupBy(x => new { x.ItemID, x.WarehouseID, x.StockType })
                                .Select(g => new { ItemID = g.Key.ItemID, WarehouseID = g.Key.WarehouseID, g.Key.StockType, Qty = g.Sum(x => x.Qty), UnitPrice = g.Sum(x => x.UnitPrice) });
                            List<Inv_Store> objInvStoreLst = new List<Inv_Store>();
                            foreach (var b in itemList)
                            {
                                if (b.StockType == "O")
                                {
                                    if (obj.Where(x => x.ItemID == b.ItemID).Any())
                                    {
                                        if (!string.IsNullOrEmpty(obj.Where(x => x.ItemID == b.ItemID).First().EmployeeCode))
                                        {
                                            var emp = iEmployeeService.GetByCode(obj.FirstOrDefault(x => x.ItemID == b.ItemID).EmployeeCode);
                                            obj.Where(x => x.ItemID == b.ItemID).ToList().ForEach(x => x.EmployeeID = emp.EmployeeID);
                                        }
                                    }
                                    var sqty = iInvStoreService.GetMany(x => x.WarehouseID == b.WarehouseID && x.StockType == "I" && x.ItemID == b.ItemID && x.StockBalance > 0).OrderBy(x => x.ID);
                                    if (sqty.Any())
                                    {
                                        int qty = b.Qty;
                                        foreach (var s in sqty)
                                        {
                                            if (qty > 0)
                                            {
                                                if (s.StockBalance >= qty)
                                                {
                                                    Inv_Store sObj = new Inv_Store()
                                                    {
                                                        EmployeeID = obj.Where(x => x.ItemID == s.ItemID).First().EmployeeID,
                                                        IsActive = true,
                                                        Qty = qty,
                                                        RequestPage = "StrOut",
                                                        StockBalance = qty,
                                                        StockInOrOutDate = obj.First().StockInOrOutDate,
                                                        CreateBy = LoginUserOfficeID.Value,
                                                        CreateDate = DateTime.Now,
                                                        ItemID = s.ItemID,
                                                        StockType = "O",
                                                        UnitPrice = s.UnitPrice,
                                                        WarehouseID = s.WarehouseID,
                                                        RequisitionID = 0,
                                                        RefStoreID = s.ID,
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
                                                    Inv_Store sObj = new Inv_Store()
                                                    {
                                                        EmployeeID = obj.Where(x => x.ItemID == s.ItemID).First().EmployeeID,
                                                        IsActive = true,
                                                        Qty = s.StockBalance,
                                                        RequestPage = "StrOut",
                                                        StockBalance = s.StockBalance,
                                                        StockInOrOutDate = obj.First().StockInOrOutDate,
                                                        CreateBy = LoginUserOfficeID.Value,
                                                        CreateDate = DateTime.Now,
                                                        ItemID = s.ItemID,
                                                        StockType = "O",
                                                        UnitPrice = s.UnitPrice,
                                                        WarehouseID = s.WarehouseID,
                                                        RequisitionID = 0,
                                                        RefStoreID = s.ID,
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
                                }
                                else if (b.StockType == "I")
                                {
                                    Inv_Store ist = new Inv_Store()
                                    {
                                        CreateBy = LoginUserOfficeID,
                                        CreateDate = DateTime.UtcNow,
                                        ID = 0,
                                        IsActive = true,
                                        ItemID = b.ItemID,
                                        Qty = b.Qty,
                                        Remarks = string.Join(",", obj.Where(x => x.ItemID == b.ItemID).Select(x => x.Remarks).ToList()),
                                        RequisitionID = 0,
                                        EmployeeID = 0,
                                        StockBalance = b.Qty,
                                        StockInOrOutDate = obj.First(x => x.ItemID == b.ItemID).StockInOrOutDate,
                                        StockType = b.StockType,
                                        UnitPrice = b.UnitPrice,
                                        VendorID = vendorID,
                                        WarehouseID = b.WarehouseID
                                    };
                                    objInvStoreLst.Add(ist);
                                    //iInvStoreService.Create(ist);
                                    totalPrice += ist.UnitPrice * ist.Qty;
                                }
                            }
                            // Acccount Voucher
                            string srt = "";
                            srt = obj.First().StockType == "O" ? "'2001','6101'" : "'2001','4535'";
                            var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in(" + srt + ")").ToList();
                            Inv_TrxMaster m = new Inv_TrxMaster()
                            {
                                CreateDate = DateTime.Now,
                                CreateUser = LoginUserOfficeID.ToString(),
                                //InActiveDate = DateTime.Now.Date,
                                //IsActive = true,
                                //IsAutoVoucher = true,
                                IsPosted = false,
                                OfficeID = LoginUserOfficeID.Value,
                                Reference = (obj.First().StockType == "I" ? "storein" : "storeout"),
                                //OrgID = 1,
                                TrxDate = obj.First().StockInOrOutDate,//DateTime.Now.Date,
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
                                    Credit = ((obj.First().StockType == "O" && acc.AccCode == "2001") ? totalPrice :
                                    (obj.First().StockType == "I" && acc.AccCode == "4535") ? totalPrice : 0),
                                    Debit = ((obj.First().StockType == "O" && acc.AccCode == "6101") ? totalPrice :
                                    (obj.First().StockType == "I" && acc.AccCode == "2001") ? totalPrice : 0),
                                    IsActive = true,
                                    Narration = acc.AccCode == "2001" ? "" : "Store " + (obj.First().StockType == "O" ? "Out" : "In"),
                                    TrxMasterID = a.TrxMasterID
                                };
                                iInvTrxDetailService.Create(d);
                            }
                            vID = a.TrxMasterID;

                            //var itmList = objInvStoreLst.Select(x => x.ItemID).Distinct();
                            var itmList = objInvStoreLst.GroupBy(g => new { g.ItemID, g.UnitPrice, g.StockType, g.WarehouseID, g.RefStoreID })
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
                                    s.Key.RefStoreID
                                    ,
                                    Qty = s.Sum(t => t.Qty)
                                }).ToList();
                            foreach (var r in itmList)
                            {
                                Inv_Store sObj = new Inv_Store()
                                {
                                    EmployeeID = obj.Where(x => x.ItemID == r.ItemID).First().EmployeeID,
                                    IsActive = true,
                                    Qty = r.Qty,
                                    RequestPage = (r.StockType == "I" ? "StrIn" : "StrOut"),
                                    StockBalance = r.Qty,
                                    StockInOrOutDate = obj.First().StockInOrOutDate,
                                    CreateBy = LoginUserOfficeID.Value,
                                    CreateDate = DateTime.Now,
                                    ItemID = r.ItemID,
                                    StockType = r.StockType,
                                    UnitPrice = r.UnitPrice,
                                    WarehouseID = r.WarehouseID,
                                    RequisitionID = 0,
                                    StoreNo = obj.First().StoreNo,
                                    ChallanNo = obj.First().ChallanNo,
                                    TrxMasterID = vID,
                                    RefStoreID = r.RefStoreID
                                };
                                iInvStoreService.Create(sObj);
                            }

                            msg = "Store " + (obj.First().StockType == "I" ? "In" : "Out") + " Success";
                            result = 1;
                            //return Json(new { message = , VID = vID, Result = 1 }, JsonRequestBehavior.AllowGet);

                            //return Json(new { message = "Item Not Found", Result = 0 }, JsonRequestBehavior.AllowGet);
                            transactionScope.Complete();
                            transactionScope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            msg = ex.Message; result = 0;
                            //return Json(new { message = ex.Message, Result = 0 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    msg = "Item Not Found";
                    result = 0;
                }
            }
            else
            {
                msg = "Item Not Found";
                result = 0;
            }
            if (result > 0)
                return Json(new { message = msg, VID = vID, Result = result }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = msg, Result = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StoreOutInfoNew(List<Inv_Store> obj)
        {
            string msg = ""; int result = 0; long vID = 0;
            if (obj != null)
            {
                if (obj.Any())
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            decimal totalPrice = 0;
                            int vendorID = obj.First().VendorID ?? 0;
                            var itemList = obj.GroupBy(x => new { x.ItemID, x.WarehouseID, x.StockType, x.ExistsPrice })
                                .Select(g => new
                                {
                                    ItemID = g.Key.ItemID,
                                    WarehouseID = g.Key.WarehouseID,
                                    g.Key.StockType,
                                    ExistsPrice = g.Key.ExistsPrice,
                                    Qty = g.Sum(x => x.Qty),
                                    UnitPrice = g.Sum(x => x.UnitPrice)
                                });
                            List<Inv_Store> objInvStoreLst = new List<Inv_Store>();
                            foreach (var b in itemList)
                            {
                                if (b.StockType == "O")
                                {
                                    if (obj.Where(x => x.ItemID == b.ItemID).Any())
                                    {
                                        if (!string.IsNullOrEmpty(obj.Where(x => x.ItemID == b.ItemID).First().EmployeeCode))
                                        {
                                            var emp = iEmployeeService.GetByCode(obj.FirstOrDefault(x => x.ItemID == b.ItemID).EmployeeCode);
                                            obj.Where(x => x.ItemID == b.ItemID).ToList().ForEach(x => x.EmployeeID = emp.EmployeeID);
                                        }
                                    }
                                    var sqty = iInvStoreService.GetMany(x => x.WarehouseID == b.WarehouseID && x.StockType == "I"
                                    && x.UnitPrice == b.ExistsPrice && x.ItemID == b.ItemID && x.StockBalance > 0).OrderBy(x => x.ID);
                                    if (sqty.Any())
                                    {
                                        int qty = b.Qty;
                                        foreach (var s in sqty)
                                        {
                                            if (qty > 0)
                                            {
                                                if (s.StockBalance >= qty)
                                                {
                                                    Inv_Store sObj = new Inv_Store()
                                                    {
                                                        EmployeeID = obj.Where(x => x.ItemID == s.ItemID).First().EmployeeID,
                                                        IsActive = true,
                                                        Qty = qty,
                                                        RequestPage = "StrOut",
                                                        StockBalance = qty,
                                                        StockInOrOutDate = obj.First().StockInOrOutDate,
                                                        CreateBy = LoginUserOfficeID.Value,
                                                        CreateDate = DateTime.Now,
                                                        ItemID = s.ItemID,
                                                        StockType = "O",
                                                        UnitPrice = s.UnitPrice,
                                                        WarehouseID = s.WarehouseID,
                                                        RequisitionID = 0,
                                                        RefStoreID = s.ID,
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
                                                    Inv_Store sObj = new Inv_Store()
                                                    {
                                                        EmployeeID = obj.Where(x => x.ItemID == s.ItemID).First().EmployeeID,
                                                        IsActive = true,
                                                        Qty = s.StockBalance,
                                                        RequestPage = "StrOut",
                                                        StockBalance = s.StockBalance,
                                                        StockInOrOutDate = obj.First().StockInOrOutDate,
                                                        CreateBy = LoginUserOfficeID.Value,
                                                        CreateDate = DateTime.Now,
                                                        ItemID = s.ItemID,
                                                        StockType = "O",
                                                        UnitPrice = s.UnitPrice,
                                                        WarehouseID = s.WarehouseID,
                                                        RequisitionID = 0,
                                                        RefStoreID = s.ID,
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
                                }
                                else if (b.StockType == "I")
                                {
                                    Inv_Store ist = new Inv_Store()
                                    {
                                        CreateBy = LoginUserOfficeID,
                                        CreateDate = DateTime.UtcNow,
                                        ID = 0,
                                        IsActive = true,
                                        ItemID = b.ItemID,
                                        Qty = b.Qty,
                                        Remarks = string.Join(",", obj.Where(x => x.ItemID == b.ItemID).Select(x => x.Remarks).ToList()),
                                        RequisitionID = 0,
                                        EmployeeID = 0,
                                        StockBalance = b.Qty,
                                        StockInOrOutDate = obj.First(x => x.ItemID == b.ItemID).StockInOrOutDate,
                                        StockType = b.StockType,
                                        UnitPrice = b.UnitPrice,
                                        VendorID = vendorID,
                                        WarehouseID = b.WarehouseID
                                    };
                                    objInvStoreLst.Add(ist);
                                    //iInvStoreService.Create(ist);
                                    totalPrice += ist.UnitPrice * ist.Qty;
                                }
                            }
                            // Acccount Voucher
                            string srt = "";
                            srt = obj.First().StockType == "O" ? "'2001','6101'" : "'2001','4535'";
                            var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in(" + srt + ")").ToList();
                            Inv_TrxMaster m = new Inv_TrxMaster()
                            {
                                CreateDate = DateTime.Now,
                                CreateUser = LoginUserOfficeID.ToString(),
                                //InActiveDate = DateTime.Now.Date,
                                //IsActive = true,
                                //IsAutoVoucher = true,
                                IsPosted = false,
                                OfficeID = LoginUserOfficeID.Value,
                                Reference = (obj.First().StockType == "I" ? "storein" : "storeout"),
                                //OrgID = 1,
                                TrxDate = obj.First().StockInOrOutDate,//DateTime.Now.Date,
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
                                    Credit = ((obj.First().StockType == "O" && acc.AccCode == "2001") ? totalPrice :
                                    (obj.First().StockType == "I" && acc.AccCode == "4535") ? totalPrice : 0),
                                    Debit = ((obj.First().StockType == "O" && acc.AccCode == "6101") ? totalPrice :
                                    (obj.First().StockType == "I" && acc.AccCode == "2001") ? totalPrice : 0),
                                    IsActive = true,
                                    Narration = acc.AccCode == "2001" ? "" : "Store " + (obj.First().StockType == "O" ? "Out" : "In"),
                                    TrxMasterID = a.TrxMasterID
                                };
                                iInvTrxDetailService.Create(d);
                            }
                            vID = a.TrxMasterID;

                            //var itmList = objInvStoreLst.Select(x => x.ItemID).Distinct();
                            var itmList = objInvStoreLst.GroupBy(g => new { g.ItemID, g.UnitPrice, g.StockType, g.WarehouseID, g.RefStoreID })
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
                                    s.Key.RefStoreID
                                    ,
                                    Qty = s.Sum(t => t.Qty)
                                }).ToList();
                            foreach (var r in itmList)
                            {
                                Inv_Store sObj = new Inv_Store()
                                {
                                    EmployeeID = obj.Where(x => x.ItemID == r.ItemID).First().EmployeeID,
                                    IsActive = true,
                                    Qty = r.Qty,
                                    RequestPage = (r.StockType == "I" ? "StrIn" : "StrOut"),
                                    StockBalance = r.Qty,
                                    StockInOrOutDate = obj.First().StockInOrOutDate,
                                    CreateBy = LoginUserOfficeID.Value,
                                    CreateDate = DateTime.Now,
                                    ItemID = r.ItemID,
                                    StockType = r.StockType,
                                    UnitPrice = r.UnitPrice,
                                    WarehouseID = r.WarehouseID,
                                    RequisitionID = 0,
                                    StoreNo = obj.First().StoreNo,
                                    ChallanNo = obj.First().ChallanNo,
                                    TrxMasterID = vID,
                                    RefStoreID = r.RefStoreID
                                };
                                iInvStoreService.Create(sObj);
                            }

                            msg = "Store " + (obj.First().StockType == "I" ? "In" : "Out") + " Success";
                            result = 1;
                            //return Json(new { message = , VID = vID, Result = 1 }, JsonRequestBehavior.AllowGet);

                            //return Json(new { message = "Item Not Found", Result = 0 }, JsonRequestBehavior.AllowGet);
                            transactionScope.Complete();
                            transactionScope.Dispose();
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            msg = ex.Message; result = 0;
                            //return Json(new { message = ex.Message, Result = 0 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    msg = "Item Not Found";
                    result = 0;
                }
            }
            else
            {
                msg = "Item Not Found";
                result = 0;
            }
            if (result > 0)
                return Json(new { message = msg, VID = vID, Result = result }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = msg, Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StoreInInfo(int itemId)
        {
            var wid = iInvWarehouseService.GetMany(x => x.OfficeID == (LoginUserOfficeID ?? 0)).First().WarehouseID;
            var obj = iInvStoreService.GetMany(x => x.ItemID == itemId && x.WarehouseID == wid && x.StockType == "I" && x.StockBalance > 0 && x.RequisitionID == 0);
            return Json(new { Result = "OK", Records = obj, TotalRecordCount = obj.Count() });
        }

        public JsonResult StoreUpdateXItem(int itemid, string storeType, string date, string slno, int? officeID)
        {
            var obj = new List<Inv_Store>();
            var wid = iInvWarehouseService.GetMany(x => x.OfficeID == (officeID ?? (LoginUserOfficeID ?? 0))).First().WarehouseID;
            if (itemid > 0 && (!string.IsNullOrEmpty(storeType) |
                !string.IsNullOrEmpty(date) | !string.IsNullOrEmpty(slno)))
            {
                DateTime dt = new DateTime();
                if (!string.IsNullOrEmpty(date))
                    dt = DateTime.Parse(date);
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    obj = db.Inv_Stores.Where(x => x.IsActive == true && x.ItemID == itemid
                    && x.RequisitionID == 0 && x.StockType == storeType //&& x.Qty > 0 && x.StockBalance > 0
                    && x.WarehouseID == wid
                    && x.StoreNo == (string.IsNullOrEmpty(slno) ? x.StoreNo : slno)
                    && x.StockInOrOutDate == (string.IsNullOrEmpty(date) ? x.StockInOrOutDate : dt)
                    ).ToList();
                }
                if (obj.Any())
                    obj.ForEach(x => x.StockDate = x.StockInOrOutDate.ToString("dd-MMM-yyyy"));
            }

            return Json(new { Result = "OK", Records = obj, TotalRecordCount = obj.Count() });
        }

        public JsonResult StoreTypeXDetails(string storeType, string date, string slno, string officeCode)
        {

            var obj = new List<InvStoreViewModel>();
            if ((!string.IsNullOrEmpty(storeType) |
                !string.IsNullOrEmpty(date) | !string.IsNullOrEmpty(slno)) && !string.IsNullOrEmpty(officeCode))
            {
                string sql = "SELECT s.*,i.ItemName,i.ItemCode FROM Inv_Store s INNER JOIN dbo.Inv_Items i " +
                    "ON s.ItemID=i.ItemID INNER JOIN dbo.Inv_Warehouse w " +
                    "ON w.WarehouseID=s.WarehouseID INNER JOIN dbo.Office o " +
                    "ON w.OfficeID = o.OfficeID WHERE o.OfficeCode = '" + officeCode + "' " +
                    "AND s.IsActive=1 AND RequisitionID = 0 AND StockType='" + storeType + "' " +
                    "" + (string.IsNullOrEmpty(slno) ? "" : "AND s.StoreNo='" + slno + "' ") +
                    "" + (string.IsNullOrEmpty(date) ? "" : "AND s.StockInOrOutDate='" + date + "'");
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    obj = db.Database.SqlQuery<InvStoreViewModel>(sql).ToList();
                }
                //var widlst = new gBankerDbContext().Database.SqlQuery<int>("select w.WarehouseID from dbo.Inv_Warehouse w " +
                //                "INNER JOIN dbo.Office o ON w.OfficeID = o.OfficeID WHERE o.OfficeCode = '" + officeCode + "'").ToList();
                //if (widlst.Any())
                //{
                //    int wid = widlst.First();
                //    DateTime dt = new DateTime();
                //    if (!string.IsNullOrEmpty(date))
                //        dt = DateTime.Parse(date);
                //    using (gBankerDbContext db = new gBankerDbContext())
                //    {
                //        obj = db.Inv_Stores.Where(x => x.IsActive == true
                //        && x.RequisitionID == 0 && x.StockType == storeType
                //        && x.WarehouseID == wid
                //        && x.StoreNo == (string.IsNullOrEmpty(slno) ? x.StoreNo : slno)
                //        && x.StockInOrOutDate == (string.IsNullOrEmpty(date) ? x.StockInOrOutDate : dt)
                //        ).ToList();
                //    }
                if (obj.Any())
                    obj.ForEach(x => x.StockDate = x.StockInOrOutDate.ToString("dd-MMM-yyyy"));
            }

            return Json(new { Result = "OK", Records = obj, TotalRecordCount = obj.Count() });
        }

        public JsonResult StoreItemBalanceUpdateXID(long stockID, int qty, string status, decimal UnitPrice)
        {
            string msg = "";
            try
            {
                var st = iInvStoreService.GetByIdLong(stockID);
                #region Update
                if (status == "u")
                {
                    if (qty > 0)
                    {
                        if (st.StockBalance == qty && st.UnitPrice == UnitPrice)
                            msg = "Data Check";
                        else
                        {
                            if (st.StockBalance > qty || st.StockBalance < qty)
                            {
                                int diff = st.Qty - st.StockBalance;
                                st.StockBalance = qty;
                                st.Qty = qty + diff;
                            }
                            if (st.UnitPrice != UnitPrice && st.Qty == st.StockBalance)
                                st.UnitPrice = UnitPrice;
                            if (st.UnitPrice != UnitPrice && st.Qty != st.StockBalance)
                                msg = "Unit Price Not Same";
                            if (msg == "")
                            {
                                st.UpdateBy = LoginUserOfficeID;
                                st.UpdateDate = DateTime.Now;
                                iInvStoreService.Update(st);
                                msg = "Success";
                            }
                        }
                    }
                    else
                        msg = "Quentity  is required";

                }
                #endregion Update

                #region Delete & Reverse
                else if (status == "d")
                {
                    if (st.StockType == "O" || st.StockType == "D")
                    {
                        if (st.RefStoreID.HasValue)
                        {
                            var refSt = iInvStoreService.GetByIdLong(st.RefStoreID.Value);
                            if (refSt == null)
                                refSt = new Inv_Store();
                            if (refSt.ID > 0)
                            {
                                //refSt.Qty = refSt.Qty + st.StockBalance;
                                refSt.StockBalance = refSt.StockBalance + st.StockBalance;
                                refSt.Remarks = st.StockType + st.StoreNo + " Reverse";
                                refSt.UpdateBy = LoginUserOfficeID;
                                refSt.UpdateDate = DateTime.Now;
                                iInvStoreService.Update(refSt);
                                st.IsActive = false;
                                st.UpdateBy = LoginUserOfficeID;
                                st.UpdateDate = DateTime.Now;
                                iInvStoreService.Update(st);
                                msg = "Success";
                            }
                            else
                            {
                                st.StockType = "I";
                                st.Remarks = st.StockType + st.StoreNo + " Reverse";
                                st.StoreNo = "R" + st.StoreNo;
                                st.RequestPage = "StrIn";
                                st.UpdateBy = LoginUserOfficeID;
                                st.UpdateDate = DateTime.Now;
                                iInvStoreService.Update(st);
                                msg = "Success";
                            }
                        }
                        else
                        {
                            st.StockType = "I";
                            st.Remarks = st.StockType + st.StoreNo + " Reverse";
                            st.StoreNo = "R" + st.StoreNo;
                            st.RequestPage = "StrIn";
                            st.UpdateBy = LoginUserOfficeID;
                            st.UpdateDate = DateTime.Now;
                            iInvStoreService.Update(st);
                            msg = "Success";
                        }

                    }
                    else if (st.Qty == st.StockBalance)
                    {
                        st.IsActive = false;
                        st.UpdateBy = LoginUserOfficeID;
                        st.UpdateDate = DateTime.Now;
                        iInvStoreService.Update(st);
                        msg = "Success";
                    }
                    else if (st.Qty != st.StockBalance)
                    {
                        var s = iInvStoreService.GetMany(x => x.RefStoreID == st.ID);
                        if (!s.Any())
                        {
                            st.IsActive = false;
                            st.UpdateBy = LoginUserOfficeID;
                            st.UpdateDate = DateTime.Now;
                            iInvStoreService.Update(st);
                            msg = "Success";
                        }
                        else
                            msg = "This Item Balance has been Used";
                    }

                }
                #endregion Delete & Reverse
            }
            catch (Exception ex)
            {
                msg = "Something is wrong";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StoreDateUpdate(string officeCode, string storeType, string storeSlno, string updateDate)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(officeCode) && !string.IsNullOrEmpty(storeType)
                && !string.IsNullOrEmpty(storeSlno) && !string.IsNullOrEmpty(updateDate))
            {
                DateTime dt = DateTime.Parse(updateDate);
                var widlst = new gBankerDbContext().Database.SqlQuery<int>("select w.WarehouseID from dbo.Inv_Warehouse w " +
                                "INNER JOIN dbo.Office o ON w.OfficeID = o.OfficeID WHERE o.OfficeCode = '" + officeCode + "'").ToList();
                if (widlst.Any())
                {
                    int wid = widlst.First();
                    var strLst = iInvStoreService.GetMany(x => x.WarehouseID == wid && x.StoreNo == storeSlno && x.StockType == storeType).ToList();
                    if (strLst.Any())
                    {
                        strLst.ForEach(x => { x.StockInOrOutDate = dt; x.UpdateBy = LoginUserOfficeID; x.UpdateDate = DateTime.Now; });
                        foreach (var str in strLst)
                            iInvStoreService.Update(str);
                        msg = "Success";
                    }
                    else
                        msg = "Data Not Found";
                }
            }
            else
                msg = "All Data not send";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion store

        public JsonResult GetItemwiseStore(int itemID)
        {
            var currentPageRecords = Mapper.Map<IEnumerable<ItemwiseStoreViewModel>, IEnumerable<ItemwiseStoreViewModel>>(new List<ItemwiseStoreViewModel>());
            int cnt = 0;
            try
            {
                var objList = new gBankerDbContext().Database.SqlQuery<ItemwiseStoreViewModel>("sp_inv_ItemwiseStore {0},{1}", itemID, LoginUserOfficeID.Value).ToList();
                cnt = objList.Count();
                currentPageRecords = Mapper.Map<IEnumerable<ItemwiseStoreViewModel>, IEnumerable<ItemwiseStoreViewModel>>(objList);
            }
            catch (Exception ex)
            {
                currentPageRecords = Mapper.Map<IEnumerable<ItemwiseStoreViewModel>, IEnumerable<ItemwiseStoreViewModel>>(new List<ItemwiseStoreViewModel>());
            }
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = cnt }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ItemXDispose(ItemXDisposeViewModel v)
        {
            int result = 0; string msg = ""; long vID = 0;
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (v != null)
                    {
                        decimal totalPrice = 0;
                        int qty = 0;

                        List<InvWarehouse> wObj = new gBankerDbContext().Database.SqlQuery<InvWarehouse>("SELECT * FROM dbo.Inv_Warehouse WHERE OfficeID=" + LoginUserOfficeID + "").ToList();
                        int storeID = wObj.First().WarehouseID;
                        var sqty = iInvStoreService.GetMany(x => x.WarehouseID == storeID && x.StockType == "I"
                            && x.ItemID == v.ItemID && x.StockBalance > 0)
                            .OrderBy(x => x.ID);
                        qty = v.Qty;
                        if (sqty.Any())
                        {
                            List<Inv_Store> objInvStoreLst = new List<Inv_Store>();
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
                                            RequestPage = "itmDsp",
                                            StockBalance = qty,
                                            StockInOrOutDate = v.Date,
                                            CreateBy = LoginUserOfficeID.Value,
                                            CreateDate = DateTime.Now,
                                            ItemID = v.ItemID,
                                            StockType = "D",
                                            UnitPrice = s.UnitPrice,
                                            WarehouseID = storeID,
                                            RequisitionID = 0,
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
                                        Inv_Store sObj = new Inv_Store()
                                        {
                                            EmployeeID = 0,
                                            IsActive = true,
                                            Qty = s.StockBalance,
                                            RequestPage = "itmDsp",
                                            StockBalance = s.StockBalance,
                                            StockInOrOutDate = v.Date,
                                            CreateBy = LoginUserOfficeID.Value,
                                            CreateDate = DateTime.Now,
                                            ItemID = v.ItemID,
                                            StockType = "D",
                                            UnitPrice = s.UnitPrice,
                                            WarehouseID = storeID,
                                            RequisitionID = 0,
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
                            // Acccount Voucher

                            var accList = new gBankerDbContext().Database.SqlQuery<AccChart>("SELECT * FROM AccChart WHERE AccCode in('2001','6101')").ToList();
                            Inv_TrxMaster m = new Inv_TrxMaster()
                            {
                                CreateDate = DateTime.Now,
                                CreateUser = LoginUserOfficeID.ToString(),
                                //InActiveDate = DateTime.Now.Date,
                                //IsActive = true,
                                //IsAutoVoucher = true,
                                IsPosted = false,
                                OfficeID = LoginUserOfficeID.Value,
                                Reference = "storedispose",
                                //OrgID = 1,
                                TrxDate = v.Date,
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
                                    Credit = (acc.AccCode == "2001" ? totalPrice : 0),
                                    Debit = (acc.AccCode == "2001" ? 0 : totalPrice),
                                    IsActive = true,
                                    Narration = acc.AccCode == "2001" ? "" : "Item Dispose",
                                    TrxMasterID = a.TrxMasterID
                                };
                                iInvTrxDetailService.Create(d);
                            }
                            vID = a.TrxMasterID;
                            var itmList = objInvStoreLst.GroupBy(g => new { g.ItemID, g.UnitPrice, g.StockType, g.WarehouseID, g.RefStoreID })
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
                                    RequestPage = "D",
                                    StockBalance = r.Qty,
                                    StockInOrOutDate = v.Date,
                                    CreateBy = LoginUserOfficeID.Value,
                                    CreateDate = DateTime.Now,
                                    ItemID = r.ItemID,
                                    StockType = r.StockType,
                                    UnitPrice = r.UnitPrice,
                                    WarehouseID = r.WarehouseID,
                                    RequisitionID = 0,
                                    StoreNo = v.StoreNo,
                                    TrxMasterID = vID,
                                    RefStoreID = r.RefStoreID
                                };
                                iInvStoreService.Create(sObj);
                            }
                            transaction.Complete();
                            msg = "Dispose Sucessfuly";
                            result = 1;
                        }
                        else
                            msg = "Store Not Available";
                    }
                    else
                        msg = "Data Not Found";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                transaction.Dispose();
            }

            return Json(new { Result = result, Message = msg, VID = vID }, JsonRequestBehavior.AllowGet);
        }
        #endregion Web Service
    }
}