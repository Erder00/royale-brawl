namespace Supercell.Laser.Logic.Team
{
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class TeamMember
    {
        public bool IsOwner;
        public long AccountId;

        public int CharacterId;
        public int SkinId;

        public int HeroTrophies;
        public int HeroHighestTrophies;
        public int HeroLevel;

        public int State;
        public bool IsReady;

        public PlayerDisplayData DisplayData;

        public void Encode(ByteStream stream)
        {
            stream.WriteBoolean(IsOwner);
            stream.WriteLong(AccountId);

            ByteStreamHelper.WriteDataReference(stream, CharacterId);
            ByteStreamHelper.WriteDataReference(stream, SkinId);
            
            stream.WriteVInt(HeroTrophies);
            stream.WriteVInt(HeroHighestTrophies);
            stream.WriteVInt(HeroLevel + 1);
            stream.WriteVInt(State);

            stream.WriteBoolean(IsReady);

            stream.WriteVInt(0); // team
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // 2

            DisplayData.Encode(stream);

            ByteStreamHelper.WriteDataReference(stream, null); // star power
            ByteStreamHelper.WriteDataReference(stream, null); // star power

            stream.WriteVInt(0);
        }
    }
}
