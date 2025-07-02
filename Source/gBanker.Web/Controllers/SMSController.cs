using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;
using gBanker.Service.SMSSenderServices.Models;
using gBanker.Core.Utility;
using System.Net;

namespace gBanker.Web.Controllers
{
    public class SMSController : BaseController
    {
        #region Private Members

        private readonly IMemberCategoryService memberCategoryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        private readonly ISMSSendMessageService smsSendMessageService;
        private readonly ISMSSenderService smsSenderService;

        #endregion

        #region Ctor

        public SMSController(IMemberCategoryService memberCategoryService,
           ISMSSendMessageService smsSendMessageService,
           ISMSSenderService smsSenderService,
           IUltimateReportService ultimateReportService, IOfficeService officeService)
        {
            this.memberCategoryService = memberCategoryService;
            this.ultimateReportService = ultimateReportService;
            this.officeService = officeService;
            this.smsSendMessageService = smsSendMessageService;
            this.smsSenderService = smsSenderService;
        }


        #endregion

        public ActionResult Index()
        {
            List<SelectListItem> items3 = new List<SelectListItem>();
            items3.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            ViewData["comtype"] = items3;
            return View();
        }

        public JsonResult GetMessageTypeList()
        {
            var param1 = new { @OfficeId = SessionHelper.LoginUserOfficeID };
            var MessageList = ultimateReportService.GetMessageType(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                MessageTypeId = row.Field<int>("MessageTypeId"),
                MessageType = row.Field<string>("MessageType")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MessageTypeId.ToString(),
                Text = x.MessageType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessageSubCategory(string MessageTypeId)
        {
            var param1 = new { @MessageTypeId = MessageTypeId };
            var MessageList = ultimateReportService.GetMessageSubCategory(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                MessageTypeId = row.Field<int>("MessageCategoryId"),
                MessageType = row.Field<string>("MessageCategoryName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MessageTypeId.ToString(),
                Text = x.MessageType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateUpdateNewGroup
         (
               string SMSGroupId
             , string txtGroupName
             , string ddlProduct
             , string ddlBranch
             , string ddlSamity
             , string Other
             , string txtPhoneNo

         )
        {
            string result = "Data Saved Successfully"; ;
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInOrganizationID.ToString());
                DateTime dt = DateTime.Now;
                var param = new
                {

                    SMSGroupId = SMSGroupId,
                    txtGroupName = txtGroupName,
                    ddlProduct = ddlProduct,
                    ddlBranch = ddlBranch,
                    ddlSamity = ddlSamity,
                    Other = Other,
                    txtPhoneNo = txtPhoneNo,
                    isActive = 1,
                    CreateUser = SessionHelper.LoginUserEmployeeID,
                    CreateDate = DateTime.Now
                };
                var val = ultimateReportService.InsertUpdateGroup(param);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList()
        {
            var param1 = new { @OfficeId = SessionHelper.LoginUserOfficeID };
            var MessageList = ultimateReportService.SMSGetProductList(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                MainProductCode = row.Field<string>("MainProductCode"),
                MainItemName = row.Field<string>("MainItemName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MainProductCode.ToString(),
                Text = x.MainItemName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSamityList(string ddlBranch)
        {

            StringBuilder sb = new StringBuilder();

            if (ddlBranch != null) //"0"
            {
                if (ddlBranch != "0")
                    sb.Append(" AND  OfficeID IN(SELECT OfficeId FROM Office WHERE OfficeCode = " + ddlBranch + " )");
            }


            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_Center_List");

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                CenterId = row.Field<int>("CenterId"),
                OfficeName = row.Field<string>("CenterName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterId.ToString(),
                Text = x.OfficeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetBranchList()
        {
            var param1 = new { @OfficeId = SessionHelper.LoginUserOfficeID };
            var MessageList = ultimateReportService.SMSGetOfficeList(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeCode.ToString(),
                Text = x.OfficeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUpdateNewMessage
      (
            string SMSMessageId
          , string MessageCategoryId
          , string MessageTypeId
          , string MessageDetails
          , string Characters
          , string MessageSize

      )
        {
            string result = "Data Saved Successfully";
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInOrganizationID.ToString());
                DateTime dt = DateTime.Now;
                var param = new
                {
                    SMSMessageId = SMSMessageId,
                    MessageCategoryId = MessageCategoryId,
                    MessageTypeId = MessageTypeId,
                    MessageDetails = MessageDetails,
                    Characters = Characters,
                    MessageSize = MessageSize,
                    isActive = 1,
                    CreateUser = SessionHelper.LoginUserEmployeeID,
                    CreateDate = DateTime.Now
                };
                var val = ultimateReportService.InsertUpdateMessage(param);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessageList(string MessageId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (MessageId != null) //"0"
                {
                    if (MessageId != "0")
                        sb.Append(" AND M.SMSMessageId =" + MessageId);

                }

                List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_Message_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new SMSViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    SMSMessageId = row.Field<int>("SMSMessageId"),
                    MessageCategoryId = row.Field<int>("MessageCategoryId"),
                    MessageTypeId = row.Field<int>("MessageTypeId"),
                    MessageCategoryName = row.Field<string>("MessageCategoryName"),
                    MessageType = row.Field<string>("MessageType"),
                    MessageDetails = row.Field<string>("MessageDetails"),
                    Characters = row.Field<int>("Characters"),
                    MessageSize = row.Field<int>("MessageSize"),
                    isActive = row.Field<bool>("isActive")

                }).ToList();

                //if (MessageId != null)
                //{
                //    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}

                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                var detail = List_ViewModel.ToList();
                var TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        public JsonResult GetGroupList()
        {
            var param1 = new { @OfficeId = SessionHelper.LoginUserOfficeID };
            var MessageList = ultimateReportService.GetGroupList(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                SMSGroupId = row.Field<int>("SMSGroupId"),
                GroupName = row.Field<string>("GroupName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.SMSGroupId.ToString(),
                Text = x.GroupName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessage(string MessageTypeId, string MessageCategoryId)
        {
            var param1 = new { @MessageTypeId = MessageTypeId, @MessageCategoryId = MessageCategoryId };
            var MessageList = ultimateReportService.GETMessage(param1);

            List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                SMSMessageId = row.Field<int>("SMSMessageId"),
                MessageCategoryId = row.Field<int>("MessageCategoryId"),
                MessageTypeId = row.Field<int>("MessageTypeId"),
                MessageDetails = row.Field<string>("MessageDetails"),


            }).ToList();

            return Json(List_ViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SENDMessage(SMSSendViewModel model)
        {
            string result = "Message Sent Successfully";
            try
            {
                //model.SENDMessageDetailsId = model.SENDMessageDetailsId + '\n' + "powered by gBanker.";
                //model.SENDMessageDetailsId = model.SENDMessageDetailsId + @"\n" + @"powered by gBanker.";

                model.SENDMessageDetailsId = SessionHelper.OrganizationName + ": " + model.SENDMessageDetailsId + @"\n" + @"powered by gBanker.";
                Int64 CreateUser = Convert.ToInt64(LoggedInOrganizationID.ToString());
                DateTime dt = DateTime.Now;
                var param = new
                {
                    ddlSENDType = model.ddlSENDType,
                    SendTo = model.SendTo,
                    ddlSendMessageType = model.ddlSendMessageType,
                    ddlSENDSubCategories = model.ddlSENDSubCategories,
                    SENDMessageDetailsId = model.SENDMessageDetailsId,
                    txtSENDWebLink = model.txtSENDWebLink != null ? model.txtSENDWebLink : "",
                    txtSENDDate = model.txtSENDDate,
                    isActive = 1,
                    CreateUser = SessionHelper.LoginUserEmployeeID,
                    CreateDate = DateTime.Now

                };

                //let's insert into [SMSSendMessage] table
                var val = ultimateReportService.SENDMessage(param);

                //for individual
                if (model.ddlSENDType != "Group")
                {
                    var response = ExecuteMessage(model.SendTo, model.SENDMessageDetailsId);
                    if(response.IsError)
                        return Json(response.Message, JsonRequestBehavior.AllowGet);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //Get sms mobile numbers
                var smsMobileNumbers = smsSendMessageService.GetSMSMobileNumbers(model.SendTo);

                if (smsMobileNumbers == null || !smsMobileNumbers.Any())
                    return Json("Recipient Mobile number not found.", JsonRequestBehavior.AllowGet);

                var smsNotSentTo = "";

                //for bulk sms send
                foreach (var p in smsMobileNumbers)
                {
                    if (string.IsNullOrWhiteSpace(p.PhoneNo)) continue;

                    var response = ExecuteMessage(p.PhoneNo, model.SENDMessageDetailsId);
                    if (response.IsError) smsNotSentTo += $"{p.PhoneNo},";
                }

                var bulkResponseMessage =string.IsNullOrWhiteSpace(smsNotSentTo)
                    ?$@"{result}":$@"SMS Send Partially Successful, But Not Sent Mobile Number(s) : {smsNotSentTo.TrimEnd(',')}";

                return Json(bulkResponseMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                result = "Message Not Sent. Error Occured.";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SENDMessage_NotUsed
    (
          string ddlSENDType
        , string SendTo
        , string ddlSendMessageType
        , string ddlSENDSubCategories
        , string SENDMessageDetailsId
        , string txtSENDWebLink
        , string txtSENDDate
    )
        {
            string result = "Message Sent Successfully"; ;
            try
            {
                //SENDMessageDetailsId = SENDMessageDetailsId + " - " + "powered by gBanker.";
                SENDMessageDetailsId = SENDMessageDetailsId + '\n' +
                    "powered by gBanker.";
                Int64 CreateUser = Convert.ToInt64(LoggedInOrganizationID.ToString());
                DateTime dt = DateTime.Now;
                var param = new
                {
                    ddlSENDType = ddlSENDType,
                    SendTo = SendTo,
                    ddlSendMessageType = ddlSendMessageType,
                    ddlSENDSubCategories = ddlSENDSubCategories,
                    SENDMessageDetailsId = SENDMessageDetailsId,
                    txtSENDWebLink = txtSENDWebLink,
                    txtSENDDate = txtSENDDate,
                    isActive = 1,
                    CreateUser = SessionHelper.LoginUserEmployeeID,
                    CreateDate = DateTime.Now

                };
                var val = ultimateReportService.SENDMessage(param);
                string MessageNotSent = "";

                if (ddlSENDType == "Group")
                {
                    var param1 = new { @GroupId = SendTo };
                    var PhoneList = ultimateReportService.GetDataWithParameter(param1, "SMS_GET_MobileNo");

                    List<SMSViewModel> List_ViewModel = new List<SMSViewModel>();
                    List_ViewModel = PhoneList.Tables[0].AsEnumerable()
                    .Select(row => new SMSViewModel
                    {
                        PhoneNo = row.Field<string>("PhoneNo")
                    }).ToList();


                    foreach (var p in List_ViewModel)
                    {
                        var response = ExecuteMessage(p.PhoneNo, SENDMessageDetailsId);
                        MessageNotSent = MessageNotSent + ", ";
                    }

                    MessageNotSent = result + " " + MessageNotSent;
                }
                else
                {
                    var response = ExecuteMessage(SendTo, SENDMessageDetailsId);
                    MessageNotSent = MessageNotSent + ", ";
                }


                // SEND  MESSAGE
                /*
                HttpClient Client = new HttpClient();
                Client.BaseAddress = new Uri("http://192.192.192.233:8091/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Client.DefaultRequestHeaders.Add("Accept", "application/json");
                //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                HttpResponseMessage message = Client.GetAsync($"api/nonmaskingsms/easysend?sender={SendTo}&message={SENDMessageDetailsId}").Result;
                if (message.IsSuccessStatusCode)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);

                 */
                // END SEND Messaage


            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExecuteMessage_NotUsed(string SendTo, string SENDMessageDetailsId)
        {
            //HttpClient Client = new HttpClient();
            //Client.BaseAddress = new Uri("http://192.192.192.233:8091/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
            //Client.DefaultRequestHeaders.Clear();
            //Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var SMSAPIUrl = ConfigurationManager.AppSettings["SMSAPIUrl"].ToString();

            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri(SMSAPIUrl);
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            // HttpResponseMessage message = Client.GetAsync($"api/nonmaskingsms/easysend?sender={SendTo}&message={SENDMessageDetailsId}").Result;

            //HttpResponseMessage message = Client.GetAsync($"api/MaskingSMS/EasySend?MobileNo={SendTo}&Message={SENDMessageDetailsId}").Result;
            var message = Client.GetAsync($"api/MaskingSMS/EasySend?MobileNo={SendTo}&Message={SENDMessageDetailsId}").Result;

            var KMesage = message.Content.ReadAsStringAsync().Result;

            var MstatusCode = KMesage.Split(',')[0];
            MstatusCode = Regex.Replace(MstatusCode, "[^0-9.]", "");
            if (MstatusCode == "200")
            {

                if (message.IsSuccessStatusCode)
                {
                    var paramForSentMessage = new { @GrettingsText = SENDMessageDetailsId, @PhoneNo = SendTo, @SMSType = "Grettings", @OfficeId = (int)SessionHelper.LoginUserOfficeID, @OrgId = (int)SessionHelper.LoginUserOrganizationID };

                    //let insert [SMS_SentLog]
                    ultimateReportService.GetDataWithParameter(paramForSentMessage, "Insert_SMS_SMSLOGDetail");
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }

            //Let's insert into SMS_SentLog for NOT SENT
            var param1 = new { @GrettingsText = SENDMessageDetailsId, @PhoneNo = SendTo, @SMSType = "Grettings", @OfficeId = (int)SessionHelper.LoginUserOfficeID, @OrgId = (int)SessionHelper.LoginUserOrganizationID };
            ultimateReportService.GetDataWithParameter(param1, "Insert_SMS_FAILEDLogSMSLOGDetail");

            Response.StatusCode = 400;
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        #region Private Methods

        private SMSSendResponse ExecuteMessage(string sendTo, string SENDMessageDetailsId)
        {
            string smiley = "ঃ"; //"😊";

            string replacedMessage = smiley + SENDMessageDetailsId.Replace("&", "and");

            var request = new SMSSendRequest
            {
                RecipientMobile = sendTo,
                Message = replacedMessage,
                RequestType = SMSSendRequestTypeConstants.SINGLE_SMS,
                MessageType = SMSMessageTypeConstants.UNICODE,
                Organization= SessionHelper.LoggedInOrganizationCode
            };

            //let's send sms
            //var response = smsSenderService.SendSMS(request);
            var response = ExecuteSendSMSNw(request);

            if (response.IsError)
            {
                //Let's insert into SMS_SentLog for NOT SENT
                var param1 = new { @GrettingsText = replacedMessage, @PhoneNo = sendTo, @SMSType = "Grettings", @OfficeId = (int)SessionHelper.LoginUserOfficeID, @OrgId = (int)SessionHelper.LoginUserOrganizationID };
                ultimateReportService.GetDataWithParameter(param1, "Insert_SMS_FAILEDLogSMSLOGDetail");

                return response;
            }

            var paramForSentMessage = new { @GrettingsText = replacedMessage, @PhoneNo = sendTo, @SMSType = "Grettings", @OfficeId = (int)SessionHelper.LoginUserOfficeID, @OrgId = (int)SessionHelper.LoginUserOrganizationID };

            //let insert [SMS_SentLog]
            ultimateReportService.GetDataWithParameter(paramForSentMessage, "Insert_SMS_SMSLOGDetail");

            return response;
        }

        public SMSSendResponse ExecuteSendSMSNw(SMSSendRequest model)
        {
            var sMSSendResponse = new SMSSendResponse();

            // Force TLS 1.2 or TLS 1.3
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            // Define variables for the dynamic parts of the URL
            string message = model.Message;

            string contactNumber = model.RecipientMobile;

            if (!model.RecipientMobile.StartsWith("88"))
            {
                contactNumber = "88" + model.RecipientMobile;
            }

            // Construct the URL using string interpolation
            string url = $"{SMSSenderConstants_Rtcom.ApiBaseUrl}?acode={SMSSenderConstants_Rtcom.AccountCode}&api_key={SMSSenderConstants_Rtcom.ApiKey}&msg={message}&senderid={SMSSenderConstants_Rtcom.SenderId}&type={SMSSenderConstants_Rtcom.MessageType}&contacts={contactNumber}&transactionType={SMSSenderConstants_Rtcom.TransactionType}";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // Add headers if needed
            request.Headers.Add("Cookie", "ci_session=mb1ip1mam84hmf7le3dqv17cjas3dito");

            // Send the request
            try
            {
                var responseX = client.SendAsync(request).GetAwaiter().GetResult();
                responseX.EnsureSuccessStatusCode();

                sMSSendResponse.IsError = false;
                sMSSendResponse.Message = $"Success";
            }
            catch (HttpRequestException ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = $"General Error: {ex.Message}";

                //Console.WriteLine($"Request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = $"General Error: {ex.Message}";
            }

            return sMSSendResponse;
        }

        #endregion
    }
}