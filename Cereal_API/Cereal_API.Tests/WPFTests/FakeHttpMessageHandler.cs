using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Cereal_API.Tests.WPFTests;

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    public HttpRequestMessage? Request { get; private set; }
    public HttpResponseMessage Response { get; set; } = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;
        return Task.FromResult(Response);
    }

    public static FakeHttpMessageHandler CreateFakeMessageHandler(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new FakeHttpMessageHandler
        {
            Response = new HttpResponseMessage(statusCode)
        };
    }

    public static FakeHttpMessageHandler CreateFakeMessageHandlerFor<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (responseObject is byte[])
        {
            return new FakeHttpMessageHandler
            {
                Response = new HttpResponseMessage(statusCode)
                {
                    Content = new ByteArrayContent(responseObject as byte[] ?? [])
                }
            };
        }

        return new FakeHttpMessageHandler
        {
            Response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(responseObject),
                    Encoding.UTF8,
                    "application/json")
            }
        };
    }
}
