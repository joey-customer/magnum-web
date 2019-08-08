using System;
using System.IO;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Barcodes;
using Magnum.Api.NoSql;
using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.ImageGenerators;

using NDesk.Options;

namespace Magnum.Consoles.Barcodes
{
    public delegate void BarcodeGenerateProgress(MBarcode bc, string dir);

	public class BarcodeGeneratorApplication : ConsoleAppBase
	{
        private LabelGenerator generator = new LabelGenerator();

        private int imgPerFolder = 100;
        private int progressPerImage = 100;

        private BarcodeGenerateProgress progressFunc = null;
        
        public BarcodeGeneratorApplication()
        {
            progressFunc = BarcodeGenerateProgressUpdate;
        }

        private void BarcodeGenerateProgressUpdate(MBarcode bc, string dir)
        {
            //Put any progress update here
        }

        public void SetUpdateProgressFunc(BarcodeGenerateProgress func)
        {
            progressFunc = func;
        }

        public void SetLabelGnerator(LabelGenerator labelGenerator)
        {
            generator = labelGenerator;
        }

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("q=|quantity=", "Number of barcode to generate", s => AddArgument("quantity", s))
            .Add("u=|url=", "QR scan URL", s => AddArgument("url", s))
            .Add("o=|outpath=", "QR image file output directory (folder)", s => AddArgument("outpath", s))
            .Add("profile=", "Product profile", s => AddArgument("profile", s))
            .Add("b=|batch=", "Batch number", s => AddArgument("batch", s));

            return options;
        }

        public void SetFilePerFolder(int num)
        {
            imgPerFolder = num;
        }

        public void SetProgressPerImage(int num)
        {
            progressPerImage = num;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string key = args["key"].ToString();
            string host = args["host"].ToString();
            string user = args["user"].ToString();
            string password = args["password"].ToString();
            string payloadUrl = args["url"].ToString();
            string batch = args["batch"].ToString();
            string prof = args["profile"].ToString();

            BarcodeProfileBase prf = (BarcodeProfileBase) BarcodeProfileFactory.CreateBarcodeProfileObject(prof);

            INoSqlContext ctx = GetNoSqlContext();
            if (ctx == null)
            {
                ctx = GetNoSqlContext("firebase", host, key, user, password);
            }

            FactoryBusinessOperation.SetContext(ctx);
            CreateBarcode opr = (CreateBarcode) FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");

            int quantity = Int32.Parse(args["quantity"].ToString());

            MBarcode param = new MBarcode();
            param.BatchNo = batch;
            param.Url = payloadUrl;            

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            generator.TemplateFile = prf.TemplateFile;
            generator.Setup();

            for (int i=1; i<=quantity; i++)
            {
                string chunk = ((i-1)/imgPerFolder).ToString().PadLeft(6, '0');
                string urlPath = string.Format("{0}_{1}_{2}/{3}", prof, param.BatchNo, timeStamp, chunk);
                string dir = string.Format("{0}/{1}", args["outpath"].ToString(), urlPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                param.Path = urlPath;
                param.CompanyWebSite = prf.CompanyWebSite;
                param.Barcode = prf.Barcode;
                param.Product = prf.Product;
                MBarcode bc = opr.Apply(param);                

                string fileName = string.Format("{0}/{1}-{2}.png", dir, bc.SerialNumber, bc.Pin);
                generator.RenderToFile(bc, fileName);
                progressFunc(bc, dir);

                Console.WriteLine("{0}|{1}|{2}|{3}|{4}", bc.SerialNumber, bc.Pin, bc.PayloadUrl, param.Barcode, prf.CompanyWebSite);
                
                if ((i % progressPerImage) == 0)
                {
                    int remain = quantity - i;
                    Console.WriteLine("Generated {0} barcodes, {1} barcodes to go...", i, remain);
                }                
            }
            
            generator.Cleanup();
            Console.WriteLine("Done generating {0} barcodes.", quantity);
            return 0;
        }
    }
}
