using System;
using System.Collections.Generic;

using Magnum.Api.Commons.Business;
using Magnum.Api.Commons.Table;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.ProductTypes
{
	public class GetProductTypeList : BusinessOperationBase, IBusinessOperationQuery<MProductType>
	{
        public IEnumerable<MProductType> Apply(MProductType dat, CTable param)
        {
            //Parameter "dat" can be use for create the filter in the future
            var ctx = GetNoSqlContext();
            IEnumerable<MProductType> products = ctx.GetObjectList<MProductType>("product_types");

            return products;
        }
    }
}
