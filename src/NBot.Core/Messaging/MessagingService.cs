using System;
using System.Linq;
using System.Reflection;
using Autofac;
using NBot.Core.Messaging.Routes;
using log4net;

namespace NBot.Core.Messaging
{
    public class MessagingService : IMessagingService
    {
        private readonly IComponentContext _container;
        private readonly ILog _log;
        private readonly IMessageRouter _router;

        public MessagingService(IMessageRouter router, IComponentContext container, ILog log)
        {
            _router = router;
            _container = container;
            _log = log;
        }

        #region IMessagingService Members

        public void Publish(IMessage message)
        {
            foreach (IRoute route in _router.GetRoutes(message))
            {
                try
                {
                    object reciever = _container.Resolve(route.Reciever);
                    MethodInfo endPoint = route.EndPoint;
                    endPoint.Invoke(reciever, BuildParameters(endPoint, message, route.GetMatchMetaData(message)));
                    _log.Info(string.Format("Published {0}:{1} to {2}", message.GetType(), message.Content, reciever.GetType().Name));
                }
                catch (Exception e)
                {
                    _log.Error(string.Format("Error while publishing message: {0}", message.Content), e);
                }
            }
        }

        public TResult Send<TResult>(IMessage message)
        {
            try
            {
                Type messageType = message.GetType();
                Type handlerType = typeof (IMessageHandler<,>).MakeGenericType(messageType, typeof (TResult));
                object handler = _container.Resolve(handlerType);
                MethodInfo handlerMethod = handlerType.GetMethod("HandleMessage");
                var result = (TResult) handlerMethod.Invoke(handler, BuildParameters(handlerMethod, message, new string[] {}));
                _log.Info(string.Format("Sent {0}:{1} to {2}", message.GetType(), message.Content, handlerType.Name));
                return result;
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error while sending message: {0}", message.Content), e);
            }

            return default(TResult);
        }

        #endregion

        private object[] BuildParameters(MethodInfo method, IMessage message, string[] metaData)
        {
            ParameterInfo[] methodParameters = method.GetParameters();
            var result = new object[methodParameters.Count()];

            for (int parameterIndex = 0; parameterIndex < result.Length; parameterIndex++)
            {
                ParameterInfo parameter = methodParameters[parameterIndex];

                if (parameter.ParameterType.IsAssignableTo<IMessage>())
                {
                    result[parameterIndex] = message;
                }
                else if (parameter.ParameterType == typeof (string[])
                         && (parameter.Name == "matches"
                             || parameter.Name == "metadata"))
                {
                    result[parameterIndex] = metaData;
                }
                else
                {
                    result[parameterIndex] = _container.Resolve(parameter.ParameterType);
                }
            }

            return result;
        }
    }
}