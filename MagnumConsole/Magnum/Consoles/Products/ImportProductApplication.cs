using System;
using System.Collections;

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
            string infile = args["infile"].ToString();
            string basedir = args["basedir"].ToString();

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");
            IStorageContext storageCtx = GetStorageContextWithAuthen("firebase");

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetStorageContext(storageCtx);
            SaveProduct opr = (SaveProduct) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");

            MProduct param = new MProduct();
            param.Code = "PJAMENAJA0002";
            param.Name = "Pjame test product naja";
            param.Language = "EN";
            param.Image1LocalPath = @"D:\temp\Oxandro10_KH0009_20190807180124\000000\8801137657-6868401728.png";

            MProductComposition comp = new MProductComposition() 
            {
                Code = "CompCode001",
                Name = "Name001"
            };
//var url = storageCtx.UploadFile("products/xxxx/test.jpg", @"D:\temp\Oxandro10_KH0009_20190807180124\000000\1496338935-9755010661.png");
//Console.WriteLine(url);

            param.Compositions.Add(comp);

            try
            {
                MProduct prd = opr.Apply(param);
                if (prd == null)
                {
                    Console.WriteLine("Done add new product [{0}] [{1}]", param.Code, param.Name);
                }
                else
                {
                    Console.WriteLine("Done update existing product [{0}] [{1}]", param.Code, param.Name);
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
