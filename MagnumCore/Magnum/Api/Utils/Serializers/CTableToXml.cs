using System;
using System.Xml;
using System.Collections;

using Magnum.Api.Commons.Table;

namespace Magnum.Api.Utils.Serializers
{
    public class CTableToXml : ICTableSerializer
    {
        private readonly CRoot root = null;

        public CTableToXml(CRoot rt)
        {
            root = rt;
        }

        public string Serialize()
        {
            String xml = createXMLString(root);
            return(xml);
        }

        private XmlElement createElementFromTable(XmlDocument doc, CTable tb)
        {
            ArrayList flds = tb.GetTableFields();
            XmlElement xmlObj = doc.CreateElement("OBJECT");
            xmlObj.SetAttribute("name", tb.GetTableName());

            foreach (CField f in flds)
            {
                XmlElement fld = doc.CreateElement("FIELD");
                fld.SetAttribute("name", f.GetName());
                String value = f.GetValue();
                fld.InnerText = value;

                xmlObj.AppendChild(fld);
            }

            Hashtable hashOfArray = tb.GetChildHash();
            foreach (String arrName in hashOfArray.Keys)
            {
                ArrayList arr = (ArrayList)hashOfArray[arrName];

                XmlElement itm = doc.CreateElement("ITEMS");
                itm.SetAttribute("name", arrName);
                xmlObj.AppendChild(itm);

                foreach (CTable t in arr)
                {
                    //Recursive
                    XmlElement o = createElementFromTable(doc, t);
                    itm.AppendChild(o);
                }
            }

            return (xmlObj);
        }

        private String createXMLString(CRoot root)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            XmlElement r = doc.DocumentElement;
            doc.InsertBefore(xmldecl, r);

            XmlElement api = doc.CreateElement("API");

            XmlElement o1 = createElementFromTable(doc, root.Param);
            XmlElement o2 = createElementFromTable(doc, root.Data);

            api.AppendChild(o1);
            api.AppendChild(o2);

            doc.AppendChild(api);
            return (doc.OuterXml);
        }        
    }
}
