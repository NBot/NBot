namespace NBot.Core
{
    public interface IRobot
    {
        string Name { get; set; }
        string Alias { get; set; }
        T GetSetting<T>(string key);
    }
}