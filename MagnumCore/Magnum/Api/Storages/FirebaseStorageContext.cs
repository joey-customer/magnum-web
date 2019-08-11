using System;
using System.IO;
using System.Threading.Tasks;

using Firebase.Storage;
using Firebase.Database.Query;
using Firebase.Auth;

namespace Magnum.Api.Storages
{    
	public class FirebaseStorageContext : IStorageContext
	{
        private FirebaseAuthLink token = null;
        private string authKey = "";

        private string bucketUrl = "";
        private string bucketUser = "";
        private string bucketPassword = "";

        private async Task AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            string firebaseUsername = bucketUser;
            string firebasePassword = bucketPassword;
            token = await authProvider.SignInWithEmailAndPasswordAsync(firebaseUsername, firebasePassword);    
        }

        private async Task<string> UploadStorageData(string storagePath, Stream fileStream)
        {
            var opt = new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken),
                ThrowOnCancel = true
            };

            var storage = new FirebaseStorage(bucketUrl, opt);

            var downloadUrl = await storage
                .Child(storagePath)
                .PutAsync(fileStream);  

            return downloadUrl;          
        }

        public void Authenticate(string url, string key, string user, string passwd)
        {
            authKey = key;
            bucketUrl = url;
            bucketUser = user;
            bucketPassword = passwd;

            AuthenToFirebase().Wait();
        }

        public string UploadFile(string bucketPath, string filePath)
        {
            var stream = File.Open(filePath, FileMode.Open);
            var url = UploadFile(bucketPath, stream);

            return url;
        }

        public string UploadFile(string bucketPath, Stream fileStream)
        {
            var t = UploadStorageData(bucketPath, fileStream);
            var url = t.Result;
            return url;
        }

        public void DownloadFile(string bucketPath, string fileLocalPath)
        {

        }
    }
}
