using System;
using System.Collections;
using System.Collections.Generic;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Products;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;

using NDesk.Options;

namespace Magnum.Consoles.Products
{
	public class ImportProductApplication : ConsoleAppBase
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

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");
            IStorageContext storageCtx = GetStorageContextWithAuthen("firebase");

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetStorageContext(storageCtx);
            SaveProduct opr = (SaveProduct) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");

GetProductList listOpr = (GetProductList) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductList");

            MProduct param = new MProduct();
            param.Code = "PJAMENAJA0004";
            param.Image1LocalPath = @"D:\temp\Oxandro10_KH0009_20190807180124\000000\8801137657-6868401728.png";

            MProductComposition comp = new MProductComposition() 
            {
                Code = "CompCode002",
            };

            MGenericDescription desc = new MGenericDescription() 
            {
                Language = "TH",
                Name = "Name_TH",
            };            

            param.Compositions.Add(comp);
            param.Descriptions.Add(desc.Language, desc);

            try
            {
IEnumerable<MProduct> products = listOpr.Apply(param, null);
foreach (MProduct prod in products)
{
    Console.WriteLine("DEBUG : [{0}] [{1}]", prod.Code, prod.Descriptions.Count);
}
                MProduct prd = opr.Apply(param);
                if (prd == null)
                {
                    Console.WriteLine("Done add new product [{0}]", param.Code);
                }
                else
                {
                    Console.WriteLine("Done update existing product [{0}]", param.Code);
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
