using System;
using System.IO;

namespace Magnum.Api.Storages
{
	public interface IStorageContext
	{
        void Authenticate(string url, string key, string user, string passwd);
        string UploadFile(string bucketPath, string filePath);
        string UploadFile(string bucketPath, Stream fileStream);
        void DownloadFile(string bucketPath, string fileLocalPath);
    }    
}
