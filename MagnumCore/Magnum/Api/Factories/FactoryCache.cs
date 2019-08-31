using System;
using System.Collections;
using System.Reflection;

using Magnum.Api.Caches;

using Microsoft.Extensions.Logging;

namespace Magnum.Api.Factories
{
    public static class FactoryCache
    {
        private static ILoggerFactory loggerFactory = null;
        private static Hashtable classMaps = new Hashtable();
        private static Hashtable objectMaps = new Hashtable();

        private static void addClassConfig(string apiName, string fqdn)
        {
            classMaps.Add(apiName, fqdn);
        }

        static FactoryCache()
        {
            initClassMap();
        }

        private static void initClassMap()
        {
            addClassConfig("CachePageContents", "Magnum.Api.Caches.CachePageContents");
            addClassConfig("CacheProductTypeList", "Magnum.Api.Caches.CacheProductTypeList");
            addClassConfig("CacheProductList", "Magnum.Api.Caches.CacheProductList");
        }

        public static ICache GetCacheObject(string name)
        {
            string className = (string)classMaps[name];
            if (className == null)
            {
                throw new ArgumentNullException(String.Format("Cache name not found [{0}]", name));
            }

            ICache cacheObj = (ICache)objectMaps[name];
            if (cacheObj == null)
            {
                //Create just only one time and reuse it later
                //Using lazy approach

                Assembly asm = Assembly.GetExecutingAssembly();
                cacheObj = (ICache)asm.CreateInstance(className);

                objectMaps[name] = cacheObj;

                if (loggerFactory != null)
                {
                    Type t = cacheObj.GetType();
                    ILogger logger = loggerFactory.CreateLogger(t);
                    cacheObj.SetLogger(logger);
                }
            }

            return (cacheObj);
        }

        public static void SetLoggerFactory(ILoggerFactory logFact)
        {
            loggerFactory = logFact;
        }
    }

}