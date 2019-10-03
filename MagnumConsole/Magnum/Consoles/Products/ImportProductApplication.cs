using System;
using System.IO;
using System.Collections;


using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.Products;

using Its.Onix.Core.Applications;
using Its.Onix.Core.Utils;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Storages;
using Its.Onix.Core.Commons.Table;

using Microsoft.Extensions.Logging;

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
            ILogger logger = GetLogger();
            
            Hashtable args = GetArguments();
            string basedir = args["basedir"].ToString();

            CTable t = XmlToCTable();

            INoSqlContext ctx = GetNoSqlContextWithAuthen("FirebaseNoSqlContext");
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            IStorageContext storageCtx = GetStorageContextWithAuthen("FirebaseStorageContext");
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
                    mpd.NameColor = pd.GetFieldValue("NameColor");
                    mpd.NameBgColor = pd.GetFieldValue("NameBgColor");
        
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
                        mdc.Extra1 = desc.GetFieldValue("Extra1");

                        mpd.Descriptions.Add(mdc.Language, mdc);     
                    } 

                    ArrayList compositionGroups = pd.GetChildArray("CompositionGroups");
                    foreach (CTable cg in compositionGroups)
                    {
                        MProductCompositionGroup mpcg = new MProductCompositionGroup();
                        mpcg.PerUnit = cg.GetFieldValue("PerUnit");

                        ArrayList compositions = cg.GetChildArray("Compositions");
                        foreach (CTable comp in compositions)
                        {
                            MProductComposition mpc = new MProductComposition();
                            mpc.Code = comp.GetFieldValue("Code");    
                            mpc.Quantity = Double.Parse(comp.GetFieldValue("Quantity")); 
                            mpc.Unit = comp.GetFieldValue("Unit");                      

                            mpcg.Compositions.Add(mpc);

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
                        mpd.CompositionGroups.Add(mpcg);
                    }

                    ArrayList performances = pd.GetChildArray("Performances");
                    foreach (CTable perf in performances)
                    {
                        MProductPerformance mpp = new MProductPerformance();
                        mpp.Code = perf.GetFieldValue("Code");    
                        mpp.Quantity = Double.Parse(perf.GetFieldValue("Quantity")); 
                        mpp.Unit = perf.GetFieldValue("Unit");                      

                        mpd.Performances.Add(mpp);

                        ArrayList compositionDescs = perf.GetChildArray("Descriptions");
                        foreach (CTable desc in compositionDescs)
                        {
                            MGenericDescription mdc = new MGenericDescription();
                            mdc.Language = desc.GetFieldValue("Language");
                            mdc.Name = desc.GetFieldValue("Name");    
                            mdc.ShortDescription = desc.GetFieldValue("ShortDescription"); 
                            mdc.LongDescription1 = desc.GetFieldValue("LongDescription1");
                            mdc.LongDescription2 = desc.GetFieldValue("LongDescription2");                        

                            mpp.Descriptions.Add(mdc.Language, mdc);
                        }                         
                    } 

                    LogUtils.LogInformation(logger , "Adding product : [{0}]", mpd.Code);
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
