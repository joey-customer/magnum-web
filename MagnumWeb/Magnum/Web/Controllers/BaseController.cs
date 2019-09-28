using System.Collections;
using System.Linq;

using Its.Onix.Core.Factories;
using Its.Onix.Core.Caches;
using Its.Onix.Core.Smtp;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Its.Onix.Erp.Models;
using Its.Onix.Core.Business;

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
            ViewBag.Lang = "EN";
            ViewBag.PageViews = GetViews();
            ViewBag.OrderShipped = GetOrderShipped();
            ViewBag.Uptime = GetDaysUpTime();

        }

        private dynamic GetViews()
        {
            var matrixOpr = GetMatrixIncreaseOperation();
            MMatrix dat = new MMatrix();
            dat.Key = "views";
            dat.Value = 1;

            int views = matrixOpr.Apply(dat);
            return String.Format("{0:n0}", views); ;
        }

        private dynamic GetOrderShipped()
        {
            var matrixOpr = GetMatrixRetrieveOperation();
            MMatrix dat = new MMatrix();
            dat.Key = "shipped";

            int views = matrixOpr.Apply(dat);
            return String.Format("{0:n0}", views); ;
        }

        private string GetDaysUpTime()
        {
            string establishedDate = GetEstablishedDate();
            DateTime since = DateTime.Parse(establishedDate);
            DateTime today = DateTime.Now;
            double dayDiff = Math.Floor((today - since).TotalDays);
            return String.Format("{0:n0}", dayDiff);
        }

        public virtual string GetEstablishedDate()
        {
            return ViewBag.Contents["cfg/Established_Date"].Values[ViewBag.Lang];
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
        public virtual IBusinessOperationManipulate<MMatrix> GetMatrixRetrieveOperation()
        {
            return (IBusinessOperationManipulate<MMatrix>)FactoryBusinessOperation.CreateBusinessOperationObject("RetrieveMatrix");
        }
        public virtual IBusinessOperationManipulate<MMatrix> GetMatrixIncreaseOperation()
        {
            return (IBusinessOperationManipulate<MMatrix>)FactoryBusinessOperation.CreateBusinessOperationObject("IncreaseAndRetrieveMatrix");
        }
    }
}
