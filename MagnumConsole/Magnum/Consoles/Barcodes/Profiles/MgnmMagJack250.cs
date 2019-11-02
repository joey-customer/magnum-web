using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
    public class MgnmMagJack250 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "MAGJA250";
            Product = "MAGJA250";
        }
    }
}