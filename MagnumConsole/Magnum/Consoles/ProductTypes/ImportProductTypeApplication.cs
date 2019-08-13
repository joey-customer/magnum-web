using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.ProductTypes;
using Magnum.Api.NoSql;
using Magnum.Api.Commons.Table;
using Magnum.Api.Utils.Serializers;

using NDesk.Options;

namespace Magnum.Consoles.ProductTypes
{
	public class ImportProductTypeApplication : ConsoleAppBase
	{        
        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("if=|infile=", "XML Import file", s => AddArgument("infile", s))
            .Add("b=|basedir=", "Local image base directory", s => AddArgument("basedir", s));
            
            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string infile = args["infile"].ToString();
            string basedir = args["basedir"].ToString();

            string[] paths = {basedir, infile};
            string importFile = Path.Combine(paths);
            string xml = File.ReadAllText(importFile);

            XmlToCTable ds = new XmlToCTable(xml);
            CRoot root = ds.Deserialize();
            CTable t = root.Data;

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            SaveProductType opr = (SaveProductType) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProductType");

            try
            {
                ArrayList types = t.GetChildArray("ProductTypes");
                foreach (CTable pt in types)
                {
                    MProductType mpt = new MProductType();
                    mpt.Code = pt.GetFieldValue("Code");

                    ArrayList descs = pt.GetChildArray("Descriptions");
                    foreach (CTable desc in descs)
                    {
                        MGenericDescription mdc = new MGenericDescription();
                        mdc.Language = desc.GetFieldValue("Language");
                        mdc.Name = desc.GetFieldValue("Name");    
                        mdc.ShortDescription = desc.GetFieldValue("ShortDescription"); 
                        mdc.LongDescription = desc.GetFieldValue("LongDescription");                        

                        mpt.Descriptions.Add(mdc.Language, mdc);
                        Console.WriteLine("Adding product type : [{0}] [{1}]", mpt.Code, mdc.Name);
                    } 

                    opr.Apply(mpt);                  
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e.Message);
            }
            
            return 0;
        }
    }
}
