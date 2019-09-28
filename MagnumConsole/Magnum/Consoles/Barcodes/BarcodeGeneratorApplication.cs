using System;
using System.IO;
using System.Collections;

using Microsoft.Extensions.Logging;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.Barcodes;
using Its.Onix.Core.Utils;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Applications;

using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.ImageGenerators;
using Magnum.Consoles.Factories;

using NDesk.Options;
using Its.Onix.Core.Business;

namespace Magnum.Consoles.Barcodes
{
    public delegate void BarcodeGenerateProgress(MBarcode bc, string dir);

    public class BarcodeGeneratorApplication : ConsoleAppBase
    {
        private ILogger logger;

        private StreamWriter csvStream;
        private StreamWriter txtStream;

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
            .Add("g=|generate=", "Image generation flag", s => AddArgument("generate", s))
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

        private void CreateExportFile(string profile, MBarcode param, string outputPath, string timeStamp)
        {
            string exportFile = string.Format("{0}_{1}_{2}", profile, param.BatchNo, timeStamp);
            string[] paths = { outputPath, exportFile };
            string exportPath = Path.Combine(paths);

            string csvFile = String.Format("{0}.csv", exportPath);
            string txtFile = String.Format("{0}.txt", exportPath);

            txtStream = new StreamWriter(txtFile);
            txtStream.WriteLine("Serial, PIN, QR (Url), Barcode, Web Site");

            csvStream = new StreamWriter(csvFile);
            csvStream.WriteLine("Serial, PIN, QR (Url), Barcode, Web Site");
        }

        private void CloseExportFiles()
        {
            csvStream.Close();
            txtStream.Close();
        }

        private void WriteExportFile(BarcodeProfileBase prf, MBarcode bc, MBarcode param)
        {
            string csvLine = String.Format("=\"{0}\",=\"{1}\",\"{2}\",\"{3}\",\"{4}\"", bc.SerialNumber, bc.Pin, bc.PayloadUrl, param.Barcode, prf.CompanyWebSite);
            string txtLine = String.Format("{0},{1},{2},{3},{4}", bc.SerialNumber, bc.Pin, bc.PayloadUrl, param.Barcode, prf.CompanyWebSite);

            LogUtils.LogInformation(logger, txtLine);

            txtStream.WriteLine(txtLine);
            csvStream.WriteLine(csvLine);
        }

        protected override int Execute()
        {
            logger = GetLogger();

            Hashtable args = GetArguments();
            string payloadUrl = args["url"].ToString();
            string batch = args["batch"].ToString();
            string prof = args["profile"].ToString();
            string outputPath = args["outpath"].ToString();

            string generate = (string)args["generate"];
            if (generate == null)
            {
                generate = "";
            }

            bool imageGenerate = generate.Equals("Y");

            BarcodeProfileBase prf = (BarcodeProfileBase)BarcodeProfileFactory.CreateBarcodeProfileObject(prof);
            INoSqlContext ctx = GetNoSqlContextWithAuthen("FirebaseNoSqlContext");

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetLoggerFactory(FactoryConsoleApplication.GetLoggerFactory());
            CreateBarcode opr = (CreateBarcode)FactoryBusinessOperation.CreateBusinessOperationObject("CreateBarcode");

            int quantity = Int32.Parse(args["quantity"].ToString());

            MBarcode param = new MBarcode();
            param.BatchNo = batch;
            param.Url = payloadUrl;

            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            generator.TemplateFile = prf.TemplateFile;
            generator.Setup();

            CreateExportFile(prof, param, outputPath, timeStamp);

            for (int i = 1; i <= quantity; i++)
            {
                string chunk = ((i - 1) / imgPerFolder).ToString().PadLeft(6, '0');
                string urlPath = string.Format("{0}_{1}_{2}/{3}", prof, param.BatchNo, timeStamp, chunk);
                string dir = string.Format("{0}/{1}", outputPath, urlPath);

                if (!Directory.Exists(dir) && imageGenerate)
                {
                    Directory.CreateDirectory(dir);
                }

                param.Path = urlPath;
                param.CompanyWebSite = prf.CompanyWebSite;
                param.Barcode = prf.Barcode;
                param.Product = prf.Product;
                MBarcode bc = opr.Apply(param);

                string fileName = string.Format("{0}/{1}-{2}.png", dir, bc.SerialNumber, bc.Pin);
                if (imageGenerate)
                {
                    generator.RenderToFile(bc, fileName);
                }

                progressFunc(bc, dir);
                WriteExportFile(prf, bc, param);

                if ((i % progressPerImage) == 0)
                {
                    int remain = quantity - i;
                    LogUtils.LogInformation(logger, "Generated {0} barcodes, {1} barcodes to go...", i, remain);
                }
            }

            CloseExportFiles();

            generator.Cleanup();
            LogUtils.LogInformation(logger, "Done generating {0} barcodes.", quantity);

            int shipped = UpdateTotalShipped(quantity);
            LogUtils.LogInformation(logger, "Updated shipped number to {0}.", shipped);


            return 0;
        }

        private int UpdateTotalShipped(int value)
        {
            var matrixOpr = GetMatrixIncreaseOperation();
            MMatrix dat = new MMatrix();
            dat.Key = "shipped";
            dat.Value = value;

            int shipped = matrixOpr.Apply(dat);
            return shipped;
        }

        public virtual IBusinessOperationManipulate<MMatrix> GetMatrixIncreaseOperation()
        {
            return (IBusinessOperationManipulate<MMatrix>)FactoryBusinessOperation.CreateBusinessOperationObject("IncreaseAndRetrieveMatrix");
        }
    }
}
