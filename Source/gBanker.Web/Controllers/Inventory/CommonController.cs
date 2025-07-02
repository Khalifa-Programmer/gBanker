using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gBanker.Web.Controllers.Inventory
{
    public class CommonController : BaseController
    {
        #region Variables
        private readonly IInv_CategoryOrSubCategoryService iCateService;
        private readonly IInv_ItemsService iITemService;
        private readonly IInv_ItemPriceDetailsService iItemPriceService;
        private readonly IInvWarehouseService iInvWarehouseService;
        private readonly IInvStoreItemService iInvStoreItemService;
        private readonly IOrganizationService iOrganizationService;
        private readonly IOfficeService iIOfficeService;
        private readonly IInvStoreService iInvStoreService;
        private readonly IInv_VendorService iInv_VendorService;
        private readonly IEmployeeService iEmployeeService;
        public CommonController
            (IInv_CategoryOrSubCategoryService iCateService
            , IInv_ItemsService iITemService
            , IInv_ItemPriceDetailsService iItemPriceService
            , IInvWarehouseService iInvWarehouseService
            , IInvStoreItemService iInvStoreItemService
            , IOrganizationService iOrganizationService
            , IOfficeService iIOfficeService
            , IInvStoreService iInvStoreService
            , IInv_VendorService iInvVendorService,
            IEmployeeService iEmployeeService)
        {
            this.iCateService = iCateService;
            this.iITemService = iITemService;
            this.iItemPriceService = iItemPriceService;
            this.iInvWarehouseService = iInvWarehouseService;
            this.iInvStoreItemService = iInvStoreItemService;
            this.iOrganizationService = iOrganizationService;
            this.iIOfficeService = iIOfficeService;
            this.iInvStoreService = iInvStoreService;
            this.iInv_VendorService = iInvVendorService;
            this.iEmployeeService = iEmployeeService;
        }
        #endregion Variables
        // GET: Common
        #region Common Setup
        [HttpGet]
        public ActionResult Category()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SubCategory()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Items()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Vendor()
        {
            return View();
        }
        #endregion Common Setup

        #region Web Service
        #region Category
        public JsonResult CategorySetup(Inv_CategoryOrSubCategory obj, string status, bool isAutoGenerate)
        {
            var result = 0;
            var message = "";
            try
            {
                var c = iCateService.GetAll().ToList();
                if (c.Any())
                    c = c.Where(x => x.ParentCategoryID == 0).ToList();
                if (status == "I")
                {
                    bool ins = false;

                    if (c.Any())
                    {
                        if (c.Where(x => x.CategorySubCategoryName == obj.CategorySubCategoryName && x.IsActive == true).Any())
                        { result = 0; message = "Category Name Already Exists"; }
                        else
                            ins = true;
                    }
                    else
                        ins = true;
                    if (ins)
                    {
                        int count = c.Count() + 1;
                        if (isAutoGenerate)
                            obj.CateorSubCateCode = count < 10 ? "0" + count.ToString() : count.ToString();
                        else if (string.IsNullOrEmpty(obj.CateorSubCateCode)) obj.CateorSubCateCode = c.Count() < 10 ? "0" + (c.Count() + 1).ToString() : (c.Count() + 1).ToString();

                        if (c.Where(x => x.CateorSubCateCode == obj.CateorSubCateCode).Any())
                        { result = 0; message = "Category Code Already Exists"; }
                        else
                        {
                            obj.CreateBy = LoginUserOfficeID;
                            obj.CreateDate = DateTime.UtcNow;
                            obj.IsActive = true;
                            iCateService.Create(obj);
                            result = 1;
                            message = "Save Success";
                        }
                    }
                }
                else if (status == "U")
                {
                    if (c.Where(x => x.CategorySubCategoryName == obj.CategorySubCategoryName && x.NameInBangla == obj.NameInBangla && x.IsActive == true).Any())
                    { result = 0; message = "Category Name Already Exists"; }
                    else
                    {
                        var cata = iCateService.GetById(obj.CategorySubCategoryID);
                        cata.CategorySubCategoryName = obj.CategorySubCategoryName;
                        cata.NameInBangla = obj.NameInBangla;
                        cata.UpdateBy = LoginUserOfficeID;
                        cata.UpdateDate = DateTime.UtcNow;
                        cata.IsActive = true;
                        iCateService.Update(cata);
                        result = 1;
                        message = "Update Success";
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCategory()
        {
            var cateList = iCateService.GetAll().ToList();
            if (cateList.Any())
                cateList = cateList.Where(x => x.IsActive == true && x.ParentCategoryID == 0).ToList();
            else
                cateList = new List<Inv_CategoryOrSubCategory>();
            var currentPageRecords = Mapper.Map<IEnumerable<Inv_CategoryOrSubCategory>, IEnumerable<Inv_CategoryOrSubCategory>>(cateList);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = cateList.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByCategoryID(int ids, string status)
        {
            var cata = iCateService.GetById(ids);
            if (status == "u")
                return Json(new { Result = "OK", Records = cata }, JsonRequestBehavior.AllowGet);
            else if (status == "d")
            {
                cata.IsActive = false;
                iCateService.Update(cata);
                return Json(new { message = "Delete Successfull" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion Category

        #region SubCategory
        public JsonResult SubCategorySetup(Inv_CategoryOrSubCategory obj, string status, bool isAutoGenerate)
        {
            var result = 0;
            var message = "";
            try
            {
                var c = iCateService.GetAll().ToList();
                if (c.Any())
                    c = c.Where(x => x.ParentCategoryID > 0).ToList();
                if (status == "I")
                {
                    bool ins = false;
                    if (c.Any())
                    {
                        if (c.Where(x => x.CategorySubCategoryName == obj.CategorySubCategoryName && x.IsActive == true).Any())
                        { result = 0; message = "Subcategory Name Already Exists"; }
                        else
                            ins = true;
                    }
                    else
                        ins = true;
                    if (ins)
                    {
                        int count = c.Count() + 1;
                        if (isAutoGenerate)
                            obj.CateorSubCateCode = count < 10 ? "0" + count.ToString() : count.ToString();
                        else if (string.IsNullOrEmpty(obj.CateorSubCateCode)) obj.CateorSubCateCode = c.Count() < 10 ? "0" + (c.Count() + 1).ToString() : (c.Count() + 1).ToString();

                        if (c.Where(x => x.CateorSubCateCode == obj.CateorSubCateCode).Any())
                        { result = 0; message = "Subcategory Code Already Exists"; }
                        else
                        {
                            obj.CreateBy = LoginUserOfficeID;
                            obj.CreateDate = DateTime.UtcNow;
                            obj.IsActive = true;
                            iCateService.Create(obj);
                            result = 1;
                            message = "Save Success";
                        }
                    }
                }
                else if (status == "U")
                {
                    if (c.Where(x => x.CategorySubCategoryName == obj.CategorySubCategoryName && x.NameInBangla == obj.NameInBangla && x.IsActive == true).Any())
                    { result = 0; message = "Subcategory Name Already Exists"; }
                    else
                    {
                        var cata = iCateService.GetById(obj.CategorySubCategoryID);
                        cata.ParentCategoryID = obj.ParentCategoryID;
                        cata.CategorySubCategoryName = obj.CategorySubCategoryName;
                        cata.NameInBangla = obj.NameInBangla;
                        cata.UpdateBy = LoginUserOfficeID;
                        cata.UpdateDate = DateTime.UtcNow;
                        cata.IsActive = true;
                        iCateService.Update(cata);
                        result = 1;
                        message = "Update Success";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSubCategory()
        {
            var param = new { @isactive = true };
            var subList = iCateService.GetAllSubCategory(param);
            var currentPageRecords = Mapper.Map<IEnumerable<Inv_SubcategoryViewModel>, IEnumerable<Inv_SubcategoryViewModel>>(subList);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = subList.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBySubCategoryID(int ids, string status)
        {
            var cata = iCateService.GetById(ids);
            if (status == "u")
                return Json(new { Result = "OK", Records = cata }, JsonRequestBehavior.AllowGet);
            else if (status == "d")
            {
                cata.IsActive = false;
                iCateService.Update(cata);
                return Json(new { message = "Delete Successfull" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        // Category ID wise Subcategory Load 
        public JsonResult GetSubCategoryXCategory(int cID)
        {
            var cate = iCateService.GetAll();
            if (cate.Any())
                cate = cate.Where(x => x.IsActive == true && x.ParentCategoryID == cID);
            var currentPageRecords = Mapper.Map<IEnumerable<Inv_CategoryOrSubCategory>, IEnumerable<Inv_CategoryOrSubCategory>>(cate);
            return Json(new { Result = "OK", Records = currentPageRecords }, JsonRequestBehavior.AllowGet);
        }
        #endregion SubCategory

        #region Item
        public JsonResult AddUpdateItem(Inv_Items obj, bool isAutoGenerate)
        {
            var result = 0;
            var message = "";
            try
            {
                // Update
                if (obj.ItemID > 0)
                {
                    var itemObj = iITemService.GetById(obj.ItemID);
                    itemObj.ItemName = obj.ItemName;
                    itemObj.ItemNameInBangle = obj.ItemNameInBangle;
                    itemObj.ItemShortName = obj.ItemShortName;
                    itemObj.SubCatagoryID = obj.SubCatagoryID;
                    itemObj.CategoryID = obj.CategoryID;
                    itemObj.ItemDetails = obj.ItemDetails;
                    itemObj.Unit = obj.Unit;
                    itemObj.UpdateBy = LoginUserOfficeID;
                    itemObj.UpdateDate = DateTime.UtcNow;
                    itemObj.MinStockLevel = obj.MinStockLevel;
                    iITemService.Update(itemObj);
                    result = 1;
                    message = "Update Success";

                }
                // Insert
                else
                {
                    var itemList = iITemService.GetAll();
                    string code = ""; bool s = false;
                    if (obj.CategoryID > 0)
                    {
                        string c = iCateService.GetById(obj.CategoryID.Value).CateorSubCateCode;
                        code = (string.IsNullOrEmpty(c) ? "00" : c);
                    }

                    if (obj.SubCatagoryID > 0)
                    {
                        string c = iCateService.GetById(obj.SubCatagoryID.Value).CateorSubCateCode;
                        code += (string.IsNullOrEmpty(c) ? "00" : c);
                    }

                    if (itemList.Any())
                        s = true;
                    else
                        s = true;
                    if (s)
                    {
                        int count = itemList.Count() + 1;
                        if (isAutoGenerate)
                            obj.ItemCode = code + (count < 10 ? "00" + count.ToString() : count < 100 ? "0" + count.ToString() : count.ToString());
                        else if (string.IsNullOrEmpty(obj.ItemCode))
                            obj.ItemCode = code + (count < 10 ? "00" + count.ToString() : count < 100 ? "0" + count.ToString() : count.ToString());
                    }
                    obj.CreateBy = LoginUserOfficeID;
                    obj.CreateDate = DateTime.UtcNow;
                    iITemService.Create(obj);
                    result = 1;
                    message = "Save Success";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByItemID(int itemID, string status)
        {
            var item = iITemService.GetById(itemID);
            if (status == "u")
                return Json(new { Result = "OK", Records = item }, JsonRequestBehavior.AllowGet);
            else if (status == "d")
            {
                item.IsActive = false;
                iITemService.Update(item);
                return Json(new { message = "Delete Successfull" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllItem(int? jtStartIndex = null, int? jtPageSize = null, string jtSorting = null)
        {
            var param = new { @itemID = 0 };
            var objList = iITemService.GetAllItems(param).OrderBy(x => x.ItemID).ToList();
            var filterList = new List<Inv_ItemViewModel>();
            if (jtStartIndex.HasValue && jtPageSize.HasValue)
                filterList = objList.Skip(jtStartIndex.Value).Take(jtPageSize.Value).ToList();
            else
                filterList = objList;

            var currentPageRecords = Mapper.Map<IEnumerable<Inv_ItemViewModel>, IEnumerable<Inv_ItemViewModel>>(filterList);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objList.Count() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemXQty(int itemid, int wid, int qty)
        {
            try
            {
                var sqty = iInvStoreService.GetMany(x => x.WarehouseID == wid && x.StockType == "I" && x.ItemID == itemid && x.StockBalance > 0).Sum(x => x.StockBalance);

                if (sqty >= qty)
                    return Json(new { Msg = 1 }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Msg = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemXCategory(int categoryid)
        {
            var itemList = iITemService.GetMany(x => x.CategoryID == categoryid);

            return Json(new { Result = "OK", Records = itemList }, JsonRequestBehavior.AllowGet);
        }

        #region Unit Price
        public JsonResult AddUnitPrice(Inv_ItemPriceDetails obj)
        {
            var result = 0;
            var message = "";
            try
            {
                // obj = new Inv_ItemPriceDetails();
                //obj.ItemID = itemID;
                //obj.OfficeID = 0;
                //obj.UnitPrice = uprice;
                //obj.EffectiveDate = DateTime.Parse(effectiveDate);
                obj.ChangeBy = LoginUserOfficeID;
                obj.ChangeDate = DateTime.UtcNow;
                iItemPriceService.Create(obj);
                result = 1;
                message = "Price Add Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion Unit Price
        #endregion Item

        #region Vendor
        public JsonResult VendorSetup(Inv_Vendor obj, string status)
        {
            var result = 0;
            var message = "";
            try
            {
                if (status == "I")
                {
                    var v = iInv_VendorService.GetMany(x => x.VendorName == obj.VendorName && x.WharehouseID == obj.WharehouseID);

                    if (v.Any())
                    {
                        result = 0; message = "Vendor Name Already Exists";
                    }
                    else
                    {
                        obj.CreateBy = LoginUserOfficeID;
                        obj.CreateDate = DateTime.UtcNow;
                        iInv_VendorService.Create(obj);
                        result = 1;
                        message = "Save Success";
                    }
                }
                else if (status == "U")
                {
                    var v = iInv_VendorService.GetById(obj.VendorID);
                    v.IsActive = obj.IsActive;
                    v.Address = obj.Address;
                    v.MobileNo = obj.MobileNo;
                    v.VendorName = obj.VendorName;
                    v.UpdateBy = LoginUserOfficeID;
                    v.UpdateDate = DateTime.UtcNow;
                    iInv_VendorService.Update(v);
                    result = 1;
                    message = "Update Success";
                }
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllVendor()
        {
            List<VendorViewModel> objList = new gBankerDbContext().Database.SqlQuery<VendorViewModel>(
                "SELECT v.*,w.WarehouseName FROM Inv_Vendor v INNER JOIN Inv_Warehouse w ON v.WharehouseID=w.WarehouseID WHERE v.IsActive=1 AND w.OfficeID={0}"
                , LoginUserOfficeID).ToList();
            var currentPageRecords = Mapper.Map<IEnumerable<VendorViewModel>, IEnumerable<VendorViewModel>>(objList);

            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = objList.Count() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByVendorID(int ids, string status)
        {
            var v = iInv_VendorService.GetById(ids);
            if (status == "u")
                return Json(new { Result = "OK", Records = v }, JsonRequestBehavior.AllowGet);
            else if (status == "d")
            {
                v.IsActive = false;
                iInv_VendorService.Update(v);
                return Json(new { message = "Delete Successfull" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { message = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion Vendor
        public JsonResult GetAllEmployeeCodeXMaping(string empcode)
        {
            List<int> officeArr = new List<int>();
            IEnumerable<string> empCodeArr;
            int officeID = (LoginUserOfficeID ?? 0);
            if (officeID == 1)
                empCodeArr = iEmployeeService.GetMany(x => x.IsActive == true && x.EmployeeCode.Contains(empcode)).Select(x => x.EmployeeCode);
            else
            {
                officeArr.Add((LoginUserOfficeID ?? 0));
                empCodeArr = iEmployeeService.GetMany(x => x.IsActive == true && x.EmployeeCode.Contains(empcode) && officeArr.Contains(x.OfficeID)).Select(x => x.EmployeeCode);
            }

            return Json(new { Result = "OK", Records = empCodeArr }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBranch(string officeCode)
        {
            string msg = "";
            var offList = new List<string>();
            try
            {
                int officeID = (LoginUserOfficeID ?? 0);
                if (officeID == 0)
                    msg = "LogOut";
                else
                {
                    if (string.IsNullOrEmpty(officeCode) | string.IsNullOrWhiteSpace(officeCode))
                        msg = "";
                    else
                    {
                        if (officeCode.Length > 1)
                        {
                            var office = iIOfficeService.GetById(officeID);
                            if (office.OfficeLevel == 1)
                                offList = iIOfficeService.GetMany(x => (x.OfficeLevel == 1 || x.OfficeLevel == 2 || x.OfficeLevel == 4)
                                && x.FirstLevel == office.OfficeCode
                                && (x.OfficeCode.ToLower().Contains(officeCode.ToLower())
                                || x.OfficeName.ToLower().Contains(officeCode.ToLower()))
                                ).Select(x => x.OfficeCode + '-' + x.OfficeName).ToList();
                            else if (office.OfficeLevel == 2)
                            {
                                offList = iIOfficeService.GetMany(x => x.OfficeLevel == 4
                                && x.SecondLevel == office.OfficeCode
                                && (x.OfficeCode.ToLower().Contains(officeCode.ToLower())
                                || x.OfficeName.ToLower().Contains(officeCode.ToLower()))
                                ).Select(x => x.OfficeCode + '-' + x.OfficeName).ToList();
                                offList.Add(office.OfficeCode + '-' + office.OfficeName);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                offList = new List<string>();
            }

            return Json(new { Message = msg, Records = offList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUpperOffice()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iIOfficeService.GetById(LoginUserOfficeID.Value);
                string offLabel = (off.OfficeLevel == 3 ? off.SecondLevel : off.OfficeLevel == 2 ? off.FirstLevel : "");
                off = iIOfficeService.GetByOfficeCode(offLabel);
                if (off != null)
                {
                    List<InvWarehouse> wObj = iInvWarehouseService.GetMany(x => x.OfficeID == off.OfficeID).ToList();
                    return Json(new { Result = "OK", Records = wObj }, JsonRequestBehavior.AllowGet);
                }

                else
                    return Json(new { Result = "Error", Message = "This is not Valid Office" }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { Result = "Relogin" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllZoneOffice()
        {
            var zone = iIOfficeService.GetAllZoneOffice("", LoggedInOrganizationID ?? 0);
            if (LoginUserOfficeID > 1)
            {
                if (zone.Where(x => x.OfficeID == LoginUserOfficeID).Any())
                    zone = zone.Where(x => x.OfficeID == LoginUserOfficeID);
            }
            var sl = zone.Select(s => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
            return Json(sl, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemSearchXCategory(int categoryID, string item)
        {
            string msg = "";
            var objList = new List<string>();
            try
            {
                int officeID = (LoginUserOfficeID ?? 0);
                if (officeID == 0)
                    msg = "LogOut";
                else
                {
                    if (string.IsNullOrEmpty(item) | string.IsNullOrWhiteSpace(item))
                        msg = "";
                    else
                    {
                        if (categoryID > 0)
                        {
                            if (item.Length > 1)
                                objList = iITemService.GetMany(x => x.CategoryID == categoryID
                                && (x.ItemName.ToLower().Contains(item.ToLower()) || x.ItemCode.Contains(item)))
                                .Select(s => s.ItemCode + "~" + s.ItemName).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
                objList = new List<string>();
            }

            return Json(new { Message = msg, Records = objList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemIDXCategoryANDItemInfo(int categoryID, string itemCode, string itemName)
        {
            string msg = "";
            Inv_Items obj = new Inv_Items();
            try
            {
                int officeID = (LoginUserOfficeID ?? 0);
                if (officeID == 0)
                    msg = "LogOut";
                else
                {
                    if (categoryID > 0)
                    {
                        if (string.IsNullOrEmpty(itemCode) | string.IsNullOrWhiteSpace(itemCode) | categoryID == 0)
                            msg = "Data Not Complete";
                        else
                            obj = iITemService.GetMany(x => x.CategoryID == categoryID && x.ItemCode == itemCode && x.ItemName == itemName).First();

                    }
                }
            }
            catch (Exception)
            {
                obj = new Inv_Items();
            }
            if (obj == null) obj = new Inv_Items();
            return Json(new { Message = msg, Records = obj }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemWisePriceAndQty(int categoryID, string itemCode, string itemName)
        {
            string msg = "";
            List<InvStoreExistPriceViewModel> obj = new List<InvStoreExistPriceViewModel>();
            try
            {
                int officeID = (LoginUserOfficeID ?? 0);
                if (officeID == 0)
                    msg = "LogOut";
                else
                {
                    if (categoryID > 0)
                    {
                        if (string.IsNullOrEmpty(itemCode) | string.IsNullOrWhiteSpace(itemCode) | categoryID == 0)
                            msg = "Data Not Complete";
                        else
                            obj = new gBankerDbContext().Database.SqlQuery<InvStoreExistPriceViewModel>("exec inv_sp_storePriceXItem " + officeID + ",'" + itemCode + "','" + itemName + "',"+categoryID+"").ToList();

                    }
                }
            }
            catch (Exception)
            {
                obj = new List<InvStoreExistPriceViewModel>();
            }
            if (obj == null) obj = new List<InvStoreExistPriceViewModel>();
            return Json(new { Message = msg, Records = obj }, JsonRequestBehavior.AllowGet);
        }
        #endregion WEbService

        #region
        public JsonResult NoGenerate(string prefix)
        {
            if (LoginUserOfficeID.HasValue)
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var req = db.Database.SqlQuery<string>("exec sp_Inv_NoGenerate {0},'" + prefix + "'", (LoginUserOfficeID ?? 0)).ToList();
                    return Json(new { status = 1, Result = req.First() }, JsonRequestBehavior.AllowGet);
                }
            }
            else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}