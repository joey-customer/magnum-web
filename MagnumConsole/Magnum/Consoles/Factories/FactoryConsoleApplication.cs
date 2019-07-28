using System;
using System.Collections;
using System.Reflection;

using Magnum.Consoles.Commons;

namespace Magnum.Consoles.Factories
{   
    public static class FactoryConsoleApplication
    {
        private static Hashtable classMaps = new Hashtable();

        private static void addClassConfig(string apiName, string fqdn)
        {
            classMaps.Add(apiName, fqdn);
        }

        static FactoryConsoleApplication()
        {
            initClassMap();
        }

        private static void initClassMap()
        {
            addClassConfig("BarcodeGen", "Magnum.Consoles.Barcodes.BarcodeGenerator");               
        }  

        public static IConsoleApp CreateConsoleApplicationObject(string name)
        {        
            string className = (string)classMaps[name];
            if (className == null)
            {
                throw new ArgumentNullException(String.Format("Application not found [{0}]", name));
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            IConsoleApp obj = (IConsoleApp)asm.CreateInstance(className);

            return(obj);
        }
    }
 
}