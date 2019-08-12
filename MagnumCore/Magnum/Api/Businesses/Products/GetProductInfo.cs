using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class GetProductInfo : BusinessOperationBase, IBusinessOperationGetInfo<MProduct>
	{
        public MProduct Apply(MProduct dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Code and Key must not be null!!!"));
            }

            var ctx = GetNoSqlContext();
            MProduct prd = ctx.GetSingleObject<MProduct>("products", dat.Code);

            return prd;
        }
    }
}
