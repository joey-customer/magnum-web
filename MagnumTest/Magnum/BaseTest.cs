using Its.Onix.Core.Factories;
using Its.Onix.Erp.Services;

namespace Magnum
{
    public class BaseTest
    {
        public BaseTest()
        {      
            FactoryBusinessOperation.ClearRegisteredItems();
            FactoryBusinessOperation.RegisterBusinessOperations(BusinessErpOperations.GetBusinessOperationList());

            FactoryCacheContext.ClearRegisteredItems();
            FactoryCacheContext.RegisterCaches(BusinessErpCaches.BusinessErpCachesList());
        }
    }
}