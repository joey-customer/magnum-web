using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmBold300 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "BOLD300";
            Product = "BOLD300";
        }
    }
}