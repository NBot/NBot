using System.Collections.Generic;

namespace NBot.Core.Routes
{
    public interface IMessageParameterProvider
    {
        Dictionary<string, string> GetInputParameters(Message message);
    }
}