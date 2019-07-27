using System;
using System.Collections;

namespace Magnum.Api.Models
{
	public class MBarcode
	{
        int BarcodeId {get; set;}
        string SerialNumber {get; set;}
        string Pin {get; set;}
        string Barcode {get; set;}
        string Url {get; set;}
        string GeneratedDate {get; set;}
    }
}
