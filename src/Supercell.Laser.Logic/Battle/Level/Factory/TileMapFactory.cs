using System.IO;

namespace Supercell.Laser.Logic.Battle.Level.Factory
{
    using Supercell.Laser.Titan.Json;

    public static class TileMapFactory
    {
        private static LogicJSONObject jsonObject = LogicJSONParser.ParseObject(File.ReadAllText("Assets/tilemaps.json"));

        public static TileMap CreateTileMap(string mapName)
        {
            LogicJSONObject obj = jsonObject.GetJSONObject(mapName);

            return new TileMap(obj.GetJSONNumber("WIDTH").GetIntValue(),
                                    obj.GetJSONNumber("HEIGHT").GetIntValue(),
                                    obj.GetJSONString("Data").GetStringValue());
        }
    }
}
