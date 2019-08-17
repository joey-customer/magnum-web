using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmTrenA100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TRENA100";
            Product = "TRENA100";
        }
    }
}