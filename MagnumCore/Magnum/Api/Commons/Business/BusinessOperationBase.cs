using System;
using Firebase.Database;

namespace Magnum.Api.Commons.Business
{    
	public class BusinessOperationBase : IBusinessOperation
	{
        private FirebaseClient fbContext = null;

        public void SetContext(FirebaseClient context)
        {
            fbContext = context;
        }
    }
}
