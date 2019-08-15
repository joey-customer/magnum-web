using System;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using Magnum.Api.Smtp;

namespace Magnum.Api.Commons.Business
{    
	public class BusinessOperationBase : IBusinessOperation
	{
        private INoSqlContext noSqlContext = null;
        private IStorageContext storageContext = null;
        private ISmtpContext smtpContext = null;

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

        public void SetSmtpContext(ISmtpContext context)
        {
            smtpContext = context;
        }

        public ISmtpContext GetSmtpContext()
        {
            return smtpContext;
        }  

        public IStorageContext GetStorageContext()
        {
            return storageContext;
        }        
    }
}
