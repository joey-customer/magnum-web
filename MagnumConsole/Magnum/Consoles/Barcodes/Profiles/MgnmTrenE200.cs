using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmTrenE200 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TRENE200";
            Product = "TRENE200";
        }
    }
}