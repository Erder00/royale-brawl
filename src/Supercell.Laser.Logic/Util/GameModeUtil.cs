namespace Supercell.Laser.Logic.Util
{
    using Supercell.Laser.Titan.Debug;

    public static class GameModeUtil
    {
        public static bool PlayersCollectPowerCubes(int variation)
        {
            int v1 = variation - 6;
            if (v1 <= 8)
                return ((0x119 >> v1) & 1) != 0;
            else
                return false;
        }

        public static int GetRespawnSeconds(int variation)
        {
            switch (variation)
            {
                case 0:
                case 2:
                    return 3;
                case 3:
                    return 1;
                default:
                    return 5;
            }
        }

        public static bool PlayersCollectBountyStars(int variation)
        {
            return variation == 3 || variation == 15;
        }

        public static bool HasTwoTeams(int variation)
        {
            return variation != 6;
        }

        public static int GetGameModeVariation(string mode)
        {
            switch (mode)
            {
                case "CoinRush":
                    return 0;
                case "AttackDefend":
                    return 2;
                case "BossFight":
                    return 7;
                case "BountyHunter":
                    return 3;
                case "Artifact":
                    return 4;
                case "LaserBall":
                    return 5;
                case "BattleRoyale":
                    return 6;
                case "BattleRoyaleTeam":
                    return 9;
                case "Survival":
                    return 8;
                case "Raid":
                    return 10;
                case "RoboWars":
                    return 11;
                case "Tutorial":
                    return 12;
                case "Training":
                    return 13;
                default:
                    Debugger.Error("Wrong game mode!");
                    return -1;
            }
        }
    }
}
