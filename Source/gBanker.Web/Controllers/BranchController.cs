using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    [Authorize]
    public class BranchController : Controller
    {
        private readonly IBranchService branchService;

        public BranchController(IBranchService branchService)
        {
            this.branchService = branchService;
        }
        
        // GET: Branch
        public ActionResult Index()
        {
            var allBranch = branchService.GetAll();
            var vewBranch = Mapper.Map<IEnumerable<Branch>, IEnumerable < BranchViewModel >> (allBranch);
            return View(vewBranch);
        }

        // GET: Branch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Branch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branch/Create
        [HttpPost]
        public ActionResult Create(BranchViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<BranchViewModel, Branch>(model);
                    branchService.Create(entity);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception emsg)
            {
                return View();
            }
        }

        // GET: Branch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Branch/Edit/5
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

        // GET: Branch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Branch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                branchService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
