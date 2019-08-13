using System.Collections;

using NUnit.Framework;
using Magnum.Api.Commons.Table;

namespace Magnum.Api.Utils.Serializers
{
    public class CTableToXmlTest
    {
        [SetUp]
        public void Setup()
        {
        }

        private CRoot deserialize(string xml)
        {
            ICTableDeserializer deserializer = new XmlToCTable(xml);
            CRoot root = deserializer.Deserialize();

            return root;
        }

        [TestCase("GetEmployeeList", "EMPLOYEE_NAME", "Seubpong", "Seubpong")]
        [TestCase("GetEmployeeList", "EMPLOYEE_NAME", "Seub&amp;pong", "Seub&pong")]
        public void ObjectToXmlOneLevelTest(string apiName, string fieldName, string fieldValue, string expectedValue)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
<API>
    <OBJECT name='PARAM'>
        <FIELD name='FUNCTION_NAME'>{0}</FIELD>
    </OBJECT>
    <OBJECT name=''>
        <FIELD name='{1}'>{2}</FIELD>
    </OBJECT>
</API>";

            //Use the deserializer to create the CTable object
            CRoot root = deserialize(string.Format(xml, apiName, fieldName, fieldValue));
            CTableToXml serializer = new CTableToXml(root);
            string outXml = serializer.Serialize();

            CRoot xmlObj = deserialize(outXml);
            CTable param = xmlObj.Param;
            CTable dat = xmlObj.Data;

            string funcName = param.GetFieldValue("FUNCTION_NAME");
            Assert.AreEqual(apiName, funcName, "Serialize logic is wrong for param object!!!");

            string value = dat.GetFieldValue(fieldName);
            Assert.AreEqual(expectedValue, value, "Serialize logic is wrong for data object!!!");
        }     

        [TestCase("GetEmployeeList", "EMPLOYEE_NAME", "Seubpong", "Seubpong_1", 0)]
        [TestCase("GetEmployeeList", "EMPLOYEE_NAME", "Seubpong", "Seubpong_2", 1)]
        public void XmlToObjectWithArrayTest(string apiName, string fieldName, string fieldValue, string expectedValue, int index)
        {
            string xml = @"<?xml version='1.0' encoding='UTF-8'?>
<API>
    <OBJECT name='PARAM'>
        <FIELD name='FUNCTION_NAME'>{0}</FIELD>
    </OBJECT>
    <OBJECT name=''>
        <FIELD name='DUMMY_FIELD'>DUMMY_VALUE</FIELD>
        <ITEMS name='ADDRESS_ITEM'>
            <OBJECT name=''>
                <FIELD name='{1}'>{2}_1</FIELD>
                <FIELD name='SORT_ORDER'>1</FIELD>
            </OBJECT>
            <OBJECT name=''>
                <FIELD name='{1}'>{2}_2</FIELD>
                <FIELD name='SORT_ORDER'>2</FIELD>
            </OBJECT>            
        </ITEMS>        
    </OBJECT>
</API>";

            CRoot root = deserialize(string.Format(xml, apiName, fieldName, fieldValue));
            CTableToXml serializer = new CTableToXml(root);
            string outXml = serializer.Serialize();

            CRoot xmlObj = deserialize(outXml);
            CTable param = xmlObj.Param;
            CTable dat = xmlObj.Data;

            string funcName = param.GetFieldValue("FUNCTION_NAME");
            Assert.AreEqual(apiName, funcName, "Serialize logic is wrong for param object!!!");

            ArrayList arr = dat.GetChildArray("ADDRESS_ITEM");
            CTable item = (CTable) arr[index];

            string value = item.GetFieldValue(fieldName);
            Assert.AreEqual(expectedValue, value, "Serialize logic is wrong for data object!!!");
        }             
    }    
}