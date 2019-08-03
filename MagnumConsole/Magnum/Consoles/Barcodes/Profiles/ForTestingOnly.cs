using System;
using System.IO;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class ForTestingOnly : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            //Overrided TemplatFile to make it right for NUnit testing
            string tempPath = Path.GetTempPath();
            string tempFile = string.Format("{0}/{1}", tempPath, "test_label.html");
            TemplateFile = tempFile;

            File.WriteAllText(tempFile, "<html><body></body></html>");

            Barcode = "FORTESTIN777";
            Product = "FORTESTIN777";
        }
    }
}