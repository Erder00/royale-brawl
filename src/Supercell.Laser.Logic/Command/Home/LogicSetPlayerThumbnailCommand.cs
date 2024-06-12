namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetPlayerThumbnailCommand : Command
    {
        public int ThumbnailInstanceId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            ThumbnailInstanceId = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (ThumbnailInstanceId < 0) return 1;
            if (ThumbnailInstanceId > DataTables.Get(DataType.PlayerThumbnail).Count) return 2;

            homeMode.Home.ThumbnailId = GlobalId.CreateGlobalId(28, ThumbnailInstanceId);

            return 0;
        }

        public override int GetCommandType()
        {
            return 505;
        }
    }
}
