using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Contents
{
	public class SaveContent : BusinessOperationBase, IBusinessOperationGetInfo<MContent>
	{
        public MContent Apply(MContent dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Code must not be null!!!"));
            }

            // GetProductTypeInfo opr = (GetProductTypeInfo)FactoryBusinessOperation.CreateBusinessOperationObject("GetProductTypeInfo");
            // MContent prd = opr.Apply(dat);

            var ctx = GetNoSqlContext();
             
            // if (prd == null)
            // {
                //Does not exist then create new one
                string path = string.Format("contents/{0}", dat.Type + "_" + dat.Name);                 
                //Put again to eliminate the GUI_ID key
                ctx.PutData(path, "", dat);  
            // }
            // else
            // {
            //     dat.Key = prd.Key;
            //     ctx.PutData("contents", dat.Code, dat);  
            // }        

            //Return null indicates create new one, not null indicates update the existing one            
            return null;
        }
    }
}
