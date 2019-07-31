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
                string.IsNullOrEmpty(dat.Path) || 
                string.IsNullOrEmpty(dat.Pin))
            {
                throw(new ArgumentException("IP, SerialNumber, Path, PIN must not be null!!!"));
            }

            var ctx = GetContext();

            string barcode = string.Format("{0}-{1}", dat.SerialNumber, dat.Pin);
            string bcPath = string.Format("asset_barcodes/{0}/{1}", dat.Path, barcode);
            MBarcode bc = ctx.GetObjectByKey<MBarcode>(bcPath);

            dat.RegistrationDate = DateTime.Now;

            string path = "";
            if (bc == null) 
            {
                dat.Status = "NOTFOUND";
                path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
                ctx.PostData(path, dat);

                string msg = string.Format("Barcode not found [{0}]", barcode);
                throw(new ArgumentException(msg));
            }

            if (bc.IsActivated)
            {
                dat.Status = "FAILED";
                path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
                ctx.PostData(path, dat);

                string msg = string.Format("Barcode was already registered [{0}]", barcode);
                throw(new ArgumentException(msg));
            }

            //Update status back to barcode
            bc.IsActivated = true;
            ctx.PutData(bcPath, bc.Key, bc);

            dat.Status = "SUCCESS";
            path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
            ctx.PostData(path, dat);
            
            return 0;
        }
    }
}
