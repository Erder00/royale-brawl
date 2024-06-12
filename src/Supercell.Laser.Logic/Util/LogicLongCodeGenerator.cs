namespace Supercell.Laser.Logic.Util
{
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public static class LogicLongCodeGenerator
    {
        private const string HASHTAG = "#";
        private const string CONVERSION_CHARS = "0289PYLQGRJCUV";

        public static string ToCode(long logicLong)
        {
            int highValue = logicLong.GetHigherInt();

            if (highValue < 256)
            {
                return HASHTAG + Convert(((long)logicLong.GetLowerInt() << 8) | (uint)highValue);
            }

            return null;
        }

        public static long ToId(string code)
        {
            if (code.Length < 14)
            {
                string idCode = code.Substring(1);
                long id = ConvertCode(idCode);

                if (id != -1)
                {
                    return new LogicLong((int)(id % 256), (int)((id >> 8) & 0x7FFFFFFF));
                }
            }
            else
            {
                Debugger.Warning("Cannot convert the string to code. String is too long.");
            }

            return new LogicLong(-1, -1);
        }

        private static long ConvertCode(string code)
        {
            long id = 0;
            int conversionCharsCount = CONVERSION_CHARS.Length;
            int codeCharsCount = code.Length;

            for (int i = 0; i < codeCharsCount; i++)
            {
                int charIndex = CONVERSION_CHARS.IndexOf(code[i]);

                if (charIndex == -1)
                {
                    id = -1;
                    break;
                }

                id = id * conversionCharsCount + charIndex;
            }

            return id;
        }

        private static string Convert(long value)
        {
            char[] code = new char[12];

            if (value > -1)
            {
                int conversionCharsCount = CONVERSION_CHARS.Length;

                for (int i = 11; i >= 0; i--)
                {
                    code[i] = CONVERSION_CHARS[(int)(value % conversionCharsCount)];
                    value /= conversionCharsCount;

                    if (value == 0)
                    {
                        return new string(code, i, 12 - i);
                    }
                }

                return new string(code);
            }

            return null;
        }
    }
}
