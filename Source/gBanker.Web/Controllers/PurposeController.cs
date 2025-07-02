using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
//using gBanker.Web.Models;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using CrystalDecisions.Shared;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;


namespace gBanker.Web.Controllers
{
    //  [Authorize]
    public class PurposeController : BaseController
    {
        private readonly IPurposeService purposeService;
        public PurposeController(IPurposeService purposeService)
        {
            this.purposeService = purposeService;
        }

        // GET: Purpose

        public ActionResult Index()
        {

            //var allPurpose = purposeService.GetAll();
            //var viewPurpose = Mapper.Map<IEnumerable<Purpose>, IEnumerable<PurposeViewModel>>(allPurpose);
            //return View(viewPurpose);
            return View();
        }
        public JsonResult GetPurposes(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allPurposes = purposeService.GetPurposeDetailPaged(filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount,LoggedInOrganizationID);
               // var allPurposes = purposeService.GetAll().Where(p => p.IsActive == true).OrderBy(p => p.PurposeCode);
                //var totalCount = allPurposes.Count();
                //var entities = allPurposes.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Purpose>, IEnumerable<PurposeViewModel>>(allPurposes);

                //var entities = Mapper.Map<IEnumerable<Purpose>, IEnumerable<PurposeViewModel>>(allPurposes);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: Purpose/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Purpose/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Purpose/Create
        [HttpPost]
        public ActionResult Create(PurposeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<PurposeViewModel, Purpose>(model);
                     var errors = purposeService.IsValidPurpose(entity);
                     if (errors.ToList().Count == 0)
                     {
                         entity.OrgID = Convert.ToInt16( LoggedInOrganizationID);
                         //Add Validlation Logic.
                         entity.IsActive = true;
                         purposeService.Create(entity);
                         return GetSuccessMessageResult();
                     }
                     else
                     {
                         ModelState.AddModelErrors(errors);
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

        public void UpdateMethod(int Id, DateTime newValue)
        {
            //using (gBankerEntities ctx = new gBankerEntities())
            //{
            //    var query = (from q in ctx.Purposes
            //                 where q.PurposeID == Id
            //                 select q).First();
            //    query.IsActive = true;
            //    query.InActiveDate = newValue;
            //    ctx.SaveChanges();

            //}
        }
        // GET: Purpose/Edit/5
        public ActionResult Edit(int id)
        {
            if (purposeService.IsContinued(id))
            {
                var purpose = purposeService.GetById(id);
                var entity = Mapper.Map<Purpose, PurposeViewModel>(purpose);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Duplicate Product, please enter a diferent investor id and name.");
            return RedirectToAction("Index");
        }

        // POST: Purpose/Edit/5
        [HttpPost]
        public ActionResult Edit(PurposeViewModel model)
        {
            try
            {
                var entity = Mapper.Map<PurposeViewModel, Purpose>(model);
                var getPurpose = purposeService.GetById(entity.PurposeID);
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getPurpose.PurposeCode = entity.PurposeCode;
                    getPurpose.PurposeName = entity.PurposeName;
                    getPurpose.MainSector = entity.MainSector;
                    getPurpose.MainSectorName = entity.MainSectorName;
                    getPurpose.SubSector = entity.SubSector;
                    getPurpose.SubSectorName = entity.SubSectorName;
                    getPurpose.IsActive = true;
                    purposeService.Update(getPurpose);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Purpose/Delete/5
        public ActionResult Delete(int id)
        {

            var purpose = purposeService.GetById(id);
            var entity = Mapper.Map<Purpose, PurposeViewModel>(purpose);
            return View(entity);
        }

        // POST: Purpose/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                purposeService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
