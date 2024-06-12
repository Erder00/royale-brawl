namespace Supercell.Laser.Logic.Message.Home
{
    using Supercell.Laser.Logic.Command;

    public class EndClientTurnMessage : GameMessage
    {
        public int Tick;
        public int Checksum;

        public List<Command> Commands;

        public EndClientTurnMessage() : base()
        {
            Commands = new List<Command>();
        }

        public override void Decode()
        {
            Stream.ReadBoolean();
            Tick = Stream.ReadVInt();
            Checksum = Stream.ReadVInt();

            int count = Stream.ReadVInt();
            for (int i = 0; i < count; i++)
            {
                Command command = CommandManager.DecodeCommand(Stream);
                if (command == null) return;

                Commands.Add(command);
            }
        }

        public override int GetMessageType()
        {
            return 14102;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
