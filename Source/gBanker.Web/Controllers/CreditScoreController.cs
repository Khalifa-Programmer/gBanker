using gBanker.Service;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gBanker.Web.Controllers
{
    public class CreditScoreController :  BaseController
    {
         private readonly ICreditScoreService creditscoreService;

         public CreditScoreController(ICreditScoreService creditscoreService)
        {
            this.creditscoreService = creditscoreService;
          
        }
         public ActionResult ExportData(string Datefrom, string DateTo)
         {

             creditscoreService.usp_rpt_credit_score(LoginUserOfficeID,Convert.ToDateTime(Datefrom),Convert.ToDateTime(DateTo));


             GridView gv = new GridView();
             //var allRepaymentSchedule = creditscoreService.GetAll();
             var allRepaymentSchedule = creditscoreService.GetCreditScore(LoginUserOfficeID);
             var detail = allRepaymentSchedule.ToList();
             gv.DataSource = detail;
             gv.DataBind();
             Response.ClearContent();
             Response.Buffer = true;
             Response.AddHeader("content-disposition", "attachment; filename=CreditScore.xls");
             Response.ContentType = "application/ms-excel";
             Response.Charset = "";
             StringWriter sw = new StringWriter();
             HtmlTextWriter htw = new HtmlTextWriter(sw);
             gv.RenderControl(htw);
             Response.Output.Write(sw.ToString());
             Response.Flush();
             Response.End();

             return RedirectToAction("CreditScore");
         }
        // GET: CreditScore
        public ActionResult Index()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();

            
        }

        // GET: CreditScore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreditScore/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreditScore/Create
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

        // GET: CreditScore/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreditScore/Edit/5
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

        // GET: CreditScore/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreditScore/Delete/5
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
