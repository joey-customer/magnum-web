using System;

using Microsoft.Extensions.Logging;

namespace Magnum.Api.Utils
{
	public static class LogUtils
	{
        private static void Log(ILogger logger, LogLevel level, string message, params object[] theObjects)
        {
            if (logger != null)
            {
                logger.Log(level, message, theObjects);
            }
        }

        public static void LogInformation(ILogger logger, string message, params object[] theObjects)
        {
            Log(logger, LogLevel.Information, message, theObjects);
        }       
    }    
}