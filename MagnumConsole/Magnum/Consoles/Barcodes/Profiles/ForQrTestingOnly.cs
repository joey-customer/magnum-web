using System;
using System.IO;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class ForQrTestingOnly : QRProfileBase
	{
        protected override void CustomSetting()
        {
            string templateString = @"
<html>
    <body></body>
    <h1>${MESSAGE2}</h1>
    <h1>${MESSAGE1}</h1>
    <h1>${IMAGE_QR}</h1>
</html>
";

            //Overrided TemplatFile to make it right for NUnit testing
            string tempPath = Path.GetTempPath();
            string tempFile = string.Format("{0}/{1}", tempPath, "test_label.html");
            TemplateFile = tempFile;

            File.WriteAllText(tempFile, templateString);

            Message1 = "FORTESTIN777";
            Message2 = "FORTESTIN777";
            CompanyWebSite = "https://google.com";
        }
    }
}