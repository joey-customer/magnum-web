using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Magnum.Api.Models;

namespace Magnum.Api.NoSql
{    
	public class FirebaseNoSqlContext : INoSqlContext, ITokenRefreshAble
	{
        private FirebaseClient fbClient = null;
        private string authKey = "";

        private string dbUrl = "";
        private string dbUser = "";
        private string dbPassword = "";
        private DateTime lastRefreshDtm = DateTime.Now;        
        private long refreshInterval = TimeSpan.TicksPerHour / 2;

        private async Task AuthenToFirebase()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
            var token = await authProvider.SignInWithEmailAndPasswordAsync(dbUser, dbPassword);

            fbClient = new FirebaseClient(
                dbUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)               
                });   

            lastRefreshDtm = DateTime.Now; 
            //Should log something here
        }

        private FirebaseClient GetFirebaseRefresh()
        {
            long lastTick = lastRefreshDtm.Ticks;
            long currentTick = DateTime.Now.Ticks;

            if (currentTick - lastTick > refreshInterval)
            {
                AuthenToFirebase().Wait();
            }

            return fbClient;
        }

        private async Task<string> PostFirebaseData(string path, object data)
        {
            var obj = await GetFirebaseRefresh()
                .Child(path)
                .PostAsync(data, true);
            
            return obj.Key;
        }

        private async Task PutFirebaseData(string path, string key, object data)
        {
            await GetFirebaseRefresh()
                .Child(path)
                .Child(key)
                .PutAsync(data);
        }

        private async Task DeleteFirebaseData(string path, string key)
        {
            await GetFirebaseRefresh()
                .Child(path)
                .Child(key)
                .DeleteAsync();
        }

        private async Task<T> GetFirebaseData<T>(string path)
        {
            var items = await GetFirebaseRefresh()
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
            var o = await GetFirebaseRefresh()
                .Child(path)
                .Child(key)
                .OnceSingleAsync<T>();

            return o;
        }        

        private async Task<IEnumerable<T>> ListFirebaseData<T>(string path)
        {
            List<T> arr = new List<T>();

            var items = await GetFirebaseRefresh()
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

        public DateTime GetLastRefreshDtm()
        {
            return lastRefreshDtm;
        } 

        public void SetLastRefreshDtm(DateTime refreshDtm)
        {
            lastRefreshDtm = refreshDtm;
        }

        public void SetRefreshInterval(long refreshRate)
        {
            refreshInterval = refreshRate;
        } 

        public void Authenticate(string url, string key, string user, string passwd)
        {
            authKey = key;
            dbUrl = url;
            dbUser = user;
            dbPassword = passwd;

            AuthenToFirebase().Wait();
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
