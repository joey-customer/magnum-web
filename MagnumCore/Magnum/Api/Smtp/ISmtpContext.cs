using System;

namespace Magnum.Api.Smtp
{
	public interface ISmtpContext
	{
        void Send(Mail mail);
    }    
}
