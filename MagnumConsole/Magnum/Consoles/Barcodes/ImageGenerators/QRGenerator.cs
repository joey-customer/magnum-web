using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Utils;
using Its.Onix.Core.Commons.Model;

using Magnum.Consoles.Barcodes.Commons;
using Magnum.Consoles.Barcodes.HtmlConverters;

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

            htmlConverter.SetWidth(57);
            htmlConverter.SetImageFormat(1);
            htmlConverter.SetImageQuality(200);            
        }

        public void SetHtmlConverter(IHtmlConverter converter)
        {
            htmlConverter = converter;            
        }        

        protected override void TemplateParsing(BaseModel data, string qrImageFile)
        {
            currentData = (MProfile) data;
            currentQRfile = qrImageFile;
        }

        protected override string ProcessVariable(Match m)
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

        public override MemoryStream RenderToStream(BaseModel data)
        {
            MProfile bc = (MProfile) data;

            string tmpFile = string.Format("{0}.png", RandomUtils.RandomStringNum(10));
            string qrFile = CreateQR(tmpFile, bc.CompanyWebSite);
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