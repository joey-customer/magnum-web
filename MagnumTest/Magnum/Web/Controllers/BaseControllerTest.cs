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

        BaseController realController = new BaseController();

        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var mockController = new Mock<HomeController>() { CallBase = true };

            var iCacheMock = new Mock<ICacheContext>();
            mockController.Setup(foo => foo.GetContentCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductTypeCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetProductsCache()).Returns(iCacheMock.Object);
            mockController.Setup(foo => foo.GetMatricsCache()).Returns(iCacheMock.Object);

            var iBusinessOpr = new Mock<IBusinessOperationManipulate<MMetric>>();
            mockController.Setup(foo => foo.GetMetricIncreaseOperation()).Returns(iBusinessOpr.Object);
            

            mockController.Setup(foo => foo.GetEstablishedDate()).Returns("2019-01-01");

            MContent bestSellers = new MContent();
            bestSellers.Values = new Dictionary<string, string>();
            bestSellers.Values.Add("b1", "ITEM-001");
            bestSellers.Values.Add("b2", "ITEM-002");
            iCacheMock.Setup(foo => foo.GetValue("code/Best_Seller_Products")).Returns(bestSellers);

            MContent newArrivals = new MContent();
            newArrivals.Values = new Dictionary<string, string>();
            newArrivals.Values.Add("n1", "ITEM-003");
            newArrivals.Values.Add("n2", "ITEM-004");
            newArrivals.Values.Add("n3", "ITEM-005");
            iCacheMock.Setup(foo => foo.GetValue("code/New_Arrival_Products")).Returns(newArrivals);

            Dictionary<string, BaseModel> products = new Dictionary<string, BaseModel>();
            MProductType product1 = new MProductType();
            product1.Code = "ITEM-001";
            products.Add("ITEM-001", product1);
            iCacheMock.Setup(foo => foo.GetValues()).Returns(products);

            controller = mockController.Object;
            controller.ControllerContext = controllerContext;
        }

        [Test]
        public void OnActionExecuting()
        {
            controller.OnActionExecuting(null);
            ArrayList productList = (ArrayList)controller.ViewData["ProductList"];
            var productTypeList = (List<BaseModel>)controller.ViewData["ProductTypeList"];
            Assert.AreEqual(0, productList.Count);
            Assert.AreEqual(1, productTypeList.Count);
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
            var opr = realController.GetMatricsCache();
            Assert.NotNull(opr);
        }
    }
}