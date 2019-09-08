using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Utils;

namespace Magnum.Api.Businesses.Barcodes
{
    public class CreateBarcode : BusinessOperationBase, IBusinessOperationGetInfo<MBarcode>
    {
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

            bc.SerialNumber = RandomUtils.RandomStringNum(10);
            bc.Pin = RandomUtils.RandomStringNum(10);
            bc.PayloadUrl = string.Format("{0}/verification/{1}/{2}/{3}", bc.Url, bc.Path, bc.SerialNumber, bc.Pin);

            char[] serialNumber = bc.SerialNumber.ToCharArray();
            char[] pin = bc.Pin.ToCharArray();
            string path = string.Format("barcodes/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}"
                , serialNumber[0]
                , serialNumber[1]
                , serialNumber[2]
                , pin[0]
                , pin[1]
                , pin[2]
                , bc.SerialNumber
                , bc.Pin);

            var ctx = GetNoSqlContext();
            ctx.PostData(path, bc);

            return bc;
        }
    }
}
