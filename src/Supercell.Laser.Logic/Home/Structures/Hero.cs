namespace Supercell.Laser.Logic.Home.Structures
{
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    [JsonObject(MemberSerialization.OptIn)]
    public class Hero
    {
        public static readonly int[] UpgradePowerPointsTable = new int[]
        {
            20, 50, 100, 180, 310, 520, 860, 1410, 2300, 2300 + 1440
        };

        public static readonly int[] UpgradeCostTable = new int[]
        {
            20, 35, 75, 140, 290, 480, 880, 1250, 1875, 2800
        };

        [JsonProperty] public int CharacterId;
        [JsonProperty] public int CardId;

        [JsonProperty] public int Trophies;
        [JsonProperty] public int HighestTrophies;

        [JsonProperty] public int PowerPoints;
        [JsonProperty] public int PowerLevel;

        public CharacterData CharacterData => DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterId);
        public CardData CardData => DataTables.Get(DataType.Card).GetDataByGlobalId<CardData>(CardId);

        public Hero(int characterId, int cardId)
        {
            CharacterId = characterId;
            CardId = cardId;

            PowerLevel = 0;
        }

        public void AddTrophies(int trophies)
        {
            Trophies += trophies;
            HighestTrophies = LogicMath.Max(HighestTrophies, Trophies);
        }

        public void Encode(ByteStream stream)
        {
            ByteStreamHelper.WriteDataReference(stream, CharacterData);
            ByteStreamHelper.WriteDataReference(stream, null);
            stream.WriteVInt(Trophies);
            stream.WriteVInt(HighestTrophies);
            stream.WriteVInt(PowerLevel + 1);
        }
    }
}
