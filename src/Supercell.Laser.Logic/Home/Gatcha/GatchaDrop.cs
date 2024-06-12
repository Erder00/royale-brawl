namespace Supercell.Laser.Logic.Home.Gatcha
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class GatchaDrop
    {
        public int Count;
        public int DataGlobalId;
        public int PinGlobalId;
        public int Type;

        public GatchaDrop(int type)
        {
            Type = type;
        }

        public void DoDrop(HomeMode homeMode)
        {
            ClientAvatar avatar = homeMode.Avatar;

            switch (Type)
            {
                case 1: // Unlock a hero
                    CharacterData characterData = DataTables.Get(16).GetDataByGlobalId<CharacterData>(DataGlobalId);
                    if (characterData == null) return;

                    CardData cardData = DataTables.Get(23).GetData<CardData>(characterData.Name + "_unlock");
                    if (cardData == null) return;

                    avatar.UnlockHero(characterData.GetGlobalId(), cardData.GetGlobalId());
                    break;
                case 6: // Add power points
                    Hero hero = avatar.GetHero(DataGlobalId);
                    if (hero == null) return;

                    hero.PowerPoints += Count;
                    break;
                case 7: // Add gold
                    avatar.AddGold(Count);
                    break;
                case 8: // Add Gems (Bonus)
                    avatar.AddDiamonds(Count);
                    break;
                case 10:
                    homeMode.Home.UnlockedEmotes.Add(DataGlobalId);
                    break;
            }
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(Count);
            ByteStreamHelper.WriteDataReference(stream, DataGlobalId);
            stream.WriteVInt(Type);

            ByteStreamHelper.WriteDataReference(stream, 0);
            ByteStreamHelper.WriteDataReference(stream, PinGlobalId);
            ByteStreamHelper.WriteDataReference(stream, 0);
            stream.WriteVInt(0);
        }
    }
}
