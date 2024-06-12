namespace Supercell.Laser.Logic.Message.Team.Stream
{
    public class TeamChatMessage : GameMessage
    {
        public string Message { get; set; }

        public override void Decode()
        {
            base.Decode();

            Message = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 14359;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
