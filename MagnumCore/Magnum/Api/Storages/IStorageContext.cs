using System;
using System.IO;

namespace Magnum.Api.Storages
{
	public interface IStorageContext
	{
        void Authenticate(string url, string key, string user, string passwd);
        void UploadFile(string bucketPath, string filePath);
        void UploadFile(string bucketPath, Stream fileStream);
        void DownloadFile(string bucketPath, string fileLocalPath);
    }    
}
