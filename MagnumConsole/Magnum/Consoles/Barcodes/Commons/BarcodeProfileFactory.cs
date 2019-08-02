using System;
using System.Collections;
using System.Reflection;

namespace Magnum.Consoles.Barcodes.Commons
{   
    public static class BarcodeProfileFactory
    {
        private static Hashtable classMaps = new Hashtable();

        private static void addClassConfig(string apiName, string fqdn)
        {
            classMaps.Add(apiName, fqdn);
        }

        static BarcodeProfileFactory()
        {
            initClassMap();
        }

        private static void initClassMap()
        {            
            addClassConfig("Oxandro10", "Magnum.Consoles.Barcodes.Profiles.Oxandro10"); 
        }  

        public static IBarcodeProfile CreateBarcodeProfileObject(string name)
        {
            string className = (string)classMaps[name];
            if (className == null)
            {
                throw new ArgumentNullException(String.Format("Profile not found [{0}]", name));
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            IBarcodeProfile obj = (IBarcodeProfile)asm.CreateInstance(className);
            obj.Setup();

            return(obj);
        }
    }
}