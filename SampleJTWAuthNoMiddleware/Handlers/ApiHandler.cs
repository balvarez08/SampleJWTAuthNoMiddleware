using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleJTWAuthNoMiddleware.Handlers
{
    public class ApiHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //this is place if we need to add headers
            if (request.Headers.Contains("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }

            //request headers if needed
            //request.Headers.TryAddWithoutValidation("Authorization", $"bearer{--token--}");
            //request.Headers.TryAddWithoutValidation("subscription-key", "");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
