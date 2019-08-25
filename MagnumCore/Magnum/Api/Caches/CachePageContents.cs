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
        private readonly IBusinessOperationQuery<MContent> opr;

        public CachePageContents()
        {
            opr = GetContentListOperation();
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

        public virtual IBusinessOperationQuery<MContent> GetContentListOperation()
        {
            var opr = (IBusinessOperationQuery<MContent>)FactoryBusinessOperation.CreateBusinessOperationObject("GetContentList");
            return opr;
        }
    }
}