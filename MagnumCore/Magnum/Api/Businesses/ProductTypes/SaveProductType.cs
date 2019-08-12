using System;
using System.IO;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Factories;

namespace Magnum.Api.Businesses.ProductTypes
{
	public class SaveProductType : BusinessOperationBase, IBusinessOperationGetInfo<MProductType>
	{
        public MProductType Apply(MProductType dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Code must not be null!!!"));
            }

            GetProductTypeInfo opr = (GetProductTypeInfo)FactoryBusinessOperation.CreateBusinessOperationObject("GetProductTypeInfo");
            MProductType prd = opr.Apply(dat);

            var ctx = GetNoSqlContext();
             
            if (prd == null)
            {
                //Does not exist then create new one
                string path = string.Format("product_types/{0}", dat.Code);                 
                //Put again to eliminate the GUI_ID key
                ctx.PutData(path, "", dat);  
            }
            else
            {
                dat.Key = prd.Key;
                ctx.PutData("product_types", dat.Code, dat);  
            }        

            //Return null indicates create new one, not null indicates update the existing one            
            return prd;
        }
    }
}
