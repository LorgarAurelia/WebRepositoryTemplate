using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class PostPoiskVin : BaseRequest
    {
        public PostPoiskVin(string url, string postContent, string contentType) : base(HttpMethod.Post, url)
        {
            Headers.Add("W_REST", "poskvin");
            Content = new StringContent(postContent);
            Headers.Add("Content-Type", contentType);
        }
    }
}
