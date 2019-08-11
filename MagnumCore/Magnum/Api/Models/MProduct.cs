using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProduct : BaseModel
	{
        public string Code {get; set;}
        public string Name {get; set;}
        public string ShortDescription {get; set;}
        public string LongDescription {get; set;}
        public int Rating {get; set;}
        public string ProductType {get; set;}
        public string Language {get; set;}
        public string ImageUrl {get; set;}
        public string ImagePath {get; set;}
        public DateTime LastUpdateDate {get; set;}

        public List<MProductComposition> Compositions {get; set;}

        public MProduct()
        {
            Compositions = new List<MProductComposition>();
        }

        public bool IsKeyIdentifiable()
        {
            bool isError = string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Language);
            return !isError;
        }

        public bool IsKeyExist()
        {
            bool isError = string.IsNullOrEmpty(Key);
            return !isError;
        }        
    }
}
