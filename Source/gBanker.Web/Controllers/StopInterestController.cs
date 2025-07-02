using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class StopInterestController : BaseController
    {
        private readonly IStopInterestService stopInterestService;
        private readonly IAccReportService accReportService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IProcessInfoService processInfoService;
        private readonly IUltimateReportService ultimateReportService;
        public StopInterestController(IStopInterestService stopInterestService, IAccReportService accReportService, IMemberService memberService, ICenterService centerService, IOfficeService officeService, IProcessInfoService processInfoService, IUltimateReportService ultimateReportService)
        {
            this.stopInterestService = stopInterestService;
            this.accReportService = accReportService;
            this.memberService = memberService;
            this.centerService = centerService;
            this.officeService = officeService;
            this.processInfoService = processInfoService;
            this.ultimateReportService = ultimateReportService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var message = "";
            var checkClosingDate = processInfoService.CheckClosingDtByOfficeId((int)LoginUserOfficeID);
            if (checkClosingDate != null)
            {
                message = "Please day close before stop interest form entry, back to the main page";
                return Json(new { message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = new StopInterestViewModel();
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                var lastClosingDate = accReportService.GetLastClosingDate(param);

                if (!IsDayInitiated)
                {
                    model.StopInterestDate = Convert.ToDateTime(lastClosingDate.Tables[0].Rows[0][1].ToString());
                }
                else
                {
                    model.StopInterestDate = Convert.ToDateTime(lastClosingDate.Tables[0].Rows[0][1].ToString());
                }
                MapDropDownList(model);
                return View(model);
            }

        }
        private void MapDropDownList(StopInterestViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;
            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            //var prodTypeList = new List<SelectListItem>();
            //prodTypeList.Add(new SelectListItem { Text = "Please Select", Value = "", Selected = true });
            //prodTypeList.Add(new SelectListItem { Text = "Loan", Value = "1" });
            //prodTypeList.Add(new SelectListItem { Text = "Savings", Value = "0" });
            //model.ProductTypeList = prodTypeList;
        }

        public JsonResult GetProductList(int prodType)
        {
            var param = new { prodType = prodType };
            var getProduct = ultimateReportService.GetDataWithParameter(param, "GetProductList");
            var viewProduct = getProduct.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Field<string>("ProductName"),
                Value = Convert.ToInt16(p.Field<int>("ProductID")).ToString()
            }).ToList();

            return Json(viewProduct, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, StopInterestViewModel model)
        {
            try
            {
                var entity = Mapper.Map<StopInterestViewModel, StopInterest>(model);
                if (ModelState.IsValid)
                {
                    entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);

                    entity.IsActive = true;
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = LoggedInEmployeeID.ToString();
                    var data = stopInterestService.Create(entity);

                    var getOfficeID = data.OfficeID;
                    var getCenterID = data.CenterID;
                    var getMemberID = data.MemberID;
                    var getProductID = data.ProductID;
                    var getProdType = data.ProdType;

                    var param = new
                    {
                        OfficeID = getOfficeID,
                        CenterID = getCenterID,
                        MemberID = getMemberID,
                        ProductID = getProductID,
                        ProdType = getProdType,
                    };
                    ultimateReportService.GetDataWithParameter(param, "SummaryUpdateByStopInterest");

                    return GetSuccessMessageResult("Data Saved Successfully");
                }
                else
                    return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }            
        }
        public JsonResult GetStopInterestDetails(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                var param = new { OfficeID = LoginUserOfficeID };
                var getData = ultimateReportService.GetDataWithParameter(param, "GetStopInterestDetails");
                var viewData = getData.Tables[0].AsEnumerable().Select(p => new StopInterestViewModel
                {
                    StopInterestID = p.Field<long>("StopInterestID"),
                    OfficeName = p.Field<string>("OfficeName"),
                    CenterName = p.Field<string>("CenterName"),
                    MemberName = p.Field<string>("MemberName"),
                    ProductName = p.Field<string>("ProductName"),
                    StopInterestDateView = p.Field<string>("StopInterestDate"),
                    Remarks = p.Field<string>("Remarks")
                }).ToList();
                
                var TotalCount = viewData.Count();
                var currentPageRecords = viewData.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

    }
}