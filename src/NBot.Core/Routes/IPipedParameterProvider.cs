using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Core.Routes
{
    public interface IPipedParameterProvider
    {
        Dictionary<string, string> GetInputParameters(string[] values);
    }
}
