using System;

namespace Magnum.Api.Utils
{
    public class BarcodeUtils
    {
        public static string BuildBarcodePath(string tableName, string strSerial, string strPin)
        {
            char[] serialNumber = strSerial.ToCharArray();
            char[] pin = strPin.ToCharArray();
            return String.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}", tableName
                        , serialNumber[0], serialNumber[1], serialNumber[2]
                        , pin[0], pin[1], pin[2], strSerial, strPin);
        }
    }
}