namespace NBot.Core.Routes
{
    public interface IRoomSecurityRoute
    {
        IRoute InnerRoute { get; }
    }
}