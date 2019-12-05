using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

using Its.Onix.Core.Business;
using Its.Onix.Core.Factories;
using Its.Onix.Erp.Models;
using Its.Onix.Core.Notifications;

using Magnum.Api.Utils;
using Magnum.Web.Utils;

namespace Magnum.Web.Controllers
{
    public class VerificationController : BaseController
    {
        [HttpGet("verification/{product}/{group}/{serial}/{pin}")]
        public IActionResult URLCheck(String product, String group, String serial, String pin)
        {
            MRegistration param = new MRegistration();
            param.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
            param.Pin = pin;
            param.SerialNumber = serial;

            return VerifyProduct(param);
        }

        [HttpPost("verification")]
        public IActionResult WebCheck(MBarcode form)
        {
            form.SerialNumber = StringUtils.StripTagsRegex(form.SerialNumber);
            form.Pin = StringUtils.StripTagsRegex(form.Pin);
            MRegistration param = new MRegistration();
            param.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
            param.Pin = form.Pin;
            param.SerialNumber = form.SerialNumber;

            return VerifyProduct(param);
        }

        private void LineNotify(MRegistration param, string message)
        {
            string token = Environment.GetEnvironmentVariable("MAGNUM_LINE_TOKEN");
            LineNotification line = new LineNotification();
            
            line.SetNotificationToken(token);

            string lineMsg = String.Format(
                "\nMagnumWeb Product Verification\nSerial:{0}\nPin:{1}\nMessage:{2}\n", 
                param.SerialNumber, param.Pin, message);

            line.Send(lineMsg);
        }

        private string FilterSensitive(string str)
        {
            Regex regex = new Regex("https://.+");
            string cleanString = regex.Replace(str, "********Filter*******");

            return cleanString;
        }

        private IActionResult VerifyProduct(MRegistration param)
        {
            IBusinessOperationManipulate<MRegistration> operation = GetCreateRegistrationOperation();
            try
            {                
                operation.Apply(param);
                ViewBag.Serial = param.SerialNumber;
                ViewBag.PIN = param.Pin;

                LineNotify(param, "Production verification successfully.");

                return View("Success");
            }
            catch (Exception e)
            {
                ViewBag.Message = FilterSensitive(e.Message);
                LineNotify(param, e.Message);

                return View("Fail");
            }
        }

        public virtual IBusinessOperationManipulate<MRegistration> GetCreateRegistrationOperation()
        {
            return (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
        }
    }
}
