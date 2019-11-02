using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Its.Onix.Core.Commons.Model;
using Its.Onix.Core.Caches;
using Its.Onix.Erp.Models;
using System.Collections.Generic;
using System.Collections;
using Its.Onix.Core.Business;

namespace Magnum.Web.Controllers
{
    public class BaseControllerTest : BaseTest
    {
        public BaseControllerTest() : base()
        {
        }

        BaseController controller;
        Mock<HomeController> mockController;
        Mock<ICacheContext> iCacheMock;

        BaseController realController = new BaseController();

        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            mockController = new Mock<HomeController>() { CallBase = true };

            iCacheMock = new Mock<ICacheContext>();
            mockController.Setup(foo => foo.GetContentCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductTypeCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductsCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetMetricsCache()).Returns(iCacheMock.Object);

            var iBusinessOpr = new Mock<IBusinessOperationManipulate<MMetric>>();
            mockController.Setup(foo => foo.GetMetricIncreaseOperation()).Returns(iBusinessOpr.Object);

            MContent newArrivals = new MContent();
            newArrivals.Values = new Dictionary<string, string>();
            newArrivals.Values.Add("n1", "ITEM-003");
            newArrivals.Values.Add("n2", "ITEM-004");
            newArrivals.Values.Add("n3", "ITEM-005");
            iCacheMock.Setup(foo => foo.GetValue("code/New_Arrival_Products")).Returns(newArrivals);

            Dictionary<string, BaseModel> cacheData = new Dictionary<string, BaseModel>();
            MProductType product1 = new MProductType();
            product1.Code = "ITEM-001";
            cacheData.Add("ITEM-001", product1);
            iCacheMock.Setup(foo => foo.GetValues()).Returns(cacheData);

            controller = mockController.Object;
            controller.ControllerContext = controllerContext;

            MContent establishedDate = new MContent();
            establishedDate.Values = new Dictionary<string, string>();
            establishedDate.Values.Add("EN", "2019-01-01");

            MContent bestSellers = new MContent();
            bestSellers.Values = new Dictionary<string, string>();
            bestSellers.Values.Add("b1", "ITEM-001");
            bestSellers.Values.Add("b2", "ITEM-002");
            iCacheMock.Setup(foo => foo.GetValue("code/Best_Seller_Products")).Returns(bestSellers);

            MMetric shipped = new MMetric();
            shipped.Value = 2000;
            iCacheMock.Setup(foo => foo.GetValue("shipped")).Returns(shipped);

            var contentCache = new Dictionary<string, BaseModel>();
            contentCache["cfg/Established_Date"] = establishedDate;
            cacheData.Add("cfg/Established_Date", establishedDate);
            contentCache["code/Best_Seller_Products"] = bestSellers;
            controller.ViewBag.Contents = contentCache;
            controller.ViewBag.Lang = "EN";

        }

        [Test]
        public void OnActionExecuting()
        {
            controller.OnActionExecuting(null);
            ArrayList productList = (ArrayList)controller.ViewData["ProductList"];
            var productTypeList = (List<BaseModel>)controller.ViewData["ProductTypeList"];
            Assert.AreEqual(0, productList.Count);
            Assert.AreEqual(2, productTypeList.Count);
            Assert.AreEqual("ITEM-001", ((MProductType)productTypeList[0]).Code);
        }

        [Test]
        public void GetContentCache()
        {
            var cache = realController.GetContentCache();
            Assert.NotNull(cache);
        }

        [Test]
        public void GetProductTypeCache()
        {
            var cache = realController.GetProductTypeCache();
            Assert.NotNull(cache);
        }

        [Test]
        public void GetProductsCache()
        {
            var cache = realController.GetProductsCache();
            Assert.NotNull(cache);
        }

        [Test]
        public void GetMetricIncreaseOperation()
        {
            var opr = realController.GetMetricIncreaseOperation();
            Assert.NotNull(opr);
        }

        [Test]
        public void GetMatricsCache()
        {
            var opr = realController.GetMetricsCache();
            Assert.NotNull(opr);
        }

        [Test]
        public void GetEstablishedDate()
        {
            var result = controller.GetEstablishedDate();
            Assert.AreEqual("2019-01-01", result);
        }

        [Test]
        public void GetSmtpContext()
        {
            var result = controller.GetSmtpContext();
            Assert.NotNull(result);
        }


        [Test]
        public void GetOrderShipped()
        {
            var result = controller.GetOrderShipped();
            Assert.AreEqual("2,000", result);
        }

        [Test]
        public void GetOrderShippedError()
        {
            var e = new System.Exception();
            iCacheMock.Setup(foo => foo.GetValue("shipped")).Throws(e);
            var result = controller.GetOrderShipped();
            Assert.AreEqual("0", result);
        }
    }
}