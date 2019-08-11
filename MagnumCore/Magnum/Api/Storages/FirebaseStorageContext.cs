using System;
using System.IO;
using System.Threading.Tasks;

using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;

namespace Magnum.Api.Storages
{    
	public class FirebaseStorageContext : IStorageContext
	{
        private FirebaseClient fbClient = null;
        private string authKey = "";

        private string bucketUrl = "";
        private string bucketUser = "";
        private string bucketPassword = "";

        private async Task AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            string firebaseUsername = bucketUser;
            string firebasePassword = bucketPassword;
            var token = await authProvider.SignInWithEmailAndPasswordAsync(firebaseUsername, firebasePassword);

            fbClient = new FirebaseClient(
                bucketUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)
                });            
        }

        private async Task PostFirebaseData(string path, object data)
        {
            await fbClient
                .Child(path)
                .PostAsync(data, true);
        }

        public void Authenticate(string url, string key, string user, string passwd)
        {
            authKey = key;
            bucketUrl = url;
            bucketUser = user;
            bucketPassword = passwd;

            AuthenToFirebase().Wait();
        }

        public void UploadFile(string bucketPath, string filePath)
        {
        }

        public void UploadFile(string bucketPath, Stream fileStream)
        {

        }

        public void DownloadFile(string bucketPath, string fileLocalPath)
        {

        }
    }
}
