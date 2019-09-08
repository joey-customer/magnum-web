using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmDropstanP100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "DROPS100";
            Product = "DROPS100";
        }
    }
}