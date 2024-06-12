namespace Supercell.Laser.Logic.Message.Club
{
    public class AskForJoinableAllianceListMessage : GameMessage
    {
        public override int GetMessageType()
        {
            return 14303;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
