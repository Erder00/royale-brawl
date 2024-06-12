namespace Supercell.Laser.Logic.Avatar
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public enum AllianceRole
    {
        None = 0,
        Member = 1,
        Leader = 2,
        Elder = 3,
        CoLeader = 4
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientAvatar
    {
        [JsonProperty] public long AccountId;
        [JsonProperty] public string PassToken;

        [JsonProperty] public string Name;
        [JsonProperty] public bool NameSetByUser;
        [JsonProperty] public int TutorialsCompletedCount = 2;

        [JsonIgnore] public HomeMode HomeMode;

        [JsonProperty] public int Gold;
        [JsonProperty] public int Diamonds;

        [JsonProperty] public List<Hero> Heroes;

        [JsonProperty] public int TrioWins;
        [JsonProperty] public int SoloWins;

        [JsonProperty] public int Tokens;
        [JsonProperty] public int StarTokens;

        [JsonProperty] public bool IsDev;
        [JsonProperty] public bool IsPremium;

        [JsonProperty] public long AllianceId;
        [JsonProperty] public AllianceRole AllianceRole;

        [JsonProperty] public DateTime LastOnline;

        [JsonProperty] public List<Friend> Friends;
        [JsonProperty] public bool Banned;

        [JsonIgnore] public int PlayerStatus;
        [JsonIgnore] public long TeamId;

        [JsonIgnore] public long BattleId;
        [JsonIgnore] public long UdpSessionId;
        [JsonIgnore] public int TeamIndex;
        [JsonIgnore] public int OwnIndex;

        [JsonProperty] public int RollsSinceGoodDrop;

        public int Trophies
        {
            get
            {
                int result = 0;
                foreach (Hero hero in Heroes.ToArray())
                {
                    result += hero.Trophies;
                }
                return result;
            }
        }

        public int HighestTrophies
        {
            get
            {
                int result = 0;
                foreach (Hero hero in Heroes.ToArray())
                {
                    result += hero.HighestTrophies;
                }
                return result;
            }
        }

        public int GetUnlockedBrawlersCountWithRarity(string rarity)
        {
            return Heroes.Count(x => x.CardData.Rarity == rarity);
        }

        public void ResetTrophies()
        {
            foreach (Hero hero in Heroes.ToArray())
            {
                hero.Trophies = 0;
                hero.HighestTrophies = 0;
            }
        }

        public void AddTrophies()
        {
            foreach (Hero hero in Heroes.ToArray())
            {
                hero.Trophies = 1250;
                hero.HighestTrophies = 1250;
            }
        }

        public int GetUnlockedHeroesCount()
        {
            return Heroes.Count;
        }

        public void UnlockHero(int characterId, int cardId)
        {
            Hero heroEntry = new Hero(characterId, cardId);
            Heroes.Add(heroEntry);
        }

        public bool HasHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId) != null;
        }

        public Hero GetHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId);
        }

        public bool UseDiamonds(int count)
        {
            if (count > Diamonds) return false;

            Diamonds -= count;
            return true;
        }

        public bool UseGold(int count)
        {
            if (count > Gold) return false;

            Gold -= count;
            return true;
        }

        public void SetTrophies(int count)
        {
            foreach (Hero hero in Heroes.ToArray())
            {
                hero.Trophies = count;
                hero.HighestTrophies = count;
            }
        }

        public void AddDiamonds(int count)
        {
            Diamonds += count;
        }

        public void AddGold(int count)
        {
            Gold += count;
        }

        public void AddTokens(int count)
        {
            HomeMode.Home.BrawlPassTokens += count;
        }

        public void AddStarTokens(int count)
        {
            StarTokens += count;
        }

        public void RemoveDiamonds(int count)
        {
            Diamonds -= count;
        }

        public void RemoveGold(int count)
        {
            Gold -= count;
        }

        public void RemoveStarTokens(int count)
        {
            StarTokens -= count;
        }

        public bool UseTokens(int count)
        {
            if (count > Tokens) return false;

            Tokens -= count;
            return true;
        }

        public bool UseStarTokens(int count)
        {
            if (count > StarTokens) return false;

            StarTokens -= count;
            return true;
        }

        public ClientAvatar()
        {
            Name = "Brawler";

            Gold = 100;
            Diamonds = 0;

            Heroes = new List<Hero>();

            IsDev = false;
            IsPremium = false;

            AllianceRole = AllianceRole.None;
            AllianceId = -1;

            LastOnline = DateTime.UtcNow;
            Friends = new List<Friend>();
        }

        public void SkipTutorial()
        {
            TutorialsCompletedCount = 2;
        }

        public bool IsTutorialState()
        {
            return TutorialsCompletedCount < 2;
        }

        public Friend GetRequestFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState != 4);
        }

        public Friend GetAcceptedFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState == 4);
        }

        public Friend GetFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id);
        }

        public int Checksum
        {
            get
            {
                ChecksumEncoder encoder = new ChecksumEncoder();
                Encode(encoder);
                return encoder.GetCheckSum();
            }
        }

        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVLong(AccountId);
            Stream.WriteVLong(AccountId);
            Stream.WriteVLong(AccountId);

            Stream.WriteString(Name);
            Stream.WriteBoolean(NameSetByUser);
            Stream.WriteInt(-1);

            Stream.WriteVInt(8);
            {
                Stream.WriteVInt(3 + Heroes.Count);
                {
                    ByteStreamHelper.WriteDataReference(Stream, 5000001);
                    Stream.WriteVInt(Tokens);

                    ByteStreamHelper.WriteDataReference(Stream, 5000008);
                    Stream.WriteVInt(Gold);

                    ByteStreamHelper.WriteDataReference(Stream, 5000009);
                    Stream.WriteVInt(StarTokens);

                    foreach (Hero hero in Heroes)
                    {
                        ByteStreamHelper.WriteDataReference(Stream, hero.CardData);
                        Stream.WriteVInt(1);
                    }
                }

                Stream.WriteVInt(Heroes.Count);
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(hero.Trophies);
                }

                Stream.WriteVInt(Heroes.Count); 
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(hero.HighestTrophies);
                }

                Stream.WriteVInt(0);

                Stream.WriteVInt(Heroes.Count);
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(hero.PowerPoints);
                }

                Stream.WriteVInt(Heroes.Count);
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(hero.PowerLevel);
                }

                Stream.WriteVInt(0);

                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(Diamonds);
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
            Stream.WriteVInt(TutorialsCompletedCount);
        }
    }
}
