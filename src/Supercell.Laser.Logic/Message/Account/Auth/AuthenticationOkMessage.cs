namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class AuthenticationOkMessage : GameMessage
    {
        public long AccountId;
        public string PassToken;
        public string ServerEnvironment;

        public AuthenticationOkMessage() : base()
        {
            ;
        }

        public override void Encode()
        {
            Stream.WriteLong(AccountId);
            Stream.WriteLong(AccountId);

            Stream.WriteString(PassToken);
            Stream.WriteString(null);
            Stream.WriteString(null);

            Stream.WriteInt(GameVersion.MAJOR);
            Stream.WriteInt(GameVersion.BUILD);
            Stream.WriteInt(0);

            Stream.WriteString(ServerEnvironment);

            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
        }

        public override int GetMessageType()
        {
            return 20104;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
