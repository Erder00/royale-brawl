namespace Supercell.Laser.Logic.Battle.Input
{
    using Supercell.Laser.Titan.DataStream;

    public class ClientInput
    {
        public ClientInput()
        {
            ;
        }

        public int Index;
        public int Type;

        public int X, Y;

        public bool AutoAttack;
        public int AutoAttackTarget; // global id

        public long OwnerSessionId;

        public void Decode(BitStream Stream)
        {
            Index = Stream.ReadPositiveInt(15);
            Type = Stream.ReadPositiveInt(4);

            X = Stream.ReadInt(15);
            Y = Stream.ReadInt(15);

            AutoAttack = Stream.ReadBoolean();

            if (Type == 9) // use emote
            {
                Stream.ReadPositiveInt(3); // emote index
            }

            if (AutoAttack)
            {
                if (Stream.ReadBoolean())
                {
                    AutoAttackTarget = Stream.ReadPositiveInt(14); // global id
                }
            }
        }
    }
}
