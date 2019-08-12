using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProduct : BaseModel
	{
        public string Code {get; set;}
        public int Rating {get; set;}
        public string ProductType {get; set;}

        public string Image1Url {get; set;}
        public string Image1LocalPath {get; set;}
        public string Image1StoragePath {get; set;}

        public DateTime LastUpdateDate {get; set;}

        public List<MProductComposition> Compositions {get; set;}
        public Dictionary<string, MGenericDescription> Descriptions {get; set;}

        public MProduct()
        {
            Compositions = new List<MProductComposition>();
            Descriptions = new Dictionary<string, MGenericDescription>();
        }

        public bool IsKeyIdentifiable()
        {
            bool isError = string.IsNullOrEmpty(Code);
            return !isError;
        }   
    }
}
