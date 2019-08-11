using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class DeleteProduct : BusinessOperationBase, IBusinessOperationManipulate<MProduct>
	{
        public int Apply(MProduct dat)
        {
            if (!dat.IsKeyIdentifiable() || !dat.IsKeyExist())
            {
                throw(new ArgumentException("Language, Code, Key must not be null!!!"));
            }

            var ctx = GetNoSqlContext();
            string prdPath = string.Format("products/{0}/{1}", dat.Code, dat.Language);
            int rowAffected = ctx.DeleteData(prdPath, dat);

            return rowAffected;
        }
    }
}
