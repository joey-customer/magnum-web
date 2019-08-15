using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        private void AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));

            fbClient = new FirebaseClient(
                dbUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => LoginAsync(authProvider)                    
                });            
        }

        private async Task<string> LoginAsync(FirebaseAuthProvider authProvider)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(dbUser, dbPassword);
            return token.FirebaseToken;
        }

        private async Task<string> PostFirebaseData(string path, object data)
        {
            var obj = await fbClient
                .Child(path)
                .PostAsync(data, true);
            
            return obj.Key;
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

        private async Task<T> GetFirebaseSingleData<T>(string path, string key)
        {
            var o = await fbClient
                .Child(path)
                .Child(key)
                .OnceSingleAsync<T>();

            return o;
        }        

        private async Task<IEnumerable<T>> ListFirebaseData<T>(string path)
        {
            List<T> arr = new List<T>();

            var items = await fbClient
                .Child(path)
                .OrderByKey()
                .OnceAsync<T>();

            foreach (var item in items)
            {         
                T obj = item.Object;
                (obj as BaseModel).Key = item.Key;

                arr.Add(obj);
            }

            return arr;
        }

        public void Authenticate(string url, string key, string user, string passwd)
        {
            authKey = key;
            dbUrl = url;
            dbUser = user;
            dbPassword = passwd;

            AuthenToFirebase();
        }

        public string PostData(string path, object data)
        {
            var key = PostFirebaseData(path, data).Result;
            return key;
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

        public T GetSingleObject<T>(string path, string key) where T : BaseModel
        {
            T o = GetFirebaseSingleData<T>(path, key).Result;
            return o;
        }

        public IEnumerable<T> GetObjectList<T>(string path) where T : BaseModel
        {
            IEnumerable<T> arr = ListFirebaseData<T>(path).Result;
            return arr;
        }

    }
}
