using gBanker.Web.RestApi.Models.Entity;
using gBanker.Web.RestApi.Models.ResponseModels;
using System.Net;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class Login : ApiResponse
    {
        public HttpStatusCode status;
        public ApiSetting apiSetting
        {
            get; set;
        }
        public string userName;
        public string userId;
        public string message;
        public LoginSucessData data;
        public string guid;
        public int orgId;

        public Login(HttpStatusCode s,string uid,string uname, string m, string guid,int orgId, LoginSucessData data)
        {
            this.status = s;
            this.userId = uid; //18211
            this.userName = uname;
            this.message = m;
            this.guid = guid;
            this.data = data;
            this.orgId = orgId;
        }

       
    }
}