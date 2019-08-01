using System;

namespace Magnum.Api.Models
{
	public class MRegistration : BaseModel
	{
        public string SerialNumber {get; set;}
        public string Pin {get; set;}
        public DateTime RegistrationDate {get; set;}
        public string Status {get; set;}
        public string Path {get; set;}
        public string IP {get; set;}
    }
}
