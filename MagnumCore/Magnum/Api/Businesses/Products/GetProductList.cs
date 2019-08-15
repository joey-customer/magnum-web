using System;
using System.Collections.Generic;

using Magnum.Api.Commons.Business;
using Magnum.Api.Commons.Table;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Products
{
	public class GetProductList : BusinessOperationBase, IBusinessOperationQuery<MProduct>
	{
        public IEnumerable<MProduct> Apply(MProduct dat, CTable param)
        {
            //Parameter "dat" can be use for create the filter in the future
            var ctx = GetNoSqlContext();
            IEnumerable<MProduct> products = ctx.GetObjectList<MProduct>("products");

            return products;
        }
    }
}
