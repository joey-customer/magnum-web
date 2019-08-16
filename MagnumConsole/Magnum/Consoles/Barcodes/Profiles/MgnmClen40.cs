using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmClen40 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "CLEN 40";
            Product = "CLEN 40";
        }
    }
}