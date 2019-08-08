using System;
using Microsoft.AspNetCore.Mvc;
using Magnum.Api.Factories;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using System.Net;

namespace MagnumWeb.Controllers
{
    public class VerificationController : BaseController
    {
        [HttpGet("verification/{product}/{group}/{serial}/{pin}")]
        public IActionResult Check(String product, String group, String serial, String pin)
        {
            IBusinessOperationManipulate<MRegistration> operation = GetCreateRegistrationOperation();
            MRegistration param = new MRegistration();
            IPAddress remoteIPAddress = ControllerContext.HttpContext.Connection.RemoteIpAddress;
            param.IP = remoteIPAddress.ToString();
            param.Pin = pin;
            param.SerialNumber = serial;
            param.Path = string.Format("{0}/{1}", product, group);

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

        public virtual IBusinessOperationManipulate<MRegistration> GetCreateRegistrationOperation()
        {
            return (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
        }
    }
}
