using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;
using BasicDataAccess;
//using gBanker.Service;
//using gBanker.Data.CodeFirstMigration;

namespace PaynearmeCallbacks
{
    public class ExampleConfirmationHandler : IConfirmationHandler
    {
        //#region Variables
        //private readonly IPNMConfirmService pnmConfirmService;
        //public ExampleConfirmationHandler(IPNMConfirmService pnmConfirmService)
        //{
        //    this.pnmConfirmService = pnmConfirmService;                        
        //}
        //#endregion

        private ILog logger;

        public ExampleConfirmationHandler()
        {
            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void HandleConfirmationRequest(NameValueCollection parameters, HttpResponse response)
        {
            logger.Info("Handling /confirm with ExampleConfirmationHandler");

            string signature = SignatureUtils.Signature(parameters, Constants.SECRET);
            if (signature.Equals(parameters["signature"]))
            {

                // Do some extra functions that the 'echo server' does for debugging

                String special = parameters["site_order_annotation"];
                if (special != null)
                {
                    if (special.StartsWith("confirm_delay_"))
                    {
                        int delay = Convert.ToInt32(special.Substring(special.LastIndexOf('_') + 1)) * 1000;
                        logger.Info("Delaying response by " + delay + " seconds");
                        Thread.Sleep(delay);
                    }
                    else if (special.Equals("confirm_bad_xml"))
                    {
                        logger.Info("Responding with bad/broken xml");
                        response.Output.Write("<result");
                        response.Output.Flush();
                        logger.Debug("End handleConfirmationRequest (early: bad xml)");
                        return;
                    }
                    else if (special.Equals("confirm_blank"))
                    {
                        logger.Info("Responding with a blank/empty response");
                        logger.Debug("End handleConfirmationRequest (early: blank response)");
                        return;
                    }
                    else if (special.Equals("confirm_redirect"))
                    {
                        logger.Info("Redirecting to /");
                        response.Redirect("/");
                        logger.Debug("End handleConfirmationRequest (early: redirect)");
                        return;
                    }
                }

                // If the url contains the parameter test=true (part of the signed params too!) then we flag this.
                // Do not handle test=true requests as real requests.
                bool isTest = parameters["test"] != null && Convert.ToBoolean(parameters["test"]);
                if (isTest)
                {
                    logger.Info("This confirmation request is a TEST!");
                }

                String identifier = parameters["site_order_identifier"];
                String pnmOrderIdentifier = parameters["pnm_order_identifier"];
                String status = parameters["status"];

                /* You must lookup the pnm_payment_identifier in your business system and prevent double posting.
                   In the event of a duplicate callback from PayNearMe ( this can sometimes happen in a race or
                   retry condition) you must respond to all duplicates, but do not post the payment.
           
                   No stub code is provided for this check, and is left to the responsibility of the implementor.
           
                   Now that you have responded to a /confirm, you need to keep a record of this pnm_payment_identifier.
                 * Insert/Update
                */
                if (pnmOrderIdentifier != null || !pnmOrderIdentifier.Equals(""))
                {
                    var entry_dt = DateTime.Now;
                    var due_to_site_amount = Convert.ToDecimal(string.IsNullOrEmpty(parameters["due_to_site_amount"]) ? "0" : parameters["due_to_site_amount"]);
                    var due_to_site_currency = parameters["due_to_site_currency"];
                    var net_payment_amount = Convert.ToDecimal(string.IsNullOrEmpty(parameters["net_payment_amount"]) ? "0" : parameters["net_payment_amount"]);
                    var net_payment_currency = parameters["net_payment_currency"];
                    var order_payee_identifier = parameters["order_payee_identifier"];
                    var payment_amount = Convert.ToDecimal(string.IsNullOrEmpty(parameters["payment_amount"]) ? "0" : parameters["payment_amount"]);
                    var payment_currency = parameters["payment_currency"];
                    var payment_timestamp = parameters["payment_timestamp"];
                    var payment_timestamp_dt = Convert.ToDateTime(parameters["payment_timestamp"]);
                    var pnm_order_identifier = parameters["pnm_order_identifier"];
                    var pnm_payment_identifier = parameters["pnm_payment_identifier"];
                    var pnm_withheld_amount = Convert.ToDecimal(string.IsNullOrEmpty(parameters["pnm_withheld_amount"]) ? "0" : parameters["pnm_withheld_amount"]);
                    var pnm_withheld_currency = parameters["pnm_withheld_currency"];
                    var pnm_signature = parameters["signature"];
                    var site_customer_identifier = parameters["site_customer_identifier"];
                    var site_identifier = parameters["site_identifier"];
                    var standin = Convert.ToBoolean(string.IsNullOrEmpty(parameters["standin"]) ? "True" : parameters["standin"]);
                    var pnm_status = parameters["status"];
                    var test = Convert.ToBoolean(string.IsNullOrEmpty(parameters["test"]) ? "True" : parameters["test"]);
                    var timestamp = parameters["timestamp"];
                    var version = parameters["version"];                    

                    var storeProcedureName = "SP_Set_PNM_Confirm";
                    var param = new { entry_dt = entry_dt, due_to_site_amount = due_to_site_amount, due_to_site_currency = due_to_site_currency, net_payment_amount = net_payment_amount, net_payment_currency = net_payment_currency, order_payee_identifier = order_payee_identifier, payment_amount = payment_amount, payment_currency = payment_currency, payment_timestamp = payment_timestamp, payment_timestamp_dt = payment_timestamp_dt, pnm_order_identifier = pnm_order_identifier, pnm_payment_identifier = pnm_payment_identifier, pnm_withheld_amount = pnm_withheld_amount, pnm_withheld_currency = pnm_withheld_currency, signature = pnm_signature, site_customer_identifier = site_customer_identifier, site_identifier = site_identifier, standin = standin, status = pnm_status, test = test, timestamp = timestamp, version = version };
                    using (var gbData = new gBankerDataAccess())
                    {
                        int query_result =  gbData.ExecuteNonQuery(storeProcedureName, param);
                    }                
                }

                if (pnmOrderIdentifier == null || pnmOrderIdentifier.Equals(""))
                {
                    logger.Error("pnm_order_identifier is empty or null, do not respond!");
                    throw new RequestException("pnm_order_identifier is missing", 400);
                }

                if (status != null && status.Equals("decline"))
                {
                    logger.Info("Status: declined, do not credit (site_order_identifier: " + identifier + ")");
                }

                logger.Info("Response sent for pnm_order_identifier: " + pnmOrderIdentifier + ", site_order_identifier: " + identifier);

                /* Now that you have responded to a /confirm, you need to keep a record
                   of this pnm_payment_identifier and DO NOT POST any other /confirm 
                   requests for that pnm_payment_identifier, however you  should still
                   respond to all confirm requests, even duplicates.
                */

                response.ContentType = "application/xml";

                ConfirmationResponsebuilder builder = new ConfirmationResponsebuilder("2.0");
                builder.PnmOrderIdentifier = pnmOrderIdentifier;

                builder.Build().Save(response.Output);
                response.Output.Flush();

                logger.Debug("End handleConfirmationRequest");
            }
            else
            {
                /**
                       *  InvalidSignatureException
                       *   Invalid Signature is a special case of exception that throws an HTTP Error.  With the
                       *   exception of Invalid Signature and Internal Server errors, it is expected that the callback
                       *   response be properly formatted XML per the PayNearMe specification.
                       *
                       *   This is a security exception and may highlight a configuration problem (wrong secret or
                       *   siteIdentifier) OR it may highlight a possible payment injection from a source other than
                       *   PayNearMe.  You may choose to notify your IT department when this error class is raised.
                       *   PayNearMe strongly recommends that your callback listeners be whitelisted to ONLY allow
                       *   traffic from PayNearMe IP addresses.
                       *
                       *   When this class of error is raised in a production environment you may choose to not respond
                       *   to PayNearMe, which will trigger a timeout exception, leading to PayNearMe to retry the
                       *   callbacks up to 40 times.  If the error persists, callbacks will be suspended.
                       *
                       *   In development environment this default message will aid with debugging.
                       */

                logger.Warn("Invalid signature for /confirm");
                logger.Warn("  Got: " + parameters["signature"] + ", expected: " + signature);
                throw new RequestException("Invalid signature for /confirm", 400);
            }
        }

    }
}
