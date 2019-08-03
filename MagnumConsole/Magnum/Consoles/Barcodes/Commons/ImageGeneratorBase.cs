using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Magnum.Consoles.Barcodes.HtmlConverters;

using Magnum.Api.Models;
using QRCoder;

namespace Magnum.Consoles.Barcodes.Commons
{
	public abstract class ImageGeneratorBase : IImageGenerator
	{
		public string TemplateFile { get; set; }

        public void Setup()
		{
			//Common stub setup can be added here
			CustomSetup();
		}

        public void Cleanup()
		{
			//Common stub cleanup can be added here
			CustomCleanup();
		}

        public abstract MemoryStream RenderToStream(BaseModel data);
        
		public void RenderToFile(BaseModel data, string fileName)
		{
			MemoryStream ms = RenderToStream(data);
			SaveToFile(ms, fileName);

			ms.Close();
			ms.Dispose();
		}

        protected string CreateQR(string fname, string paylaodUrl)
        {
            string tmpDir = Path.GetTempPath();
            string tmpPath = string.Format("{0}/{1}", tmpDir, fname);

            Uri uri = new Uri(paylaodUrl);
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

        protected MemoryStream ParseTemplate(IHtmlConverter htmlConverter, List<string> templateLines, BaseModel data, string qrImageFile)
        {
            string content = "";

            Regex regex = new Regex(@"(?<variable>\$\{.+\})");
			TemplateParsing(data, qrImageFile);

            StringBuilder bld = new StringBuilder();
            foreach (string line in templateLines)
            {
                string replaceString = regex.Replace(line, ProcessVariable);
                bld.Append(replaceString);
            }

            content = bld.ToString();
            var bytes = htmlConverter.FromHtmlString(content);
            
            MemoryStream ms = new MemoryStream(bytes);

            return ms;
        }

		protected abstract void CustomSetup();
		protected abstract void CustomCleanup();
		protected abstract void SaveToFile(MemoryStream ms, string fileName);	

		protected abstract string ProcessVariable(Match m);
		protected abstract void TemplateParsing(BaseModel data, string qrImageFile);
    }
}