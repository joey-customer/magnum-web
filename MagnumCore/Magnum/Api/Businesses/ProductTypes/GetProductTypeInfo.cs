using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.ProductTypes
{
	public class GetProductTypeInfo : BusinessOperationBase, IBusinessOperationGetInfo<MProductType>
	{
        public MProductType Apply(MProductType dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Code and Key must not be null!!!"));
            }

            var ctx = GetNoSqlContext();
            MProductType prd = ctx.GetSingleObject<MProductType>("product_types", dat.Code);

            return prd;
        }
    }
}
