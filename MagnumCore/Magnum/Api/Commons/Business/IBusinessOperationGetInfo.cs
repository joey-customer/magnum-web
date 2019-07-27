using System;
using System.Collections.Generic;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperationGetInfo<T> : IBusinessOperation where T : class
	{
        T Apply(T dat);
    }
}
