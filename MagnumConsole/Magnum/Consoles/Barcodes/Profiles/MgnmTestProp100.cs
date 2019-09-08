using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmTestProp100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TESTP100";
            Product = "TESTP100";
        }
    }
}