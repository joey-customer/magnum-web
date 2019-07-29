using System;
using Magnum.Api.NoSql;
using NDesk.Options;

namespace Magnum.Consoles.Commons
{
	public interface IConsoleApp
	{
        int Run();
        OptionSet CreateOptionSet();
        void SetNoSqlContext(INoSqlContext context);
        INoSqlContext GetNoSqlContext();
    }
}
