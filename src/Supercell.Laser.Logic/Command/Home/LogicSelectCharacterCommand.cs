namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSelectCharacterCommand : Command
    {
        public int CharacterInstanceId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            CharacterInstanceId = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            int globalId = GlobalId.CreateGlobalId(16, CharacterInstanceId);
            if (homeMode.Avatar.HasHero(globalId))
            {
                homeMode.Home.CharacterId = globalId;
                homeMode.CharacterChanged.Invoke(globalId);
                return 0;
            }

            return -1;
        }

        public override int GetCommandType()
        {
            return 525;
        }
    }
}
