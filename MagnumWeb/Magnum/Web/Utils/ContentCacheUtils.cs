using System;
using System.Collections.Generic;
using Magnum.Api.Businesses.Contents;
using Magnum.Api.Factories;
using Magnum.Api.Models;

namespace Magnum.Api.Utils
{
    public class ContentCacheUtils
    {
        private Dictionary<string, Dictionary<string, string>> contents;

        private DateTime lastRefreshTime;

        static ContentCacheUtils instance = new ContentCacheUtils();

        private ContentCacheUtils()
        {
        }

        public static ContentCacheUtils GetInstance()
        {
            return instance;
        }

        public Dictionary<string, Dictionary<string, string>> GetContents()
        {
            if (this.contents == null || IsRefreshTime())
            {
                this.contents = LoadContents();
                this.lastRefreshTime = DateTime.Now;
            }

            return this.contents;
        }

        private bool IsRefreshTime()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime - lastRefreshTime;

            if (diff.TotalMinutes > 5)
            {
                return true;
            }
            return false;
        }

        private Dictionary<string, Dictionary<string, string>> LoadContents()
        {
            Dictionary<string, Dictionary<string, string>> contents = new Dictionary<string, Dictionary<string, string>>();

            GetContentList opr = (GetContentList)FactoryBusinessOperation.CreateBusinessOperationObject("GetContentList");
            IEnumerable<MContent> mContents = opr.Apply(null, null);
            foreach (var item in mContents)
            {
                contents[item.Type + "/" + item.Name] = item.Value;
            }
            return contents;
        }
    }
}