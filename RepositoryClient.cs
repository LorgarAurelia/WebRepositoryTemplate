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
        private bool _isAllowAutoRedirection;
        public CookieContainer CookieContainer { get; private set; } = new();
        public WebProxy ProxyDealer { get; set; }
        public string BaseProxyRequestUrl { get; set; }
        public Uri ProxyUrl { get; set; }

        public RepositoryClient( bool isAllowAutoRedirection = false )
        {
            _isAllowAutoRedirection = isAllowAutoRedirection;
        }
        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage requestMessage)
        {
            var netClient = CreateClient();
            Activity.Current = null;

            var responce = await netClient.SendAsync(requestMessage);

            return responce;
        }
        
        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = _isAllowAutoRedirection,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Proxy = ProxyDealer,
                CookieContainer = CookieContainer
            };

            handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            
            HttpClient client = new(handler);
            
            return client;
        }
        public void SetAutoRedirection(bool isAllowAutoRedirection)
        {
            _isAllowAutoRedirection = isAllowAutoRedirection;
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
        public void SetProxy(string proxyAddress)
        {
            ProxyDealer = new(proxyAddress);
        }
        public void AddCookieCollectionToContainer(CookieCollection cookies)
        {
            CookieContainer.Add(cookies);
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
