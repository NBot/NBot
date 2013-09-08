using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NBot.Core.Messaging.Attributes;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging
{
    public class MessageRouter : IMessageRouter
    {
        private readonly List<IRoute> _routes = new List<IRoute>();

        #region IMessageRouter Members

        public void AddRoute(IRoute route)
        {
            _routes.Add(route);
        }

        public IEnumerable<IRoute> GetRoutes(IMessage message)
        {
            return _routes.Where(r => r.IsMatch(message));
        }

        public void BuildHandlerRoutes(IEnumerable<IHandleMessages> messageHandlers)
        {
            foreach (var handlerType in messageHandlers.Select(h => h.GetType()))
            {
                foreach (var methodInfo in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    object[] messageAttributes = methodInfo.GetCustomAttributes(typeof(HandleMessageAttribute), true);

                    foreach (HandleMessageAttribute customAttibute in messageAttributes)
                    {
                        AddRoute(customAttibute.CreateRoute(handlerType, methodInfo));
                    }
                }
            }
        }

        public void BuildRecieverRoutes(IEnumerable<IRecieveMessages> messageRecievers)
        {
            foreach (Type recieverType in messageRecievers.Select(r => r.GetType()))
            {
                foreach (MethodInfo methodInfo in recieverType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    object[] messageAttributes = methodInfo.GetCustomAttributes(typeof(RecieveMessageAttribute), true);

                    foreach (RecieveMessageAttribute customAttibute in messageAttributes)
                    {
                        AddRoute(customAttibute.CreateRoute(recieverType, methodInfo));
                    }
                }
            }
        }

        #endregion
    }
}