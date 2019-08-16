using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmMagJack250 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "MAG JACK 250";
            Product = "MAG JACK 250";
        }
    }
}