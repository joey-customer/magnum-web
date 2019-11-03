using System;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace Magnum.Web
{
    public class ProgramTest
    {
        [TestCase("Y")]
        [TestCase("N")]
        [TestCase("")]
        public void CreateWebHostBuilder(string cerificate)
        {
            Environment.SetEnvironmentVariable("MAGNUM_CERTIFICATE_ON", cerificate);
            IWebHostBuilder result = Program.CreateWebHostBuilder(null);
            Assert.NotNull(result);
        }

        [Test]
        public void BuildTest()
        {
            Environment.SetEnvironmentVariable("MAGNUM_CERTIFICATE_ON", "N");
            IWebHostBuilder webBuilder = Program.CreateWebHostBuilder(null);
            IWebHost webHost = webBuilder.Build();
            Assert.NotNull(webHost);
        }
    }
}