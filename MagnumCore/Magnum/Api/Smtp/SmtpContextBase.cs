using System;
using System.Net.Mail;
using System.Net;

namespace Magnum.Api.Smtp
{
	public abstract class SmtpContextBase : ISmtpContext
	{
        private string smtpHost = "";
        private int smtpPort = 0;
        private string smtpUser = "";
        private string smtpPassword = "";

        protected SmtpContextBase()
        {
        }

        protected void SetSmtpConfig(string host, int port, string user, string password)
        {
            smtpHost = host;
            smtpPort = port;
            smtpUser = user;
            smtpPassword = password;
        }

        public void Send(Mail mail)
        {                   
            SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUser, smtpPassword);
            
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(mail.From);
            mailMessage.To.Add(mail.To);
            mailMessage.Body = mail.Body;
            mailMessage.Subject = mail.Subject;
            // Will need to add - client dot Send(mailMessage); here
        }
    }    
}
