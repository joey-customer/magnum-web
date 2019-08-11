using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Factories;

namespace Magnum.Api.Businesses.Products
{
	public class SaveProduct : BusinessOperationBase, IBusinessOperationGetInfo<MProduct>
	{
        public MProduct Apply(MProduct dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Language and Code must not be null!!!"));
            }

            GetProductInfo opr = (GetProductInfo) FactoryBusinessOperation.CreateBusinessOperationObject("GetProductInfo");
            MProduct prd = opr.Apply(dat);

            var ctx = GetContext();
            string path = string.Format("products/{0}/{1}", dat.Code, dat.Language);

            if (prd == null)
            {
                //Does not exist then create new one
                dat.LastUpdateDate = DateTime.Now;
                ctx.PostData(path, dat);                
            }
            else
            {
                prd.LastUpdateDate = DateTime.Now;
                ctx.PutData(path, prd.Key, prd);  
            }        

            //Return null indicates create new one, not null indicates update the existing one            
            return prd;
        }
    }
}
