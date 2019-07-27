using System;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Magnum.Api.Utils
{
    public class LibSettingTest
    {
        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable ("MAGNUM_CONNECTION_STR", "DUMMY1");
            Environment.SetEnvironmentVariable ("MAGNUM_EXTERNAL_APPLICATION_KEY", "DUMMY2");
            Environment.SetEnvironmentVariable ("MAGNUM_OAUTH_KEY", "DUMMY3");
        }

        private LibSetting getLibSettingInstance()
        {
            LibSetting instance = LibSetting.GetInstance();
            return instance;
        }

        private IConfigurationRoot getConfigurationRoot()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();

            IConfigurationRoot cfg = builder.Build();
            return cfg;
        }

        [TestCase]
        public void PassValuesToLibSettingTest()
        {
            LibSetting lib = getLibSettingInstance();
            IConfigurationRoot cfg = getConfigurationRoot();
            lib.Configuration = cfg;
        
            string connStr = lib.ConnectionString;
            Assert.AreEqual("DUMMY1", connStr, "MAGNUM_CONNECTION_STR string mismatch !!!"); 

            string extKey = lib.ExternalApplicationKey;
            Assert.AreEqual("DUMMY2", extKey, "MAGNUM_EXTERNAL_APPLICATION_KEY string mismatch !!!");  

            string oauthKey = lib.OAuthKey;
            Assert.AreEqual("DUMMY3", oauthKey, "MAGNUM_OAUTH_KEY string mismatch !!!");

            IConfigurationRoot cfgOut = lib.Configuration;
            Assert.NotNull(cfgOut, "Configuration is null !!!");
        }

        [TestCase]
        public void InjectValuesToLibSettingTest()
        {
            LibSetting lib = getLibSettingInstance();
            IConfigurationRoot cfg = getConfigurationRoot();
            lib.Configuration = cfg;
        
            lib.ConnectionString = "INJECT1";
            Assert.AreEqual("INJECT1", lib.ConnectionString, "MAGNUM_CONNECTION_STR string mismatch !!!"); 

            lib.ExternalApplicationKey = "INJECT2";
            Assert.AreEqual("INJECT2", lib.ExternalApplicationKey, "MAGNUM_EXTERNAL_APPLICATION_KEY string mismatch !!!");  

            lib.OAuthKey = "INJECT3";
            Assert.AreEqual("INJECT3", lib.OAuthKey, "MAGNUM_OAUTH_KEY string mismatch !!!");
        }                                                
    }    
}