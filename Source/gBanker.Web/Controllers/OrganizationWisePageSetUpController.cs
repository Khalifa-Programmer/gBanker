using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.Controllers
{
    public class OrganizationWisePageSetUpController : BaseController
    {
        private readonly IAspNetRoleService roleService;
        private readonly ISecurityService securityService;
        private readonly IAspNetOrgModuleService OrgModuleService;
        private readonly IPageWiseSecurityService pageWiseSecurityService;
        public OrganizationWisePageSetUpController(IAspNetRoleService roleService, ISecurityService securityService, IAspNetOrgModuleService OrgModuleService,IPageWiseSecurityService pageWiseSecurityService)
        {
            this.roleService = roleService;
            this.securityService = securityService;
            this.OrgModuleService = OrgModuleService;
            this.pageWiseSecurityService = pageWiseSecurityService;
        }
        // GET: OrganizationWisePageSetUp
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult RoleSecurityGrid(int? parentMenuId, int? roleId)
        {

            try
            {
                IEnumerable<AspNetSecurityModule> modules;
                if (parentMenuId.HasValue && LoggedInOrganizationID.HasValue)

                    modules = securityService.GetAllModulesForParentOrganizationWise(parentMenuId.Value, Convert.ToInt16(LoggedInOrganizationID.Value), Convert.ToInt16(LoggedInOrganizationID.Value));
                else
                    modules = securityService.GetAllPrentModuleOrganizationWise(LoggedInOrganizationID).Where(m => m.AspNetSecurityModuleId != 1);
                var entites = Mapper.Map<IEnumerable<AspNetSecurityModule>, IEnumerable<AspNetSecurityModuleViewModel>>(modules);
                return Json(new { Result = "OK", Records = entites });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult RoleSecurityCreate(Dictionary<string, bool> SelectedList, Dictionary<string, string> SecurityList)
        {
            try
            {
                var roleModules = new List<AspNetOrgModule>();
                foreach (var module in SelectedList)
                {
                    var id = module.Key.Split("_".ToCharArray());
                    var selected = module.Value;
                    if (id.Length == 2 && id[0] == "chk")
                    {
                        var securityLevel = SecurityList.Where(w => w.Key.Split("_".ToCharArray()).Length == 2 && w.Key.Split("_".ToCharArray())[1] == id[1]).FirstOrDefault();
                       // var lvlValue = int.Parse(securityLevel.Value);
                        var roleModule = new AspNetOrgModule() { OrgID = int.Parse(LoggedInOrganizationID.ToString()), AspNetSecurityModuleId = int.Parse(id[1]), IsSelectedForRole = selected };
                        roleModules.Add(roleModule);
                    }

                }
                pageWiseSecurityService.CreateSecurityRole(roleModules);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        // GET: OrganizationWisePageSetUp/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrganizationWisePageSetUp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizationWisePageSetUp/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrganizationWisePageSetUp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrganizationWisePageSetUp/Edit/5
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

        // GET: OrganizationWisePageSetUp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrganizationWisePageSetUp/Delete/5
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
