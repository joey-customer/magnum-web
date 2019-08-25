using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Magnum.Web.Models;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Web.Utils;
using Magnum.Api.Utils;
using Magnum.Api.Caches;

using System;

namespace Magnum.Web.Controllers
{
    public class HomeController : Controller
    {
        public virtual ICache GetContentCache()
        {
            return FactoryCache.GetCacheObject("CachePageContents");
        }

        public IActionResult Index()
        {
            var contentCache = GetContentCache();
            ViewBag.Contents = contentCache.GetValues();
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
            var contentCache = GetContentCache();
            ViewBag.Contents = contentCache.GetValues();
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
