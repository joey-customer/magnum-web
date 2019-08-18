using System;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using Magnum.Api.Smtp;

using Microsoft.Extensions.Logging;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperation
	{
        void SetNoSqlContext(INoSqlContext context);
        void SetStorageContext(IStorageContext context);
        void SetSmtpContext(ISmtpContext context); 

        void SetLogger(ILogger logger);
        ILogger GetLogger();
    }
}
