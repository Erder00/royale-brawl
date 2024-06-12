namespace Supercell.Laser.Logic.Message.Team
{
    public class TeamSetMemberReadyMessage : GameMessage
    {
        public bool IsReady { get; set; }

        public override void Decode()
        {
            IsReady = Stream.ReadBoolean();
        }

        public override int GetMessageType()
        {
            return 14355;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
