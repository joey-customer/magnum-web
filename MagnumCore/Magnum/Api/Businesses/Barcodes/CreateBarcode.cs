using System;
using System.Text;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Barcodes
{
	public class CreateBarcode : BusinessOperationBase, IBusinessOperationGetInfo<MBarcode>
	{
        private string RandomString(int size)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  

            for (int i = 0; i < size; i++)  
            {
                int idx = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                char ch = Convert.ToChar(idx);  
                builder.Append(ch);
            }

            return builder.ToString();
        } 

        private string RandomStringNum(int size)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  

            for (int i = 0; i < size; i++)  
            {
                int idx = Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48));
                char ch = Convert.ToChar(idx);  
                builder.Append(ch);
            }

            return builder.ToString();
        } 

        public MBarcode Apply(MBarcode dat)
        {
            MBarcode bc = new MBarcode();
            bc.BatchNo = dat.BatchNo;
            bc.Url = dat.Url;
            bc.Product = dat.Product;
            bc.GeneratedDate = DateTime.Now;
            bc.IsActivated = false;
            bc.Path = dat.Path;

            bc.CompanyWebSite = dat.CompanyWebSite;
            bc.Barcode = dat.Barcode;
            bc.Product = dat.Product;

            bc.SerialNumber = RandomStringNum(10);
            bc.Pin = RandomStringNum(10);
            bc.PayloadUrl = string.Format("{0}/verification/{1}/{2}/{3}", bc.Url, bc.Path, bc.SerialNumber, bc.Pin);

            string path = string.Format("asset_barcodes/{0}/{1}-{2}", bc.Path, bc.SerialNumber, bc.Pin);
            
            var ctx = GetContext();
            ctx.PostData(path, bc);

            return bc;
        }
    }
}
