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
    public class GroupController : BaseController
    {
        private readonly IGroupService groupService;
        private readonly IOfficeService officeService;
        public GroupController(IGroupService groupService, IOfficeService officeService)
          {
              this.groupService = groupService;
              this.officeService = officeService;
             
          }
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGroupInformation(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                
                var groupInfo = groupService.GetGroupDetail(Convert.ToInt16(LoggedInOrganizationID),LoginUserOfficeID);
                var totalCount = groupInfo.Count();
                var entities = groupInfo.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<DBGroupDeatil>, IEnumerable<GroupViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(GroupViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var statusMode = new List<SelectListItem>();
            statusMode.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            statusMode.Add(new SelectListItem() { Text = "InActive", Value = "0" });
            model.StatusMode = statusMode.AsEnumerable();

        }
        // GET: Group/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Group/Create
        public ActionResult Create()
        {
            var model = new GroupViewModel();

            model.GroupStatus = 1;
            MapDropDownList(model);
            return View(model);
        }

        // POST: Group/Create
        [HttpPost]
        public ActionResult Create(GroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<GroupViewModel, Group>(model);
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = groupService.IsValidGroup(entity);
                    if (errors.ToList().Count == 0)
                    {

                        //Add Validlation Logic.
                        entity.IsActive = true;
                        entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);

                        groupService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                    {
                      
                        return GetErrorMessageResult(errors);
                    }
                }
                return GetErrorMessageResult();
               

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int id)
        {
            if (groupService.IsContinued(id))
            {
                var group = groupService.GetById(id);
                var entity = Mapper.Map<Group, GroupViewModel>(group);
                MapDropDownList(entity);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Invalid Group");
            return RedirectToAction("Index");
        }

        // POST: Group/Edit/5
        [HttpPost]
        public ActionResult Edit(GroupViewModel model)
        {
            try
            {
                var entity = Mapper.Map<GroupViewModel, Group>(model);
                var GetGroupDetail = groupService.GetById(entity.GroupID);
                if (ModelState.IsValid)
                {
                    GetGroupDetail.FormationDate = entity.FormationDate;
                    GetGroupDetail.GroupStatus = entity.GroupStatus;
                    groupService.Update(GetGroupDetail);
                    return GetSuccessMessageResult();
                }
                // TODO: Add update logic here

                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Group/Delete/5
        public ActionResult Delete(int id)
        {
            var group = groupService.GetById(id);
            var entity = Mapper.Map<Group, GroupViewModel>(group);
            return View(entity);
        }

        // POST: Group/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                groupService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
