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
                throw(new ArgumentException("Language, Code and Key must not be null!!!"));
            }

            var ctx = GetNoSqlContext();
            string prdPath = string.Format("products/{0}/{1}", dat.Code, dat.Language);
            MProduct prd = ctx.GetObjectByKey<MProduct>(prdPath);

            return prd;
        }
    }
}
