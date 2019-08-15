using System;
using System.IO;
using Magnum.Api.Storages;

namespace Magnum.Api.Factories
{    
	public class MockedStorageContext : IStorageContext
	{
        public void Authenticate(string url, string key, string user, string passwd)
        {
            //Do nothing
        }

        public string UploadFile(string bucketPath, string filePath)
        {
            return "";
        }

        public string UploadFile(string bucketPath, Stream fileStream)
        {
            return "";
        }

        public void DownloadFile(string bucketPath, string fileLocalPath)
        {
            //Do nothing
        }
    }
}
