using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using Magnum.Api.Models;

namespace Magnum.Api.Caches
{
    public abstract class CacheBase : ICache
    {
        private ILogger appLogger;
        private long tc = TimeSpan.TicksPerMinute * 5;
        private DateTime lastRefreshTime;
        private Dictionary<string, BaseModel> contents;  

        protected abstract Dictionary<string, BaseModel> LoadContents();

        public void SetLogger(ILogger logger)
        {
            appLogger = logger;
        }

        public ILogger GetLogger()
        {
            return appLogger;
        }

        public void SetRefreshInterval(long tickCount)
        {
            tc = tickCount;
        }

        public DateTime GetLastRefreshDtm()
        {
            return lastRefreshTime;
        }

        public void SetLastRefreshDtm(DateTime dtm)
        {
            lastRefreshTime = dtm;
        }

        public Dictionary<string, BaseModel> GetValues()
        {
            if (this.contents == null || IsRefreshTime())
            {
                this.contents = LoadContents();
                SetLastRefreshDtm(DateTime.Now);
            }

            return this.contents;
        }

        public BaseModel GetValue(string key)
        {
            var values = GetValues();
            BaseModel content = (BaseModel) values[key];

            return content;
        }

        private bool IsRefreshTime()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime - lastRefreshTime;

            bool isExpire = (diff.Ticks > tc);
            return isExpire;
        }        
    }
}