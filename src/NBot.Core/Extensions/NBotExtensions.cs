using Autofac;
using NBot.Core.Brains;
using NBot.Core.Messaging.ContentFilters;

namespace NBot.Core.Extensions
{
    public static class NBotExtensions
    {

        public static NBot UseFileBrain(this NBot target, string brainFolder = ".\\brain")
        {
            target.Register(builder => builder.Register(c => new FileBrain(brainFolder)).As<IBrain>());
            return target;
        }


        public static NBot UseSimpleBrain(this NBot target)
        {
            target.Register(builder => builder.Register(c => new SimpleBrain()).As<IBrain>());
            return target;
        }

        public static NBot UseHandleBars(this NBot target)
        {
            target.Register(builder => builder.Register(c => new BrainDataContentFilter(c.Resolve<IBrain>())).As<IContentFilter>());
            return target;
        }
    }
}
