namespace Supercell.Laser.Logic.Notification
{
    using Supercell.Laser.Titan.DataStream;

    public abstract class BaseNotification
    {
        public string Text { get; set; }

        public virtual void Encode(ByteStream stream)
        {
            stream.WriteInt(1);
            stream.WriteBoolean(true);
            stream.WriteInt(1);
            stream.WriteString("the test");
        }

        public abstract int GetNotificationType();
    }
}
