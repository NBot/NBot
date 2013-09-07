namespace NBot.Core.Messaging
{
    public class EmptyMessage : IMessage
    {
        public EmptyMessage()
        {
            Content = string.Empty;
            RoomId = 0;
        }

        #region IMessage Members

        public int RoomId { get; set; }
        public string Content { get; set; }

        #endregion
    }
}