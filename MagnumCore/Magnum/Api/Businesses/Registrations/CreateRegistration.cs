using System;
using Magnum.Api.Commons.Business;
using Magnum.Api.Models;

namespace Magnum.Api.Businesses.Registrations
{
	public class CreateRegistration : BusinessOperationBase, IBusinessOperationManipulate<MRegistration>
	{
        public int Apply(MRegistration dat)
        {
            return 0;
        }
    }
}
