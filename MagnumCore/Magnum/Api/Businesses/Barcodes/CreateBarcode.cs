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
            
            bc.SerialNumber = RandomStringNum(10);
            bc.Pin = RandomString(10);
            bc.PayloadUrl = string.Format("{0}/verification/{1}/{2}", bc.Url, bc.SerialNumber, bc.Pin);

            return bc;
        }
    }
}
