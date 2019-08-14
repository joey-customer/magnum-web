using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Magnum.Api.Smtp
{
	public class SendGridSmtpContextTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("pjame.fb@gmail.com", "noreply@xxxx.com")]
        public void SendEmailTest(string to, string from)
        {
            SendGridSmtpContext ctx = new SendGridSmtpContext();

            Mail m = new Mail();
            m.From = from;
            m.To = to;            
            m.Subject = "This is sent from unit test";
            m.IsHtmlContent = false;
            m.Body = "This is body text sent from unit test";
            m.BCC = "";
            m.CC = "";

            ctx.Send(m);
        }                      
    }
}
