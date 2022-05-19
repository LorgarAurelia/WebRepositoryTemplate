using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class PostEmptyPoiskVin : BaseRequest
    {
        public PostEmptyPoiskVin(string url, string contentType) : base(HttpMethod.Post, url)
        {
            Headers.Add("W_REST", "poskvin");
            Headers.TryAddWithoutValidation("Content-Type", contentType);
        }
    }
}
