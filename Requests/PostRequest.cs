using System.Net.Http;

namespace WebRepositoryTemplate.Requests
{
    public class PostRequest : DefaultRequest
    {
        public PostRequest(string url, string postContent, string contentType) : base(HttpMethod.Post, url)
        {
            Content = new StringContent(postContent);
            Headers.Add("Content-Type", contentType);
        }
    }
}
