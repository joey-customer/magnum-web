using System;
using System.Collections;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.ProductTypes;

using Its.Onix.Core.Utils;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Commons.Table;
using Its.Onix.Core.Applications;

using Microsoft.Extensions.Logging;
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
            ILogger logger = GetLogger();
            CTable t = XmlToCTable();

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
                        mdc.LongDescription1 = desc.GetFieldValue("LongDescription");                        

                        mpt.Descriptions.Add(mdc.Language, mdc);
                        LogUtils.LogInformation(logger , "Adding product type : [{0}] [{1}]", mpt.Code, mdc.Name);
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
