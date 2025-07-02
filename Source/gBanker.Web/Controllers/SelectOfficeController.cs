using gBanker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class SelectOfficeController : BaseController
    {
        private readonly IOfficeService officeService;
        public SelectOfficeController(IOfficeService officeService)
        {
            
            this.officeService = officeService;
           
        }
        //
        // GET: /SelectOffice/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /SelectOffice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SelectOffice/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SelectOffice/Create
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

        //
        // GET: /SelectOffice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SelectOffice/Edit/5
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
        // GET: /SelectOffice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SelectOffice/Delete/5
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
