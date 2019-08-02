using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Drawing;

using Magnum.Api.Models;
using Magnum.Consoles.Barcodes.Commons;

using QRCoder;
using CoreHtmlToImage;

namespace Magnum.Consoles.Barcodes.ImageGenerators
{
	public class LabelGenerator : ImageGeneratorBase
	{
        private Hashtable varMapsProperty = new Hashtable();
        private MBarcode currentData = null;
        private string currentQR = "";

        protected override void CustomSetup()
        {
            varMapsProperty.Clear();

            varMapsProperty["${SERIAL_NO}"] = "SerialNumber";
            varMapsProperty["${BARCODE_TEXT}"] = "Barcode";
            varMapsProperty["${PIN_NO}"] = "Pin";
            varMapsProperty["${WEB_SITE}"] = "CompanyWebSite";
            varMapsProperty["${IMAGE_QR}"] = "";
        }

        private void ParseTemplate(MBarcode data, string qrImageFile)
        {
            string template = @"D:\dev\projects\magnum_workspace\magnum-web\MagnumConsole\Magnum\Consoles\Barcodes\ImageGenerators\Templates\magnum_label.html";
            string[] lines = File.ReadAllLines(template);  
  
            Regex regex = new Regex(@"(?<variable>\$\{.+\})");
            currentData = data;
            currentQR = qrImageFile;

            foreach (string line in lines)
            {
                string replaceString = regex.Replace(line, ProcessVariable);
Console.WriteLine(replaceString);              
            }
        }

        private string ProcessVariable(Match m)
        {
            string varName = m.Groups["variable"].Value;
            string value = "";

            if (varName.Equals("${SERIAL_NO}"))
            {
                value = currentData.SerialNumber;
            }        
            else if (varName.Equals("${BARCODE_TEXT}"))
            {
                value = currentData.Barcode;
            }   
            else if (varName.Equals("${PIN_NO}"))
            {
                value = currentData.Pin;
            }  
            else if (varName.Equals("${WEB_SITE}"))
            {
                value = currentData.CompanyWebSite;
            } 
            else if (varName.Equals("${IMAGE_QR}"))
            {
                value = currentQR;
            } 

            return value;
        }

        private void TestHtmlToImage2()
        {                    
            var converter = new HtmlConverter();
            var bytes = converter.FromUrl(@"C:\Users\pjame\Desktop\barcode\index.html");
            File.WriteAllBytes(@"D:\temp\image.jpg", bytes);
        }

        private string CreateQR(MBarcode data)
        {
            string tmpDir = Path.GetTempPath();
            string tmpFile = string.Format("{0}_{1}", data.SerialNumber, data.Pin);
            string tmpPath = string.Format("{0}/{1}.png", tmpDir, tmpFile);

            Uri uri = new Uri(data.PayloadUrl);
            string payload = uri.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(200);
            
            var ms = new MemoryStream(qrCodeAsPngByteArr);
            Bitmap bmp = new Bitmap(ms);
            bmp.Save(tmpPath);

            return tmpPath; 
        }        

        public override MemoryStream RenderToStream(BaseModel data)
        {
            MBarcode bc = (MBarcode) data;

            string qrFile = CreateQR(bc);
            ParseTemplate(bc, qrFile);

            var ms = new MemoryStream();
            return ms;
        }

        protected override void CustomCleanup()
        {
        }

        protected override void SaveToFile(MemoryStream ms, string fileName)
        {
            Bitmap bmp = new Bitmap(ms);
            bmp.Save(fileName);            
        }	
    }
}