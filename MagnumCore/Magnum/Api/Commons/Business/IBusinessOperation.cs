using System;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperation
	{
        void SetNoSqlContext(INoSqlContext context);
        void SetStorageContext(IStorageContext context);
    }
}
