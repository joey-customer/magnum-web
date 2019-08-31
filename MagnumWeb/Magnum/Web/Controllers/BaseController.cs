using System.Collections;
using System.Linq;
using Magnum.Api.Caches;
using Magnum.Api.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Magnum.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoadProductTypeList();
            LoadContents();
            //Program generates Modal every pages, assign this to prevent null pointer exception.
            ViewBag.ProductList = new ArrayList();
        }

        private void LoadContents()
        {
            var contentCache = GetContentCache();
            ViewBag.Contents = contentCache.GetValues();
        }

        private void LoadProductTypeList()
        {
            var productTypeCache = GetProductTypeCache();
            var productTypeList = productTypeCache.GetValues();
            ViewBag.ProductTypeList = productTypeList.Select(kvp => kvp.Value).ToList();
        }

        public virtual ICache GetProductTypeCache()
        {
            return FactoryCache.GetCacheObject("CacheProductTypeList");
        }

        public virtual ICache GetContentCache()
        {
            return FactoryCache.GetCacheObject("CachePageContents");
        }

                public virtual ICache GetProductsCache()
        {
            return FactoryCache.GetCacheObject("CacheProductList");
        }
    }
}
