using System;

namespace Magnum.Api.Commons.Table
{
	class CField
	{		
		private readonly String name;
		private String value;
		
		public CField(String t, String v, String fldName)
		{
			value = v;
			name = fldName;			
		}
		
		public String GetValue()
		{
			return(value);
		}
		
		public String GetName()
		{
			return(name);
		}
		
		public void SetValue(String v)
		{
			value = v;
		}
	}
}
