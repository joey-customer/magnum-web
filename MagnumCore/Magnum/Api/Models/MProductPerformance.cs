using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProductPerformance : BaseModel
	{
        public string Code {get; set;}
        public double Quantity {get; set;}
        public string Unit {get; set;}

        public Dictionary<string, MGenericDescription> Descriptions {get; set;}

        public MProductPerformance()
        {
            Descriptions = new Dictionary<string, MGenericDescription>();
        }
    }
}
