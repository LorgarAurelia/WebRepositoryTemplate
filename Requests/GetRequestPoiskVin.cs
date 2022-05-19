using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class GetRequestPoiskVin : BaseRequest
    {
        public GetRequestPoiskVin(string url) : base(HttpMethod.Get, url)
        {
            Headers.Add("W_REST", "poskvin");
        }
    }
}
