using System;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.ProductTypes;
using Magnum.Api.NoSql;
using Magnum.Api.Commons.Table;
using Magnum.Api.Utils;

using Microsoft.Extensions.Logging;

using NDesk.Options;

namespace Magnum.Consoles.ProductTypes
{
	public class ImportProductTypeApplication : ConsoleAppBase
	{        
        private int i = 0;

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
