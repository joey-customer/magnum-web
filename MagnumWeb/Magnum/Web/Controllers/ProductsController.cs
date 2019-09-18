using Microsoft.AspNetCore.Mvc;
using Its.Onix.Erp.Models;
using System.Collections;
using System.Linq;

namespace Magnum.Web.Controllers
{
    public class ProductsController : BaseController
    {
        [HttpGet("Products")]
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
    }
}
