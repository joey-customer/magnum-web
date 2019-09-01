using System.Collections.Generic;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Api.Models;

namespace Magnum.Api.Caches
{
    public class CacheProductTypeList : CacheBase
    {
        private IBusinessOperationQuery<MProductType> opr;

        public CacheProductTypeList()
        {
            opr = (IBusinessOperationQuery<MProductType>)FactoryBusinessOperation.CreateBusinessOperationObject("GetProductTypeList");
        }

        protected override Dictionary<string, BaseModel> LoadContents()
        {
            var map = new Dictionary<string, BaseModel>();
            IEnumerable<MProductType> mProductTypes = opr.Apply(null, null);

            foreach (var productType in mProductTypes)
            {
                map[productType.Code] = productType;
            }

            return map;
        }

        public void SetOperation(IBusinessOperationQuery<MProductType> opr)
        {
            this.opr = opr; 
        }
    }
}