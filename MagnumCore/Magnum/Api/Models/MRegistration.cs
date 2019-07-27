using System;
using System.Collections;

namespace Magnum.Api.Models
{
	public class MRegistration
	{
        public int RegistrationId {get; set;}
        public int BarcodeId {get; set;}        
        public string RegistrationDate {get; set;}
        public string Status {get; set;}
    }
}
