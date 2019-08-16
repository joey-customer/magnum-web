using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class MgnmTestProp100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TESTPROP100";
            Product = "TESTPROP100";
        }
    }
}