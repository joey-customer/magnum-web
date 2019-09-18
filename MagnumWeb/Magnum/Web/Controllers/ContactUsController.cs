using Microsoft.AspNetCore.Mvc;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Web.Utils;
using Magnum.Api.Utils;

using System;
using Magnum.Api.Smtp;

namespace Magnum.Web.Controllers
{
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
            string validationMessage = ValidateContactUsForm(form);
            if (ValidateContactUsForm(form) != null)
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

                SendEmail(form);

                ViewBag.Message = "Your message has been received and we will contact you soon.";
            }
            return View("Contact");
        }

        private void SendEmail(MContactUs form)
        {
            string emailTo = Environment.GetEnvironmentVariable("MAGNUM_EMAIL_TO");
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
            }
            else
            {
                //TODO write log, email will not be sent.
            }
        }

        public virtual ISmtpContext GetSmtpContext()
        {
            return new SendGridSmtpContext();
        }

        public string ValidateContactUsForm(MContactUs form)
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
            return validationMessage;
        }

        public virtual IBusinessOperationManipulate<MContactUs> GetSaveContactUsOperation()
        {
            return (IBusinessOperationManipulate<MContactUs>)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContactUs");
        }
    }
}
