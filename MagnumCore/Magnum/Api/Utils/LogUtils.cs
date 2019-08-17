using System;

using Microsoft.Extensions.Logging;

namespace Magnum.Api.Utils
{
	public static class LogUtils
	{
        private static void Log(ILogger logger, LogLevel level, string message)
        {
            if (logger != null)
            {
                logger.Log(level, message);
            }
        }

        public static void LogInformation(ILogger logger, string message)
        {
            Log(logger, LogLevel.Information, message);
        }       
    }    
}