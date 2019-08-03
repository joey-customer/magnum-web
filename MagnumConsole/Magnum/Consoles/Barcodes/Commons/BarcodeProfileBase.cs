using System;

namespace Magnum.Consoles.Barcodes.Commons
{
	public abstract class BarcodeProfileBase : IBarcodeProfile
	{        
        public string TemplateFile {get; set;}
        //2of5 or UPC barcode to represent the SKU number
        public string Barcode {get; set;}
        //Product Code or Name
        public string Product {get; set;}
        public string CompanyWebSite {get; set;}

        protected abstract void CustomSetting();

        public virtual void Setup()
        {
            TemplateFile = "configs/templates/magnum_label.html";
            CompanyWebSite = "www.magnum-pharmacy.com";
            
            CustomSetting(); 
        }
    }
}