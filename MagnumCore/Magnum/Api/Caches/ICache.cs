using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using Magnum.Api.Models;

namespace Magnum.Api.Caches
{
	public interface ICache
	{
        void SetRefreshInterval(long tickCount);
        DateTime GetLastRefreshDtm();
        void SetLastRefreshDtm(DateTime dtm);
        
        Dictionary<string, BaseModel> GetValues();
        BaseModel GetValue(string key);

        void SetLogger(ILogger logger);
        ILogger GetLogger();        
    }    
}
