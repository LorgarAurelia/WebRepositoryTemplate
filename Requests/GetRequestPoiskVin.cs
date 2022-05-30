using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class GetRequest : DefaultRequest
    {
        public GetRequest(string url) : base(HttpMethod.Get, url)
        {
        }
    }
}
