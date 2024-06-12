namespace Supercell.Laser.Logic.Data
{
    public class ProjectileData : LogicData
    {
        public ProjectileData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string ParentProjectileForSkin { get; set; }

        public int Speed { get; set; }

        public string FileName { get; set; }

        public string BlueSCW { get; set; }

        public string RedSCW { get; set; }

        public string BlueExportName { get; set; }

        public string RedExportName { get; set; }

        public string ShadowExportName { get; set; }

        public string BlueGroundGlowExportName { get; set; }

        public string RedGroundGlowExportName { get; set; }

        public string PreExplosionBlueExportName { get; set; }

        public string PreExplosionRedExportName { get; set; }

        public int PreExplosionTimeMs { get; set; }

        public string HitEffectEnv { get; set; }

        public string HitEffectChar { get; set; }

        public string MaxRangeReachedEffect { get; set; }

        public string CancelEffect { get; set; }

        public int Radius { get; set; }

        public bool Indirect { get; set; }

        public bool ConstantFlyTime { get; set; }

        public int TriggerWithDelayMs { get; set; }

        public int BouncePercent { get; set; }

        public int Gravity { get; set; }

        public int EarlyTicks { get; set; }

        public int HideTime { get; set; }

        public int Scale { get; set; }

        public int RandomStartFrame { get; set; }

        public string SpawnAreaEffectObject { get; set; }

        public string SpawnAreaEffectObject2 { get; set; }

        public string SpawnCharacter { get; set; }

        public string SpawnItem { get; set; }

        public string TrailEffect { get; set; }

        public bool ShotByHero { get; set; }

        public bool IsBeam { get; set; }

        public bool IsBouncing { get; set; }

        public int DistanceAddFromBounce { get; set; }

        public string Rendering { get; set; }

        public bool PiercesCharacters { get; set; }

        public bool PiercesEnvironment { get; set; }

        public bool PiercesEnvironmentLikeButter { get; set; }

        public int PushbackStrength { get; set; }

        public bool VariablePushback { get; set; }

        public bool DirectionAlignedPushback { get; set; }

        public int ChainsToEnemies { get; set; }

        public int ChainBullets { get; set; }

        public int ChainSpread { get; set; }

        public int ChainTravelDistance { get; set; }

        public string ChainBullet { get; set; }

        public int ExecuteChainSpecialCase { get; set; }

        public int DamagePercentStart { get; set; }

        public int DamagePercentEnd { get; set; }

        public int DamagesConstantlyTickDelay { get; set; }

        public int FreezeStrength { get; set; }

        public int FreezeDurationMS { get; set; }

        public int StunLengthMS { get; set; }

        public int ScaleStart { get; set; }

        public int ScaleEnd { get; set; }

        public bool AttractsPet { get; set; }

        public int LifeStealPercent { get; set; }

        public bool PassesEnvironment { get; set; }

        public int PoisonDamagePercent { get; set; }

        public bool DamageOnlyWithOneProj { get; set; }

        public int HealOwnPercent { get; set; }

        public bool CanGrowStronger { get; set; }

        public bool HideFaster { get; set; }

        public bool GrapplesEnemy { get; set; }

        public int KickBack { get; set; }

        public bool UseColorMod { get; set; }

        public int RedAdd { get; set; }

        public int GreenAdd { get; set; }

        public int BlueAdd { get; set; }

        public int RedMul { get; set; }

        public int GreenMul { get; set; }

        public int BlueMul { get; set; }

        public bool GroundBasis { get; set; }

        public int MinDistanceForSpread { get; set; }

        public bool IsFriendlyHomingMissile { get; set; }

        public bool IsBoomerang { get; set; }

        public bool CanHitAgainAfterBounce { get; set; }

        public bool IsHomingMissile { get; set; }

        public bool BlockUltiCharge { get; set; }

        public int PoisonType { get; set; }

        public int TravelType { get; set; }

        public int SteerStrength { get; set; }

        public int SteerIgnoreTicks { get; set; }

        public int HomeDistance { get; set; }

        public int SteerLifeTime { get; set; }
    }
}
