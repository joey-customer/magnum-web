using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class TestPlex300 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TESTPLEX300";
            Product = "TESTPLEX300";
        }
    }
}