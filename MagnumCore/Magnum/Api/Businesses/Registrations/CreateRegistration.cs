using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Utils;

using Microsoft.Extensions.Logging;

namespace Magnum.Api.Businesses.Registrations
{
	public class CreateRegistration : BusinessOperationBase, IBusinessOperationManipulate<MRegistration>
	{
        public int Apply(MRegistration dat)
        {
            ILogger logger = GetLogger();

            if (string.IsNullOrEmpty(dat.IP) || 
                string.IsNullOrEmpty(dat.SerialNumber) || 
                string.IsNullOrEmpty(dat.Pin))
            {
                throw(new ArgumentException("IP, SerialNumber, PIN must not be null!!!"));
            }

            var ctx = GetNoSqlContext();

            string barcode = string.Format("{0}-{1}", dat.SerialNumber, dat.Pin);
            string bcPath = string.Format("barcodes/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}"
                , dat.SerialNumber.ToCharArray()[0]
                , dat.SerialNumber.ToCharArray()[1]
                , dat.SerialNumber.ToCharArray()[2]
                , dat.Pin.ToCharArray()[0]
                , dat.Pin.ToCharArray()[1]
                , dat.Pin.ToCharArray()[2]
                , dat.SerialNumber
                , dat.Pin);
            MBarcode bc = ctx.GetObjectByKey<MBarcode>(bcPath);

            dat.RegistrationDate = DateTime.Now;

            string path = "";
            if (bc == null) 
            {
                dat.Status = "NOTFOUND";
                path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
                ctx.PostData(path, dat);

                string msg = string.Format("Serial number and PIN not found [{0}]", barcode);
                LogUtils.LogInformation(logger, msg);

                throw(new ArgumentException(msg));
            }

            if (bc.IsActivated)
            {
                dat.Status = "FAILED";
                path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
                ctx.PostData(path, dat);

                string msg = string.Format("Serial number and PIN has already been registered [{0}] since [{1}]", barcode, bc.ActivatedDate);
                LogUtils.LogInformation(logger, msg);

                throw(new ArgumentException(msg));
            }

            //Update status back to barcode
            bc.IsActivated = true;
            bc.ActivatedDate = DateTime.Now;
            ctx.PutData(bcPath, bc.Key, bc);

            dat.Status = "SUCCESS";
            path = string.Format("registrations/{0}/{1}", dat.Status, barcode);
            ctx.PostData(path, dat);

            string infoMsg = string.Format("Successfully registered serial number and PIN [{0}]", barcode);
            LogUtils.LogInformation(logger, infoMsg);
            
            return 0;
        }
    }
}
