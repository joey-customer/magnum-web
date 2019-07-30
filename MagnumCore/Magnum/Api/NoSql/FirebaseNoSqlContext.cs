using System;
using System.Threading.Tasks;

using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;

namespace Magnum.Api.NoSql
{    
	public class FirebaseNoSqlContext : INoSqlContext
	{
        private FirebaseClient fbClient = null;
        private string authKey = "";

        private string dbUrl = "";
        private string dbUser = "";
        private string dbPassword = "";

        private async Task AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            string firebaseUsername = dbUser;
            string firebasePassword = dbPassword;
            var token = await authProvider.SignInWithEmailAndPasswordAsync(firebaseUsername, firebasePassword);

            fbClient = new FirebaseClient(
                dbUrl,
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

        private async Task<T> GetFirebaseData<T>(string path, string key)
        {
            var item = await fbClient
                .Child(path)
                .OnceSingleAsync<T>();

            return item;
        }

        public void Authenticate(string url, string key, string user, string passwd)
        {
            authKey = key;
            dbUrl = url;
            dbUser = user;
            dbPassword = passwd;

            AuthenToFirebase().Wait();
        }

        public object PostData(string path, object data)
        {
            PostFirebaseData(path, data).Wait();
            return data;
        } 

        public T GetObjectByKey<T>(string path, string key)
        {
            T o = GetFirebaseData<T>(path, key).Result;
            return o;
        }
    }
}
