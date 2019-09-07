using System;

namespace Magnum.Api.Models
{
	public class MGenericDescription : BaseModel
	{
        public string Language {get; set;}
        public string Name {get; set;}
        public string ShortDescription {get; set;}
        public string LongDescription1 {get; set;}   
        public string LongDescription2 {get; set;}   
        public string Extra1 {get; set;}
    }
}
