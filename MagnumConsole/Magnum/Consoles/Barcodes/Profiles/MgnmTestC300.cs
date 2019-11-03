using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
    public class MgnmTestC300 : BarcodeProfileBase
	{
        protected override void CustomSetting()
        {
            Barcode = "TESTC300";
            Product = "TESTC300";
        }
    }
}