using AutoMapper;
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
    //[Authorize]
    public class MemberCategoryController : BaseController
    {

         private readonly IMemberCategoryService memberCategoryService;
         public MemberCategoryController(IMemberCategoryService memberCategoryService)
         {
             this.memberCategoryService = memberCategoryService;
         }


        //
        // GET: /MemberCategory/
        public ActionResult Index()
        {
            //var allMemberCategory = memberCategoryService.GetAll();
            //var viewCategory = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(allMemberCategory);
            
            //return View(viewCategory);
            return View();
        }

        //
        public JsonResult GetMemberCategoryList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allMemberCategory = memberCategoryService.GetAll().Where(m => m.IsActive == true && m.OrgID==LoggedInOrganizationID).OrderBy(m => m.MemberCategoryCode);
                var totalCount = allMemberCategory.Count();
                var entities = allMemberCategory.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var entities = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(allMemberCategory);
                //return Json(new { Result = "OK", Records = entities });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: /MemberCategory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MemberCategory/Create
        public ActionResult Create()
        {
            
            ////===================================================
            ////Below line of code will set the default value in the view....
            //var defaultCategory = new MemberCategoryViewModel();
            //MapDropdown(defaultCategory);
            //return View(defaultCategory);

            return View();
        }

        //private void MapDropdown(MemberCategoryViewModel model)
        //{
        //    var allMemberCategory = memberCategoryService.GetAll();
        //    var viewCategory = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(allMemberCategory);

        //    model.CategoryList = viewCategory.Select(s => new SelectListItem() { Text = s.CategoryName, Value = s.MemberCategoryID.ToString() });

        //}

        //
        // POST: /MemberCategory/Create
        [HttpPost]
        public ActionResult Create(MemberCategoryViewModel model)
        {
            try
            {
                model.IsActive = true;
                model.InActiveDate = System.DateTime.Now;
                var entity = Mapper.Map<MemberCategoryViewModel, MemberCategory>(model);
                if (ModelState.IsValid)
                {
                    var errors = memberCategoryService.IsValidMemberCategory(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.OrgID = Convert.ToInt16( LoggedInOrganizationID);
                        memberCategoryService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //
        // GET: /MemberCategory/Edit/5
        public ActionResult Edit(int id)
        {
            var memberCategory = memberCategoryService.GetById(id);
            var entity = Mapper.Map<MemberCategory, MemberCategoryViewModel>(memberCategory);
            return View(entity);
        }

        //
        // POST: /MemberCategory/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberCategoryViewModel model)
        {
            try
            {
                var entity = Mapper.Map<MemberCategoryViewModel, MemberCategory>(model);
                var getMemberCategory = memberCategoryService.GetById(entity.MemberCategoryID);
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getMemberCategory.CategoryName = entity.CategoryName;
                    getMemberCategory.CategoryNameBng = entity.CategoryNameBng;
                    getMemberCategory.CategoryShortName = entity.CategoryShortName;
                    getMemberCategory.CategoryShortNameBng = entity.CategoryShortNameBng;
                    getMemberCategory.MemberCategoryCode = entity.MemberCategoryCode;
                    getMemberCategory.AdmissionFee = entity.AdmissionFee;
                    getMemberCategory.PassBookFee = entity.PassBookFee;
                    getMemberCategory.LoanFormFee = entity.LoanFormFee;
                    memberCategoryService.Update(getMemberCategory);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /MemberCategory/Delete/5
        public ActionResult Delete(int id)
        {
            //var memberCategory = memberCategoryService.GetById(id);
            //var entity = Mapper.Map<MemberCategory, MemberCategoryViewModel>(memberCategory);
            //return View(entity);

            return View();
        }

        //
        // POST: /MemberCategory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {


            try
            {
                memberCategoryService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
