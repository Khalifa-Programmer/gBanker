using gBanker.Core.Utility;
using gBanker.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private readonly IAspNetUserService userService;
        public ForgotPasswordController(UserManager<ApplicationUser> UserManager, IAspNetUserService userService)
        {
            this.UserManager = UserManager;
            this.userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Index(string gb_fgt_psd, string user_id_fire)
        {            
            var message = "";
            var appUser = userService.GetMany(p=> p.UserName == user_id_fire).FirstOrDefault();
            if (appUser == null || string.IsNullOrWhiteSpace(appUser.Email) || (!CommonHelper.IsValidEmail(appUser.Email)))
            {                
                message = "Please insert user Email from user creation page, then try again";
                return Json(new { message = message }, JsonRequestBehavior.AllowGet);
            }           
            
            var model = new ResetPasswordModel { Email = appUser.Email };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetPasswordResetLink(string EmailID)
        {
            string message = "";
            var updateResetPasswordCode = userService.GetMany(p => p.Email == EmailID).FirstOrDefault();
            if (updateResetPasswordCode != null)
            {
                //using (gBankerDbContext dc = new gBankerDbContext())
                //{
                var account = userService.GetMany(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //var resetCode = UserManager.GenerateUserToken("Reset_Password", account.Id);
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //dc.Configuration.ValidateOnSaveEnabled = false;
                    if (account.ResetPasswordCode == null)
                        //dc.SaveChanges();
                        userService.Create(account);
                    else
                    {
                        updateResetPasswordCode.ResetPasswordCode = resetCode;
                        userService.Update(updateResetPasswordCode);
                    }
                    message = "Reset password link has been sent to your registered Email Account.";
                }
                else
                {
                    message = "Registered Email not found";
                }
                //}
            }
            else
            {
                message = "Registered Email not found";
            }
            ViewBag.Message = message;
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/ForgotPassword/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var toAddress = emailID;

            string subject = "";
            string mailBody = "";
            string signature = "<p style='margin-top: 20px;'>Best Regards,<br/>gBanker MFI<br/>Grameen Communications</p>";

            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                mailBody = "<br/>" +
                    "<br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                mailBody = "<div style='font-family: Arial, sans-serif;'>" +
                            "<h2 style='color: #3498db;'>Reset Your Password</h2>" +
                            "<p>Hi,</p>" +
                            "<p>We received a request to reset your account password. Please click on the below link to reset your password:</p>" +
                            "<p style='margin-top: 20px;'><a href='" + link + "' style='padding: 10px 20px; background-color: #3498db; color: #fff; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>" +
                            signature +
                            "</div>";
            }
            SendMail(toAddress, mailBody, subject);
        }
        //public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        //{
        //    var verifyUrl = "/ForgotPassword/" + emailFor + "/" + activationCode;
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
        //    var toAddress = emailID;

        //    string subject = "";
        //    string mailBody = "";
        //    if (emailFor == "VerifyAccount")
        //    {
        //        subject = "Your account is successfully created!";
        //        mailBody = "<br/>" +
        //            "<br/>We are excited to tell you that your Dotnet Awesome account is" +
        //            " successfully created. Please click on the below link to verify your account" +
        //            " <br/><br/><a href='" + link + "'>" + link + "</a> ";
        //    }
        //    else if (emailFor == "ResetPassword")
        //    {
        //        subject = "Reset Password";
        //        mailBody = "Hi,<br/>We got request for reset your account password. Please click on the below link to reset your password." +
        //            "<br/><br/><a href=" + link + ">Reset Password link</a>";
        //    }
        //    SendMail(toAddress, mailBody, subject);
        //}

        private void SendMail(string toAddress, string mailBody, string subject)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(toAddress);
            const string style = "<p style=" + "font-family:Cambria;font-size:11pt" + ">";//Calibri
            mailMessage.Body = style + mailBody + "</p>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            var fromMessage = ConfigurationManager.AppSettings["messageFrom"];
            mailMessage.From = new MailAddress(fromMessage);
            var client = new SmtpClient();
            var smtpHost = ConfigurationManager.AppSettings["smtpHost"];
            client.Host = smtpHost; //Set your smtp host address

            var portId = ConfigurationManager.AppSettings["portID"];

            client.Port = Convert.ToInt32(portId); // Set your smtp port address
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;  //new added
            var credentialId = ConfigurationManager.AppSettings["credentialID"];
            var credentialPassword = ConfigurationManager.AppSettings["credentialPassword"];
            client.Credentials = new NetworkCredential(credentialId, credentialPassword); //account name and password
            client.Send(mailMessage);
        }
        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (gBankerDbContext dc = new gBankerDbContext())
            {
                var user = dc.AspNetUsers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public JsonResult SetNewPassword(ResetPasswordModel model)
        {
            var message = "";
            int result = 0;

            var user = userService.GetByToken(model.ResetCode);
            if (user != null)
            {
                //UserManager.RemovePassword(user.Id);
                //UserManager.AddPassword(user.Id, model.ConfirmPassword);
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.ConfirmPassword);
                user.UpdateDate = DateTime.Now.Date;
                userService.Update(user);
                result = 1;
                message = "New password updated successfully";
            }
            ViewBag.Message = message;
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}