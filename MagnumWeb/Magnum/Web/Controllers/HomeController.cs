using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Magnum.Web.Models;
using Magnum.Api.Models;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Web.Utils;
using Magnum.Api.Utils;
using Magnum.Api.Caches;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;

namespace Magnum.Web.Controllers
{
    // Custom controller.

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            string productType = Request.Query["productType"].ToString();
            var productsMap = GetProductsCache().GetValues();
            var productList = productsMap.Select(kvp => kvp.Value).ToList();

            ArrayList productsByType = new ArrayList();
            foreach (var productBase in productList)
            {
                MProduct product = (MProduct)productBase;
                if (productType.Equals(product.ProductType))
                {
                    productsByType.Add(product);
                }
            }
            ViewBag.ProductList = productsByType;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
