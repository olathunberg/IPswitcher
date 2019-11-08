#nullable enable
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TTech.IP_Switcher.Features.IpSwitcher.Resources;

namespace TTech.IP_Switcher.Helpers
{
    public static class PublicIpHelper
    {
        internal static async Task<string> GetExternalIp()
        {
            var uriList = ConfigurationManager.AppSettings["publicIpUrls"]?.Split(';');

            if (uriList == null)
                return string.Empty;

            string publicIPAddress = string.Empty;
            foreach (var uri in uriList)
            {
                publicIPAddress = await RequestExtenalIp(uri);
                publicIPAddress = publicIPAddress.Replace("\n", "");

                if (publicIPAddress != null && ValidateStringAsIpAddress(publicIPAddress))
                    break;
            }

            if (publicIPAddress != null && ValidateStringAsIpAddress(publicIPAddress))
                return publicIPAddress;
            else
                return IpSwitcherViewModelLoc.SearchFailed;
        }

        private static async Task<string> RequestExtenalIp(string uri)
        {
            if (WebRequest.Create(uri) is HttpWebRequest request)
            {
                request.UserAgent = "curl"; // this simulate curl linux command

                string publicIPAddress;

                request.Method = "GET";

                try
                {
                    using (var response = await request.GetResponseAsync())
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        publicIPAddress = reader.ReadToEnd();
                    }
                }
                catch (Exception)
                {
                    publicIPAddress = string.Empty;
                }

                return publicIPAddress;
            }

            return string.Empty;
        }

        private static bool ValidateStringAsIpAddress(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length == 4 && IPAddress.TryParse(value, out IPAddress ipAddr);
        }
    }
}
