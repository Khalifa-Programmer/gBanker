using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
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
    public class SecurityController : BaseController
    {
        private readonly IAspNetRoleService roleService;
        private readonly ISecurityService securityService;
       // private readonly IOrganizationWiseRoleSecurityService OrganizationWiseRoleSecurityService;
        public SecurityController(IAspNetRoleService roleService, ISecurityService securityService)
        {
            this.roleService = roleService;
            this.securityService = securityService;
            //this.OrganizationWiseRoleSecurityService = OrganizationWiseRoleSecurityService;
        }
        //private readonly IAspNetRoleService roleService;
        //private readonly ISecurityService securityService;
        //public SecurityController(IAspNetRoleService roleService, ISecurityService securityService, IOrganizationWiseRoleSecurityService OrganizationWiseRoleSecurityService)
        //{
        //    this.roleService = roleService;
        //    this.securityService = securityService;
        //    this.OrganizationWiseRoleSecurityService = OrganizationWiseRoleSecurityService;
        //}
        public ActionResult UserRole()
        {
            return View();
        }
        //
        // GET: /Security/
        //Irfan
        public ActionResult UserRoleCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRoleCreate(FormCollection form)
        {
            return View();
        }

        public ActionResult RoleSecurity()
        {
            MapDropdownListValues();
            return View();
        }


        public JsonResult RoleSecurityGrid(int? parentMenuId, int? roleId)
        {

            //try
            //{

            //    IEnumerable<AspNetSecurityModule> modules;
            //    if (parentMenuId.HasValue && roleId.HasValue)
            //        modules = OrganizationWiseRoleSecurityService.GetAllModulesForParent(parentMenuId.Value, roleId.Value, LoggedInOrganizationID.Value);
            //    else
            //        modules = OrganizationWiseRoleSecurityService.GetAllPrentModule(LoggedInOrganizationID.Value).Where(m => m.AspNetSecurityModuleId != 1);
            //    var entites = Mapper.Map<IEnumerable<AspNetSecurityModule>, IEnumerable<AspNetSecurityModuleViewModel>>(modules);
            //    return Json(new { Result = "OK", Records = entites });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Result = "ERROR", Message = ex.Message });
            //}
            try
            {
                IEnumerable<AspNetSecurityModule> modules;
                if (parentMenuId.HasValue && roleId.HasValue)
                    //modules = securityService.GetAllModulesForParent(parentMenuId.Value, roleId.Value);
                    modules = securityService.GetAllModulesORgForParent(parentMenuId.Value, roleId.Value, Convert.ToInt16(LoggedInOrganizationID.Value));
                else
                    modules = securityService.GetAllPrentModule().Where(m => m.AspNetSecurityModuleId != 1);
                    //modules = securityService.GetAllPrentOrgModule(LoggedInOrganizationID).Where(m => m.AspNetSecurityModuleId != 1);
                var entites = Mapper.Map<IEnumerable<AspNetSecurityModule>, IEnumerable<AspNetSecurityModuleViewModel>>(modules);
                return Json(new { Result = "OK", Records = entites });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            //try
            //{
            //    IEnumerable<AspNetSecurityModule> modules;
            //    if (parentMenuId.HasValue && roleId.HasValue)
            //        modules = securityService.GetAllModulesForParent(parentMenuId.Value, roleId.Value);
            //    else
            //        modules = securityService.GetAllPrentModule().Where(m => m.AspNetSecurityModuleId != 1);
            //    var entites = Mapper.Map<IEnumerable<AspNetSecurityModule>, IEnumerable<AspNetSecurityModuleViewModel>>(modules);
            //    return Json(new { Result = "OK", Records = entites });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Result = "ERROR", Message = ex.Message });
            //}
        }
        [HttpPost]
        public JsonResult RoleSecurityCreate(Dictionary<string, bool> SelectedList, Dictionary<string, string> SecurityList, int roleId)
        {
            try
            {
                var roleModules = new List<AspNetRoleModule>();
                foreach (var module in SelectedList)
                {
                    var id = module.Key.Split("_".ToCharArray());
                    var selected = module.Value;
                    if (id.Length == 2 && id[0] == "chk")
                    {
                        var securityLevel = SecurityList.Where(w => w.Key.Split("_".ToCharArray()).Length == 2 && w.Key.Split("_".ToCharArray())[1] == id[1]).FirstOrDefault();
                        var lvlValue = int.Parse(securityLevel.Value);
                        var roleModule = new AspNetRoleModule() { RoleId = roleId.ToString(), ModuleId = int.Parse(id[1]), SecurityLevelId = lvlValue, IsActive = true, IsSelectedForRole = selected, CreatedBy = User.Identity.Name };
                        roleModules.Add(roleModule);
                    }

                }
                securityService.CreateSecurityRole(roleModules);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        private void MapDropdownListValues()
        {
            var roleList = roleService.GetAll().ToList();
            roleList.Insert(0, new AspNetRole() { Id = "0", Name = "Select Role" });
            ViewBag.RoleList = roleList.Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });
        }
    }
}
