﻿using System;
using Microsoft.AspNetCore.Mvc;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;

namespace MagnumWeb.Controllers
{
    public class VerificationController : BaseController
    {
        private IBusinessOperationManipulate<MRegistration> opr = null;

        public IBusinessOperationManipulate<MRegistration> Opr { get => opr; set => opr = value; }

        [HttpGet("verification/{product}/{group}/{serial}/{pin}")]
        public IActionResult Check(String product, String group, String serial, String pin)
        {
            if (Opr == null)
            {
                Opr = (IBusinessOperationManipulate<MRegistration>)FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");
            }

            MRegistration param = new MRegistration();
            param.IP = getRemoteIP();
            param.Pin = pin;
            param.SerialNumber = serial;
            param.Path = string.Format("{0}/{1}", product, group);

            try
            {
                Opr.Apply(param);
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

        public virtual string getRemoteIP()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}
