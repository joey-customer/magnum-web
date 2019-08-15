using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
	public class MProductType : BaseModel
	{
        public string Code {get; set;}

        public Dictionary<string, MGenericDescription> Descriptions {get; set;}

        public MProductType()
        {
            Descriptions = new Dictionary<string, MGenericDescription>();
        }

        public bool IsKeyIdentifiable()
        {
            bool isError = string.IsNullOrEmpty(Code);
            return !isError;
        }   
    }
}
