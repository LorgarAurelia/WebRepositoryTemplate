using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public abstract class DefaultRequest : HttpRequestMessage
    {
        public DefaultRequest(HttpMethod requestMethod, string url) : base(requestMethod, url)
        {
            Headers.Add("Accept", "application/json, text/plain, */*");
            Headers.Add("Accept-Encoding", "gzip, deflate");
            Headers.Add("Accept-Language", "ru-RU,ru;q=0.9");
            Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
        }
    }
}
