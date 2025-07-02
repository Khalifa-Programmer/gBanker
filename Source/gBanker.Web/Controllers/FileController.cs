using gBanker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using gBanker.Web.ViewModels;
using gBanker.Data.CodeFirstMigration;
namespace gBanker.Web.Controllers
{
    public class FileController : BaseController
    {
         #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IFileUploadService officialFileUploadService;
        private readonly IOfficeService officeService;
        public FileController(IEmployeeService employeeService, IFileUploadService officialFileUploadService, IOfficeService officeService)
        {
            this.employeeService = employeeService;
            this.officialFileUploadService = officialFileUploadService;
            this.officeService = officeService;
        }
        #endregion
        public JsonResult GetDownloadList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                long TotCount;
                int sl = 0;

                List<FileUploadViewModel> List_OfficialFileUploadViewModel = new List<FileUploadViewModel>();
                var downloadDetail = officialFileUploadService.GetAll().Where(w => w.IsActive == true && w.IsDownloadable == true);
                foreach (var d in downloadDetail)
                {
                    sl = sl + 1;
                    var dData = new FileUploadViewModel();
                    dData.FileUploadId = d.FileUploadId;
                    dData.UploadType = d.UploadType;
                    dData.FileName = d.FileName;
                    dData.FileLocation = d.FileLocation;
                    dData.IsDownloadable = d.IsDownloadable;
                    dData.UploadBy = d.UploadBy;
                    dData.UploadDate = d.UploadDate;
                    dData.IsActive = d.IsActive;
                    //dData.UploadDateMsg = Convert.ToDateTime(d.UploadDate).ToString("dd-MMM-yyyy");
                    //dData.OfficeName = officeService.GetById(Convert.ToInt32(d.UploadBy)).OfficeName;
                    //dData.rowSl = sl;
                    List_OfficialFileUploadViewModel.Add(dData);
                }

                var detail = List_OfficialFileUploadViewModel.ToList();

                var currentPageRecords = detail.Skip(jtStartIndex).Take(jtPageSize);
                TotCount = currentPageRecords.Count();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult DownloadFile(string FileUploadId)
        {
            var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");
            var location = dData.FileLocation + "/" + dData.FileName;
            return File(location, "application/pdf", dData.FileName);
            //return View();
        }
        public ActionResult FileUpload()
        {
            ViewData["SaveStatus"] = "0";
            return View();
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file, string UploadType, bool IsDownloadable)
        {

           
            var fileUploadId = 0;
            if (file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var fileType = Path.GetFileName(file.ContentType);
                if (officialFileUploadService.IsValidFile(fileName).Count() == 0)
                {
                    FileUpload officialFileUploadCreate = new FileUpload();
                    officialFileUploadCreate.UploadType = UploadType;
                    officialFileUploadCreate.FileType = fileType;
                    officialFileUploadCreate.FileName = fileName;
                    officialFileUploadCreate.FileLocation = "~/App_Data";
                    officialFileUploadCreate.IsDownloadable = IsDownloadable;
                    officialFileUploadCreate.UploadBy = Convert.ToDecimal(string.IsNullOrEmpty(LoginUserOfficeID.ToString()) ? "0" : LoginUserOfficeID.ToString());
                    officialFileUploadCreate.UploadDate = DateTime.Now;
                   
                    officialFileUploadCreate.IsActive = true;
                    officialFileUploadCreate.CreateUser = Convert.ToString(LoggedInEmployeeID);
                    ////officialFileUploadCreate.CreateUser = Convert.ToInt64(string.IsNullOrEmpty(LoggedInEmployeeID.ToString()) ? "0" : LoggedInEmployeeID.ToString());
                    officialFileUploadCreate.CreateDate = DateTime.Now;
                    officialFileUploadService.Create(officialFileUploadCreate);

                    fileUploadId = officialFileUploadCreate.FileUploadId;

                    var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                    file.SaveAs(path);
                    //ViewData["SaveStatus"] = "1";

                }
            }
            return Json(new { data = fileUploadId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FileDownload()
        {
            return View();
        }
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        // GET: File/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: File/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: File/Create
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

        // GET: File/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: File/Edit/5
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

        // GET: File/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: File/Delete/5
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
