using System;
using System.Threading.Tasks;
using Magnum.Api.NoSql;

namespace Magnum.Api.Factories
{    
	public class MockedNoSqlContext : INoSqlContext
	{
        public void Authenticate(string url, string key, string user, string passwd)
        {
        }

        public object PostData(string path, object data)
        {
            return null;
        }

        public T GetObjectByKey<T>(string path, string key)
        {
            return default(T);
        }
    }
}
