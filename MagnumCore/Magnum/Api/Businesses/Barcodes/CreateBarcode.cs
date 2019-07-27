using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Barcodes
{
	public class CreateBarcode : BusinessOperationBase, IBusinessOperationManipulate<MBarcode>
	{
        public int Apply(MBarcode dat)
        {
            return 0;
        }
    }
}
