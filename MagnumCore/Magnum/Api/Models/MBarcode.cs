using System;
using System.Collections;

namespace Magnum.Api.Models
{
	public class MBarcode : BaseModel
	{
        //2of5 or UPC barcode to represent the SKU number
        public string Barcode {get; set;}
        //Product Code or Name
        public string Product {get; set;}
        public string CompanyWebSite {get; set;}

        public bool IsActivated {get; set;}

        //BatchNo will be the same for each barcode generated
        public string BatchNo {get; set;}
        
        public string SerialNumber {get; set;}
        public string Pin {get; set;}        
        public string Url {get; set;}
        public string PayloadUrl {get; set;}

        //Firebase path to the barcode
        public string Path {get; set;}

        //Date barcode is created
        public DateTime GeneratedDate {get; set;}

        //Date barcode is registered/activated
        public DateTime ActivatedDate {get; set;}
    }
}
