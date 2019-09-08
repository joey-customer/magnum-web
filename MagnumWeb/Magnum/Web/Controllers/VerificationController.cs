using System;
using Microsoft.AspNetCore.Mvc;
using Magnum.Api.Factories;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using System.Net;
using Magnum.Api.Utils;
using Magnum.Web.Utils;

namespace Magnum.Web.Controllers
{
    public class VerificationController : BaseController
    {
        [HttpGet("verification/{product}/{group}/{serial}/{pin}")]
        public IActionResult Check(String product, String group, String serial, String pin)
        {
            IBusinessOperationManipulate<MRegistration> operation = GetCreateRegistrationOperation();
            MRegistration param = new MRegistration();
            param.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
            param.Pin = pin;
            param.SerialNumber = serial;

            try
            {
                operation.Apply(param);
                ViewBag.Serial = serial;
                ViewBag.PIN = pin;
                return View("Success");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Fail");
            }
        }

        [HttpPost("verification")]
        public IActionResult WebCheck(MBarcode form)
        {
            form.SerialNumber = StringUtils.StripTagsRegex(form.SerialNumber);
            form.Pin = StringUtils.StripTagsRegex(form.Pin);
            IBusinessOperationManipulate<MRegistration> operation = GetCreateRegistrationOperation();
            MRegistration param = new MRegistration();
            param.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
            param.Pin = form.Pin;
            param.SerialNumber = form.SerialNumber;

            try
            {
                operation.Apply(param);
                ViewBag.Serial = form.SerialNumber;
                ViewBag.PIN = form.Pin;
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
