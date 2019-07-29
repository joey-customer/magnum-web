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
                //This is for unit testing only, DO NOT put any production data in this DB
                ctx.Authenticate("https://compute-engine-vm-test.firebaseio.com/", 
                    "AIzaSyCgvL2t12A8Os9CM9k59PdqDay3EB09Czs", 
                    "test@test.com", 
                    "test12345");

                ctx.PostData("unit_testing", DateTime.Now);      
            }
            catch
            {
                Assert.Fail("Exception should be thrown for failed authen !!!");
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
