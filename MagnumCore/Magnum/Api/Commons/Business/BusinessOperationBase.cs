using System;
using Magnum.Api.NoSql;

namespace Magnum.Api.Commons.Business
{    
	public class BusinessOperationBase : IBusinessOperation
	{
        private INoSqlContext fbContext = null;

        public void SetContext(INoSqlContext context)
        {
            fbContext = context;
        }

        public INoSqlContext GetContext()
        {
            return fbContext;
        }
    }
}
