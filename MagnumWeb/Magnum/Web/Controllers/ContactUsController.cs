using System;
using Serilog;

using Microsoft.AspNetCore.Mvc;

using Its.Onix.Erp.Models;
using Its.Onix.Core.Business;
using Its.Onix.Core.Factories;
using Its.Onix.Core.Smtp;
using Its.Onix.Core.Notifications;

using Magnum.Web.Utils;
using Magnum.Api.Utils;

namespace Magnum.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ContactUsController : BaseController
    {
        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("Contact/Save")]
        public IActionResult SaveContactUs(MContactUs form)
        {
            string token = GetCpatchaToken();

            string validationMessage = ValidateContactUsForm(form, token);
            if (validationMessage != null)
            {
                ViewBag.Message = validationMessage;
            }
            else
            {
                IBusinessOperationManipulate<MContactUs> operation = GetSaveContactUsOperation();
                form.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
                form.Name = StringUtils.StripTagsRegex(form.Name);
                form.Subject = StringUtils.StripTagsRegex(form.Subject);
                form.Email = StringUtils.StripTagsRegex(form.Email);
                form.Message = StringUtils.StripTagsRegex(form.Message);

                operation.Apply(form);

                bool sendResult = SendEmail(form, token);

                if (sendResult)
                {
                    ViewBag.Message = "Your message has been received and we will contact you soon.";
                }
                else
                {
                    ViewBag.Message = "Unable to send the message, internal server error.";
                }

            }

            return View("Contact");
        }

        public virtual bool SendEmail(MContactUs form, string captchaToken)
        {
            bool result = false;
            string emailTo = GetEmailTo();
            if (emailTo != null)
            {
                Mail m = new Mail();
                m.From = "noreply@magnum-pharmacy.com";
                m.FromName = form.Name;
                m.To = emailTo;
                m.Subject = form.Subject;
                m.IsHtmlContent = true;
                m.Body = form.Email + ", " + form.Message;
                m.BCC = "";
                m.CC = "";

                ISmtpContext smtpContext = GetSmtpContext();
                smtpContext.Send(m);

                Log.Logger.Information("Email sent to [{0}]", emailTo);
                result = true;

                string shortToken = captchaToken.Substring(0, 10);
                
                LineNotification line = new LineNotification();
                string token = Environment.GetEnvironmentVariable("MAGNUM_LINE_TOKEN");
                line.SetNotificationToken(token);
                string lineMsg = String.Format(
                    "\nMagnumWeb ContactUs\nFrom:{0}\nSubject:{1}\nMessage:{2}\nCaptcha:{3}", 
                    form.Email, form.Subject, form.Message, shortToken);
                line.Send(lineMsg);
            }
            else
            {
                Log.Logger.Information("Env variable MAGNUM_EMAIL_TO not set!!!");
            }
            return result;
        }

        public virtual string GetEmailTo()
        {
            return Environment.GetEnvironmentVariable("MAGNUM_EMAIL_TO");
        }

        public string ValidateContactUsForm(MContactUs form, string token)
        {
            string validationMessage = null;
         
            if (String.IsNullOrEmpty(form.Name))
            {
                validationMessage = "Name cannot be empty.";
            }
            else if (String.IsNullOrEmpty(form.Subject))
            {
                validationMessage = "Subject cannot be empty.";
            }
            else if (String.IsNullOrEmpty(form.Message))
            {
                validationMessage = "Message cannot be empty.";
            }
            else if (String.IsNullOrEmpty(form.Email))
            {
                validationMessage = "Email cannot be empty.";
            }
            else if (String.IsNullOrEmpty(token))
            {
                validationMessage = "Please click reCaptcha checkbox to make sure you are not robot.";
            }   

            return validationMessage;
        }

        public virtual IBusinessOperationManipulate<MContactUs> GetSaveContactUsOperation()
        {
            return (IBusinessOperationManipulate<MContactUs>)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContactUs");
        }
    }
}

