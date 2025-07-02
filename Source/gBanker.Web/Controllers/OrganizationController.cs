using AutoMapper;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.Mvc;
using System.Drawing.Imaging; //Added for Image
using System.IO;
using gBanker.Web.Helpers;

namespace gBanker.Web.Controllers
{
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService organizationService;
        private readonly IUltimateReportService ultimateReportService;
        public OrganizationController(IOrganizationService organizationService, IUltimateReportService ultimateReportService)
        {
            this.organizationService = organizationService;
            this.ultimateReportService = ultimateReportService;
        }
   
        public JsonResult GetOrganizations(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                long totalCount;
                var param2 = new { @orgID = LoggedInOrganizationID };
                var getMemberTolrecord = ultimateReportService.GetOrganizationDetails(param2);

               // var allOrganization = organizationService.GetOrganizationDetailPaged(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out totalCount, LoggedInOrganizationID);
                var currentPageRecords = Mapper.Map<IEnumerable<Organization>, IEnumerable<OrganizationViewModel>>(getMemberTolrecord);
                totalCount = 1;
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message , JsonRequestBehavior.AllowGet });
            }


        }
        public byte[] GetImageFromDataBase(int Id)
        {
            var memberDetail = organizationService.GetById(Id);
            var img = memberDetail.OrgLOGO;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;
        }
        public ActionResult RetrieveImage(int id)
        {
            if (id == null)
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
            else
            {
                byte[] cover = GetImageFromDataBase(id);
                if (cover != null)
                {
                    return File(cover, "image/*");
                }
                else
                {
                    string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                    Image img = Image.FromFile(strImgPathAbsolute);
                    byte[] blnk;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        blnk = ms.ToArray();
                    }

                    return File(blnk, "image/*");
                }
            }
        }
        // GET: Organization
        public ActionResult Index()
        {
            return View();
        }

        // GET: Organization/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Organization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create
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

        // GET: Organization/Edit/5
        public ActionResult Edit(int id)
        {
            var orgservice = organizationService.GetById(id);
            var orgModel = Mapper.Map<Organization, OrganizationViewModel>(orgservice);
            return View(orgModel);
        }

        // POST: Organization/Edit/5
        [HttpPost]
        public ActionResult Edit(OrganizationViewModel model)
        {
            try
            {
              
                var entity = organizationService.GetById(Convert.ToInt32(model.OrgID));
                if (ModelState.IsValid)
                {

                    if (model.ImgFile == null)
                    {
                        var path = Server.MapPath("~/CapturedImages/");

                        var FileLocation = path + Session["CapturedImage"].ToString();
                        System.IO.File.Exists(FileLocation);
                        System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                        {
                            //Image image = System.Drawing.Image.FromFile(file.FullName);
                            ImageConverter converter = new ImageConverter();
                            byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                            entity.OrgLOGO = data;

                        }

                        //System.IO.File.Delete(FileLocation);
                        //Clear Captured Directory
                        Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    }
                    else if (model.ImgFile != null)
                    {
                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            entity.OrgLOGO = data;
                        }
                    }

                    


                    //entity.CenterID = model.CenterID;
                    entity.OrganizationCode = model.OrganizationCode;
                    entity.OrganizationName = model.OrganizationName;
                  
                   

                    organizationService.Update(entity);
                    return GetSuccessMessageResult();
                }
                else
                    return GetErrorMessageResult();
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Organization/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Organization/Delete/5
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
