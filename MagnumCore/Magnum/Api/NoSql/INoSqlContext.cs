using System;
using System.Threading.Tasks;

namespace Magnum.Api.NoSql
{
	public interface INoSqlContext
	{
        void Authenticate(string url, string key, string user, string passwd);
        object PostData(string path, object data);
        T GetObjectByKey<T>(string path, string key);
    }    
}
