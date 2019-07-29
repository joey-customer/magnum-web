using System;
using Magnum.Api.NoSql;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperation
	{
        void SetContext(INoSqlContext context);
    }
}
