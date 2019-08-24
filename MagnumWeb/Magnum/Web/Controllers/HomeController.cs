using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Magnum.Web.Models;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Web.Utils;
using Magnum.Api.Utils;
using System;
using System.Collections.Generic;
using Magnum.Api.Businesses.Contents;

namespace Magnum.Web.Controllers
{
    public class HomeController : Controller
    {
        ContentCacheUtils contentCacheUtils;

        public HomeController()
        {
            contentCacheUtils = ContentCacheUtils.GetInstance();
        }
        public IActionResult Index()
        {
            ViewBag.Contents = contentCacheUtils.GetContents();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Contents = contentCacheUtils.GetContents();
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("Home/Contact/Save")]
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

                ViewBag.Message = "Thank you for contacting us – we will get back to you soon!";
            }
            return View("Contact");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public virtual IBusinessOperationManipulate<MContactUs> GetSaveContactUsOperation()
        {
            return (IBusinessOperationManipulate<MContactUs>)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContactUs");
        }
    }
}
