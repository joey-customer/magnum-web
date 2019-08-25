using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using Magnum.Api.Utils;
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
            if (contents == null || IsRefreshTime())
            {
                contents = LoadContents();
                int cnt = contents.Count;
                SetLastRefreshDtm(DateTime.Now);

                LogUtils.LogInformation(appLogger, "Refreshed [{0}] item(s) by [{1}], tick count = [{2}]", cnt, this.GetType().Name, tc);
            }

            return contents;
        }

        public BaseModel GetValue(string key)
        {
            var values = GetValues();
            BaseModel content = values[key];

            return content;
        }

        private bool IsRefreshTime()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime - lastRefreshTime;

            bool isExpire = (diff.Ticks > tc);
            return isExpire;
        }

        public void SetContents(Dictionary<string, BaseModel> contents)
        {
            this.contents = contents;
        }
    }
}