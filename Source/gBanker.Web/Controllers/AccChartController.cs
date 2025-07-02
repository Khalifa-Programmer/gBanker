using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace gBanker.Web.Controllers
{
    public class AccChartController : BaseController
    {
        #region Variables
        private readonly IAccCategoryService accCategoryService;
        private readonly IAccChartService accChartService;
        private readonly IAccNoteService accNoteService;
        //private readonly IProductService productService;
        //private readonly ILoanSummaryService loanSummaryService;
        //private readonly IPurposeService purposeService;

        public AccChartController(IAccCategoryService accCategoryService, IAccChartService accChartService, IAccNoteService accNoteService)
        {
            this.accCategoryService = accCategoryService;
            this.accChartService = accChartService;
            this.accNoteService = accNoteService;
            //this.productService = productService;
            //this.loanSummaryService = loanSummaryService;
            //this.purposeService = purposeService;
        }
        #endregion
        [HttpPost]
        public JsonResult Edit(AccChartViewModel model)
        {
            try
            {
                var entity = accChartService.GetById(model.AccID);
                if (ModelState.IsValid)
                {
                    entity.AccName = model.AccName;
                    entity.IsTransaction = model.IsTransaction;
                    entity.OfficeLevel = model.OfficeLevel;
                    entity.ModuleID = model.ModuleID;
                    //entity.FirstLevel = model.FirstLevel;
                    //entity.SecondLevel = model.SecondLevel;
                    //entity.ThirdLevel = model.ThirdLevel;
                    //entity.FourthLevel = model.FourthLevel;
                    //entity.FifthLevel = model.FifthLevel;

                    if (model.NoteID != 0)
                        entity.NoteID = model.NoteID;
                    else
                        //entity.NoteID = null;
                        entity.NoteID = 0;

                    accChartService.Update(entity);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // KHALID
        public JsonResult GetChartList(string AccCode)
        {
            var acc = accChartService.GetAll().Where(m => m.AccLevel != 5).ToList();
            var accList = new List<AccChart>();
            accList = acc;
            var accChart = accList.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(AccCode.ToLower())).Select(m1 => new { m1.AccCode, AccName = string.Format("{0} | {1} | {2} | {3} | {4}", m1.AccCode, m1.AccName, m1.AccLevel, m1.Nature, m1.CategoryID) }).ToList();
            return Json(accChart, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateNWAccChart()
        {
            var model = new AccChartViewModel();
            MapDropDownList(model);
            model.IsTransaction = true;
            return View(model);

        }

        // KHALID
        public JsonResult SaveAccCodeNW(string ParentCode, string FirstLevel, string SecondLevel, string ThirdLevel, string FourthLevel, string AccCode, string AccName, string AccLevel, string Nature, bool IsTransaction, string CategoryID, string OfficeLevel, string ModuleID, string NoteID)
        {
            var entity = new AccChart();
            string result = "";
            string msg = "";
            if (accChartService.IsValidAccCode(AccCode))
            {
                if (FirstLevel != "")
                {
                    entity.FirstLevel = FirstLevel;
                    if (SecondLevel != "")
                    {
                        entity.SecondLevel = SecondLevel;
                        if (ThirdLevel != "")
                        {
                            entity.ThirdLevel = ThirdLevel;
                            if (FourthLevel != "")
                            {
                                entity.AccCode = AccCode;
                                entity.AccName = AccName;
                                entity.FourthLevel = FourthLevel;
                                entity.FifthLevel = AccCode;
                                entity.AccLevel = 5;
                            }
                            else
                            {
                                entity.AccCode = AccCode;
                                entity.AccName = AccName;
                                entity.FourthLevel = AccCode;
                                entity.AccLevel = 4;
                            }
                        }
                        else
                        {
                            entity.AccCode = AccCode;
                            entity.AccName = AccName;
                            entity.ThirdLevel = AccCode;
                            entity.AccLevel = 3;
                        }
                    }
                    else
                    {
                        entity.AccCode = AccCode;
                        entity.AccName = AccName;
                        entity.SecondLevel = AccCode;
                        entity.AccLevel = 2;
                    }
                }
                else
                {
                    entity.AccCode = AccCode;
                    entity.AccName = AccName;
                    entity.FirstLevel = AccCode;
                    entity.AccLevel = 1;
                }
                //entity.IsTransaction = Convert.ToBoolean(IsTransaction);
                entity.IsTransaction = IsTransaction;
                entity.CategoryID = Convert.ToInt32(CategoryID);
                entity.Nature = string.IsNullOrEmpty(Nature) ? "1" : Nature;
                entity.OfficeLevel = Convert.ToInt32(OfficeLevel);
                entity.ModuleID = Convert.ToInt32(ModuleID);
                if (NoteID != "" || Convert.ToInt32(NoteID) > 0)
                    entity.NoteID = Convert.ToInt32(NoteID);
                AccChartViewModel viewModel = new AccChartViewModel();
                entity.CreateUser = viewModel.CreateUser;
                entity.CreateDate = viewModel.CreateDate;
                entity.IsActive = true;
                entity.InActiveDate = viewModel.CreateDate;
                entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                var chk = new AccChart();
                chk = accChartService.Create(entity);

                //var emtpy = new AccChartViewModel();
                //MapDropDownList(emtpy);
                //result = "Saved Successfully.";

                if (chk.AccID > 0)
                    result = "S";
                else
                    result = "F";
            }
            else
            {
                result = "A";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Methods
        public JsonResult GetAccCodeList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;
                var codeDetail = accChartService.GetAccChartDetail(LoggedInOrganizationID, filterColumn, filterValue, out TotCount);
                var detail = codeDetail.ToList();                
                var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = TotCount });                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetAccNote()
        {

            try
            {
                var allNotes = accNoteService.GetAll().Where(m => m.IsActive == true && m.OrgID == LoggedInOrganizationID);
                var viewNote = allNotes.Select(x => x).ToList().Select(x => new ListItem
                {
                    Value = x.NoteID.ToString(),
                    Text = x.NoteNo.ToString() + " - " + x.NoteName.ToString()
                });
                var note_items = new List<ListItem>();
                note_items.Add(new ListItem() { Text = "Select None", Value = "0", Selected = true });
                note_items.AddRange(viewNote);              
                
                var noteList = note_items.ToList().Select(c => new { DisplayText = c.Text.ToString(), Value = c.Value }).OrderBy(s => s.Value);                

                return Json(new { Result = "OK", Options = noteList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteAccCode(int AccID)
        {
            try
            {
                //accChartService.Delete(AccID);
                var acc_Chart = accChartService.GetById(AccID);
                acc_Chart.IsActive = false;
                acc_Chart.InActiveDate = DateTime.Now;
                accChartService.Update(acc_Chart);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void MapDropDownList(AccChartViewModel model)
        {
            var allCategory = accCategoryService.GetAll();
            var viewCat = allCategory.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CategoryID.ToString(),
                Text = x.CategoryName.ToString()
            });
            var cat_items = new List<SelectListItem>();
            cat_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cat_items.AddRange(viewCat);
            model.CategoryList = cat_items;

            var offc_item = new List<SelectListItem>();
            offc_item.Add(new SelectListItem() { Text = "Branch Office", Value = "4" });
            offc_item.Add(new SelectListItem() { Text = "Area Office", Value = "3" });
            offc_item.Add(new SelectListItem() { Text = "Zone Office", Value = "2" });
            offc_item.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            model.OfficeList = offc_item;

            var module_item = new List<SelectListItem>();
            module_item.Add(new SelectListItem() { Text = "Accounting", Value = "1" });
            module_item.Add(new SelectListItem() { Text = "Portfolio", Value = "2" });
            module_item.Add(new SelectListItem() { Text = "Reconcile", Value = "8" });
            model.ModuleList = module_item;

            var allNotes = accNoteService.GetAll().Where(m => m.IsActive == true && m.OrgID == LoggedInOrganizationID);
            var viewNote = allNotes.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NoteID.ToString(),
                Text = x.NoteNo.ToString() + " - " + x.NoteName.ToString()
            });
            var note_items = new List<SelectListItem>();
            note_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            note_items.AddRange(viewNote);
            model.AccNoteList = note_items;


            //switch (s.InvestorID) { case 11: { return true; } default: return false; }
        }
        public JsonResult GetParentList(string acc_code)
        {
            var acc = accChartService.GetAll().Where(m => m.AccLevel != 5).ToList();
            var accList = new List<AccChart>();
            accList = acc;
            var accChart = accList.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName), m1.FirstLevel, m1.SecondLevel, m1.ThirdLevel, m1.FourthLevel, m1.FifthLevel }).ToList();
            return Json(accChart, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetChartList(string AccCode)
        //{
        //    var acc = accChartService.GetAll().Where(m => m.AccLevel != 5).ToList();
        //    var accList = new List<AccChart>();
        //    accList = acc;
        //    var accChart = accList.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(AccCode.ToLower())).Select(m1 => new { m1.AccCode, AccName = string.Format("{0} | {1} | {2} | {3} | {4}", m1.AccCode, m1.AccName, m1.AccLevel, m1.Nature, m1.CategoryID) }).ToList();
        //    return Json(accChart, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetParentCodeDetail(string ParentCode)
        {
            var acc = accChartService.GetByAccCode(ParentCode);
            if (acc != null)
            {
                var result = new { FirstLevel = acc.FirstLevel, SecondLevel = acc.SecondLevel, ThirdLevel = acc.ThirdLevel, FourthLevel = acc.FourthLevel, FifthLevel = acc.FifthLevel };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new { FirstLevel = "", SecondLevel = "", ThirdLevel = "", FourthLevel = "", FifthLevel = "" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveAccCode(string ParentCode, string FirstLevel, string SecondLevel, string ThirdLevel, string FourthLevel, string AccCode, string AccName, string AccLevel, string Nature, bool IsTransaction, string CategoryID, string OfficeLevel, string ModuleID, string NoteID)
        {
            string result = "";
            try
            {
                var entity = new AccChart();
                
                string msg = "";
                if (accChartService.IsValidAccCode(AccCode))
                {
                    if (FirstLevel != "")
                    {
                        entity.FirstLevel = FirstLevel;
                        if (SecondLevel != "")
                        {
                            entity.SecondLevel = SecondLevel;
                            if (ThirdLevel != "")
                            {
                                entity.ThirdLevel = ThirdLevel;
                                if (FourthLevel != "")
                                {
                                    entity.AccCode = AccCode;
                                    entity.AccName = AccName;
                                    entity.FourthLevel = FourthLevel;
                                    entity.FifthLevel = AccCode;
                                    entity.AccLevel = 5;
                                }
                                else
                                {
                                    entity.AccCode = AccCode;
                                    entity.AccName = AccName;
                                    entity.FourthLevel = AccCode;
                                    entity.AccLevel = 4;
                                }
                            }
                            else
                            {
                                entity.AccCode = AccCode;
                                entity.AccName = AccName;
                                entity.ThirdLevel = AccCode;
                                entity.AccLevel = 3;
                            }
                        }
                        else
                        {
                            entity.AccCode = AccCode;
                            entity.AccName = AccName;
                            entity.SecondLevel = AccCode;
                            entity.AccLevel = 2;
                        }
                    }
                    else
                    {
                        entity.AccCode = AccCode;
                        entity.AccName = AccName;
                        entity.FirstLevel = AccCode;
                        entity.AccLevel = 1;
                    }
                    //entity.IsTransaction = Convert.ToBoolean(IsTransaction);
                    entity.IsTransaction = IsTransaction;
                    entity.CategoryID = Convert.ToInt32(CategoryID);
                    entity.Nature = string.IsNullOrEmpty(Nature) ? "1" : Nature;
                    entity.OfficeLevel = Convert.ToInt32(OfficeLevel);
                    entity.ModuleID = Convert.ToInt32(ModuleID);
                    if (NoteID != "" || Convert.ToInt32(NoteID) > 0)
                        entity.NoteID = Convert.ToInt32(NoteID);
                    AccChartViewModel viewModel = new AccChartViewModel();
                    entity.CreateUser = viewModel.CreateUser;
                    entity.CreateDate = viewModel.CreateDate;
                    entity.IsActive = true;
                    entity.InActiveDate = viewModel.CreateDate;
                    entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                    var chk = new AccChart();
                    chk = accChartService.Create(entity);

                    //var emtpy = new AccChartViewModel();
                    //MapDropDownList(emtpy);
                    //result = "Saved Successfully.";

                    if (chk.AccID > 0)
                        result = "S";
                    else
                        result = "F";
                }
                else
                {
                    result = "A";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //    //return GetErrorMessageResult(e);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        // GET: AccChart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexNW()
        {
            return View();
        }

        // GET: AccChart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccChart/Create
        public ActionResult Create()
        {
            var model = new AccChartViewModel();
            MapDropDownList(model);
            model.IsTransaction = true;
            return View(model);

        }

        // POST: AccChart/Create
        [HttpPost]
        public ActionResult Create(AccChartViewModel model)
        {
            return View();
            //try
            //{
            //    var entity = Mapper.Map<AccChartViewModel, AccChart>(model);

            //    //Add Validlation Logic.

            //    if (ModelState.IsValid)
            //    {

            //        string msg = "";
            //        if (accChartService.IsValidAccCode(entity.AccCode))
            //        {
            //            if (model.FirstLevel != null)
            //            {
            //                entity.FirstLevel = model.FirstLevel;
            //                if (model.SecondLevel != null)
            //                {
            //                    entity.SecondLevel = model.SecondLevel;
            //                    if (model.ThirdLevel != null)
            //                    {
            //                        entity.ThirdLevel = model.ThirdLevel;
            //                        if(model.FourthLevel != null)
            //                        {
            //                            entity.FourthLevel = model.FourthLevel;
            //                            entity.FifthLevel = model.AccCode;
            //                            entity.AccLevel = 5;
            //                        }
            //                        else
            //                        {
            //                            entity.FourthLevel = model.AccCode;
            //                            entity.AccLevel = 4;
            //                        }                                 
            //                    }
            //                    else
            //                    {
            //                        entity.ThirdLevel = model.AccCode;
            //                        entity.AccLevel = 3;
            //                    }
            //                }
            //                else
            //                {
            //                    entity.SecondLevel = model.AccCode;
            //                    entity.AccLevel = 2;
            //                }
            //            }
            //            else
            //            {
            //                entity.FirstLevel = model.AccCode;
            //                entity.AccLevel = 1;
            //            }
            //            entity.IsTransaction = model.IsTransaction;
            //            accChartService.Create(entity);
            //            var emtpy = new AccChartViewModel();
            //            MapDropDownList(emtpy);

            //            return View(emtpy);
            //        }
            //        else
            //        {
            //            var emtpy = new AccChartViewModel();
            //            MapDropDownList(emtpy);
            //            ModelState.AddModelError("Validation", msg);
            //        }

            //    }
            //    MapDropDownList(model);
            //    return View(model);

            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: AccChart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //// POST: AccChart/Edit/5
        //[HttpPost]
        //public JsonResult Edit(AccChartViewModel model)
        //{
        //    try
        //    {
        //        var entity = accChartService.GetById(model.AccID);
        //        if (ModelState.IsValid)
        //        {
        //            entity.AccName = model.AccName;
        //            entity.IsTransaction = model.IsTransaction;
        //            entity.OfficeLevel = model.OfficeLevel;
        //            entity.ModuleID = model.ModuleID;
        //            if (model.NoteID != 0)
        //                entity.NoteID = model.NoteID;
        //            else
        //                entity.NoteID = null;
        //            accChartService.Update(entity);
        //        }
        //        return Json(new { Result = "OK" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        // GET: AccChart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccChart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
