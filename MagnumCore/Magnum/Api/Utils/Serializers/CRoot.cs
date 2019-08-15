using System;
using System.Collections;

using Magnum.Api.Commons.Table;

namespace Magnum.Api.Utils.Serializers
{
	public class CRoot
    {
		private CTable param = null;
		private CTable data = null;

		public CRoot(CTable prm, CTable dta)
		{
            param = prm;
            data = dta;
		}
		
        public CTable Param
        {
            get
            {
                return (param);
            }

            set
            {
                param = value;
            }
        }

        public CTable Data
        {
            get
            {
                return (data);
            }

            set
            {
                data = value;
            }
        }
    }
}
