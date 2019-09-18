using System.Collections;
using System.Linq;

using Its.Onix.Core.Factories;
using Its.Onix.Core.Caches;
using Its.Onix.Core.Smtp;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Magnum.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
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

        public virtual ICacheContext GetProductTypeCache()
        {
            return FactoryCacheContext.GetCacheObject("CacheProductTypeList");
        }

        public virtual ICacheContext GetContentCache()
        {
            return FactoryCacheContext.GetCacheObject("CachePageContents");
        }

        public virtual ICacheContext GetProductsCache()
        {
            return FactoryCacheContext.GetCacheObject("CacheProductList");
        }

        public virtual ISmtpContext GetSmtpContext()
        {
            var ctx = FactorySmtpContext.CreateSmtpObject("SendGridSmtpContext");
            ctx.SetSmtpConfigByEnv("MAGNUM_SMTP_HOST", "MAGNUM_SMTP_PORT", "MAGNUM_SMTP_USER", "MAGNUM_SMTP_PASSWORD");

            return ctx;
        }        
    }
}
