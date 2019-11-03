using NUnit.Framework;

namespace Magnum.Consoles.Barcodes.HtmlConverters
{
    public class Html2ImageConverterTest : BaseTest
    {
        public Html2ImageConverterTest() : base()
        {
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void CallMethodAndNoExceptionTest()
        {
            //To cover test coverage

            Html2ImageConverter cvt = new Html2ImageConverter();
            cvt.SetWidth(10);

            cvt.SetImageFormat(1);
            cvt.SetImageFormat(0);

            cvt.SetImageQuality(200);
            byte[] result = cvt.FromHtmlString("<html></html>");
            Assert.AreEqual(631, result.Length);
        }
    }
}