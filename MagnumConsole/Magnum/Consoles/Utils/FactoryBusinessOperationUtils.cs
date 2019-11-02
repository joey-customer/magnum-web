using System;
using Its.Onix.Core.Factories;
using Its.Onix.Erp.Services;

namespace Magnum.Consoles.Utils
{
    public static class FactoryBusinessOperationUtils
    {
        private static bool isLoad = false;

        public static void LoadBusinessOperations()
        {
            if (!isLoad)
            {
                var servicesList = BusinessErpOperations.GetInstance().ExportedServicesList();
                try
                {
                    FactoryBusinessOperation.RegisterBusinessOperations(servicesList);
                }
                catch (Exception)
                {
                    FactoryBusinessOperation.ClearRegisteredItems();
                    FactoryBusinessOperation.RegisterBusinessOperations(servicesList);
                }

                isLoad = true;
            }
        }
    }
}