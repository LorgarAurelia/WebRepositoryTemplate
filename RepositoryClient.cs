using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using WebRepositoryTemplate.Exceptions;

namespace WebRepositoryTemplate
{
    public class RepositoryClient
    {
        private List<HttpStatusCode> _acceptableStatusCodes;
        public static CookieContainer CookieContainer { get; set; } = new();
        public static WebProxy ProxyDealer { get; set; }
        public string BaseProxyRequestUrl { get; set; }
        public Uri ProxyUrl { get; set; }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage)
        {
            CheckAcceptableStatusCodes();

            var netClient = CreateClient();
            Activity.Current = null;

            using var responce = await netClient.SendAsync(requestMessage);

            if (_acceptableStatusCodes.Contains(responce.StatusCode) == false)
                throw new ResponceExceptions(requestMessage, responce);

            return responce;
        }
        private void CheckAcceptableStatusCodes()
        {
            if (_acceptableStatusCodes == null)
                throw new AggregateException("You need setup acceptable status codes at first");
        }
        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Proxy = ProxyDealer,
                CookieContainer = CookieContainer
            };

            handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            HttpClient client = new(handler);

            return client;
        }
        public void SetAcceptableStatusCodes(List<HttpStatusCode> acceptableStatusCodes)
        {
            _acceptableStatusCodes = acceptableStatusCodes;
        }
        public void SetProxy(string proxyAddress, Uri dealerUrl = null)
        {
            ProxyDealer = new WebProxy(proxyAddress);
            if (dealerUrl != null)
            {
                ProxyUrl = dealerUrl;
                BaseProxyRequestUrl = ProxyUrl.ToString().Replace("start", "");
            }
        }
        public CookieCollection GetAllCookies()
        {
            CookieCollection cookieCollection = new CookieCollection();

            Hashtable table = (Hashtable)CookieContainer.GetType().InvokeMember("m_domainTable",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            CookieContainer,
                                                                            new object[] { });

            foreach (var tableKey in table.Keys)
            {
                String str_tableKey = (string)tableKey;

                if (str_tableKey[0] == '.')
                {
                    str_tableKey = str_tableKey.Substring(1);
                }

                SortedList list = (SortedList)table[tableKey].GetType().InvokeMember("m_list",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.GetField |
                                                                            BindingFlags.Instance,
                                                                            null,
                                                                            table[tableKey],
                                                                            new object[] { });

                foreach (var listKey in list.Keys)
                {
                    String url = "https://" + str_tableKey + (string)listKey;
                    cookieCollection.Add(CookieContainer.GetCookies(new Uri(url)));
                }
            }

            return cookieCollection;
        }
    }
}
