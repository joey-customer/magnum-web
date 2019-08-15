using System.Collections;
using NUnit.Framework;

namespace Magnum.Api.Commons.Table
{
    public class CTableTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetFieldNotFoundTest()
        {
            string fname = "FIELD1";
            int value = 9999;

            CTable tb = new CTable();

            tb.SetFieldValue(fname, value);
            string s = tb.GetFieldValue("NOTFOUND_FIELD");

            Assert.AreEqual("", s, "Expected empty of field is unknown!!!");
        }

        [Test]
        public void SetAndTheGetFieldIntEqual()
        {
            string fname = "FIELD1";
            int value = 9999;

            CTable tb = new CTable();

            tb.SetFieldValue(fname, value);
            int i = tb.GetFieldValueInt(fname);

            Assert.AreEqual(value, i, "Set and then get value from same field, they need to be equal!!!");
        }

        [Test]
        public void SetAndTheGetFieldDoubleEqual()
        {
            string fname = "FIELD1";
            double value = 9999.999;

            CTable tb = new CTable();

            tb.SetFieldValue(fname, value);
            string i = tb.GetFieldValue(fname);

            Assert.AreEqual(value.ToString(), i, "Set and then get value from same field, they need to be equal!!!");
        }

        [Test]
        public void SetAndTheGetFieldIntString()
        {
            string fname = "FIELD1";
            string value = "dummy";

            CTable tb = new CTable();

            tb.SetFieldValue(fname, value);
            string fldValue = tb.GetFieldValue(fname);

            Assert.AreEqual(value, fldValue, "Set and then get value from same field, they need to be equal!!!");
        } 

        [Test]
        public void TableNameShouldDefaultByConstructor()
        {
            string tableName = "DUMMY_TABLENAME";

            CTable tb = new CTable(tableName);
            string name = tb.GetTableName();

            Assert.AreEqual(tableName, name, "Table name should be equal from constructor!!!");
        }   

        [Test]
        public void AllowDuplicateFieldNameUseLastValue()
        {
            CTable tb = new CTable();
            string fname = "FIELD1";

            tb.SetFieldValue(fname, "DUMMY1");
            tb.SetFieldValue(fname, "DUMMY2");
            tb.SetFieldValue(fname, "DUMMY3");
            
            string value = tb.GetFieldValue(fname);

            Assert.AreEqual("DUMMY3", value, "Last field set value should be used!!!");
        }   


        [Test]
        public void RemoveArrayFromObjectTest()
        {
            CTable tb = new CTable();
            string fname = "FIELD1";
            tb.SetFieldValue(fname, "DUMMY1");

            tb.AddChildArray("ITEMS", new ArrayList());
            tb.RemoveChildArray("ITEMS");

            ArrayList arr = tb.GetChildArray("ITEMS");        

            Assert.AreEqual(null, arr, "Array returned should be null!!!");
        }      

        [Test]
        public void CloneAllTest()
        {
            CTable tb = new CTable();
            string fname = "FIELD1";
            tb.SetFieldValue(fname, "DUMMY1");

            ArrayList arr = new ArrayList();

            CTable dat1 = new CTable();
            dat1.SetFieldValue("OBJ1_FIELD1", "1");
            arr.Add(dat1);

            CTable dat2 = new CTable();
            dat2.SetFieldValue("OBJ2_FIELD1", "2");
            arr.Add(dat2);

            tb.AddChildArray("ITEMS", arr);

            CTable clone = tb.CloneAll();

            ArrayList cloneArr = clone.GetChildArray("ITEMS");
            CTable cloneDat1 = (CTable) cloneArr[0];
            CTable cloneDat2 = (CTable) cloneArr[1];

            Assert.AreEqual(2, cloneArr.Count, "Number of item in array need to be match!!!");
            Assert.AreEqual("1", cloneDat1.GetFieldValue("OBJ1_FIELD1"), "Value from clone object need to match!!!");
            Assert.AreEqual("2", cloneDat2.GetFieldValue("OBJ2_FIELD1"), "Value from clone object need to match!!!");
        }                

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void AllowDuplicateFieldNameSet(int num)
        {
            CTable tb = new CTable();
            string fname = "FIELD";

            for (int i=1; i<=num; i++)
            {
                //Set same field
                tb.SetFieldValue(fname + i, "DUMMY");
                tb.SetFieldValue(fname + i, "DUMMY");
            }
            
            ArrayList fields = tb.GetTableFields();
            int cnt = fields.Count;

            Assert.AreEqual(num, cnt, "Field count should be only {0}!!!", num);
        }                           
    }
}