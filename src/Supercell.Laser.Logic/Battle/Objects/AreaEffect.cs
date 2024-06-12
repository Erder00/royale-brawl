namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;

    public class AreaEffect : GameObject
    {
        private Character m_source;

        private int m_ticksElapsed;
        private int m_damage;

        private List<Character> m_alreadyDamagedList;

        public AreaEffect(int classId, int instanceId) : base(classId, instanceId)
        {
            m_alreadyDamagedList = new List<Character>();
        }

        public AreaEffectData EffectData => DataTables.Get(DataType.AreaEffect).GetDataByGlobalId<AreaEffectData>(DataId);

        public override void Tick()
        {
            m_ticksElapsed++;

            if (EffectData.Type == "Damage")
            {
                if (m_damage == 0) m_damage = EffectData.Damage;

                GameObject[] objects = GameObjectManager.GetGameObjects();
                foreach (GameObject gameObject in objects)
                {
                    if (gameObject.GetObjectType() != 1) continue;

                    Character character = (Character)gameObject;
                    if (character == null) continue;

                    if (m_alreadyDamagedList.Contains(character)) continue;
                    if (character.GetIndex() / 16 == m_source.GetIndex() / 16) continue;
                    if (character.GetPosition().GetDistance(Position) > GetRadius()) continue;

                    character.CauseDamage(m_source, m_damage);
                    m_alreadyDamagedList.Add(character);
                }
            }
            else if (EffectData.Type == "BulletExplosion")
            {
                if (m_ticksElapsed == 1)
                {
                    ProjectileData projectileData = DataTables.Get(6).GetData<ProjectileData>(EffectData.BulletExplosionBullet);
                    int a = 0;
                    for (int i = 0; i < EffectData.CustomValue; i++)
                    {
                        Projectile projectile = new Projectile(6, projectileData.GetInstanceId());
                        projectile.ShootProjectile(a, m_source, 400, EffectData.BulletExplosionBulletDistance / 2, false);
                        projectile.SetPosition(GetX(), GetY(), 400);
                        a += 360 / EffectData.CustomValue;
                        GameObjectManager.AddGameObject(projectile);
                    }
                }
            }
            else if (EffectData.Type == "Dot")
            {
                if (m_ticksElapsed % 20 == 0) m_alreadyDamagedList.Clear();

                GameObject[] objects = GameObjectManager.GetGameObjects();
                foreach (GameObject gameObject in objects)
                {
                    if (gameObject.GetObjectType() != 1) continue;

                    Character character = (Character)gameObject;
                    if (character == null) continue;

                    if (m_alreadyDamagedList.Contains(character)) continue;
                    if (character.GetIndex() / 16 == m_source.GetIndex() / 16) continue;
                    if (character.GetPosition().GetDistance(Position) > GetRadius()) continue;

                    character.CauseDamage(m_source, m_damage);
                    m_alreadyDamagedList.Add(character);
                }
            }
        }

        public override void Encode(BitStream bitStream, bool isOwnObject, int visionTeam)
        {
            base.Encode(bitStream, isOwnObject, visionTeam);

            bitStream.WritePositiveInt(GetFadeCounter(), 4);
            bitStream.WritePositiveIntMax127(0);
        }

        public void SetSource(Character source)
        {
            m_source = source;
        }

        public void SetDamage(int damage)
        {
            m_damage = damage;
        }

        public override int GetRadius()
        {
            return EffectData.Radius;
        }

        public override bool ShouldDestruct()
        {
            return m_ticksElapsed >= EffectData.TimeMs / 50;
        }

        public override int GetObjectType()
        {
            return 3;
        }
    }
}
