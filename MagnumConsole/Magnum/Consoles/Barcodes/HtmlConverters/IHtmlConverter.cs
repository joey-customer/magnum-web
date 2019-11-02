namespace Magnum.Consoles.Barcodes.HtmlConverters
{
    public interface IHtmlConverter
	{
        void SetWidth(int w);
        void SetImageFormat(int fmt);
        void SetImageQuality(int quality);

        byte[] FromHtmlString(string html);
    }
}
