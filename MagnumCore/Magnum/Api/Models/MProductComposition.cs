using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProductComposition : BaseModel
	{
        public string Code {get; set;}
        public double Quantity {get; set;}
        public string Unit {get; set;}

        public Dictionary<string, MGenericDescription> Descriptions;

        public MProductComposition()
        {
            Descriptions = new Dictionary<string, MGenericDescription>();
        }
    }
}
