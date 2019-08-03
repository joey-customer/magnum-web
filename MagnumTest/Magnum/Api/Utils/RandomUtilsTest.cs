using System;
using System.Collections;
using NUnit.Framework;

namespace Magnum.Api.Utils
{
    public class RandomUtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(5, 10)]
        [TestCase(50, 30)]
        public void RandomStringTest(int tryCount, int size)
        {
            Hashtable uniqueList = new Hashtable();

            for (int i=1; i<=tryCount; i++)
            {
                string key = RandomUtils.RandomString(size);

                //Will modify the same entry if "key" is duplicate
                uniqueList[key] = key;
            }

            Assert.AreEqual(tryCount, uniqueList.Count, "Random strings are duplicate !!!");         
        }

        [TestCase(5, 10)]
        [TestCase(50, 30)]
        public void RandomStringNumTest(int tryCount, int size)
        {
            Hashtable uniqueList = new Hashtable();

            for (int i=1; i<=tryCount; i++)
            {
                string key = RandomUtils.RandomStringNum(size);

                //Will modify the same entry if "key" is duplicate
                uniqueList[key] = key;
            }

            Assert.AreEqual(tryCount, uniqueList.Count, "Random strings are duplicate !!!");         
        }        
    }
}