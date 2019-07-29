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
        private FirebaseAuthProvider authProvider = null;
        private string authKey = "";

        private string dbUrl = "";
        private string dbUser = "";
        private string dbPassword = "";

        private async Task AuthenToFirebase()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
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
            var t = await fbClient
            .Child(path)
            .PostAsync(data, true);
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
    }
}
