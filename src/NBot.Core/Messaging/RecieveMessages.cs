using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;

namespace NBot.Core.Messaging
{
    public class RecieveMessages : IRecieveMessages
    {
        protected HttpClient CreateHttpClient(string url)
        {
            var handler = new HttpClientHandler();
            return CreateHttpClient(url, handler);
        }

        protected HttpClient CreateCompressionHttpClient(string url)
        {
            var handler = new HttpClientHandler
                              {
                                  AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                              };

            return CreateHttpClient(url, handler);
        }

        private HttpClient CreateHttpClient(string url, HttpClientHandler handler)
        {
            var client = new HttpClient(handler) { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Add("UserAgent", "NBot");
            return client;
        }

        protected T GetRandomItem<T>(IEnumerable<T> items)
        {
            var enumerable = items as T[] ?? items.ToArray();
            var index = new Random().Next(0, enumerable.Count());
            return enumerable[index];
        }
    }
}