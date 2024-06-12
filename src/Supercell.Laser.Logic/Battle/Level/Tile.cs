namespace Supercell.Laser.Logic.Battle.Level
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;

    public class Tile
    {
        public readonly char Code;
        public readonly int X, Y;

        public readonly TileData Data;

        private bool Destructed;

        public static int TileCodeToInstanceId(char code)
        {
            switch (code)
            {

                default:
                    return -1;
            }
        }

        public Tile(char code, int x, int y)
        {
            Code = code;
            X = x;
            Y = y;

            foreach (LogicData data in DataTables.Get(DataType.Tile).Datas)
            {
                TileData tileData = data as TileData;
                if (tileData != null)
                {
                    if (tileData.TileCode[0] == code)
                    {
                        Data = tileData;
                        break;
                    }
                }
            }

            if (Data == null)
            {
                Data = DataTables.Get(DataType.Tile).GetData<TileData>(0);
            }
        }

        public void Destruct()
        {
            Destructed = true;
        }

        public bool IsDestructed()
        {
            return Destructed;
        }

        public int GetCheckSum()
        {
            return 0;
        }
    }
}
