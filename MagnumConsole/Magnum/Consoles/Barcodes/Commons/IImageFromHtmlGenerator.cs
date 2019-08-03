using System;
using System.IO;

using Magnum.Consoles.Barcodes.HtmlConverters;

namespace Magnum.Consoles.Barcodes.Commons
{
	public interface IImageFromHtmlGenerator
	{
        void SetHtmlConverter(IHtmlConverter converter);
    }
}
