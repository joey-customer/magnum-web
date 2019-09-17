using Its.Onix.Core.Factories;
using Its.Onix.Erp.Services;

namespace Magnum.Consoles
{
    public class BaseTest
    {
        private static bool isLoad = false;
        public BaseTest()
        {            
            if (!isLoad)
            {
                FactoryBusinessOperation.RegisterBusinessOperations(BusinessErpOperations.GetBusinessOperationList());
                FactoryCacheContext.RegisterCaches(BusinessErpCaches.BusinessErpCachesList());
                isLoad = true;
            }
        }
    }
}