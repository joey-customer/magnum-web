using System;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Products;
using Magnum.Api.NoSql;

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

            FactoryBusinessOperation.SetContext(ctx);
            SaveProduct opr = (SaveProduct) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");

            MProduct param = new MProduct();
            param.Code = "PJAMENAJA0002";
            param.Name = "Pjame test product naja";
            param.Language = "EN";

            MProductComposition comp = new MProductComposition() 
            {
                Code = "CompCode001",
                Name = "Name001"
            };

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

                DeleteProduct delOpr = (DeleteProduct) FactoryBusinessOperation.CreateBusinessOperationObject("DeleteProduct");
                delOpr.Apply(prd);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e.Message);
            }
            
            return 0;
        }
    }
}
