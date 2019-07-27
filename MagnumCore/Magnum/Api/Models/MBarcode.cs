using System;
using System.Collections;

namespace Magnum.Api.Models
{
	public class MBarcode
	{
        public int BarcodeId {get; set;}
        public string SerialNumber {get; set;}
        public string Pin {get; set;}
        public string Barcode {get; set;}
        public string Url {get; set;}
        public string GeneratedDate {get; set;}
    }
}
