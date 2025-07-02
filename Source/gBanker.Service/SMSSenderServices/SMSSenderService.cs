using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using gBanker.Service.SMSSenderServices.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace gBanker.Service
{
    public class SMSSenderService : ISMSSenderService
    {
        #region Private Members

        private readonly ISMSSenderRepository repository;

        #endregion

        #region Ctor

        public SMSSenderService(ISMSSenderRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<SentLogSMSSummaryModel>> GetSentLogSMSSummaryByFilter(BaseSearchFilter filter)
        {
            var listing = await repository.GetSentLogSMSSummaryByFilter(filter);

            return listing;
        }
        public SMSSendResponse SendSMS(SMSSendRequest request)
        {
            SMSSendResponse sMSSendResponse = new SMSSendResponse();
            sMSSendResponse.IsError = true;

            try
            {
                if (request.Organization == OrganiationConstants.BuroBangladesh)
                {
                    //Send GP SMS
                    IRestResponse response = SendGPSMS(request);

                    var gpSMSSendResponse = new JavaScriptSerializer().Deserialize<SMSGPSendResponse>(response.Content);
                    sMSSendResponse.IsError = gpSMSSendResponse.statusCode == SMSApiResponseConstants.FAILED;
                    sMSSendResponse.Message = "Success! SMS Sent";
                }
                else
                {
                    //Send ADN SMS
                    sMSSendResponse = SendADNSMS(request, sMSSendResponse);
                }
            }
            catch (Exception ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = ex.Message;
            }

            return sMSSendResponse;
        }


        #endregion


        #region Private Methods

        private SMSSendResponse SendADNSMS(SMSSendRequest request, SMSSendResponse sMSSendResponse)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{SMSSenderConstants.ApiUrl}/v1/secure/send-sms";
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("api_key", SMSSenderConstants.ApiKey);
                reqparm.Add("api_secret", SMSSenderConstants.ApiSecret);
                reqparm.Add("request_type", request.RequestType);
                reqparm.Add("message_type", request.MessageType);
                reqparm.Add("mobile", request.RecipientMobile);
                reqparm.Add("message_body", request.Message);

                byte[] responsebytes = client.UploadValues(url, "POST", reqparm);
                string responsebody = Encoding.UTF8.GetString(responsebytes);

                sMSSendResponse = new JavaScriptSerializer().Deserialize<SMSSendResponse>(responsebody);
                sMSSendResponse.IsError = sMSSendResponse.api_response_code == SMSApiResponseConstants.FAILED;
                sMSSendResponse.Message = sMSSendResponse.api_response_message;
            }

            return sMSSendResponse;
        }

        private IRestResponse SendGPSMS(SMSSendRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://gpcmp.grameenphone.com/ecmapigw/webresources/ecmapigw.v2");
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddHeader("Content-Type", "application/json");

            var messageBody = ($@"{request.Message}").Replace("\n", "\\n");

            var body = @"{
                        " + "\n" +
                                    @" ""username"": ""Buroadmin"",
                        " + "\n" +
                                    @" ""password"": ""!Buro1234@api"",
                        " + "\n" +
                                    @" ""apicode"": ""1"",
                        " + "\n" +
                                    $@" ""msisdn"": ""{request.RecipientMobile}"",
                        " + "\n" +
                                    @" ""countrycode"": ""880"",
                        " + "\n" +
                                    @" ""cli"": ""buro bd"",
                        " + "\n" +
                                    @" ""messagetype"": ""1"",
                        " + "\n" +
                                    $@" ""message"": ""{messageBody}"",
                        " + "\n" +
                                    @" ""messageid"": ""0""
                        " + "\n" +
                                    @"}";

            requestPost.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(requestPost);
            return response;
        }

        #endregion
    }
}
