using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.Helpers
{
    public class LoginExternalHelper
    {
        public static ShareInfo ValidateAndGetSharedInfo(string data)
        {
            var si = new ShareInfo();
            try
            {
                var result = Cryptor.ActionDecrypt(data);
                var parts = result.Split("_".ToCharArray());
                if (parts.Length == 8)
                {
                    var uriId = int.Parse(parts[0]);
                    var LoginID = parts[1];
                    var minRange = int.Parse(parts[2]);
                    var maxRange = int.Parse(parts[3]);
                    var randomNumber = int.Parse(parts[4]);
                    var expDate = parts[5];
                    var officeId = parts[6];
                    var password = parts[7];
                    // var val = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}", urlId, userInfo.LoginID, minRange, maxRange, randomNumber, expDate.ToString("MM/dd/yyyy"), officeId, userInfo.Password);

                    if (randomNumber >= minRange && randomNumber <= maxRange)
                    {
                        if (DateTime.Today > DateTime.Parse(expDate))
                        {
                            si.ErrorDescription = "Link share is expired.";
                            si.Status = "E";
                        }
                        else
                        {
                            si.RedirectUrl = "";
                            si.UrlId = uriId;
                            si.LoginID = LoginID;
                            si.Status = "S";
                            si.Password = password;
                            si.OfficeId = int.Parse(officeId);
                        }
                    }
                    else
                        si.Status = "E";
                }
                else
                    si.Status = "E";
            }
            catch (Exception)
            {
                return null;
            }
            return si;
        }
    }

    [Serializable]
    public class ShareInfo
    {
        public int UrlId { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public int OfficeId { get; set; }
        public string Status { get; set; }
        public string ErrorDescription { get; set; }
        public string RedirectUrl { get; set; }
    }
}