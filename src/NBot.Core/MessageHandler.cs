using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;

namespace NBot.Core
{
    public abstract class MessageHandler : IMessageHandler
    {
        protected IRestClient GetJsonServiceClient(string baseUrl)
        {
            return new JsonServiceClient(baseUrl);
        }

        protected IRestClient GetGZipedJsonServiceClient(string baseurl)
        {
            var result = new JsonServiceClient(baseurl)
            {
                LocalHttpWebRequestFilter = request =>
                {
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                }
            };
            return result;
        }

        protected T GetRandomItem<T>(IEnumerable<T> items)
        {
            var enumerable = items as T[] ?? items.ToArray();
            var index = new Random().Next(0, enumerable.Count());
            return enumerable[index];
        }

        protected int GetRandomNumber(int min, int max)
        {
            return new Random().Next(min, max);
        }

        protected string UrlEncode(string input)
        {
            return Uri.EscapeUriString(input);
        }
    }
}