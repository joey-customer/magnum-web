using System;

namespace Magnum.Api.Smtp
{
	public class SendGridSmtpContext : SmtpContextBase
	{
        public SendGridSmtpContext() : base()
        {
            string password = Environment.GetEnvironmentVariable("MAGNUM_SMTP_PASSWORD");
            string host = Environment.GetEnvironmentVariable("MAGNUM_SMTP_HOST");
            string username = Environment.GetEnvironmentVariable("MAGNUM_SMTP_USER");
            string port = Environment.GetEnvironmentVariable("MAGNUM_SMTP_PORT");

            SetSmtpConfig(host, Int32.Parse(port), username, password);
        }
    }    
}
