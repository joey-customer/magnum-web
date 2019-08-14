using System;

namespace Magnum.Api.Smtp
{
	public abstract class SmtpContextBase : ISmtpContext
	{
        private readonly string smtpHost = "";
        private readonly int smtpPort = 0;
        private readonly string smtpUser = "";
        private readonly string smtpPassword = "";

        public SmtpContextBase(string host, int port, string user, string password)
        {
            smtpHost = host;
            smtpPort = port;
            smtpUser = user;
            smtpPassword = password;
        }

        public void Send(Mail mail)
        {
            //Do sending stuff here
        }
    }    
}
