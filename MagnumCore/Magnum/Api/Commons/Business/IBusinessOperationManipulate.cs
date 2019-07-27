using System;
using System.Collections.Generic;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperationManipulate<T> : IBusinessOperation where T : class
	{
        int Apply(T dat);
    }
}
