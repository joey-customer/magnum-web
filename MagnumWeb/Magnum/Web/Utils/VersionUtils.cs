using System;
using System.Reflection;

namespace Magnum.Web.Utils
{    
    public static class VersionUtils
    {
        public static string GetVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            return version;
        }     
    }    
}