using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class SmsConfigController : BaseController
    {
        #region Variables
        private readonly ISmsConfigService smsConfigService;
        public SmsConfigController(ISmsConfigService smsConfigService)
        {
            this.smsConfigService = smsConfigService;              
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        // GET: SmsConfig
        public ActionResult Index()
        {
            return View();
        }

        // GET: SmsConfig/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SmsConfig/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SmsConfig/Create
        [HttpPost]
        public ActionResult Create(SmsConfigViewModel model)
        {
            try
            {
                var entity = Mapper.Map<SmsConfigViewModel, SmsConfig>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = 1;
                    var allreadyExist = smsConfigService.GetByOrgID(Convert.ToInt32(entity.OrgID));

                    if (allreadyExist == null)
                    {
                        entity.AccSID = smsConfigService.Encrypt(model.AccSID);
                        entity.AuthToken = smsConfigService.Encrypt(model.AuthToken);
                        entity.PhoneNo = smsConfigService.Encrypt(model.PhoneNo);
                        entity.IsActive = true;
                        entity.OrgID = 1;
                        smsConfigService.Create(entity);
                        return GetSuccessMessageResult("SMS Configuration Saved Successfully.");
                    }
                    else
                    {
                        allreadyExist.AccSID = smsConfigService.Encrypt(model.AccSID);
                        allreadyExist.AuthToken = smsConfigService.Encrypt(model.AuthToken);
                        allreadyExist.PhoneNo = smsConfigService.Encrypt(model.PhoneNo);
                        allreadyExist.IsActive = true;
                        smsConfigService.Update(allreadyExist);
                        return GetSuccessMessageResult("SMS Configuration Updated Successfully.");
                    }
                }
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: SmsConfig/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SmsConfig/Edit/5
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

        // GET: SmsConfig/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SmsConfig/Delete/5
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
