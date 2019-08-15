using System;
using System.Xml;
using System.Collections;

using Magnum.Api.Commons.Table;

namespace Magnum.Api.Utils.Serializers
{
    public class XmlToCTable : ICTableDeserializer
    {
        private readonly string xml;

        public XmlToCTable(string content)
        {
            xml = content;
        }

        public CRoot Deserialize()
        {
            CRoot root = XMLToRootObject();
            return(root);
        }

        private CRoot XMLToRootObject()
        {
            CRoot root = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                root = getRootObject(doc.ChildNodes[1]);
            }
            catch (Exception e)
            {
                CTable prm = new CTable("");
                CTable data = new CTable("");

                prm.SetFieldValue("ERROR_CODE", "1");
                prm.SetFieldValue("ERROR_DESC", e.Message);

                root = new CRoot(prm, data);
            }

            return (root);
        }

        //Send OBJECT node to this function
        private CTable populateTableObject(XmlNode node)
        {
            CTable table = new CTable(node.Attributes["name"].Value);

            foreach (XmlNode n1 in node.ChildNodes)
            {
                if (n1.Name.Equals("FIELD"))
                {
                    table.SetFieldValue(n1.Attributes["name"].Value, n1.InnerText);
                }
                else if (n1.Name.Equals("ITEMS"))
                {
                    String arrName = n1.Attributes["name"].Value;

                    ArrayList arr = new ArrayList();
                    table.AddChildArray(arrName, arr);

                    foreach (XmlNode n2 in n1.ChildNodes)
                    {
                        //Only OBJECTs are here
                        if (n2.Name.Equals("OBJECT"))
                        {
                            //Recursive
                            CTable child = populateTableObject(n2);
                            arr.Add(child);
                        }
                    }
                }
            }

            return (table);
        }

        private CRoot getRootObject(XmlNode node)
        {
            CTable param = null;
            CTable data = null;
            CRoot root = new CRoot(null, null);

            int idx = 0;
            foreach (XmlNode n1 in node.ChildNodes)
            {
                if (idx == 0)
                {
                    param = populateTableObject(n1);
                }
                else if (idx == 1)
                {
                    data = populateTableObject(n1);
                }

                idx++;
            }

            root.Data = data;
            root.Param = param;

            return (root);
        }
    }
}
