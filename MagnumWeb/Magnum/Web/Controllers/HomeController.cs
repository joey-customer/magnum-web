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
            ArrayList bestSellerList = new ArrayList();
            ArrayList newArrivalList = new ArrayList();
            ArrayList productList = new ArrayList();

            MContent bestSellerCodes = (MContent)GetContentCache().GetValue("code/Best_Seller_Products");
            foreach (string code in bestSellerCodes.Values.Select(kvp => kvp.Value).ToList())
            {
                MProduct product = (MProduct)GetProductsCache().GetValue(code);
                bestSellerList.Add(product);
                productList.Add(product);
            }

            MContent newArrivalCodes = (MContent)GetContentCache().GetValue("code/New_Arrival_Products");

            foreach (string code in newArrivalCodes.Values.Select(kvp => kvp.Value).ToList())
            {
                MProduct product = (MProduct)GetProductsCache().GetValue(code);
                newArrivalList.Add(product);
                productList.Add(product);
            }
            ViewBag.BestSellerList = bestSellerList;
            ViewBag.NewArrivalList = newArrivalList;
            ViewBag.ProductList = productList;
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
