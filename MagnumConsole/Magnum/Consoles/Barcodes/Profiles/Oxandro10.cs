using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class Oxandro10 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "2009091112";
            Product = "OXANDRO 10";
        }
    }
}