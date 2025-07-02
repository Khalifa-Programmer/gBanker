using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccNoteController : BaseController
    {
        #region Variables
        private readonly IAccNoteService accNoteService;
        public AccNoteController(IAccNoteService accNoteService)
        {
            this.accNoteService = accNoteService;            
            //this.centerService = centerService;
            //this.productService = productService;
            //this.loanSummaryService = loanSummaryService;
            //this.purposeService = purposeService;
        }
        #endregion

        #region Methods
        public JsonResult GetAccNoteList(int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            try
            {
                var noteDetail = accNoteService.GetAll().Where(m => m.IsActive == true && m.OrgID == LoggedInOrganizationID);               
                //var detail = codeDetail.ToList();
                var viewDetail = Mapper.Map<IEnumerable<AccNote>, IEnumerable<AccNoteViewModel>>(noteDetail);
                List<AccNoteViewModel> detail = new List<AccNoteViewModel>();
                int row_indx = 1;
                foreach (var vd in viewDetail)
                {
                    var details = new AccNoteViewModel() { SlNo = row_indx, NoteID = vd.NoteID, NoteNo = vd.NoteNo, NoteName = vd.NoteName, IsActive = vd.IsActive };
                    detail.Add(details);
                    row_indx++;
                }
                var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = row_indx - 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Events
        // GET: AccNote
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccNote/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccNote/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccNote/Create
        [HttpPost]
        public ActionResult Create(AccNoteViewModel model)
        {
            try
            {
                var entity = Mapper.Map<AccNoteViewModel, AccNote>(model);
                if (ModelState.IsValid)
                {
                    var errors = accNoteService.IsValidNote(entity);
                     if (errors.ToList().Count == 0)
                     {
                         entity.NoteNo = model.NoteNo;
                         entity.NoteName = model.NoteName;
                         entity.IsActive = true;
                         entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                         accNoteService.Create(entity);
                         return GetSuccessMessageResult();
                     }
                     else
                         return GetErrorMessageResult(errors);
                }
                else
                    return GetErrorMessageResult();
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: AccNote/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccNote/Edit/5
        [HttpPost]
        public ActionResult Edit(AccNoteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = accNoteService.GetById(model.NoteID);
                    entity.NoteNo = model.NoteNo;
                    entity.NoteName = model.NoteName;
                    entity.IsActive = model.IsActive;
                    if (model.IsActive == false)
                        entity.InActiveDate = DateTime.Now;
                    accNoteService.Update(entity);
                    return Json(new { Result = "OK" });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Fail to Update" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: AccNote/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccNote/Delete/5
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
