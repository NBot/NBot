using System;
using System.Diagnostics;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    [Tag("Fun","Language")]
    public class PirateTranslator : MessageHandler
    {
        [Help(Syntax = "pirate <text>",
            Description = "The provided text will be translated into pirate.",
            Example = "pirate hello")]
        [Respond("pirate {{text}}")]
        public void PirateTranslate(Message message, IMessageClient client, string text)
        {
            Translate(message, client, text);
        }

        private void Translate(Message message, IMessageClient client, string textToTranslate)
        {
            try
            {
                var result = GetJsonServiceClient(string.Format("http://isithackday.com/arrpi.php?text={0}&format=json", UrlEncode(textToTranslate)))
                    .Get<string>("pirate");

                client.ReplyTo(message, result);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-PirateTranslator", ex.ToString());
                client.ReplyTo(message, "PirateTranslator crashed!");
            }
        }
    }
}
