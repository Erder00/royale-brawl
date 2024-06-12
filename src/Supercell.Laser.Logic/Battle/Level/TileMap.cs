namespace Supercell.Laser.Logic.Battle.Level
{
    using System;
    using System.Collections.Generic;

    public class TileMap
    {
        public readonly int Width, Height;
        private readonly Tile[,] Tiles;

        public int LogicWidth => TileToLogic(Width);
        public int LogicHeight => TileToLogic(Height);

        public List<Tile> SpawnPoints;
        public List<Tile> SpawnPointsTeam1;
        public List<Tile> SpawnPointsTeam2;

        public TileMap(int width, int height, string data)
        {
            Width = width;
            Height = height;

            SpawnPoints = new List<Tile>();
            SpawnPointsTeam1 = new List<Tile>();
            SpawnPointsTeam2 = new List<Tile>();

            char[] chars = data.ToCharArray();
            int idx = 0;

            Tiles = new Tile[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Tiles[i, j] = new Tile(chars[idx], TileToLogic(j), TileToLogic(i));
                    if (chars[idx] == '1')
                    {
                        SpawnPoints.Add(Tiles[i, j]);
                        SpawnPointsTeam1.Add(Tiles[i, j]);
                    }
                    else if (chars[idx] == '2')
                    {
                        SpawnPoints.Add(Tiles[i, j]);
                        SpawnPointsTeam2.Add(Tiles[i, j]);
                    }

                    idx++;
                }
            }
        }

        public Tile GetTile(int x, int y, bool isTile = false)
        {
            if (!isTile)
            {
                x = LogicToTile(x);
                y = LogicToTile(y);
            }

            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Tiles[y, x];
            }

            return null;
        }

        public static int LogicToTile(int logicValue)
        {
            return logicValue / 300;
        }

        public static int TileToLogic(int tile)
        {
            return 300 * tile + 150;
        }

        internal Tile[,] GetTiles()
        {
            return Tiles;
        }
    }
}
