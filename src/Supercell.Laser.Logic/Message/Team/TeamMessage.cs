namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Logic.Team;

    public class TeamMessage : GameMessage
    {
        public TeamEntry Team;

        public override void Encode()
        {
            Team.Encode(Stream);
        }

        public override int GetMessageType()
        {
            return 24124;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
