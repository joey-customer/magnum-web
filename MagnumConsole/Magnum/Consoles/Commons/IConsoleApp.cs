using System;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using NDesk.Options;

using Microsoft.Extensions.Logging;

namespace Magnum.Consoles.Commons
{
	public interface IConsoleApp
	{
        int Run();
        OptionSet CreateOptionSet();
        void SetNoSqlContext(INoSqlContext context);
        INoSqlContext GetNoSqlContext();

        void SetStorageContext(IStorageContext context);
        IStorageContext GetStorageContext();

        void SetLogger(ILogger logger);
        ILogger GetLogger();
    }
}
