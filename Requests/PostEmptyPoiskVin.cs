using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class EmptyPostRequest : DefaultRequest
    {
        public EmptyPostRequest(string url, string contentType) : base(HttpMethod.Post, url)
        {
            Headers.TryAddWithoutValidation("Content-Type", contentType);
        }
    }
}
