using System;

using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
	public class NandroPlex300 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "NANDROPLEX300";
            Product = "NANDROPLEX300";
        }
    }
}