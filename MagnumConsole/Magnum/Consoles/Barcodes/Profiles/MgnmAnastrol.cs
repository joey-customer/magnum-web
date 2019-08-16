using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmAnastrol : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "ANASTROL";
            Product = "ANASTROL";
        }
    }
}