using System;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;
using ServiceStack.Text;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Joke", "XKCD", "Comic")]
    public class Xkcd : MessageHandler
    {
        [Help(Syntax = "<xkcd me <Optional: Number>", Description = "Displays the latest XKCD.com comic or the specific Xkcd comic number if provided.", Example = "nbot xkcd me 600")]
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
            catch(Exception)
            {
                 jsonResponse = jsonClient.Get<string>("/1/info.0.json");               
            }

            result.value = JsonObject.Parse(jsonResponse);
            var imgUrl = result.value.Child("img").Replace("\\", "");
            var altText = result.value.Child("alt");

            //Reply with two messages back to back
            client.ReplyTo(message, imgUrl);
            client.ReplyTo(message, altText);
        }

        private class RootObject
        {
            public string type { get; set; }
            public JsonObject value { get; set; }
        }

        //{"month": "11", "num": 1294, "link": "", "year": "2013", "news": "", "safe_title": "Telescope Names", 
        //"transcript": "", "alt": "The Thirty Meter Telescope will be renamed The Flesh-Searing Eye on the Volcano.", 
        //"img": "http:\/\/imgs.xkcd.com\/comics\/telescope_names.png", "title": "Telescope Names", "day": "22"}

        private class Value
        {
            public int month { get; set; }
            public string num { get; set; }
            public string link { get; set; }
            public string year { get; set; }
            public string news { get; set; }
            public string safe_title { get; set; }
            public string transcript { get; set; }
            public string alt { get; set; }
            public string img { get; set; }
            public string title { get; set; }
            public string day { get; set; }
        }
 
    }
}