using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProductCompositionGroup : BaseModel
	{
        public string PerUnit {get; set;}

        public List<MProductComposition> Compositions {get; set;}

        public MProductCompositionGroup()
        {
            Compositions = new List<MProductComposition>();
        }
    }
}
