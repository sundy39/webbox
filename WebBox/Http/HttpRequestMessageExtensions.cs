using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebBox.Web.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetAccept(this HttpRequestMessage request)
        {
            var mediaTypes = request.Headers.Accept.Where(p => p.MediaType.Contains("/xml") || p.MediaType.Contains("/json")).OrderByDescending(p => p.Quality ?? 1);
            if (mediaTypes.Count() > 0)
            {
                var mediaType = mediaTypes.First();
                if (mediaType.MediaType.Contains("/xml"))
                {
                    return "xml";
                }
            }
            return "json";
        }
    }
}