using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebRepositoryTemplate.Exceptions
{
    class ResponceExceptions : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public Uri RequestUrl { get; }
        public HttpMethod RequestMethod { get; }
        public HttpResponseHeaders ResponceHeaders { get; }
        public override string Message { get; }

        public ResponceExceptions(HttpRequestMessage request, HttpResponseMessage responce)
        {
            StatusCode = responce.StatusCode;
            RequestUrl = request.RequestUri;
            RequestMethod = request.Method;
            ResponceHeaders = responce.Headers;
            Message = $"Site return uncorrect responce. Details: \n" +
                $"Request url: {RequestUrl} \n" +
                $"Request method: {RequestMethod.Method} \n" +
                $"Status codes: {StatusCode} \n" +
                $"Responce Headers: {ResponceHeaders}";
        }
    }
}
