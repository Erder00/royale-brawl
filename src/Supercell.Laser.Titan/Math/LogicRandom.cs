namespace Supercell.Laser.Titan.Math
{
    using Supercell.Laser.Titan.DataStream;

    public class LogicRandom
    {
        private int Seed;

        public LogicRandom()
        {
            // LogicRandom.
        }

        public LogicRandom(int seed)
        {
            this.Seed = seed;
        }

        public int GetIteratedRandomSeed()
        {
            return this.Seed;
        }

        public void SetIteratedRandomSeed(int value)
        {
            this.Seed = value;
        }

        public int Rand(int max)
        {
            if (max > 0)
            {
                Seed = IterateRandomSeed();
                int tmpVal = Seed;

                if (tmpVal < 0)
                {
                    tmpVal = -tmpVal;
                }

                return tmpVal % max;
            }

            return 0;
        }

        public int IterateRandomSeed()
        {
            int seed = this.Seed;

            if (seed == 0)
            {
                seed = -1;
            }

            int tmp = seed ^ (seed << 13) ^ ((seed ^ (seed << 13)) >> 17);
            int tmp2 = tmp ^ (32 * tmp);

            return tmp2;
        }

        public void Decode(ByteStream stream)
        {
            this.Seed = stream.ReadVInt();
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(this.Seed);
        }
    }
}
