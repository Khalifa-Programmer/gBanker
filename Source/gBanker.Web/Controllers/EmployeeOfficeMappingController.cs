using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class EmployeeOfficeMappingController : BaseController
    {

        private readonly IOfficeService officeService;
        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        public EmployeeOfficeMappingController(IOfficeService officeService, IEmployeeOfficeMappingService employeeOfficeService)
        {
            this.officeService = officeService;
            this.employeeOfficeService = employeeOfficeService;
        }
        //
        // GET: /EmployeeOfficeMapping/
        public ActionResult Index()
        {
            MapDropdownValues();
            MapDropdownHeadValues();
            return View();
        }
        private void MapDropdownValues()
        {
            ViewBag.ZoneList = officeService.GetAllZoneOffice("100000",Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
           /// ViewBag.HeadList = officeService.GetHeadOffice("100000", Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
        }
        private void MapDropdownHeadValues()
        {
            ViewBag.HeadList = officeService.GetAllZoneOffice1("100000", Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
        }

        public JsonResult GetAreaList(string zoneCode)
        {
            try
            {
                var areaOffices = officeService.GetAllAreaOfficeForZone("100000", zoneCode, Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
                return Json(new { Result = "OK", Options = areaOffices }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }    
        }

        public JsonResult LoadOffice(string zoneCode, string areaCode, string employeeCode)
        {
            try
            {
                var offices = officeService.GetAllBranchesForArea("100000", zoneCode, areaCode, Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
                var employeeOfficeMappings = employeeOfficeService.GetEmployeeOfficeMappings(employeeCode).ToList();
                foreach (var empoff in employeeOfficeMappings)
                {
                    var mappingExits = offices.Where(w => w.Value == empoff.Office.OfficeID.ToString()).FirstOrDefault();
                    if (mappingExits != null)
                    {
                        mappingExits.Selected = true;
                    }
                }
                return Json(new { Result = "OK", Options = offices }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //
        // GET: /EmployeeOfficeMapping/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /EmployeeOfficeMapping/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Dictionary<string, bool> SelectedList, string employeeCode)
        {
            try
            {
                var mappings = new List<EmployeeOfficeMapping>();
                int OrgID = Convert.ToInt16(LoggedInOrganizationID);
                foreach (var module in SelectedList)
                {
                    var id = module.Key.Split("_".ToCharArray());
                    var selected = module.Value;
                    if (id.Length == 2 && id[0] == "chk" && selected == true)
                    {
                        var officeId = id[1];
                        
                        var mapping = new EmployeeOfficeMapping() { OfficeID = int.Parse(officeId), CreateDate = DateTime.Now, CreateUser = User.Identity.Name,OrgID=OrgID,IsSelected = selected };
                        mappings.Add(mapping);

                    }
                }
                
                employeeOfficeService.CreateEmployeeOfficeMapping(OrgID,employeeCode, mappings);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        //
        // GET: /EmployeeOfficeMapping/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /EmployeeOfficeMapping/Edit/5
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

        //
        // GET: /EmployeeOfficeMapping/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /EmployeeOfficeMapping/Delete/5
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
    }
}
