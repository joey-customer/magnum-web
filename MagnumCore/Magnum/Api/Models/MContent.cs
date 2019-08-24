using System;
using System.Collections.Generic;

namespace Magnum.Api.Models
{
    public class MContent : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<String, String> Value { get; set; }

        public bool IsKeyIdentifiable()
        {
            bool isError = string.IsNullOrEmpty(Name);
            return !isError;
        }
    }
}
