using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Magnum.Web.Models;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Web.Utils;
using Magnum.Api.Utils;

namespace Magnum.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("Home/Contact/Save")]
        public IActionResult SaveContactUs(MContactUs form)
        {
            IBusinessOperationManipulate<MContactUs> operation = GetSaveContactUsOperation();
            form.IP = RemoteUtils.GetRemoteIPAddress(ControllerContext);
            form.Name = StringUtils.StripTagsRegex(form.Name);
            form.Subject = StringUtils.StripTagsRegex(form.Subject);
            form.Email = StringUtils.StripTagsRegex(form.Email);
            form.Message = StringUtils.StripTagsRegex(form.Message);

            operation.Apply(form);

            ViewBag.Message = "Thank you for contacting us – we will get back to you soon!";

            return View("Contact");
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
