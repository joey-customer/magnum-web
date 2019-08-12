using System;
using System.Collections;
using NUnit.Framework;

namespace Magnum.Api.Models
{
	public class ModelsTest
	{
        private ArrayList models = new ArrayList();

        [SetUp]
        public void Setup()
        {
            models.Add(new MBarcode());
            models.Add(new MRegistration());
            models.Add(new MProduct());
            models.Add(new MProductComposition());
            models.Add(new MGenericDescription());
        }

        [TestCase]
        public void ModelPopulatePropertiesTest()
        {
            foreach (var model in models)
            {
                var props = model.GetType().GetProperties();
                foreach(var prop in props) 
                {
                    object oldValue = null;

                    if (prop.PropertyType == typeof(int))
                    {
                        oldValue = 99999;                        
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        oldValue = 69696.99;
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        oldValue = "THIS IS DUMMY STRING";
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        oldValue = DateTime.Now;
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        oldValue = false;
                    }

                    prop.SetValue(model, oldValue);
                    var newValue = prop.GetValue(model);

                    Assert.AreEqual(oldValue, newValue, "Property value should be the same for set and get !!!");
                }   
            }         
        }
    }
}
