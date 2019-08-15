using System;
using System.Collections.Generic;
using Magnum.Api.Models;

namespace Magnum.Api.NoSql
{
	public interface INoSqlContext
	{
        void Authenticate(string url, string key, string user, string passwd);
        string PostData(string path, object data);
        object PutData(string path, string key, object data);
        T GetObjectByKey<T>(string path) where T : BaseModel;
        T GetSingleObject<T>(string path, string key) where T : BaseModel;
        int DeleteData(string path, BaseModel data);
        IEnumerable<T> GetObjectList<T>(string path) where T : BaseModel;
    }    
}
