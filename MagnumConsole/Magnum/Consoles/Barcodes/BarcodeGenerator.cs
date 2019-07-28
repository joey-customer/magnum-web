using System;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Utils;
using Magnum.Api.Businesses.Barcodes;

using NDesk.Options;

namespace Magnum.Consoles.Barcodes
{
	public class BarcodeGenerator : ConsoleAppBase
	{
        public override OptionSet CreateOptionSet()
        {
            ClearArgument();

            var options = new OptionSet() 
            {
                { "h=|host",     "Firebase URL", s => AddArgument("url", s) },
                { "k=|key=",      "Oauth key to access Firebase", s => AddArgument("key", s) },
                { "q=|quantity=", "Number of barcode to generate", s => AddArgument("quantity", s) },
                { "u=|url=",      "QR scan URL", s => AddArgument("url", s) },
                { "p=|product=",  "Product code", s => AddArgument("product", s) },
                { "b=|batch=",    "Batch number", s => AddArgument("batch", s) },
            };

            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            LibSetting setting = LibSetting.GetInstance();
            
            CreateBarcode opr = (CreateBarcode) FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");

            int quantity = Int32.Parse(args["quantity"].ToString());

            MBarcode param = new MBarcode();
            param.BatchNo = args["batch"].ToString();
            param.Url = args["url"].ToString();

            for (int i=1; i<=quantity; i++)
            {
                MBarcode bc = opr.Apply(param);
                if ((i % 100) == 0)
                {
                    int remain = quantity - i;
                    Console.WriteLine("Generated {0} barcodes, {1} barcodes to go...", i, remain);
                }

                Console.WriteLine("SerialNo=[{0}], Pin=[{1}]", bc.SerialNumber, bc.Pin);
            }
            
            Console.WriteLine("Done generating {0} barcodes.", quantity);
            return 0;
        }
    }
}
