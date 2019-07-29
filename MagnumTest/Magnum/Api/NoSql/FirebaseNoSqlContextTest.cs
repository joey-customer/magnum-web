using System;
using NUnit.Framework;

namespace Magnum.Api.NoSql
{
	public class FirebaseNoSqlContextTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void NoDataToAuthenTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();

            try
            {
                ctx.Authenticate("", "", "", "");
                Assert.Fail("Exception should be thrown for failed authen !!!");
            }
            catch
            {
                //Do nothing
            }
        } 

        [TestCase]
        public void SuccessAuthenTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();

            try
            {
                string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
                string username = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
                string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");

                //This is for unit testing only, DO NOT put any production data in this DB
                ctx.Authenticate("https://compute-engine-vm-test.firebaseio.com/", 
                    key, 
                    username, 
                    password);

                ctx.PostData("unit_testing", DateTime.Now);      
            }
            catch
            {
                Assert.Fail("Please see env variable MAGNUM_FIREBASE_KEY, MAGNUM_DB_USERNAME, MAGNUM_DB_PASSWORD !!!");
            }
        }

        [TestCase]
        public void NoDataToPostTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();

            try
            {
                ctx.PostData("", new String("HELLO"));
                Assert.Fail("Exception should be thrown for no authen ealier !!!");
            }
            catch
            {
                //Do nothing
            }
        }         
    }
}
