using NUnit.Framework;

namespace Magnum.Consoles.Utils
{
    public class FactoryBusinessOperationUtilsTest
    {
        [Test]
        public void LoadBusinessOperationsTest()
        {
            //Load
            FactoryBusinessOperationUtils.LoadBusinessOperations();

            //No reload
            FactoryBusinessOperationUtils.LoadBusinessOperations();
        }
    }
}