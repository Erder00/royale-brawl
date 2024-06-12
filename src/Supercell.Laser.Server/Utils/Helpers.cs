namespace Supercell.Laser.Server.Utils
{
    using System;

    public static class Helpers
    {
        private const string STRING_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string RandomString(int length)
        {
            char[] result = new char[length];
            Random rand = new Random();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = STRING_CHARACTERS[rand.Next(STRING_CHARACTERS.Length)];
            }

            return new String(result);
        }
    }
}
