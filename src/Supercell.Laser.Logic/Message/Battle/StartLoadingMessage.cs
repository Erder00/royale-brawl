namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.Util;

    public class StartLoadingMessage : GameMessage
    {
        public List<BattlePlayer> Players;
        public int OwnIndex;
        public int TeamIndex;

        public int LocationId;
        public int GameMode;

        public int SpectateMode;

        public StartLoadingMessage() : base()
        {
            Players = new List<BattlePlayer>();
        }

        public override void Encode()
        {
            Stream.WriteInt(Players.Count);
            Stream.WriteInt(OwnIndex);
            Stream.WriteInt(TeamIndex);

            Stream.WriteInt(Players.Count);
            foreach (BattlePlayer player in Players)
            {
                player.Encode(Stream);
            }

            Stream.WriteInt(0); // array

            Stream.WriteInt(0); // array

            Stream.WriteInt(0); // randomseed

            Stream.WriteVInt(GameMode);
            Stream.WriteVInt(1);
            Stream.WriteVInt(1);
            Stream.WriteBoolean(true);
            Stream.WriteVInt(SpectateMode);
            Stream.WriteVInt(0);
            ByteStreamHelper.WriteDataReference(Stream, LocationId);
            Stream.WriteBoolean(false);
        }

        public override int GetMessageType()
        {
            return 20559;
        }

        public override int GetServiceNodeType()
        {
            return 4;
        }
    }
}
