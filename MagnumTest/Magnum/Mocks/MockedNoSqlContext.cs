using System;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Commons.Model;

namespace Its.Onix.Erp.Businesses.Mocks
{    
	public class MockedNoSqlContext : INoSqlContext
	{
        private BaseModel m = null;

        public void Authenticate(string url, string key, string user, string passwd)
        {
        }

        public void SetReturnObjectByKey<T>(T obj) where T : BaseModel
        {
            m = obj;
        }

        public string PostData(string path, object data)
        {
            return "";
        }

        public object PutData(string path, string key, object data)
        {
            return null;
        }

        public int DeleteData(string path, BaseModel data)
        {
            return 1;
        }

        public T GetObjectByKey<T>(string path) where T : BaseModel
        {
            return (T) m;
        }

        public T GetSingleObject<T>(string path, string key) where T : BaseModel
        {
            return (T) m;
        }

        public IEnumerable<T> GetObjectList<T>(string path) where T : BaseModel
        {
            return null;
        }    

        public ILogger GetLogger()
        {
            return null;
        }

        public void SetLogger(ILogger logger)
        {

        }         
    }
}
