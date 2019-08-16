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
            addClassConfig("ForTestingOnly", "Magnum.Consoles.Barcodes.Profiles.ForTestingOnly"); 
            addClassConfig("ForQrTestingOnly", "Magnum.Consoles.Barcodes.Profiles.ForQrTestingOnly"); 

            addClassConfig("MgnmAnastrol", "Magnum.Consoles.Barcodes.Profiles.MgnmAnastrol"); 
            addClassConfig("MgnmBold300", "Magnum.Consoles.Barcodes.Profiles.MgnmBold300"); 
            addClassConfig("MgnmClen40", "Magnum.Consoles.Barcodes.Profiles.MgnmClen40"); 
            addClassConfig("MgnmDbol10", "Magnum.Consoles.Barcodes.Profiles.MgnmDbol10"); 
            addClassConfig("MgnmDropstanP100", "Magnum.Consoles.Barcodes.Profiles.MgnmDropstanP100"); 
            addClassConfig("MgnmMagJack250", "Magnum.Consoles.Barcodes.Profiles.MgnmMagJack250"); 
            addClassConfig("MgnmNandroPlex300", "Magnum.Consoles.Barcodes.Profiles.MgnmNandroPlex300"); 
            addClassConfig("MgnmOxandro10", "Magnum.Consoles.Barcodes.Profiles.MgnmOxandro10"); 
            addClassConfig("MgnmPrimo100", "Magnum.Consoles.Barcodes.Profiles.MgnmPrimo100"); 
            addClassConfig("MgnmStanol10", "Magnum.Consoles.Barcodes.Profiles.MgnmStanol10"); 
            addClassConfig("MgnmStanolAQ100", "Magnum.Consoles.Barcodes.Profiles.MgnmStanolAQ100");
            addClassConfig("MgnmTestC300", "Magnum.Consoles.Barcodes.Profiles.MgnmTestC300"); 
            addClassConfig("MgnmTestE300", "Magnum.Consoles.Barcodes.Profiles.MgnmTestE300"); 
            addClassConfig("MgnmTestPlex300", "Magnum.Consoles.Barcodes.Profiles.MgnmTestPlex300"); 
            addClassConfig("MgnmTestProp100", "Magnum.Consoles.Barcodes.Profiles.MgnmTestProp100"); 
            addClassConfig("MgnmTrenA100", "Magnum.Consoles.Barcodes.Profiles.MgnmTrenA100");    
            addClassConfig("MgnmTrenE200", "Magnum.Consoles.Barcodes.Profiles.MgnmTrenE200");             

            addClassConfig("CompanyQR", "Magnum.Consoles.Barcodes.Profiles.CompanyQR"); 
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

        public static Hashtable GetKnownClassList()
        {
            return classMaps;
        }
    }
}