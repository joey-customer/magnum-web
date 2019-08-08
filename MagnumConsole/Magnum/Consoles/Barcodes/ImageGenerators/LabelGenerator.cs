using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

using Magnum.Api.Models;
using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.HtmlConverters;

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
            htmlConverter.SetWidth(780);
            htmlConverter.SetImageFormat(1);
            htmlConverter.SetImageQuality(300);

            templateLines.Clear();
            string[] lines = File.ReadAllLines(TemplateFile);
            templateLines = new List<string>(lines);
        }

        public void SetHtmlConverter(IHtmlConverter converter)
        {
            htmlConverter = converter;
        }        

        protected override void TemplateParsing(BaseModel data, string qrImageFile)
        {
            currentData = (MBarcode) data;
            currentQRfile = qrImageFile;
        }

        protected override string ProcessVariable(Match m)
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

        public override MemoryStream RenderToStream(BaseModel data)
        {
            MBarcode bc = (MBarcode) data;

            string tmpFile = string.Format("{0}_{1}.png", bc.SerialNumber, bc.Pin);
            string qrFile = CreateQR(tmpFile, bc.PayloadUrl);

            var ms = ParseTemplate(htmlConverter, templateLines, bc, qrFile);
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