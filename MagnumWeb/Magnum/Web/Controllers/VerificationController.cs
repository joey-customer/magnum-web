using System;
using Microsoft.AspNetCore.Mvc;
using Magnum.Api.Factories;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
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

        private IActionResult VerifyProduct(MRegistration param)
        {
            IBusinessOperationManipulate<MRegistration> operation = GetCreateRegistrationOperation();
            try
            {
                operation.Apply(param);
                ViewBag.Serial = param.SerialNumber;
                ViewBag.PIN = param.Pin;
                return View("Success");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Fail");
            }
        }

        public virtual IBusinessOperationManipulate<MRegistration> GetCreateRegistrationOperation()
        {
            return (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
        }
    }
}
