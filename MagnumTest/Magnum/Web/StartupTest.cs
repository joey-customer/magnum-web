using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Magnum.Web
{
    public class StartupTest
    {
        [Test]
        public void Test()
        {
            IConfiguration config = new Mock<IConfiguration>().Object;
            Startup startup = new Startup(config);
            Assert.AreEqual(config, startup.Configuration);
        }
    }
}