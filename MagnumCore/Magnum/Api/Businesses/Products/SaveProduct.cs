using System;
using System.IO;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Factories;

namespace Magnum.Api.Businesses.Products
{
	public class SaveProduct : BusinessOperationBase, IBusinessOperationGetInfo<MProduct>
	{
        private void PopulateImageProperty(MProduct dat, string fieldName)
        {
            //In the future if we have more than 1 image field then we should change this function 
            var storageCtx = GetStorageContext();

            string image1Ext = Path.GetExtension(dat.Image1LocalPath);
            string image1Path = string.Format("products/{0}/{1}{2}", dat.Code, "Image1", image1Ext);
            var image1Url = storageCtx.UploadFile(image1Path, dat.Image1LocalPath);
            
            dat.Image1Url = image1Url;
            dat.Image1StoragePath = image1Path;
        }

        public MProduct Apply(MProduct dat)
        {
            if (!dat.IsKeyIdentifiable())
            {
                throw(new ArgumentException("Language and Code must not be null!!!"));
            }

            GetProductInfo opr = (GetProductInfo)FactoryBusinessOperation.CreateBusinessOperationObject("GetProductInfo");
            MProduct prd = opr.Apply(dat);

            var ctx = GetNoSqlContext();            

            PopulateImageProperty(dat, "Image1");
            dat.LastUpdateDate = DateTime.Now;

            string path = string.Format("products/{0}/{1}", dat.Code, dat.Language);  
            if (prd == null)
            {
                //Does not exist then create new one
                ctx.PostData(path, dat);
            }
            else
            {
                dat.Key = prd.Key;
                ctx.PutData(path, dat.Key, dat);  
            }        

            //Return null indicates create new one, not null indicates update the existing one            
            return prd;
        }
    }
}
