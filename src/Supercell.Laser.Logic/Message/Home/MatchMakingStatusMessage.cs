namespace Supercell.Laser.Logic.Message.Home
{
    public class MatchMakingStatusMessage : GameMessage
    {
        public int Seconds;
        public int Found;
        public int Max;
        public bool ShowTips;

        public override void Encode()
        {
            Stream.WriteInt(Seconds);
            Stream.WriteInt(Found);
            Stream.WriteInt(Max);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteBoolean(ShowTips);
        }

        public override int GetMessageType()
        {
            return 20405;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
