using System;
using System.Text;

namespace Magnum.Api.Utils
{    
    public static class RandomUtils
    {
        public static string RandomString(int size)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  

            for (int i = 0; i < size; i++)  
            {
                int idx = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                char ch = Convert.ToChar(idx);  
                builder.Append(ch);
            }

            return builder.ToString();
        } 

        public static string RandomStringNum(int size)  
        {  
            StringBuilder builder = new StringBuilder();  
            Random random = new Random();  

            for (int i = 0; i < size; i++)  
            {
                int idx = Convert.ToInt32(Math.Floor(10 * random.NextDouble() + 48));
                char ch = Convert.ToChar(idx);  
                builder.Append(ch);
            }

            return builder.ToString();
        }           
    }    
}