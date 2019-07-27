using System;
using Microsoft.Extensions.Configuration;

namespace Magnum.Api.Utils
{    
    public class LibSetting
    {
        private static LibSetting instance = new LibSetting();

        private IConfigurationRoot config = null;
        private string connStr = "";
        private string secretKey = "";
        private string apiKey = "";

        private LibSetting()
        {
        }

        public static LibSetting GetInstance()
        {
            return(instance);
        }

        public IConfigurationRoot Configuration
        {
            set 
            {
                connStr = value["MAGNUM_CONNECTION_STR"];
                apiKey = value["MAGNUM_EXTERNAL_APPLICATION_KEY"];
                secretKey = value["MAGNUM_OAUTH_KEY"];

                instance.config = value;
            }

            get
            {
                return(instance.config);
            }
        }

        public string ConnectionString
        {
            set 
            {
                instance.connStr = value;
            }

            get
            {
                return(instance.connStr);
            }
        }  

        public string OAuthKey
        {
            set 
            {
                instance.secretKey = value;
            }

            get
            {
                return(instance.secretKey);
            }
        }  

        public string ExternalApplicationKey
        {
            set 
            {
                instance.apiKey = value;
            }

            get
            {
                return(instance.apiKey);
            }
        }                                
    }
}