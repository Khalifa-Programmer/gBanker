using gBanker.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Filters;
using gBanker.Web.Helpers;
using gBanker.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccountInfoController : Controller
    {
        private readonly IAspNetRoleService roleService;
        private readonly IEmployeeService employeeService;
        private UserManager<ApplicationUser> UserManager;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;

        public AccountInfoController(IAspNetRoleService roleService, IEmployeeService employeeService, UserManager<ApplicationUser> userManager, IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.roleService = roleService;
            this.employeeService = employeeService;
            this.UserManager = userManager;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
        }

        public ActionResult RegisterInfo()
        {
            MapDropdownListValues();

            return View();
        }

        private void MapDropdownListValues()
        {
            var roleList = roleService.GetAll().ToList();

            var specificRoles = new HashSet<string> { "3", "7", "11", "12","2" };

            roleList = roleList.Where(r => specificRoles.Contains(r.Id)).ToList();

            roleList.Insert(0, new AspNetRole() { Id = "0", Name = "Select Role" });

            ViewBag.RoleList = roleList.Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });
        }


        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> RegisterInfo(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = employeeService.GetByCode(model.UserName);
                if (employee != null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        EmployeeID = employee.EmployeeID,
                        FirstName = employee.EmpName,
                        RoleId = model.RoleId,
                        Email = model.Email,
                        UpdateDate = DateTime.Now.Date
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return Json(new { Result = "OK", Message = "Login Created successfully." }, JsonRequestBehavior.AllowGet);
                        //await SignInAsync(user, isPersistent: false);
                        //return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var msg = "";
                        foreach (var r in result.Errors)
                        {
                            msg = string.Format("{0}<br/>{1}", msg, r);
                        }
                        return Json(new { Result = "ERROR", Message = msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { Result = "ERROR", Message = "Invalid Employee Code." }, JsonRequestBehavior.AllowGet);
                // ModelState.AddModelError("UserName", "Invalid Employee Code.");
            }
            return Json(new { Result = "ERROR", Message = "Please correct required fields." }, JsonRequestBehavior.AllowGet);
        }


    }
}