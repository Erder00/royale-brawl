namespace Supercell.Laser.Logic.Friends
{
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Titan.DataStream;

    public class Friend
    {
        public long AccountId;
        public int Trophies;
        public PlayerDisplayData DisplayData;

        public int FriendState;
        public int FriendReason;

        [JsonIgnore] public ClientAvatar Avatar => LogicServerListener.Instance.GetAvatar(AccountId);

        public void Encode(ByteStream stream)
        {
            stream.WriteLong(AccountId);

            stream.WriteString(null);
            stream.WriteString(null);
            stream.WriteString(null);
            stream.WriteString(null);
            stream.WriteString(null);
            stream.WriteString(null);

            stream.WriteInt(Avatar.Trophies);
            stream.WriteInt(FriendState);
            stream.WriteInt(FriendReason);
            stream.WriteInt(0);
            stream.WriteInt(0);

            stream.WriteBoolean(false); // Alliance entry

            stream.WriteString(null);
            stream.WriteInt(LogicServerListener.Instance.IsPlayerOnline(AccountId) ? 0 : (int)(DateTime.UtcNow - Avatar.LastOnline).TotalSeconds); // Last online time
            if (stream.WriteBoolean(DisplayData != null))
            {
                DisplayData.Encode(stream);
            }
        }
    }
}
