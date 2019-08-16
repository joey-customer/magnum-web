using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class DoNotUse : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            CompanyWebSite = "www.for-testing-only.com";
            Barcode = "DO NOT USE";
            Product = "DO NOT USE";
        }
    }
}