using System;
using System.IO;
using System.Threading.Tasks;

using Firebase.Storage;
using Firebase.Auth;

namespace Magnum.Api.Storages
{    
	public class FirebaseStorageContext : IStorageContext
	{
        private FirebaseStorage fbStorage = null;

        private string authKey = "";
        private string bucketUrl = "";
        private string bucketUser = "";
        private string bucketPassword = "";        

        private void AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            var opt = new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => LoginAsync(authProvider),
                ThrowOnCancel = true
            };

            fbStorage = new FirebaseStorage(bucketUrl, opt);              
        }

        private async Task<string> LoginAsync(FirebaseAuthProvider authProvider)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(bucketUser, bucketPassword);
            return token.FirebaseToken;
        }

        private async Task<string> UploadStorageData(string storagePath, Stream fileStream)
        {
            var downloadUrl = await fbStorage
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

            AuthenToFirebase();
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
            //Do nothing
        }
    }
}
