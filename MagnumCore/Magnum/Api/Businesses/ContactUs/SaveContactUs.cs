using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Utils;

namespace Magnum.Api.Businesses.ContactUs
{
	public class SaveContactUs : BusinessOperationBase, IBusinessOperationManipulate<MContactUs>
	{
        public int Apply(MContactUs dat)
        {
            DateTime currentDate = DateTime.Now;
            dat.LastMaintDate = currentDate;

            string path = string.Format("contactus/{0}/{1}", currentDate.Year, currentDate.Month);
            
            var ctx = GetNoSqlContext();
            ctx.PostData(path, dat);

            return 0;
        }
    }
}
