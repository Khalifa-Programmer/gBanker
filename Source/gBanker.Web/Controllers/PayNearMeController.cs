using PaynearmeCallbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class PayNearMeController : Controller
    {
        //
        // GET: /PayNearMe/
        public ActionResult Authorize(string signature, string test, string site_order_annotation, string pnm_order_identifier, string site_order_identifier)
        {
            IAuthorizationHandler handler = new ExampleAuthorizationHandler();
            handler.HandleAuthorizationRequest(HttpContext.Request.QueryString, HttpContext.ApplicationInstance.Response);
            return new EmptyResult();
        }
        public ActionResult Confirm(string signature, string test, string site_order_annotation, string pnm_order_identifier, string site_order_identifier)
        {
            try
            {
                IConfirmationHandler handler = new ExampleConfirmationHandler();
                handler.HandleConfirmationRequest(HttpContext.Request.QueryString, HttpContext.ApplicationInstance.Response);
            }
            catch (RequestException e)
            {
                /**
                * Internal Server Error
                *  Internal Server Error is a special case of exception that throws an HTTP Error.  With the exception
                *  of Invalid Signature and Internal Server errors, it is expected that the callback response be
                *  properly formatted XML per the PayNearMe specification.
                *
                *  When this class of error is raised in a production environment you may choose to not respond to
                *  PayNearMe, which will trigger a timeout exception, leading to PayNearMe to retry the callbacks up
                *  to 40 times.  If the error persists, callbacks will be suspended.
                *
                *  This error may highlight a server outage in your infrastructure. You may choose to notify your IT
                *  department when this error class is raised.
                */
                HttpContext.Response.ContentType = "text/plain";
                HttpContext.Response.StatusCode = e.StatusCode;
                HttpContext.Response.Output.WriteLine(e.Message);
                HttpContext.Response.Output.Flush();
            }
            return new EmptyResult();
        }
	}
}