using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.NoSql;
using Magnum.Api.Commons.Table;
using Magnum.Api.Businesses.Contents;
using Magnum.Api.Utils;

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
                        string lang = "EN";
                        string txt = value.GetFieldValue(lang);
                        mc.Values = new Dictionary<string, string>();
                        mc.Values[lang] = txt;

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
