using NUnit.Framework;

namespace Magnum.Consoles.Utils
{
    public class FactoryBusinessOperationUtilsTest
    {
        [Test]
        public void LoadBusinessOperationsTest()
        {
            FactoryBusinessOperationUtils.LoadBusinessOperations();
        }
    }
}