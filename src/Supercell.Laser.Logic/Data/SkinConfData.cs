namespace Supercell.Laser.Logic.Data
{
    public class SkinConfData : LogicData
    {
        public SkinConfData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string Character { get; set; }

        public string Model { get; set; }

        public string GadgetModel { get; set; }

        public string PortraitCameraFile { get; set; }

        public string IntroCameraFile { get; set; }

        public bool MirrorIntro { get; set; }

        public string IdleAnim { get; set; }

        public string WalkAnim { get; set; }

        public string PrimarySkillAnim { get; set; }

        public string SecondarySkillAnim { get; set; }

        public string PrimarySkillRecoilAnim { get; set; }

        public string PrimarySkillRecoilAnim2 { get; set; }

        public string SecondarySkillRecoilAnim { get; set; }

        public string SecondarySkillRecoilAnim2 { get; set; }

        public string ReloadingAnim { get; set; }

        public string PushbackAnim { get; set; }

        public string HappyAnim { get; set; }

        public string HappyLoopAnim { get; set; }

        public string SadAnim { get; set; }

        public string SadLoopAnim { get; set; }

        public string HeroScreenIdleAnim { get; set; }

        public string HeroScreenAnim { get; set; }

        public string HeroScreenLoopAnim { get; set; }

        public string SignatureAnim { get; set; }

        public string HappyEnterAnim { get; set; }

        public string SadEnterAnim { get; set; }

        public string ProfileAnim { get; set; }

        public string IntroAnim { get; set; }

        public string BossAutoAttackAnim { get; set; }

        public string BossAutoAttackRecoilAnim { get; set; }

        public string BossAutoAttackRecoilAnim2 { get; set; }

        public string GadgetActiveAnim { get; set; }

        public string GadgetRecoilAnim { get; set; }

        public string IdleFace { get; set; }

        public string WalkFace { get; set; }

        public string HappyFace { get; set; }

        public string HappyLoopFace { get; set; }

        public string SadFace { get; set; }

        public string SadLoopFace { get; set; }

        public string HeroScreenIdleFace { get; set; }

        public string HeroScreenFace { get; set; }

        public string HeroScreenLoopFace { get; set; }

        public string SignatureFace { get; set; }

        public string ProfileFace { get; set; }

        public string IntroFace { get; set; }

        public string HappyEffect { get; set; }

        public string SadEffect { get; set; }

        public bool PetInSameSprite { get; set; }

        public bool AttackLocksAttackAngle { get; set; }

        public bool UltiLocksAttackAngle { get; set; }

        public string MainAttackProjectile { get; set; }

        public string UltiProjectile { get; set; }

        public string MainAttackEffect { get; set; }

        public string UltiAttackEffect { get; set; }

        public bool UseBlueTextureInMenus { get; set; }

        public string MainAttackUseEffect { get; set; }

        public string UltiUseEffect { get; set; }

        public string UltiEndEffect { get; set; }

        public string MeleeHitEffect { get; set; }

        public string SpawnEffect { get; set; }

        public string UltiLoopEffect { get; set; }

        public string UltiLoopEffect2 { get; set; }

        public string AreaEffect { get; set; }

        public string AreaEffectStarPower { get; set; }

        public string SpawnedItem { get; set; }

        public string KillCelebrationSoundVO { get; set; }

        public string InLeadCelebrationSoundVO { get; set; }

        public string StartSoundVO { get; set; }

        public string UseUltiSoundVO { get; set; }

        public string TakeDamageSoundVO { get; set; }

        public string DeathSoundVO { get; set; }

        public string AttackSoundVO { get; set; }

        public string BoneEffect1 { get; set; }

        public string BoneEffect2 { get; set; }

        public string BoneEffect3 { get; set; }

        public string BoneEffect4 { get; set; }

        public string BoneEffectUse { get; set; }

        public string AutoAttackProjectile { get; set; }

        public string ProjectileForShockyStarPower { get; set; }

        public string IncendiaryStarPowerAreaEffect { get; set; }

        public string MoveEffect { get; set; }

        public string StillEffect { get; set; }

        public string ChargedShotEffect { get; set; }

        public bool DisableHeadRotation { get; set; }
    }
}
