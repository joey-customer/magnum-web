using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MContactUs : BaseModel
	{
        public string Name {get; set;}
        public string Subject {get; set;}
        public string Email {get; set;}
        public string Message {get; set;}
        public string IP {get; set;}
    }
}
