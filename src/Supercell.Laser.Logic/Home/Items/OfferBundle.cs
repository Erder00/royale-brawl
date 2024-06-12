namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class OfferBundle
    {
        public List<Offer> Items;
        public int Currency;
        public int Cost;
        public bool IsDailyDeals;
        public bool Purchased;
        public DateTime EndTime;

        public int OldCost;

        public string Title;
        public string BackgroundExportName;

        public int State;

        public OfferBundle()
        {
            Items = new List<Offer>();
            State = 0;
        }

        public void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(Items.Count);  // RewardCount
            foreach (Offer gemOffer in Items)
            {
                gemOffer.Encode(Stream);
            }

            Stream.WriteVInt(Currency); // currency

            Stream.WriteVInt(Cost); // cost

            Stream.WriteVInt((int)(EndTime - DateTime.UtcNow).TotalSeconds); // Seconds left
            
            Stream.WriteVInt(State); // State
            Stream.WriteVInt(0); // ??

            Stream.WriteBoolean(Purchased); // already bought

            Stream.WriteVInt(0); // ??

            Stream.WriteBoolean(IsDailyDeals); // is daily deals
            Stream.WriteVInt(OldCost); // Old cost???
            Stream.WriteInt(0);
            Stream.WriteString(Title); // Name
            Stream.WriteBoolean(false);
            Stream.WriteString(null);
            Stream.WriteVInt(0);
            Stream.WriteBoolean(false);
        }
    }
}
