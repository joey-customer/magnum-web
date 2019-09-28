using System;
using System.Collections;

using Microsoft.Extensions.Logging;

using Its.Onix.Erp.Models;
using Its.Onix.Core.Applications;

using NDesk.Options;
using Its.Onix.Erp.Businesses.Barcodes;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;
using Magnum.Consoles.Factories;
using Its.Onix.Erp.Utils;

namespace Magnum.Consoles.Barcodes
{

    public class BarcodeMigrateApplication : ConsoleAppBase
    {
        private ILogger logger;
        private INoSqlContext ctx;

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("if=|infile=", "Import file", s => AddArgument("infile", s))
            .Add("u=|url=", "QR scan URL", s => AddArgument("url", s));
            return options;
        }

        protected override int Execute()
        {
            logger = GetLogger();
            ctx = GetNoSqlContextWithAuthen("FirebaseNoSqlContext");

            Hashtable args = GetArguments();
            string inputFile = args["infile"].ToString();
            string url = args["url"].ToString();
            string batch = "0000";
            string chunk = "0000";
            string prof = "MIGRATE";

            CreateBarcode opr = GetCreateBarcodeOperation();

            System.IO.StreamReader file = new System.IO.StreamReader(inputFile);
            string text;
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            while ((text = file.ReadLine()) != null)
            {
                if ("".Equals(text.Trim())
                    && !text.Trim().StartsWith("Serial")
                )
                {
                    logger.LogInformation("Migrating: " + text);
                    string[] barcodeData = text.Split(",");
                    string serialNumber = barcodeData[0];
                    string pin = barcodeData[1];
                    if (IsBarcodeExisting(serialNumber, pin))
                    {
                        logger.LogWarning("The barcode is existing. [" + serialNumber + "][" + pin + "]");
                        logger.LogWarning("Result: SKIP");
                    }
                    else
                    {
                        MBarcode bc = new MBarcode();
                        bc.BatchNo = batch;
                        bc.Url = url;
                        bc.GeneratedDate = DateTime.Now;
                        bc.IsActivated = false;
                        bc.Path = string.Format("{0}_{1}_{2}/{3}", prof, batch, timeStamp, chunk);
                        bc.SerialNumber = serialNumber;
                        bc.Pin = pin;
                        bc.PayloadUrl = barcodeData[2];
                        bc.Barcode = barcodeData[3];
                        bc.Product = barcodeData[3];
                        bc.CompanyWebSite = barcodeData[4];
                        bc.GeneratedDate = DateTime.Now;
                        bc.IsActivated = false;
                        try
                        {
                            opr.Apply(bc);
                            logger.LogInformation("Result: SUCCESS");
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e.StackTrace);
                            logger.LogError("Result: FAIL");
                        }
                    }


                }
            }
            if (file != null)
            {
                file.Dispose();
            }

            return 0;
        }

        private bool IsBarcodeExisting(string serialNumber, string pin)
        {
            string bcPath = BarcodeUtils.BuildBarcodePath("barcodes", serialNumber, pin);
            MBarcode bc = ctx.GetObjectByKey<MBarcode>(bcPath);
            if (bc != null)
            {
                return true;
            }
            return false;
        }

        private CreateBarcode GetCreateBarcodeOperation()
        {
            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetLoggerFactory(FactoryConsoleApplication.GetLoggerFactory());
            CreateBarcode opr = (CreateBarcode)FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");
            return opr;
        }
    }
}
