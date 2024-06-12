namespace Supercell.Laser.Server.Database.Cache
{
    using Supercell.Laser.Server.Database.Models;
    using System.Collections.Generic;

    public static class AccountCache
    {
        private static Dictionary<long, Account> CachedAccounts;
        private static Thread _thread;

        public static int Count
        {
            get
            {
                return CachedAccounts.Count;
            }
        }

        public static void Init()
        {
            CachedAccounts = new Dictionary<long, Account>();
            _thread = new Thread(Update);
            _thread.Start();
        }

        public static bool Started = true;

        private static void Update()
        {
            while (Started)
            {
                SaveAll();
                Thread.Sleep(1000 * 30);
            }
        }

        public static void SaveAll()
        {
            try
            {
                foreach (var avatar in CachedAccounts.Values)
                {
                    try
                    {
                        Accounts.Save(avatar);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unhandled exception while saving account: " + ex.Message + " trace: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception) { }
        }

        public static bool IsAccountCached(long id)
        {
            return CachedAccounts.ContainsKey(id);
        }

        public static Account GetAccount(long id)
        {
            if (CachedAccounts.ContainsKey(id))
            {
                return CachedAccounts[id];
            }
            return null;
        }

        public static void Cache(Account account)
        {
            if (!CachedAccounts.ContainsKey(account.AccountId))
            {
                CachedAccounts.Add(account.AccountId, account);
            }
            else
            {
                CachedAccounts[account.AccountId] = account;
            }
        }
    }
}
