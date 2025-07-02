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
    public class SchedulerController : BaseController
    {
        #region Variables
        private readonly ISchedulerService schedulerService;        
        public SchedulerController(ISchedulerService schedulerService)
        {
            this.schedulerService = schedulerService;            
        }
        #endregion

        #region Methods
        private void MapDropDownList(SchedulerViewModel model)
        {
            var freq_item = new List<SelectListItem>();
            freq_item.Add(new SelectListItem() { Text = "Daily", Value = "D" });
            freq_item.Add(new SelectListItem() { Text = "Monthly", Value = "M" });
            freq_item.Add(new SelectListItem() { Text = "Yearly", Value = "Y" });
            model.FrequencyList = freq_item;

            var hour_item = new List<SelectListItem>();
            hour_item.Add(new SelectListItem() { Text = "1", Value = "1" });
            hour_item.Add(new SelectListItem() { Text = "2", Value = "2" });
            hour_item.Add(new SelectListItem() { Text = "3", Value = "3" });
            hour_item.Add(new SelectListItem() { Text = "4", Value = "4" });
            hour_item.Add(new SelectListItem() { Text = "5", Value = "5" });
            hour_item.Add(new SelectListItem() { Text = "6", Value = "6" });
            hour_item.Add(new SelectListItem() { Text = "7", Value = "7" });
            hour_item.Add(new SelectListItem() { Text = "8", Value = "8" });
            hour_item.Add(new SelectListItem() { Text = "9", Value = "9" });
            hour_item.Add(new SelectListItem() { Text = "10", Value = "10" });
            hour_item.Add(new SelectListItem() { Text = "11", Value = "11" });
            hour_item.Add(new SelectListItem() { Text = "12", Value = "12" });
            hour_item.Add(new SelectListItem() { Text = "13", Value = "13" });
            hour_item.Add(new SelectListItem() { Text = "14", Value = "14" });
            hour_item.Add(new SelectListItem() { Text = "15", Value = "15" });
            hour_item.Add(new SelectListItem() { Text = "16", Value = "16" });
            hour_item.Add(new SelectListItem() { Text = "17", Value = "17" });
            hour_item.Add(new SelectListItem() { Text = "18", Value = "18" });
            hour_item.Add(new SelectListItem() { Text = "19", Value = "19" });
            hour_item.Add(new SelectListItem() { Text = "20", Value = "20" });
            hour_item.Add(new SelectListItem() { Text = "21", Value = "21" });
            hour_item.Add(new SelectListItem() { Text = "22", Value = "22" });
            hour_item.Add(new SelectListItem() { Text = "23", Value = "23" });
            hour_item.Add(new SelectListItem() { Text = "24", Value = "24" });
            model.HourList = hour_item;

            var mins_item = new List<SelectListItem>();
            mins_item.Add(new SelectListItem() { Text = "0", Value = "0" });
            mins_item.Add(new SelectListItem() { Text = "1", Value = "1" });
            mins_item.Add(new SelectListItem() { Text = "2", Value = "2" });
            mins_item.Add(new SelectListItem() { Text = "3", Value = "3" });
            mins_item.Add(new SelectListItem() { Text = "4", Value = "4" });
            mins_item.Add(new SelectListItem() { Text = "5", Value = "5" });
            mins_item.Add(new SelectListItem() { Text = "6", Value = "6" });
            mins_item.Add(new SelectListItem() { Text = "7", Value = "7" });
            mins_item.Add(new SelectListItem() { Text = "8", Value = "8" });
            mins_item.Add(new SelectListItem() { Text = "9", Value = "9" });
            mins_item.Add(new SelectListItem() { Text = "10", Value = "10" });
            mins_item.Add(new SelectListItem() { Text = "11", Value = "11" });
            mins_item.Add(new SelectListItem() { Text = "12", Value = "12" });
            mins_item.Add(new SelectListItem() { Text = "13", Value = "13" });
            mins_item.Add(new SelectListItem() { Text = "14", Value = "14" });
            mins_item.Add(new SelectListItem() { Text = "15", Value = "15" });
            mins_item.Add(new SelectListItem() { Text = "16", Value = "16" });
            mins_item.Add(new SelectListItem() { Text = "17", Value = "17" });
            mins_item.Add(new SelectListItem() { Text = "18", Value = "18" });
            mins_item.Add(new SelectListItem() { Text = "19", Value = "19" });
            mins_item.Add(new SelectListItem() { Text = "20", Value = "20" });
            mins_item.Add(new SelectListItem() { Text = "21", Value = "21" });
            mins_item.Add(new SelectListItem() { Text = "22", Value = "22" });
            mins_item.Add(new SelectListItem() { Text = "23", Value = "23" });
            mins_item.Add(new SelectListItem() { Text = "24", Value = "24" });
            mins_item.Add(new SelectListItem() { Text = "25", Value = "25" });
            mins_item.Add(new SelectListItem() { Text = "26", Value = "26" });
            mins_item.Add(new SelectListItem() { Text = "27", Value = "27" });
            mins_item.Add(new SelectListItem() { Text = "28", Value = "28" });
            mins_item.Add(new SelectListItem() { Text = "29", Value = "29" });
            mins_item.Add(new SelectListItem() { Text = "30", Value = "30" });
            mins_item.Add(new SelectListItem() { Text = "31", Value = "31" });
            mins_item.Add(new SelectListItem() { Text = "32", Value = "32" });
            mins_item.Add(new SelectListItem() { Text = "33", Value = "33" });
            mins_item.Add(new SelectListItem() { Text = "34", Value = "34" });
            mins_item.Add(new SelectListItem() { Text = "35", Value = "35" });
            mins_item.Add(new SelectListItem() { Text = "36", Value = "36" });
            mins_item.Add(new SelectListItem() { Text = "37", Value = "37" });
            mins_item.Add(new SelectListItem() { Text = "38", Value = "38" });
            mins_item.Add(new SelectListItem() { Text = "39", Value = "39" });
            mins_item.Add(new SelectListItem() { Text = "40", Value = "40" });
            mins_item.Add(new SelectListItem() { Text = "41", Value = "41" });
            mins_item.Add(new SelectListItem() { Text = "42", Value = "42" });
            mins_item.Add(new SelectListItem() { Text = "43", Value = "43" });
            mins_item.Add(new SelectListItem() { Text = "44", Value = "44" });
            mins_item.Add(new SelectListItem() { Text = "45", Value = "45" });
            mins_item.Add(new SelectListItem() { Text = "46", Value = "46" });
            mins_item.Add(new SelectListItem() { Text = "47", Value = "47" });
            mins_item.Add(new SelectListItem() { Text = "48", Value = "48" });
            mins_item.Add(new SelectListItem() { Text = "49", Value = "49" });
            mins_item.Add(new SelectListItem() { Text = "50", Value = "50" });
            mins_item.Add(new SelectListItem() { Text = "51", Value = "51" });
            mins_item.Add(new SelectListItem() { Text = "52", Value = "52" });
            mins_item.Add(new SelectListItem() { Text = "53", Value = "53" });
            mins_item.Add(new SelectListItem() { Text = "54", Value = "54" });
            mins_item.Add(new SelectListItem() { Text = "55", Value = "55" });
            mins_item.Add(new SelectListItem() { Text = "56", Value = "56" });
            mins_item.Add(new SelectListItem() { Text = "57", Value = "57" });
            mins_item.Add(new SelectListItem() { Text = "58", Value = "58" });
            mins_item.Add(new SelectListItem() { Text = "59", Value = "59" });
            mins_item.Add(new SelectListItem() { Text = "60", Value = "60" });
            model.MinsList = mins_item;
        }
        #endregion

        #region Events
        // GET: Scheduler
        public ActionResult Index()
        {
            return View();
        }

        // GET: Scheduler/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Scheduler/Create
        public ActionResult Create()
        {
            
            SchedulerViewModel model = new SchedulerViewModel();
            MapDropDownList(model);
            var item = schedulerService.GetAll();
            if (item.Count > 0)
            {
                foreach (var t in item)
                {
                    model.SchedulerName = t.SchedulerName;                    
                    model.Description = t.Description;                    
                    model.hours = t.StartTime.Value.Hour;
                    model.mins = t.StartTime.Value.Minute;
                    model.Frequency = t.Frequency;
                    model.SchedulerID = t.SchedulerID;
                    model.mode = "U";
                }
            }
            else
            {
                model.mode = "S";
                model.SchedulerID = 0;
            }
            return View(model);
        }

        // POST: Scheduler/Create
        [HttpPost]
        public ActionResult Create(SchedulerViewModel model)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                TimeSpan ts = new TimeSpan(model.hours, model.mins, 0);
                startTime = startTime.Date + ts;
                int runEvery = 0;
                if (model.Frequency == "D")
                    runEvery = 1;
                else if (model.Frequency == "M")
                    runEvery = 30;
                else if (model.Frequency == "Y")
                    runEvery = 365;                
                int result = schedulerService.InsertSchedule(model.SchedulerName, model.Description, startTime, runEvery, model.Frequency, true, DateTime.Now, LoggedInEmployeeID.ToString(), model.mode, model.SchedulerID);
                if(result > 0)                    
                    return GetSuccessMessageResult("Saved Successfully.");
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Scheduler/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Scheduler/Edit/5
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

        // GET: Scheduler/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Scheduler/Delete/5
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
