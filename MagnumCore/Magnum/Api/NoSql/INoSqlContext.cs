using System;
using System.Threading.Tasks;
using Magnum.Api.Models;

namespace Magnum.Api.NoSql
{
	public interface INoSqlContext
	{
        void Authenticate(string url, string key, string user, string passwd);
        object PostData(string path, object data);
        object PutData(string path, string key, object data);
        T GetObjectByKey<T>(string path) where T : BaseModel;
    }    
}
