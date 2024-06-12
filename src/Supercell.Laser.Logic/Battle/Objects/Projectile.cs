namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public class Projectile : GameObject
    {
        public ProjectileData ProjectileData => DataTables.Get(DataType.Projectile).GetDataByGlobalId<ProjectileData>(DataId);

        private List<int> AlreadyDamagedObjectsGlobalIds;

        private GameObject Source;
        private int Angle;
        private int Damage;
        private int CastingTime;

        private int TicksActive;
        private bool ShouldDestructImmediately;

        private bool IsUltiWeapon;

        private LogicVector2 TargetPosition;

        private int m_destroyedTicks;

        private CharacterData SummonedCharacter;

        public int MaxRange;

        public Projectile(int classId, int instanceId) : base(classId, instanceId)
        {
            TicksActive = -1;
            FullTravelTicks = -1;
            Z = 500;

            TargetPosition = new LogicVector2();
            AlreadyDamagedObjectsGlobalIds = new List<int>();
        }

        private int m_totalDelta;

        public override void Tick()
        {
            if (IsDestroyed())
            {
                if (m_destroyedTicks < 1)
                {
                    TargetReached();
                }

                m_destroyedTicks++;
                return;
            }

            if (!ProjectileData.Indirect)
            {
                if (m_totalDelta > CastingTime * 180) return;
            }

            int deltaX = (int)(((float)LogicMath.Cos(Angle) / 20000) * ProjectileData.Speed);
            int deltaY = (int)(((float)LogicMath.Sin(Angle) / 20000) * ProjectileData.Speed);

            m_totalDelta += ProjectileData.Speed / 20;

            Position.X += deltaX;
            Position.Y += deltaY;

            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();

            Tile tile = tileMap.GetTile(TileMap.LogicToTile(Position.X), TileMap.LogicToTile(Position.Y), true);
            if (tile == null)
            {
                ShouldDestructImmediately = true;
                return;
            }

            if (!ProjectileData.Indirect)
            {
                if (!tile.IsDestructed() && tile.Data.BlocksProjectiles && !(tile.Data.IsDestructibleNormalWeapon || (tile.Data.IsDestructible && IsUltiWeapon)))
                {
                    ShouldDestructImmediately = true;
                }
                else if (tile.Data.IsDestructibleNormalWeapon)
                {
                    tile.Destruct();
                }
                else if (tile.Data.IsDestructible && IsUltiWeapon)
                {
                    tile.Destruct();
                }
            }

            if (Position.X <= 250 || Position.Y <= 250) ShouldDestructImmediately = true;

            if (!ProjectileData.Indirect)
            {
                HandleCollisions();
            }

            if (ProjectileData.Indirect)
            {
                if (FullTravelTicks < 0)
                {
                    int distance = Position.GetDistance(TargetPosition);
                    FullTravelTicks = distance / (ProjectileData.Speed / 20);
                    //Console.WriteLine(MaxRange);
                    FullTravelTicks = LogicMath.Min(FullTravelTicks, MaxRange);
                }

                if (TicksActive < (FullTravelTicks / 2))
                {
                    Z += (ProjectileData.Gravity / 20) * (FullTravelTicks - TicksActive);
                   // Console.WriteLine("DELTA X: " + (FullTravelTicks - TicksActive));
                }
                else
                {
                    int tmp = (FullTravelTicks) - TicksActive;
                    int deltaZ = (ProjectileData.Gravity / 20) * (TicksActive - tmp);
                    //Console.WriteLine("DELTA Z: " + deltaZ);
                    if (deltaZ > 0) Z -= deltaZ;
                }

                if (TicksActive >= FullTravelTicks) ShouldDestructImmediately = true;
            }

            if (!GameObjectManager.GetBattle().IsInPlayArea(Position.X, Position.Y))
            {
                ShouldDestructImmediately = true;
                SetForcedInvisible();
            }

            TicksActive++; 
        }

        private int FullTravelTicks;

        public void SetTargetPosition(int x, int y)
        {
            TargetPosition.Set(x, y);
        }

        public void SetSummonedCharacter(CharacterData data)
        {
            SummonedCharacter = data;
        }

        private void HandleCollisions()
        {
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject == null) continue;
                if (gameObject.GetObjectType() != 1) continue;
                if (!gameObject.IsAlive()) continue;
                if (AlreadyDamagedObjectsGlobalIds.Contains(gameObject.GetGlobalID())) continue;

                int teamIndex = gameObject.GetIndex() / 16;
                if (teamIndex == this.GetIndex() / 16) continue;

                int radius1 = gameObject.GetRadius();
                int radius2 = this.GetRadius();

                if (Position.GetDistance(gameObject.GetPosition()) <= radius1 + radius2)
                {
                    // Collision!
                    if (!ProjectileData.PiercesCharacters) ShouldDestructImmediately = true;

                    AlreadyDamagedObjectsGlobalIds.Add(gameObject.GetGlobalID());

                    Character character = (Character)gameObject;
                    if (ProjectileData.Name != "RocketGirlProjectile") character.CauseDamage((Character)Source, Damage);
                    if (ProjectileData.PoisonType != 0)
                    {
                        character.AddPoison((Character)Source, ((int)(((float)ProjectileData.PoisonDamagePercent / 100) * (float)Damage)), 4, ProjectileData.PoisonType, false);
                    }

                    return;
                }
            }
        }

        private void TargetReached()
        {
            if (ProjectileData.SpawnAreaEffectObject != null)
            {
                CreateAreaEffect(ProjectileData.SpawnAreaEffectObject);
            }
            if (ProjectileData.SpawnAreaEffectObject2 != null)
            {
                CreateAreaEffect(ProjectileData.SpawnAreaEffectObject2);
            }
            if (SummonedCharacter != null)
            {
                Character character = new Character(16, SummonedCharacter.GetInstanceId());
                character.SetPosition(GetX(), GetY(), 0);
                character.SetIndex(Source.GetIndex());
                GameObjectManager.AddGameObject(character);
            }
        }

        public override void OnDestruct()
        {
            ;
        }

        private void CreateAreaEffect(string name)
        {
            AreaEffectData data = DataTables.Get(DataType.AreaEffect).GetData<AreaEffectData>(name);

            AreaEffect effect = new AreaEffect(17, data.GetInstanceId());
            effect.SetPosition(GetX(), GetY(), 0);
            effect.SetIndex(GetIndex());
            effect.SetDamage(Damage);
            effect.SetSource((Character)Source);
            
            GameObjectManager.AddGameObject(effect);
        }

        private bool IsDestroyed()
        {
            return ((m_totalDelta > CastingTime * 180 && !ProjectileData.Indirect) || ShouldDestructImmediately);
        }

        public override bool ShouldDestruct()
        {
            return ((m_totalDelta > CastingTime * 180 && !ProjectileData.Indirect) || ShouldDestructImmediately) && m_destroyedTicks > 2;
        }

        public override void Encode(BitStream bitStream, bool isOwnObject, int visionTeam)
        {
            base.Encode(bitStream, isOwnObject, visionTeam);

            int effect = 0;
            if (m_totalDelta > CastingTime * 180 && !ProjectileData.Indirect) effect = 1;
            if (ShouldDestructImmediately) effect = 3;

            bitStream.WritePositiveIntMax7(effect); // next effect
            bitStream.WriteBoolean(ProjectileData.IsBouncing);
            if (ProjectileData.IsBouncing)
            {
                bitStream.WritePositiveIntMax1023(0);
            }

            if (ProjectileData.TriggerWithDelayMs != 0 || ProjectileData.PreExplosionTimeMs != 0)
                bitStream.WritePositiveVIntMax65535(0);

            if (ProjectileData.PreExplosionTimeMs != 0)
            {
                bitStream.WritePositiveVIntMax65535(0);
                bitStream.WritePositiveVIntMax65535(0);
            }

            bitStream.WritePositiveIntMax1023(0); // Total path

            if (ProjectileData.Rendering != "DoNotRotateClip")
                bitStream.WritePositiveIntMax511(Angle);

            bitStream.WriteBoolean(false);
        }

        public void ShootProjectile(int angle, GameObject owner, int damage, int castingTime, bool isUlti)
        {
            TicksActive = 0;
            Angle = angle;
            Damage = damage;
            CastingTime = castingTime;

            Source = owner;
            SetIndex(owner.GetIndex());

            Position.X = owner.GetX();
            Position.Y = owner.GetY();

            IsUltiWeapon = isUlti;
        }

        public override int GetObjectType()
        {
            return 2;
        }
    }
}
