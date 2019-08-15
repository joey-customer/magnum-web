using System;
using System.Collections;

namespace Magnum.Api.Utils.Serializers
{
	public interface ICTableDeserializer
	{
        CRoot Deserialize();
    }
}
