using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace NBot.CampfireAdapter
{
    public class CampfireRoomListener
    {
        private readonly CampfireRoomListenerContext _context;

        public CampfireRoomListener(CampfireRoomListenerContext context)
        {
            _context = context;
        }

        public CampfireRoomListenerContext Context
        {
            get { return _context; }
        }

        public void StartListening()
        {
            ThreadPool.QueueUserWorkItem(Listen, Context);
        }

        private static void Listen(object state)
        {
            var context = (CampfireRoomListenerContext)state;

            try
            {
                var request = context.CreateRequest();
                var response = request.GetResponse();
                Console.WriteLine("Listening to Room {0}", context.RoomId);

                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        while (stream.CanRead && context.IsListening)
                        {
                            int value = stream.ReadByte();

                            if (value == -1)
                                break;
                            if (value == 32)
                                continue;

                            // message is hear time to read.....
                            string message = ((char)value).ToString();

                            do
                            {
                                value = stream.ReadByte();

                                if (value == -1)
                                    break;
                                message += (char)value;
                            } while (value != 13);

                            var messageRecievedContext = new CampfireMessageRecievedContext(message, context.MessageRecieved);

                            if (messageRecievedContext.Message.UserId != context.UserId.ToString(CultureInfo.InvariantCulture))
                            {
                                ThreadPool.QueueUserWorkItem(ProcessMessage, messageRecievedContext);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                Listen(context);
            }
        }

        public void StopListening()
        {
            Context.StopListening();
        }

        private static void ProcessMessage(object state)
        {
            var messageRecievedContext = (CampfireMessageRecievedContext)state;
            messageRecievedContext.ProcessMessage();
        }
    }
}
