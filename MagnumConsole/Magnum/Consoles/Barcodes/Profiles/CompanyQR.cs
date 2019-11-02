using Magnum.Consoles.Barcodes.Commons;

namespace Magnum.Consoles.Barcodes.Profiles
{
    public class CompanyQR : QRProfileBase
	{
        protected override void CustomSetting()
        {
            TemplateFile = "configs/templates/magnum_qr.html";
            CompanyWebSite = "https://www.magnum-pharmacy.com";   
            
            Message1 = "Visit us or verify you product at";         
            Message2 = "magnum-pharmacy.com";            
        }
    }
}