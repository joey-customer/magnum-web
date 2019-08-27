using System;
using System.Collections.Generic;
using Magnum.Api.Businesses.Contents;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Api.Models;

namespace Magnum.Api.Caches
{
    public class CachePageContents : CacheBase
    {
        private IBusinessOperationQuery<MContent> opr;

        public CachePageContents()
        {
            opr = (IBusinessOperationQuery<MContent>)FactoryBusinessOperation.CreateBusinessOperationObject("GetContentList");
        }

        protected override Dictionary<string, BaseModel> LoadContents()
        {
            var map = new Dictionary<string, BaseModel>();
            IEnumerable<MContent> mContents = opr.Apply(null, null);

            foreach (var content in mContents)
            {
                map[content.Type + "/" + content.Name] = content;
            }

            return map;
        }

        public void SetOperation(IBusinessOperationQuery<MContent> opr)
        {
            this.opr = opr; 
        }
    }
}