namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Quest;

    public class BattleEndMessage : GameMessage
    {
        public BattleEndMessage() : base()
        {
            ProgressiveQuests = new List<Quest>();
        }

        public int Result;
        public int TokensReward;
        public int TrophiesReward;
        public List<BattlePlayer> Players;
        public List<Quest> ProgressiveQuests;
        public BattlePlayer OwnPlayer;
        public bool StarToken;

        public int GameMode;

        public bool IsPvP;

        public override void Encode()
        {
            Stream.WriteVInt(GameMode); // game mode
            Stream.WriteVInt(Result);

            Stream.WriteVInt(TokensReward); // tokens reward
            Stream.WriteVInt(TrophiesReward); // trophies reward
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(StarToken);
            Stream.WriteBoolean(false); // no experience
            Stream.WriteBoolean(false); // no tokens left
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(IsPvP); // is PvP
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);

            Stream.WriteVInt(-1);
            Stream.WriteBoolean(false);

            Stream.WriteVInt(Players.Count);
            foreach (BattlePlayer player in Players)
            {
                Stream.WriteBoolean(player.AccountId == OwnPlayer.AccountId); // is own player
                Stream.WriteBoolean(player.TeamIndex != OwnPlayer.TeamIndex); // is enemy
                Stream.WriteBoolean(false); // Star player

                ByteStreamHelper.WriteDataReference(Stream, player.CharacterId);
                Stream.WriteVInt(0); // skin

                Stream.WriteVInt(player.Trophies); // trophies
                Stream.WriteVInt(0);
                Stream.WriteVInt(player.HeroPowerLevel + 1); // power level
                bool isOwn = player.AccountId == OwnPlayer.AccountId;
                Stream.WriteBoolean(isOwn);
                if (isOwn)
                {
                    Stream.WriteLong(player.AccountId);
                }

                player.DisplayData.Encode(Stream);
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(2);
            {
                Stream.WriteVInt(1);
                Stream.WriteVInt(OwnPlayer.Trophies); // Trophies
                Stream.WriteVInt(OwnPlayer.HighestTrophies); // Highest Trophies

                Stream.WriteVInt(5);
                Stream.WriteVInt(100);
                Stream.WriteVInt(100);
            }

            ByteStreamHelper.WriteDataReference(Stream, 28000000);

            Stream.WriteBoolean(false);

            if (Stream.WriteBoolean(ProgressiveQuests.Count > 0))
            {
                Stream.WriteVInt(ProgressiveQuests.Count);
                foreach (Quest quest in ProgressiveQuests)
                {
                    quest.Encode(Stream);
                }
            }
        }

        public override int GetMessageType()
        {
            return 23456;
        }

        public override int GetServiceNodeType()
        {
            return 27;
        }
    }
}
