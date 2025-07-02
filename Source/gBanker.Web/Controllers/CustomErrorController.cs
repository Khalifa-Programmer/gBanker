using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        public ActionResult Index()
        {
            var validationError = Session["UNAUTHORIZED_ACCES"] as string;
            if (string.IsNullOrEmpty(validationError))
                validationError = "Exception occured. Please contact with your administrator to fix the error.";      
            ViewBag.UnauthorizedAccessError = validationError;
            Session["UNAUTHORIZED_ACCES"] = null;
            return View();
        }
    }
}