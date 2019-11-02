using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
    public class MgnmPrimo100 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "PRIMO100";
            Product = "PRIMO100";
        }
    }
}