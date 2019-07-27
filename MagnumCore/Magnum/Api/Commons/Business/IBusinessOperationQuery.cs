using System;
using System.Collections.Generic;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperationQuery<T> : IBusinessOperation where T : class
	{
        IEnumerable<T> Apply(T dat);
    }
}
