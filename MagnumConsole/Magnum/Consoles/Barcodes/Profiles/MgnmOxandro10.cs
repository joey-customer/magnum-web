using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmOxandro10 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "OXANDR10";
            Product = "OXANDR10";
        }
    }
}