using System;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Storages;
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
