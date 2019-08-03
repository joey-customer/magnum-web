using System;
using CoreHtmlToImage;

namespace Magnum.Consoles.Barcodes.HtmlConverters
{
	public class Html2ImageConverter : IHtmlConverter
	{
        private readonly HtmlConverter converter = new HtmlConverter();
        private int width = 1024;
        private int imgQuality = 200;
        private ImageFormat imgFormat = ImageFormat.Jpg;

        public void SetWidth(int w)
        {
            width = w;
        }

        public void SetImageFormat(int fmt)
        {
            if (fmt == 0)
            {
                imgFormat = ImageFormat.Jpg;
            }
            else
            {
                imgFormat = ImageFormat.Png;
            }
        }

        public void SetImageQuality(int quality)
        {
            imgQuality = quality;
        }

        public byte[] FromHtmlString(string html)
        {
            byte[] bytes = converter.FromHtmlString(html, width, imgFormat, imgQuality);
            return bytes;
        }
    }
}
