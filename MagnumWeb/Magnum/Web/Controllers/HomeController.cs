using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Magnum.Web.Models;
using Magnum.Api.Models;
using System.Collections;
using System.Linq;

namespace Magnum.Web.Controllers
{
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
