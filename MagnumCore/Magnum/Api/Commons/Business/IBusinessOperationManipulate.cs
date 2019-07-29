using System;
using System.Collections.Generic;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperationManipulate<in T> : IBusinessOperation where T : class
	{
        int Apply(T dat);
    }
}