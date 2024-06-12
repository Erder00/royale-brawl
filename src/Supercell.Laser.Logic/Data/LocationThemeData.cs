namespace Supercell.Laser.Logic.Data
{
    public class LocationThemeData : LogicData
    {
        public LocationThemeData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string TileSetPrefix { get; set; }

        public string MaskedEnvironmentSCW { get; set; }

        public string Blocking1SCW { get; set; }

        public string Blocking1Mesh { get; set; }

        public int Blocking1AngleStep { get; set; }

        public string Blocking2SCW { get; set; }

        public string Blocking2Mesh { get; set; }

        public int Blocking2AngleStep { get; set; }

        public string Blocking3SCW { get; set; }

        public string Blocking3Mesh { get; set; }

        public int Blocking3AngleStep { get; set; }

        public string Blocking4SCW { get; set; }

        public string Blocking4Mesh { get; set; }

        public int Blocking4AngleStep { get; set; }

        public string RespawningWallSCW { get; set; }

        public string RespawningWallMesh { get; set; }

        public int RespawningWallAngleStep { get; set; }

        public string RespawningForestSCW { get; set; }

        public string ForestSCW { get; set; }

        public string DestructableSCW { get; set; }

        public string DestructableMesh { get; set; }

        public int DestructableAngleStep { get; set; }

        public string DestructableSCW_CN { get; set; }

        public string DestructableMesh_CN { get; set; }

        public int DestructableAngleStep_CN { get; set; }

        public string FragileSCW { get; set; }

        public string FragileMesh { get; set; }

        public int FragileAngleStep { get; set; }

        public string FragileSCW_CN { get; set; }

        public string FragileMesh_CN { get; set; }

        public int FragileAngleStep_CN { get; set; }

        public string WaterTileSCW { get; set; }

        public string FenceSCW { get; set; }

        public string IndestructibleSCW { get; set; }

        public string IndestructibleMesh { get; set; }

        public string BenchSCW { get; set; }

        public string LaserBallSkinOverride { get; set; }

        public string MineGemSpawnSCWOverride { get; set; }

        public string LootBoxSkinOverride { get; set; }

        public string ShowdownBoostSCWOverride { get; set; }

        public int MapPreviewBGColorRed { get; set; }

        public int MapPreviewBGColorGreen { get; set; }

        public int MapPreviewBGColorBlue { get; set; }

        public string MapPreviewGemGrabSpawnHoleExportName { get; set; }

        public string MapPreviewBallExportName { get; set; }

        public string MapPreviewGoal1ExportName { get; set; }

        public string MapPreviewGoal2ExportName { get; set; }

        public string MapPreviewCNOverrides { get; set; }
    }
}
