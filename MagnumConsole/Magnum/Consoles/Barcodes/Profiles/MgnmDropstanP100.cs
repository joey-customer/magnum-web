using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmDropstanP100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "DROSTAN P100";
            Product = "DROSTAN P100";
        }
    }
}