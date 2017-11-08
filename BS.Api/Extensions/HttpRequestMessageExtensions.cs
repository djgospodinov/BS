using BS.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using BS.Common.Models;
using BS.LicenseServer.Services;
using BS.LicenseServer;
using NLog;

namespace BS.Api.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        public static bool IsIpAllowed(this HttpRequestMessage request)
        {
            if (!request.GetRequestContext().IsLocal)
            {
                try
                {
                    var restritedIps = IpFilterCache.Instance.GetAll()
                        .ToList();

                    if (restritedIps != null && restritedIps.Any())
                    {
                        string val = request.GetClientIpAddress();
                        if (restritedIps.Any(x => x.Address == val && !x.Denied))
                        {
                            return true;
                        }

                        _logger.Log(LogLevel.Info, string.Format("Ip {0} is either not listed or blacklisted,Access to api denied.", val));

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex);
                }
            }

            return true;
        }

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            //Web-hosting
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }
            //Self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }
            //Owin-hosting
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic ctx = request.Properties[OwinContext];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }
            return null;
        }
    }
}