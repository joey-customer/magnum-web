using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Magnum.Api.Models;

namespace Magnum.Api.Storages
{
	public class FirebaseStorageContextTest
	{
        [SetUp]
        public void Setup()
        {
        }

        [TestCase]
        public void NoDataToAuthenTest()
        {
            //Just to cover the test coverage

            IStorageContext ctx = new FirebaseStorageContext();

            try
            {
                ctx.Authenticate("", "", "", "");                
            }
            catch
            {
                Assert.Fail("Exception should not be thrown for failed authen !!!");
            }
        } 

        private void Authen(IStorageContext ctx)
        {
            string bucket = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_BUCKET");
            string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
            string username = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");

            //This is for unit testing only, DO NOT put any production data in this DB
            ctx.Authenticate(bucket,
                key,
                username,
                password);
        }

        [TestCase]
        public void SuccessAuthenTest()
        {
            //Just to cover the test coverage

            IStorageContext ctx = new FirebaseStorageContext();

            try
            {
                Authen(ctx);
            }
            catch
            {
                Assert.Fail("Please see env variable MAGNUM_FIREBASE_KEY, MAGNUM_DB_USERNAME, MAGNUM_DB_PASSWORD !!!");
            }
        }

        [TestCase]
        public void UploadFromFileStreamTest()
        {
            //Just to cover the test coverage

            IStorageContext ctx = new FirebaseStorageContext();

            try
            {
                Authen(ctx);
                var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello world!"));
                ctx.UploadFile("unit_test/test1.txt", stream);

                //For test coverage
                ctx.DownloadFile("unit_test/test.txt", "");
            }
            catch
            {
                Assert.Fail("File upload failed !!!");
            }
        }    


        [TestCase]
        public void UploadFromFilePathTest()
        {
            //Just to cover the test coverage

            IStorageContext ctx = new FirebaseStorageContext();

            try
            {
                Authen(ctx);

                string[] lines = { "First line", "Second line", "Third line" };
                string[] paths = {Path.GetTempPath(), "test2.txt"};
                string tmpPath = Path.Combine(paths);

                File.WriteAllLines(tmpPath, lines);
                ctx.UploadFile("unit_test/test2.txt", tmpPath);

                //For test coverage
                ctx.DownloadFile("unit_test/test.txt", "");
            }
            catch
            {
                Assert.Fail("File upload failed !!!");
            }
        }                         
    }
}
