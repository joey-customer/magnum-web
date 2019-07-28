using System;
using System.Collections;
using NDesk.Options;

namespace Magnum.Consoles.Commons
{
	public interface IConsoleApp
	{
        int Run();
        void SetArguments(Hashtable args);
        OptionSet CreateOptionSet();
    }
}
