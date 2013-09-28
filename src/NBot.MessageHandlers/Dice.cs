using System;
using System.Threading;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;
using ServiceStack.Text;

namespace NBot.MessageHandlers
{
    [Tag("Productivity")]
    public class Dice : MessageHandler
    {
        [Help(Syntax = "roll dice <sides>",
            Description = "Given the input phrase, an ASCII drawing will be returned for that phrase.",
            Example = "roll dice 64")]
        [Respond("roll( dice)?( {{sides}})?")]
        public void RollDice(Message message, IMessageClient client, string sides)
        {
            var dice = new Random(DateTime.Now.Millisecond);

            //default to typical 6 side die
            int numSides = 6;
            
            if (string.IsNullOrEmpty(sides) || int.TryParse(sides, out numSides))
            {
                if (numSides <= 0)
                {
                    //negative number
                    var result = "I'm sorry, i will not roll a " + numSides.ToString() +
                                 " sided dice without adult supervision. That would tear a hole in the universe...";
                    
                    client.ReplyTo(message, result);

                    Thread.Sleep(5000);

                    client.ReplyTo(message, "No... you don't qualify as an adult");
                }
                else
                {
                    //good number
                    var result = "You rolled a " + dice.Next(1, numSides + 1) + " on a " + numSides.ToString() + " sided dice";
                    client.ReplyTo(message, result);
                }
            }
            else
            {
                //non number, or too large a number
                var result = "I can't roll a " + sides + " I am but a meager .net bot";
                client.ReplyTo(message, result);
            }
            
        }
    }
}