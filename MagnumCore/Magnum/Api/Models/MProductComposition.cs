using System;

namespace Magnum.Api.Models
{
	public class MProductComposition : BaseModel
	{
        public string Code {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public double Quantity {get; set;}
        public string Unit {get; set;}
    }
}
