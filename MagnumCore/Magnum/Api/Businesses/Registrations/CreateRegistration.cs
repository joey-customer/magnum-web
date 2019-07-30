using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Registrations
{
	public class CreateRegistration : BusinessOperationBase, IBusinessOperationManipulate<MRegistration>
	{
        public int Apply(MRegistration dat)
        {
            if (string.IsNullOrEmpty(dat.IP) || 
                string.IsNullOrEmpty(dat.SerialNumber) || 
                string.IsNullOrEmpty(dat.Pin))
            {
                throw(new ArgumentException("IP, SerialNumber, PIN must not be null!!!"));
            }

            var ctx = GetContext();

            string barcode = string.Format("{0}-{1}", dat.SerialNumber, dat.Pin);
            string key = string.Format("barcodes/{0}", barcode);
            MBarcode bc = ctx.GetObjectByKey<MBarcode>(key, "");

            dat.RegistrationDate = DateTime.Now;
            string path = string.Format("registrations/{0}", barcode);

            if (bc == null) 
            {
                dat.Status = "NOTFOUND";
                ctx.PostData(path, dat);

                string msg = string.Format("Barcode not found [{0}]", barcode);
                throw(new ArgumentException(msg));
            }

            if (bc.IsActivated)
            {
                dat.Status = "FAILED";
                ctx.PostData(path, dat);

                string msg = string.Format("Barcode was already registered [{0}]", barcode);
                throw(new ArgumentException(msg));
            }
Console.WriteLine("DEBUG [{0}]", bc.SerialNumber);
            //Update status back to barcode

            dat.Status = "SUCCESS";
            ctx.PostData(path, dat);
            
            return 0;
        }
    }
}
