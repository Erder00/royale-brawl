namespace Supercell.Laser.Logic.Util
{
    public static class LogicLongUtil
    {
        public static int GetHigherInt(this long value)
        {
            return (int)(value >> 32);
        }

        public static int GetLowerInt(this long value)
        {
            return (int)value;
        }
    }
}
