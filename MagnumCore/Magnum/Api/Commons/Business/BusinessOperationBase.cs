using System;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;

namespace Magnum.Api.Commons.Business
{    
	public class BusinessOperationBase : IBusinessOperation
	{
        private INoSqlContext noSqlContext = null;
        private IStorageContext storageContext = null;

        public void SetNoSqlContext(INoSqlContext context)
        {
            noSqlContext = context;
        }

        public INoSqlContext GetNoSqlContext()
        {
            return noSqlContext;
        }

        public void SetStorageContext(IStorageContext context)
        {
            storageContext = context;
        }

        public IStorageContext GetStorageContext()
        {
            return storageContext;
        }        
    }
}
