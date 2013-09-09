using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NBot.Core.Messaging;

namespace NBot.Plugins.Extensions
{
    public static class NBotExtensions
    {
        public static Core.NBot RegisterPlugin(this Core.NBot target, string pluginName)
        {
            target.Register(b =>
                b.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(NBotExtensions))).Where(t => t.IsAssignableTo<RecieveMessages>() && t.Name == pluginName)
                    .AsSelf()
                    .AsImplementedInterfaces());
            return target;
        }

       public static Core.NBot ConfigureJoinMe(this Core.NBot target, string authCode)
       {
           target.AddSetting("JoinMeAuthCode", authCode);
           return target;
       }

       public static Core.NBot ConfigureStackOverflowSearch(this Core.NBot target, string ackappsApiKey)
       {
           target.AddSetting("AckappsApiKey", ackappsApiKey);
           return target;
       }
    }
}
