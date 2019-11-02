using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
    public class MgnmTestE300 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TESTE300";
            Product = "TESTE300";
        }
    }
}