using System;
using System.Collections.Generic;
using Magnum.Api.Commons.Table;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperationQuery<T> : IBusinessOperation where T : class
	{
        IEnumerable<T> Apply(T dat, CTable param);
    }
}
