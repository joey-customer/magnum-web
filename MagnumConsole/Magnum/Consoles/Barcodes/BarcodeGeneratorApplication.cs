using System;
using System.IO;
using System.Collections;
using System.Drawing;

using QRCoder;
using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Barcodes;
using Magnum.Api.NoSql;

using NDesk.Options;

namespace Magnum.Consoles.Barcodes
{
    public delegate void BarcodeGenerateProgress(MBarcode bc, string dir);

	public class BarcodeGeneratorApplication : ConsoleAppBase
	{
        private int imgPerFolder = 1000;
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

        public override OptionSet CreateOptionSet()
        {
            ClearArgument();

            var options = new OptionSet() 
            {
                { "h=|host",     "Firebase URL", s => AddArgument("host", s) },
                { "k=|key=",      "Oauth key to access Firebase", s => AddArgument("key", s) },
                { "q=|quantity=", "Number of barcode to generate", s => AddArgument("quantity", s) },
                { "u=|url=",      "QR scan URL", s => AddArgument("url", s) },
                { "p=|product=",  "Product code", s => AddArgument("product", s) },
                { "o=|outpath=",  "QR image file output directory (folder)", s => AddArgument("outpath", s) },
                { "b=|batch=",    "Batch number", s => AddArgument("batch", s) },
                { "user=",    "Firebase username", s => AddArgument("user", s) },
                { "password=",    "Firebase password", s => AddArgument("password", s) },
            };

            return options;
        }

        private void GenerateQR(MBarcode data, string dir)
        {
            Uri generator = new Uri(data.PayloadUrl);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

            Bitmap bmp;
            using (var ms = new MemoryStream(qrCodeAsPngByteArr))
            {
                bmp = new Bitmap(ms);
            }

            string fileName = string.Format("{0}/{1}-{2}.png", dir, data.SerialNumber, data.Pin);
            bmp.Save(fileName);
        }

        public void SetFilePerFoler(int num)
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

            INoSqlContext ctx = GetNoSqlContext();
            if (ctx == null)
            {
                ctx = new FirebaseNoSqlContext();
                ctx.Authenticate(host, key, user, password);
            }

            FactoryBusinessOperation.SetContext(ctx);
            CreateBarcode opr = (CreateBarcode) FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");

            int quantity = Int32.Parse(args["quantity"].ToString());

            MBarcode param = new MBarcode();
            param.BatchNo = batch;
            param.Url = payloadUrl;

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            for (int i=1; i<=quantity; i++)
            {
                string chunk = ((i-1)/imgPerFolder).ToString().PadLeft(5, '0');
                string dir = string.Format("{0}/{1}_{2}/{3}", args["outpath"].ToString(), param.BatchNo, timeStamp, chunk);        
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                MBarcode bc = opr.Apply(param);
                GenerateQR(bc, dir);                
                progressFunc(bc, dir);

                Console.WriteLine("SerialNo=[{0}], Pin=[{1}]", bc.SerialNumber, bc.Pin);
                
                if ((i % progressPerImage) == 0)
                {
                    int remain = quantity - i;
                    Console.WriteLine("Generated {0} barcodes, {1} barcodes to go...", i, remain);
                }                
            }
            
            Console.WriteLine("Done generating {0} barcodes.", quantity);
            return 0;
        }
    }
}
