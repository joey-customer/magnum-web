using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Magnum.Api.Utils
{
    public static class RemoteUtils
    {
        public static string GetRemoteIPAddress(ControllerContext ControllerContext)
        {
            IPAddress remoteIPAddress = ControllerContext.HttpContext.Connection.RemoteIpAddress;
            string address = Regex.Replace(remoteIPAddress.ToString(), "^.*:", "");
            return address;
        }
    }
}
