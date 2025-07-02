using AutoMapper;
//using gBanker.Data.Db;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class HolidayController : BaseController
    {
        #region variables
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IHolidayService holidayService;
        private readonly IUltimateReportService ultimateReportService;
        public HolidayController(IHolidayService holidayService, IOfficeService officeService, ICenterService centerService, IUltimateReportService ultimateReportService)
        {
            this.holidayService = holidayService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(HolidayViewModel model)
        {
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID==LoggedInOrganizationID).ToList();
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            //if (model.OfficeID > 0) 
            //{ 
            var allCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            center_items.AddRange(viewCenter);
            model.CenterList = center_items;
            //}
            //else
            //{
            //    var empty = new HolidayViewModel();            
            //    var blnk_items = new List<SelectListItem>();
            //    model.CenterList = blnk_items;
            //}

            var holiday_type = new List<SelectListItem>();
            holiday_type.Add(new SelectListItem() { Text = "Govt. Holiday", Value = "Govt" });
            holiday_type.Add(new SelectListItem() { Text = "Official", Value = "Office" });
           // holiday_type.Add(new SelectListItem() { Text = "Weekly", Value = "Weekly" });
            model.HolidayTypeList = holiday_type;

            var weekly_day = new List<SelectListItem>();
            weekly_day.Add(new SelectListItem() { Text = "Friday", Value = "Friday" });
            weekly_day.Add(new SelectListItem() { Text = "Saturday", Value = "Saturday" });
            weekly_day.Add(new SelectListItem() { Text = "Sunday", Value = "Sunday" });
            model.WeeklyList = weekly_day;

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCenterList(string office_id)
        {
            if (String.IsNullOrEmpty(office_id))
            {
                throw new ArgumentNullException("office_id");
            }
            var getCenterByOfficeId = centerService.GetAll().Where(m => m.OfficeID == Convert.ToInt32(office_id) && m.OrgID == Convert.ToInt16(LoggedInOrganizationID)).OrderBy(m => m.CenterCode);
            var viewCenter = getCenterByOfficeId.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = string.Format("{0}, {1}", x.CenterCode, x.CenterName)
            });
            var center_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHolidayInfo(int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null,string filterValue=null)
        {
            try
            {
                IEnumerable<DBholidayDetailModel> getCenterByOfficeId;
                var Qtype="";
                //if (filterValue==null)
                //{
                //    var param3 = new { @OfficeID = LoginUserOfficeID, @OrgID = LoggedInOrganizationID, @Qtype = 1, @SearchDate = filterValue };
                //    var getMemberTolrecordEmp = ultimateReportService.GetHolidayListSearch(param3);
                //    getCenterByOfficeId = getMemberTolrecordEmp;
                //    var totCount = getCenterByOfficeId.Count();
                //    var currentPageRecords = getCenterByOfficeId.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

                //}
                //else
                //{
                //    var param3 = new { @OfficeID = LoginUserOfficeID, @OrgID = LoggedInOrganizationID, @Qtype = 2, @SearchDate = filterValue };
                //    var getMemberTolrecordEmp = ultimateReportService.GetHolidayListSearch(param3);
                //    getCenterByOfficeId = getMemberTolrecordEmp;
                //    var totCount = getCenterByOfficeId.Count();
                //    var currentPageRecords = getCenterByOfficeId.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

                //}
                if (filterValue == "")
                {
                    Qtype = "1";
                }
                else
                {
                    Qtype = "2";

                }

                var param3 = new { @OfficeID = LoginUserOfficeID, @OrgID = LoggedInOrganizationID, @Qtype = Qtype, @SearchDate = filterValue };
                var getMemberTolrecordEmp = ultimateReportService.GetHolidayListSearch(param3);
                getCenterByOfficeId = getMemberTolrecordEmp;
                var totCount = getCenterByOfficeId.Count();
                var currentPageRecords = getCenterByOfficeId.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


                //getCenterByOfficeId = centerService.GetAll().Where(m => m.OfficeID == Convert.ToInt32(office_id) && m.OrgID == Convert.ToInt16(LoggedInOrganizationID) && m.CenterStatus  != 0 && m.EmployeeId == LoggedInEmployeeID).OrderBy(m => m.CenterCode);


                //var holidayDetail = holidayService.GetHolidayDetail(LoggedInOrganizationID.Value);


                // var detail = holidayDetail.ToList().Where(w => w.OfficeID == LoginUserOfficeID).OrderBy(o => o.CenterID).ThenBy(o => o.BusinessDate); ;
                //return Json(new { Result = "OK", Records = detail });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //public JsonResult SaveHoliday(string HolidayType, string BusinessDate, string Description)
        //{
        //    var result = "Data saved successfully.";

        //    try
        //    {
        //        var param = new { HolidayType = HolidayType, BusinessDate = BusinessDate, Description = Description };
        //        ultimateReportService.GetDataWithParameter(param, "InsertHolidayAllBranch");
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public List<DateTime> GetDaysOfYear(DateTime FromDt, DateTime ToDt, DayOfWeek day)
        {
            List<DateTime> result = new List<DateTime>();
            for (DateTime t = new DateTime(FromDt.Year, FromDt.Month, 1); t <= ToDt; t = t.AddDays(1))
            {
                if (t.DayOfWeek == day)
                {
                    result.Add(t);
                }
            }
            return result;
        }
        public JsonResult SaveYearlyHoliday(string hYear, string week_day, string holiday_desc, string offcId)
        {
            try
            {
                DayOfWeek day;
                Holiday save_val = new Holiday();
                string Result = "";
                switch (week_day)
                {
                    case "Friday":
                        day = DayOfWeek.Friday;
                        break;
                    case "Saturday":
                        day = DayOfWeek.Saturday;
                        break;
                    case "Sunday":
                        day = DayOfWeek.Sunday;
                        break;
                    default:
                        day = DayOfWeek.Friday;
                        break;
                }
                var allCenter = centerService.GetAll().Where(s => s.OfficeID == Convert.ToInt32(offcId));
                var HolidayList = new List<Holiday>();
                List<DateTime> daysInYear = GetDaysOfYear(Convert.ToDateTime("01-Jan-" + hYear), Convert.ToDateTime("31-Dec-" + hYear), day);

                Holiday entry = new Holiday();
                var errors = holidayService.IsValidHoliday(entry);
                if (errors.ToList().Count == 0)
                {

                    holidayService.SetHoliDay(LoginUserOfficeID, LoggedInOrganizationID, TransactionDate, "Y", holiday_desc, week_day, Convert.ToString(hYear), Convert.ToString(SessionHelper.LoggedInEmployeeID.ToString()));
    
                }

                //foreach (var center_id in allCenter.Select(s => s.CenterID))
                //{
                //    foreach (DateTime dt in daysInYear)
                //    {
                //        try
                //        {
                //            Holiday entry = new Holiday();
                //            entry.BusinessDate = dt;
                //            entry.OfficeID = Convert.ToInt32(offcId);
                //            entry.CenterID = center_id;
                //            entry.Description = holiday_desc;
                //            entry.HolidayType = "Weekly";
                //            entry.IsActive = true;
                //            entry.CreateUser = SessionHelper.LoggedInEmployeeID.ToString();
                //            entry.CreateDate = DateTime.Now;
                //            ///OrgID ---Updated-----------------------
                //            entry.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                //            var errors = holidayService.IsValidHoliday(entry);
                //            if (errors.ToList().Count == 0)
                //            {
                //                holidayService.Create(entry);
                //                if(entry.HolidayID > 0)
                //                    HolidayList.Add(entry);
                //            }
                //        }
                //        catch(Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //}


                if (HolidayList.Count > 0)
                {
                   //holidayService.SaveYearlyHoliday(HolidayList);
                   Result = "S";
                }
                else
                    Result = "F";

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = ex.ToString() });
            }
        }
        #endregion

        #region Events
        // GET: Holiday
        public ActionResult Index()
        {
            return View();
        }

        // GET: Holiday/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Holiday/Create
        public ActionResult Create()
        {
            var model = new HolidayViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }
        // GET: Holiday/Create
        //public ActionResult CreateNW()
        //{
        //    var model = new HolidayViewModel();
        //    MapDropDownList(model);
        //    model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
        //    return View(model);
        //}
        public ActionResult YearlyCreate()
        {
            var model = new HolidayViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }
        // POST: Holiday/Create
        [HttpPost]
        public ActionResult Create(HolidayViewModel model)
        {
            try
            {
                var entity = Mapper.Map<HolidayViewModel, Holiday>(model);
                if (ModelState.IsValid)
                {
                    var errors = holidayService.IsValidHoliday(entity);
                    if (errors.ToList().Count == 0)
                    {

                        if (entity.Description == "")
                        {
                            return GetErrorMessageResult("Pls. Description");
                        }

                        if (entity.HolidayType=="Govt")
                        {
                            holidayService.SetHoliDay(LoginUserOfficeID, LoggedInOrganizationID, entity.BusinessDate, entity.HolidayType, entity.Description, entity.CreateUser, entity.CreateUser, entity.CreateUser);
                        }
                        //else if (entity.HolidayType == "Office")
                        //{
                        //    if(entity.CenterID==0)
                        //    {
                        //        return GetErrorMessageResult("Pls. select Center");
                        //    }

                        //    holidayService.SetHoliDay(LoginUserOfficeID, LoggedInOrganizationID, entity.BusinessDate, entity.HolidayType, entity.Description, entity.CreateUser, entity.CreateUser, entity.CreateUser);

                        //}
                        else
                        {
                            ///OrgID ---Updated-----------------------
                            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                            entity.IsActive = true;
                            holidayService.Create(entity);
                        }
                       
                        return GetSuccessMessageResult();
                        //return RedirectToAction("Index");
                    }
                    else
                        return GetErrorMessageResult(errors);
                    //else

                    //ModelState.AddModelError("Validation", "Duplicate Product, please enter a diferent product id and name.");
                }
                //var empty = new HolidayViewModel();
                //MapDropDownList(empty);
                //var blnk_items = new List<SelectListItem>();
                //empty.CenterList = blnk_items;

                //return View(empty);
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                //var empty = new HolidayViewModel();
                //MapDropDownList(empty);
                //return View(empty);
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Holiday/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult CreateNW()
        {
            var model = new HolidayViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }

        // POST: Holiday/Edit/5
        [HttpPost]
        public ActionResult Edit(HolidayViewModel model)
        {
            try
            {
                var entity = holidayService.GetById(model.HolidayID);
                if (ModelState.IsValid)
                {
                    entity.BusinessDate = model.BusinessDate;
                    entity.Description = model.Description;
                    entity.HolidayType = model.HolidayType;
                    entity.IsActive = Convert.ToBoolean(model.IsActive);
                    entity.InActiveDate = DateTime.Now;
                    holidayService.Update(entity);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: Holiday/Delete/5
        //public ActionResult Delete(int id)
        //{

        //    holidayService.Inactivate(id, null);
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(int id)
        {
            var param3 = new { @HolidayID = id, @OrgID = LoggedInOrganizationID, @DeleteBy = LoggedInEmployeeID };
            var getMemberTolrecordEmp = ultimateReportService.GetDataWithParameter(param3, "SP_HolydayDelete");

            return RedirectToAction("Index");
        }

        // POST: Holiday/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                holidayService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        public JsonResult SaveHoliday(string HolidayType, string BusinessDate, string Description)
        {
            var result = "Data saved successfully.";

            try
            {
                var param = new { HolidayType = HolidayType, BusinessDate = BusinessDate, Description = Description };
                ultimateReportService.GetDataWithParameter(param, "InsertHolidayAllBranch");
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
