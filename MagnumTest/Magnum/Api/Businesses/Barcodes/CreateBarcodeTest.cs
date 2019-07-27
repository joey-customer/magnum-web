using System;
using NUnit.Framework;
using Magnum.Api.Factories;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Barcodes
{
	public class CreateBarcodeTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void CreateBarcodeWithCodeEmptyTest()
        {
            var opt = (IBusinessOperationManipulate<MBarcode>) FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");
            MBarcode bc = new MBarcode();
            opt.Apply(bc);
        } 
    }
}
