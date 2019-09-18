﻿using System;
using Serilog;

using Microsoft.AspNetCore.Mvc;

using Its.Onix.Erp.Models;
using Its.Onix.Core.Business;
using Its.Onix.Core.Factories;
using Its.Onix.Core.Smtp;

using Magnum.Web.Utils;
using Magnum.Api.Utils;

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
                //m.FromName = form.Name; //TODO: Uncomment this
                m.To = emailTo;
                m.Subject = form.Subject;
                m.IsHtmlContent = true;
                m.Body = form.Email + ", " + form.Message;
                m.BCC = "";
                m.CC = "";

                ISmtpContext smtpContext = GetSmtpContext();
                smtpContext.Send(m);

                Log.Logger.Information("Email sent to [{0}]", emailTo);
            }
            else
            {
                Log.Logger.Information("Env variable MAGNUM_EMAIL_TO not set!!!");
            }
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

