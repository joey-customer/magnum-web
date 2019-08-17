using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;
using Magnum.Api.Utils;

namespace Magnum.Api.Businesses.ContactUs
{
	public class SaveContactUs : BusinessOperationBase, IBusinessOperationGetInfo<MContactUs>
	{
        public MContactUs Apply(MContactUs data)
        {
            DateTime currentDate = DateTime.Now;
            data.LastMaintDate = currentDate;

            string path = string.Format("contactus/{0}/{1}", currentDate.Year, currentDate.Month);
            
            var ctx = GetNoSqlContext();
            ctx.PostData(path, data);

            return data;
        }
    }
}
