using System;
using System.IO;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class ForTestingOnly : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            string templateString = @"
<html>
    <body></body>
    <h1>${SERIAL_NO}</h1>
    <h1>${BARCODE_TEXT}</h1>
    <h1>${PIN_NO}</h1>
    <h1>${WEB_SITE}</h1>
    <h1>${IMAGE_QR}</h1>
</html>
";

            //Overrided TemplatFile to make it right for NUnit testing
            string tempPath = Path.GetTempPath();
            string tempFile = string.Format("{0}/{1}", tempPath, "test_label.html");
            TemplateFile = tempFile;

            File.WriteAllText(tempFile, templateString);

            Barcode = "FORTESTIN777";
            Product = "FORTESTIN777";
        }
    }
}