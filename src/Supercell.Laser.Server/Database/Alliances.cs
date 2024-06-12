namespace Supercell.Laser.Server.Database
{
    using MySql.Data.MySqlClient;
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Settings;

    public static class Alliances
    {
        private static long AllianceIdCounter;
        private static string ConnectionString;

        public static void Init(string user, string password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "127.0.0.1";
            builder.UserID = user;
            builder.Password = password;
            builder.SslMode = MySqlSslMode.Disabled;
            builder.Database = Configuration.Instance.DatabaseName;
            builder.CharacterSet = "utf8mb4";

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            ConnectionString = builder.ToString();

            AllianceCache.Init();

            AllianceIdCounter = GetMaxAllianceId();
        }

        public static long GetMaxAllianceId()
        {
            var Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT coalesce(MAX(Id), 0) FROM alliances", Connection);

            long result = Convert.ToInt64(command.ExecuteScalar());
            Connection.Close();
            return result;
        }

        public static void Create(Alliance alliance)
        {
            if (alliance == null) return;
            alliance.Id = ++AllianceIdCounter;
            string json = JsonConvert.SerializeObject(alliance);

            var Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
            MySqlCommand command = new MySqlCommand($"INSERT INTO alliances (`Id`, `Name`, `Trophies`, `Data`) VALUES ({(long)alliance.Id}, @name, {alliance.Trophies}, @data)", Connection);
            command.Parameters?.AddWithValue("@data", json);
            command.Parameters?.AddWithValue("@name", alliance.Name);
            command.ExecuteNonQuery();
            Connection.Close();

            AllianceCache.Cache(alliance);
        }

        public static void Save(Alliance alliance)
        {
            if (alliance == null) return;

            string json = JsonConvert.SerializeObject(alliance);

            var Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
            MySqlCommand command = new MySqlCommand($"UPDATE alliances SET `Trophies`='{alliance.Trophies}', `Data`=@data WHERE Id = '{(long)alliance.Id}'", Connection);
            command.Parameters?.AddWithValue("@data", json);
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public static Alliance Load(long id)
        {
            if (AllianceCache.IsAllianceCached(id))
            {
                return AllianceCache.GetAlliance(id);
            }

            var Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM alliances WHERE Id = '{id}'", Connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Alliance alliance = JsonConvert.DeserializeObject<Alliance>((string)reader["Data"]);
                AllianceCache.Cache(alliance);
                Connection.Close();
                return alliance;
            }
            Connection.Close();
            return null;
        }

        public static List<Alliance> GetRankingList()
        {
            #region GetGlobal

            var list = new List<Alliance>();

            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand($"SELECT * FROM alliances ORDER BY `Trophies` DESC LIMIT 200",
                        connection))
                    {
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                            list.Add(JsonConvert.DeserializeObject<Alliance>((string)reader["Data"]));
                    }

                    connection.Close();
                }

                return list;
            }
            catch (Exception exception)
            {
                return list;
            }

            #endregion
        }

        public static List<Alliance> GetRandomAlliances(int maxCount)
        {
            long count = Math.Min(maxCount, AllianceIdCounter);

            #region GetGlobal

            var list = new List<Alliance>();

            try
            {
                Random rand = new Random();
                for (int i = 0; i < count; i++)
                {
                    var alliance = Load(rand.NextInt64(1, AllianceIdCounter+1));
                    if (alliance != null) list.Add(alliance);
                }

                return list;
            }
            catch (Exception exception)
            {
                return list;
            }

            #endregion
        }
    }
}
