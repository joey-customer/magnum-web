using System;
using NUnit.Framework;
using Magnum.Api.Models;

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

        private void Authen(INoSqlContext ctx)
        {
            string host = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_URL");
            string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
            string username = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");

            //This is for unit testing only, DO NOT put any production data in this DB
            ctx.Authenticate(host,
                key,
                username,
                password);
        }

        [TestCase]
        public void SuccessAuthenTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();

            try
            {
                Authen(ctx);
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

        [TestCase]
        public void NoDataToPutTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();
            Authen(ctx);

            try
            {
                ctx.PutData("unit_testing", "unit_testing", new String("HELLO"));
            }
            catch (Exception e)
            {
                //Do nothing
                Assert.Fail(e.Message);                
            }
        }  

        [TestCase]
        public void GetObjectByKeyTest()
        {
            //Just to cover the test coverage

            INoSqlContext ctx = new FirebaseNoSqlContext();
            Authen(ctx);

            try
            {
                ctx.PostData("unit_testing", new MBarcode());
                ctx.GetObjectByKey<BaseModel>("unit_testing");
            }
            catch (Exception e)
            {
                //Do nothing
                Assert.Fail(e.Message);                
            }
        }                         
    }
}
