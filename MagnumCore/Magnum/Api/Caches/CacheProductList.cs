using System.Collections.Generic;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Api.Models;

namespace Magnum.Api.Caches
{
    public class CacheProductList : CacheBase
    {
        private IBusinessOperationQuery<MProduct> opr;

        public CacheProductList()
        {
            opr = (IBusinessOperationQuery<MProduct>)FactoryBusinessOperation.CreateBusinessOperationObject("GetProductList");
        }

        protected override Dictionary<string, BaseModel> LoadContents()
        {
            var map = new Dictionary<string, BaseModel>();
            IEnumerable<MProduct> mProductTypes = opr.Apply(null, null);

            foreach (var productType in mProductTypes)
            {
                map[productType.Code] = productType;
            }

            return map;
        }

        public void SetOperation(IBusinessOperationQuery<MProduct> opr)
        {
            this.opr = opr; 
        }
    }
}