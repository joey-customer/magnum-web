using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmStanolAQ100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "STANO100";
            Product = "STANO100";
        }
    }
}