using System;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack;
using ServiceStack.Text;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Joke", "XKCD", "Comic")]
    public class Xkcd : MessageHandler
    {
        [Help(Syntax = "<xkcd me <Optional: Number>",
            Description = "Displays the latest XKCD.com comic or the specific Xkcd comic number if provided.",
            Example = "nbot xkcd me 600")]
        [Respond("(xkcd me)( )?{{number}}?")]
        public void XkcdMe(Message message, IMessageClient client, string number)
        {
            IRestClient jsonClient = GetJsonServiceClient("http://xkcd.com");
            var result = new RootObject();

            string jsonResponse;

            try
            {
                jsonResponse = (string.IsNullOrEmpty(number) || number == "xkcd me")
                    ? jsonClient.Get<string>("/info.0.json")
                    : jsonClient.Get<string>(string.Format("/{0}/info.0.json", number));
            }
            catch (Exception)
            {
                jsonResponse = jsonClient.Get<string>("/1/info.0.json");
            }

            result.Value = JsonObject.Parse(jsonResponse);
            string imgUrl = result.Value.Child("img").Replace("\\", "");
            string altText = result.Value.Child("alt");

            //Reply with two messages back to back
            client.ReplyTo(message, imgUrl);
            client.ReplyTo(message, altText);
        }

        private class RootObject
        {
            public string Type { get; set; }
            public JsonObject Value { get; set; }
        }

        //{"month": "11", "num": 1294, "link": "", "year": "2013", "news": "", "safe_title": "Telescope Names", 
        //"transcript": "", "alt": "The Thirty Meter Telescope will be renamed The Flesh-Searing Eye on the Volcano.", 
        //"img": "http:\/\/imgs.xkcd.com\/comics\/telescope_names.png", "title": "Telescope Names", "day": "22"}

        private class Value
        {
            public int Month { get; set; }
            public string Num { get; set; }
            public string Link { get; set; }
            public string Year { get; set; }
            public string News { get; set; }
            public string SafeTitle { get; set; }
            public string Transcript { get; set; }
            public string Alt { get; set; }
            public string Img { get; set; }
            public string Title { get; set; }
            public string Day { get; set; }
        }
    }
}