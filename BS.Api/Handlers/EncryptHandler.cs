using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BS.Api.Handlers
{
    public class EncryptHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Content.Headers.ContentType.MediaType.Contains("bson"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;
                
                var encodedString = response.Content.ReadAsStringAsync().Result;
                response.Content = new StringContent(Base64Helper.ToBase64String(encodedString), Encoding.UTF8, "application/json");

                return response;
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}