using AutoMapper;
using gBanker.Data;
//using gBanker.Data.Db;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class UserPortalController : Controller
    {
        #region Variables
        private readonly IMemberService memberService;
        public UserPortalController(IMemberService memberService)
        {
            this.memberService = memberService;            
        }

        #endregion

        #region Methods

        #endregion

        #region Events
        // GET: UserPortal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Schedule()
        {
            return View();
        }

        // GET: UserPortal/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserPortal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserPortal/Create
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

        // GET: UserPortal/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserPortal/Edit/5
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

        // GET: UserPortal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserPortal/Delete/5
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

        #endregion
    }
}
