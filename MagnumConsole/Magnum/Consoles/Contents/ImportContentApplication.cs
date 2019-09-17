using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.Contents;
using Its.Onix.Core.Utils;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;
using Its.Onix.Core.Commons.Table;

using Magnum.Consoles.Commons;
using Microsoft.Extensions.Logging;
using NDesk.Options;

namespace Magnum.Consoles.Contents
{
    public class ImportContentApplication : ConsoleAppBase
    {
        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("if=|infile=", "XML Import file", s => AddArgument("infile", s))
            .Add("b=|basedir=", "Local XML base directory", s => AddArgument("basedir", s));

            return options;
        }

        protected override int Execute()
        {
            ILogger logger = GetLogger();
            CTable t = XmlToCTable();

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");
            FactoryBusinessOperation.SetNoSqlContext(ctx);

            SaveContent opr = (SaveContent)FactoryBusinessOperation.CreateBusinessOperationObject("SaveContent");

            try
            {
                ArrayList types = t.GetChildArray("Contents");
                foreach (CTable pt in types)
                {
                    MContent mc = new MContent();
                    mc.Name = pt.GetFieldValue("Name");
                    mc.Type = pt.GetFieldValue("Type");
                    mc.LastMaintDate = DateTime.Now;
                    ArrayList values = pt.GetChildArray("Values");

                    foreach (CTable value in values)
                    {
                        mc.Values = new Dictionary<string, string>();
                        foreach(CField field in value.GetTableFields())
                        {
                            mc.Values[field.GetName()] = field.GetValue();
                        }
                        LogUtils.LogInformation(logger , "Adding content : [{0}][{1}]", mc.Type, mc.Name);
                    }

                    opr.Apply(mc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e);
            }

            return 0;
        }
    }
}
