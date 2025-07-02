using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class HelpController : Controller
    {
        #region Variables
        #endregion

        #region Methods
        #endregion

        #region Events
        // GET: Help
        public ActionResult BakFileDownload()
        {
            var folder = Server.MapPath("~/DataBackup");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, "*.bak");
                if (files.Length > 0)
                {
                    var allFiles = new List<FileInfo>();
                    foreach (var file in files)
                    {
                        var fi = new FileInfo(file);
                        allFiles.Add(fi);
                    }

                    var sortedFirst = allFiles.OrderByDescending(o => o.CreationTime).FirstOrDefault();
                    if (sortedFirst != null)
                        return File(sortedFirst.FullName, "application/octet-stream", sortedFirst.Name);
                }
            }

            return new EmptyResult();
        }
        public ActionResult UserManual()
        {
            return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");
            //return View();
        }
        public ActionResult gBankerCoreManual()
        {
            return File("~/Content/gBanker+_Core_Software_Manual.pdf", "application/pdf", "gBanker+_Core_Software_Manual.pdf");
            //return View();
        }
        public ActionResult rptWritingToolsManual()
        {
            return File("~/Content/gBanker+_Report_Tool_Manual.pdf", "application/pdf", "gBanker+_Report_Tool_Manual.pdf");
            //return View();
        }
        public ActionResult MobileAppDownload()
        {
            var folder = Server.MapPath("~/MobileAPP");
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder);
                if (files.Length > 0)
                {
                    var allFiles = new List<FileInfo>();
                    foreach (var file in files)
                    {
                        var fi = new FileInfo(file);
                        allFiles.Add(fi);
                    }
                    var sortedFirst = allFiles.OrderByDescending(o => o.CreationTime).FirstOrDefault();
                    if (sortedFirst != null)
                        return File(sortedFirst.FullName, "application/vnd.android.package-archive", sortedFirst.Name);
                }
            }
            return new EmptyResult();
        }
        public ActionResult gBankerPlusPromotion()
        {
            return File("~/Content/gBanker+_Promotions.pdf", "application/pdf", "gBanker+_Promotions.pdf");
            //return View();
        }  
        public ActionResult Index()
        {
            return View();
        }
        // GET: Help/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Help/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Help/Create
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
        // GET: Help/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Help/Edit/5
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
        // GET: Help/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Help/Delete/5
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
