using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmDbol10 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "D BOL 10";
            Product = "D BOL 10";
        }
    }
}