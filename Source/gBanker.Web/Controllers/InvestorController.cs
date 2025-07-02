
using AutoMapper;
//using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Data.CodeFirstMigration.Db;
using System.Threading;
namespace gBanker.Web.Controllers
{
   // [Authorize]
    public class InvestorController : BaseController
    {
        private readonly IInvestorService investorService;
        public InvestorController(IInvestorService investorService)
        {
            this.investorService = investorService;
        }

        // GET: Investor
        
        public ActionResult Index()
        {

            //var allInvestor = investorService.GetAll();
            //var viewInvestor = Mapper.Map<IEnumerable<Investor>, IEnumerable<InvestorViewModel>>(allInvestor);
            //return View(viewInvestor);
            return View();
        }
        public JsonResult GetInvestors(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allinvestors = investorService.SearchInvestor(LoggedInOrganizationID);
                var totalCount = allinvestors.Count();
                var entities = allinvestors.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Investor>, IEnumerable<InvestorViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var entities = Mapper.Map<IEnumerable<Investor>, IEnumerable<InvestorViewModel>>(allinvestors);
                //return Json(new { Result = "OK", Records = entities });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: Investor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Investor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Investor/Create
        [HttpPost]
        public ActionResult Create(InvestorViewModel model)
        {
            try
            {
               
                model.IsActive = true;
                 var entity = Mapper.Map<InvestorViewModel, Investor>(model);
                if (ModelState.IsValid)
                {

                    var errors = investorService.IsValidInvestor(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.OrgID = Convert.ToInt16( LoggedInOrganizationID);
                        //Add Validlation Logic.

                        investorService.Create(entity);
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
            catch(Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public void UpdateMethod(int Id, DateTime newValue)
        {
            //using (gBankerEntities ctx = new gBankerEntities())
            //{
            //    var query = (from q in ctx.Investors
            //                 where q.InvestorID == Id
            //                 select q).First();
            //    query.IsActive = true;
            //    query.InActiveDate = newValue;
            //    ctx.SaveChanges();

            //}
        }
        // GET: Investor/Edit/5
        public ActionResult Edit(int id)
        {
            if (investorService.IsContinued(id))
            {
                var investor = investorService.GetById(id);
                var entity = Mapper.Map<Investor, InvestorViewModel>(investor);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Duplicate Investor, please enter a diferent investor id and name.");
            return RedirectToAction("Index");
        }

        // POST: Investor/Edit/5
        [HttpPost]
        public ActionResult Edit(InvestorViewModel model)
        {
            try
            {
                model.IsActive = true;
                var entity = Mapper.Map<InvestorViewModel, Investor>(model);
                var getInvestors = investorService.GetById(entity.InvestorID);
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getInvestors.InvestorCode = entity.InvestorCode;
                    getInvestors.InvestorName = entity.InvestorName;

                    investorService.Update(getInvestors);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Investor/Delete/5
        public ActionResult Delete(int id)
        {
            investorService.Inactivate(id, null);
            return RedirectToAction("Index");
            //var investor = investorService.GetById(id);
            //var entity = Mapper.Map<Investor, InvestorViewModel>(investor);
            //return View(entity);
        }

        // POST: Investor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                investorService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
