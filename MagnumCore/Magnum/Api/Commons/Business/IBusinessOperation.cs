using System;
using Firebase.Database;

namespace Magnum.Api.Commons.Business
{
	public interface IBusinessOperation
	{
        void SetContext(FirebaseClient context);
    }
}
