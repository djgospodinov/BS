using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using BS.Api.Extensions;
using BS.Common.Models;
using BS.LicenseServer.Services;

namespace BS.Api
{
    public class ApiLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    // Update the API log entry with response info
                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    
                    //TODO: find a better way
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        apiLogEntry.ResponseTimestamp = DateTime.Now;

                        if (response.Content != null)
                        {
                            apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                            apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                            apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                        }

                        // TODO: Save the API log entry to the database
                        LogToDb(apiLogEntry);
                    }

                    return response;
                }, cancellationToken);
        }

        private void LogToDb(ApiLogEntry apiLogEntry)
        {
            ApiLogService.Log(apiLogEntry);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();

            return new ApiLogEntry
            {
                //User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                //RequestContentType = context.Request.ContentType,
                RequestIpAddress = request.GetClientIpAddress(),
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }

    
}