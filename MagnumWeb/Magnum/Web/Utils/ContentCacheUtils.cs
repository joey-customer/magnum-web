using System;
using System.Collections.Generic;
using Magnum.Api.Businesses.Contents;
using Magnum.Api.Commons.Business;
using Magnum.Api.Factories;
using Magnum.Api.Models;

namespace Magnum.Api.Utils
{
    public class ContentCacheUtils
    {
        public Dictionary<string, Dictionary<string, string>> contents { get; set; }

        public DateTime lastRefreshTime { get; set; }

        static ContentCacheUtils instance = new ContentCacheUtils();

        public static ContentCacheUtils GetInstance()
        {
            return instance;
        }

        public virtual Dictionary<string, Dictionary<string, string>> GetContents()
        {
            if (this.contents == null || IsRefreshTime())
            {
                this.contents = LoadContents();
                this.lastRefreshTime = DateTime.Now;
            }

            return this.contents;
        }

        public virtual bool IsRefreshTime()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime - lastRefreshTime;

            if (diff.TotalMinutes > 5)
            {
                return true;
            }
            return false;
        }

        public virtual Dictionary<string, Dictionary<string, string>> LoadContents()
        {
            var map = new Dictionary<string, Dictionary<string, string>>();

            var opr = GetContentListOperation();
            var mContents = opr.Apply(null, null);
            foreach (var item in mContents)
            {
                map[item.Type + "/" + item.Name] = item.Value;
            }
            return map;
        }

        public virtual IBusinessOperationQuery<MContent> GetContentListOperation()
        {
            return (IBusinessOperationQuery<MContent>)FactoryBusinessOperation.CreateBusinessOperationObject("GetContentList");
        }
    }
}