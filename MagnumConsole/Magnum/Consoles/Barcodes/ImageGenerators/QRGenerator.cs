using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

using Magnum.Api.Models;
using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.HtmlConverters;
using Magnum.Api.Utils;

using QRCoder;

namespace Magnum.Consoles.Barcodes.ImageGenerators
{
	public class QRGenerator : ImageGeneratorBase, IImageFromHtmlGenerator
	{
        private IHtmlConverter htmlConverter = new Html2ImageConverter();
        private MProfile currentData = null;
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

        private MemoryStream ParseTemplate(MProfile data, string qrImageFile)
        {
            string content = "";

            Regex regex = new Regex(@"(?<variable>\$\{.+\})");
            currentData = data;
            currentQRfile = qrImageFile;

            StringBuilder bld = new StringBuilder();
            foreach (string line in templateLines)
            {
                string replaceString = regex.Replace(line, ProcessVariable);
                bld.Append(replaceString);
            }

            content = bld.ToString();

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
 
            if (varName.Equals("${MESSAGE1}"))
            {
                value = currentData.Message1;
            } 
            else if (varName.Equals("${MESSAGE2}"))
            {
                value = currentData.Message2;
            }             
            else if (varName.Equals("${IMAGE_QR}"))
            {
                value = currentQRfile;
            } 

            return value;
        }

        private string CreateQR(MProfile data)
        {
            string tmpDir = Path.GetTempPath();
            string tmpFile = string.Format("{0}", RandomUtils.RandomString(10));
            string tmpPath = string.Format("{0}/{1}.png", tmpDir, tmpFile);

            Uri uri = new Uri(data.CompanyWebSite);
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
            MProfile bc = (MProfile) data;

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