using System;
using Microsoft.AspNetCore.Mvc;
using Magnum.Api.NoSql;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.Models;

namespace MagnumWeb.Controllers
{
    public class VerificationController : BaseController
    {
        [HttpGet("verification/{product}/{group}/{serial}/{pin}")]
        public String Check(String product, String group, String serial, String pin)
        {
            var view = View();

            INoSqlContext ctx = GetNoSqlContext();
            if (ctx == null)
            {
                string host = "https://compute-engine-vm-test.firebaseio.com/";
                string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
                string user = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
                string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");
                ctx = GetNoSqlContext("firebase", host, key, user, password);
                SetNoSqlContext(ctx);
            }

            FactoryBusinessOperation.SetContext(ctx);
            CreateRegistration opr = (CreateRegistration)FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");

            MRegistration param = new MRegistration();
            param.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            param.Pin = pin;
            param.SerialNumber = serial;
            param.Path = string.Format("{0}/{1}", product, group);

            try
            {
                opr.Apply(param);
                return string.Format("Done register barcode [{0}] [{1}]", serial, pin);
            }
            catch (Exception e)
            {
                return string.Format("Error : {0}", e.Message);
            }
        }
    }
}
