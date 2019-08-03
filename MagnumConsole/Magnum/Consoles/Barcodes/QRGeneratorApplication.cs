using System;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.ImageGenerators;

using NDesk.Options;

namespace Magnum.Consoles.Barcodes
{
	public class QRGeneratorApplication : ConsoleAppBase
	{
        private QRGenerator generator = new QRGenerator();

        public QRGeneratorApplication()
        {
        }

        public void SetQrGnerator(QRGenerator qrGenerator)
        {
            generator = qrGenerator;
        }

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options
                .Add("o=|outpath=", "QR image file output directory (folder)", s => AddArgument("outpath", s))
                .Add("profile=", "Product profile", s => AddArgument("profile", s));

            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string prof = args["profile"].ToString();
            string outpath = args["outpath"].ToString();

            QRProfileBase prf = (QRProfileBase) BarcodeProfileFactory.CreateBarcodeProfileObject(prof);

            MProfile p = new MProfile();
            p.Message1 = prf.Message1;
            p.Message2 = prf.Message2;
            p.CompanyWebSite = prf.CompanyWebSite;
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileName = string.Format("{0}/{1}_{2}.png", outpath, prof, timeStamp);

            generator.TemplateFile = prf.TemplateFile;
            generator.Setup();            
            generator.RenderToFile(p, fileName);
            generator.Cleanup();

            Console.WriteLine("Done generating barcode at [{0}].", fileName);

            return 0;
        }
    }
}
