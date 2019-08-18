using System;

namespace Magnum.Api.NoSql
{
	public interface ITokenRefreshAble
	{
        void SetRefreshInterval(long refreshRate);
        DateTime GetLastRefreshDtm();
        void SetLastRefreshDtm(DateTime refreshDtm);
    }    
}
