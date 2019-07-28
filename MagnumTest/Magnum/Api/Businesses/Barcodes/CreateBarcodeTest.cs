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
        public void CreateBarcodeWithRandomStringTest()
        {
            var opt = (IBusinessOperationGetInfo<MBarcode>) FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");
            MBarcode bc = new MBarcode();
            bc.Url = "http://this_is_fake_url";

            MBarcode barcode1 = opt.Apply(bc);
            string playLoad1 = string.Format("{0}/verification/{1}/{2}", bc.Url, barcode1.SerialNumber, barcode1.Pin);

            MBarcode barcode2 = opt.Apply(bc);
            string playLoad2 = string.Format("{0}/verification/{1}/{2}", bc.Url, barcode2.SerialNumber, barcode2.Pin);

            Assert.AreNotEqual(barcode1.SerialNumber, barcode2.SerialNumber, "SerialNumber must be different!!!");
            Assert.AreNotEqual(barcode1.Pin, barcode2.Pin, "PIN must be different!!!");

            Assert.AreEqual(playLoad1, barcode1.PayloadUrl, "Payload URL incorrect!!!");
            Assert.AreEqual(playLoad2, barcode2.PayloadUrl, "Payload URL incorrect!!!");            
        } 
    }
}
