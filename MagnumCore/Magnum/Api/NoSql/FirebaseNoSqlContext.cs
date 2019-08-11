using System;
using System.Threading.Tasks;

using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Magnum.Api.Models;

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

        private async Task PutFirebaseData(string path, string key, object data)
        {
            await fbClient
                .Child(path)
                .Child(key)
                .PutAsync(data);
        }

        private async Task DeleteFirebaseData(string path, string key)
        {
            await fbClient
                .Child(path)
                .Child(key)
                .DeleteAsync();
        }

        private async Task<T> GetFirebaseData<T>(string path)
        {
            var items = await fbClient
                .Child(path)
                .OrderByKey()
                .LimitToFirst(1)
                .OnceAsync<T>();

            T o = default(T);
            foreach (var item in items)
            {
                o = item.Object;
                (o as BaseModel).Key = item.Key;
            }

            return o;
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

        public object PutData(string path, string key, object data)
        {
            PutFirebaseData(path, key, data).Wait();
            return data;
        } 

        public int DeleteData(string path, BaseModel data)
        {
            DeleteFirebaseData(path, data.Key).Wait();
            return 1;
        } 


        public T GetObjectByKey<T>(string path) where T : BaseModel
        {
            T o = GetFirebaseData<T>(path).Result;
            return o;
        }
    }
}
