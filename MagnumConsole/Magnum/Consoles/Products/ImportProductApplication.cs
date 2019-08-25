using System;
using System.IO;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Products;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using Magnum.Api.Commons.Table;
using Magnum.Api.Utils.Serializers;

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

            string[] paths = {basedir, infile};
            string importFile = Path.Combine(paths);
            string xml = File.ReadAllText(importFile);

            XmlToCTable ds = new XmlToCTable(xml);
            CRoot root = ds.Deserialize();
            CTable t = root.Data;

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            IStorageContext storageCtx = GetStorageContextWithAuthen("firebase");
            FactoryBusinessOperation.SetStorageContext(storageCtx);

            SaveProduct opr = (SaveProduct) FactoryBusinessOperation.CreateBusinessOperationObject("SaveProduct");

            try
            {
                ArrayList products = t.GetChildArray("Products");
                foreach (CTable pd in products)
                {
                    MProduct mpd = new MProduct();
                    mpd.Code = pd.GetFieldValue("Code");
                    mpd.Rating = Int32.Parse(pd.GetFieldValue("Rating"));
                    mpd.ProductType = pd.GetFieldValue("ProductType");
                    mpd.Price = Double.Parse(pd.GetFieldValue("Price"));
        
                    string[] imgPaths = {basedir, pd.GetFieldValue("Image1LocalPath")};
                    mpd.Image1LocalPath = Path.Combine(imgPaths);

                    ArrayList descs = pd.GetChildArray("Descriptions");
                    foreach (CTable desc in descs)
                    {
                        MGenericDescription mdc = new MGenericDescription();
                        mdc.Language = desc.GetFieldValue("Language");
                        mdc.Name = desc.GetFieldValue("Name");    
                        mdc.ShortDescription = desc.GetFieldValue("ShortDescription"); 
                        mdc.LongDescription1 = desc.GetFieldValue("LongDescription1");
                        mdc.LongDescription2 = desc.GetFieldValue("LongDescription2");                        

                        mpd.Descriptions.Add(mdc.Language, mdc);     
                    } 

                    ArrayList compositions = pd.GetChildArray("Compositions");
                    foreach (CTable comp in compositions)
                    {
                        MProductComposition mpc = new MProductComposition();
                        mpc.Code = comp.GetFieldValue("Code");    
                        mpc.Quantity = Double.Parse(comp.GetFieldValue("Quantity")); 
                        mpc.Unit = comp.GetFieldValue("Unit");                      

                        mpd.Compositions.Add(mpc);

                        ArrayList compositionDescs = comp.GetChildArray("Descriptions");
                        foreach (CTable desc in compositionDescs)
                        {
                            MGenericDescription mdc = new MGenericDescription();
                            mdc.Language = desc.GetFieldValue("Language");
                            mdc.Name = desc.GetFieldValue("Name");    
                            mdc.ShortDescription = desc.GetFieldValue("ShortDescription"); 
                            mdc.LongDescription1 = desc.GetFieldValue("LongDescription1");
                            mdc.LongDescription2 = desc.GetFieldValue("LongDescription2");                        

                            mpc.Descriptions.Add(mdc.Language, mdc);
                        }                         
                    } 

                    Console.WriteLine("Adding product : [{0}]", mpd.Code);
                    opr.Apply(mpd);                  
                }                 
            }
            catch (Exception e)
            {                
                Console.WriteLine("Error : {0}", e.StackTrace);
            }
            
            return 0;
        }
    }
}
