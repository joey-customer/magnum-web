using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

using Magnum.Api.Models;
using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.HtmlConverters;

using QRCoder;

namespace Magnum.Consoles.Barcodes.ImageGenerators
{
	public class LabelGenerator : ImageGeneratorBase, IImageFromHtmlGenerator
	{
        private IHtmlConverter htmlConverter = new Html2ImageConverter();

        private MBarcode currentData = null;
        private string currentQRfile = "";
        private List<string> templateLines = new List<string>();

        protected override void CustomSetup()
        {
            templateLines.Clear();
            string[] lines = File.ReadAllLines(TemplateFile);
            templateLines = new List<string>(lines);
        }

        public void SetHtmlConverter(IHtmlConverter converter)
        {
            htmlConverter = converter;
        }        

        private MemoryStream ParseTemplate(MBarcode data, string qrImageFile)
        {
            string content = "";

            Regex regex = new Regex(@"(?<variable>\$\{.+\})");
            currentData = data;
            currentQRfile = qrImageFile;

            foreach (string line in templateLines)
            {
                string replaceString = regex.Replace(line, ProcessVariable);
                content = content + replaceString;
            }

            htmlConverter.SetWidth(780);
            htmlConverter.SetImageFormat(1);
            htmlConverter.SetImageQuality(200);
            var bytes = htmlConverter.FromHtmlString(content);
            MemoryStream ms = new MemoryStream(bytes);

            return ms;
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
                value = currentQRfile;
            } 

            return value;
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
            var ms = ParseTemplate(bc, qrFile);
            return ms;
        }

        protected override void CustomCleanup()
        {
            //Do nothing
        }

        protected override void SaveToFile(MemoryStream ms, string fileName)
        {
            Bitmap bmp = new Bitmap(ms);
            bmp.Save(fileName); 

            File.Delete(currentQRfile);   
        }	
    }
}