namespace Magnum.Consoles.Barcodes.Commons
{
    public abstract class QRProfileBase : IBarcodeProfile
	{        
        public string TemplateFile {get; set;}
        public string Message1 {get; set;}
        public string Message2 {get; set;}
        public string CompanyWebSite {get; set;}

        protected abstract void CustomSetting();

        public virtual void Setup()
        {
            CustomSetting(); 
        }
    }
}