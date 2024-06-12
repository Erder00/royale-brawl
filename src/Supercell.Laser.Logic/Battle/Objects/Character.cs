namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Component;
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public class Character : GameObject
    {
        public const int MAX_SKILL_HOLD_TICKS = 15;
        public const int INTRO_TICKS = 120;

        private int m_hitpoints;
        private int m_maxHitpoints;

        private int m_state;
        private int m_angle;
        private bool m_isMoving;
        private bool m_usingUltiCurrently;
        private LogicVector2 m_movementDestination;

        private int m_tickWhenHealthRegenBlocked;
        private int m_lastSelfHealTick;

        private bool m_holdingSkill;
        private int m_skillHoldTicksGone;

        private List<Skill> m_skills;

        private int m_itemCount;

        private int m_heroLevel;

        private bool m_isBot;
        private int m_ticksSinceBotEnemyCheck = 100;
        private int m_lastAIAttackTick;

        private Character m_closestEnemy;
        private LogicVector2 m_closestEnemyPosition;

        private int m_activeChargeType;

        private bool m_isStunned;
        private int m_ticksGoneSinceStunned;

        private int m_damageMultiplier;
        private int m_lastTileDamageTick;

        private List<int> m_damageIndicator;
        private Immunity m_immunity;

        private int m_attackingTicks;

        private Poison m_poison;

        public Character(int classId, int instanceId) : base(classId, instanceId)
        {
            m_damageIndicator = new List<int>();
            m_skills = new List<Skill>();

            m_maxHitpoints = CharacterData.Hitpoints;
            m_hitpoints = m_maxHitpoints;

            m_state = 4;

            if (WeaponSkillData != null)
                m_skills.Add(new Skill(WeaponSkillData.GetGlobalId(), false));
            if (UltimateSkillData != null)
                m_skills.Add(new Skill(UltimateSkillData.GetGlobalId(), true));

            m_activeChargeType = -1;
        }

        public void AddPoison(Character pSource, int pDamage, int pTickCount, int pType, bool pSlowDown)
        {
            if (m_poison != null)
            {
                m_poison.RefreshPoison(pSource, pDamage, pTickCount);

                return;
            }

            m_poison = new Poison(pSource, pSlowDown, pDamage, pTickCount, pType);
        }

        public void SetImmunity(int time, int percentage)
        {
            m_immunity = new Immunity(time, percentage);
        }

        public void SetBot(bool isbot)
        {
            m_isBot = isbot;
        }

        public override void PreTick()
        {
            m_damageIndicator.Clear();
        }

        public CharacterData CharacterData => DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(DataId);
        public SkillData WeaponSkillData => DataTables.Get(DataType.Skill).GetData<SkillData>(CharacterData.WeaponSkill);
        public SkillData UltimateSkillData => DataTables.Get(DataType.Skill).GetData<SkillData>(CharacterData.UltimateSkill);

        public void ApplyItem(Item logicItem)
        {
            if (logicItem.ItemData.Name == "BattleRoyaleBuff")
            {
                if (GameObjectManager.GetBattle().GetGameModeVariation() == 6)
                {
                    m_itemCount++;
                }

                int delta = ((int)(((float)10 / 100) * (float)CharacterData.Hitpoints));
                m_maxHitpoints += delta;
                m_hitpoints = LogicMath.Min(m_hitpoints + delta, m_maxHitpoints);
                m_damageMultiplier++;
            }

            if (logicItem.ItemData.Name == "Money")
            {
                BattlePlayer player = GameObjectManager.GetBattle().GetPlayer(GetGlobalID());
                if (player != null)
                {
                    player.AddScore(1);
                }
            }

            if (logicItem.ItemData.Name == "Point" && GameObjectManager.GetBattle().GetGameModeVariation() == 0)
            {
                BattlePlayer player = GameObjectManager.GetBattle().GetPlayer(GetGlobalID());
                if (player != null)
                {
                    m_itemCount++;
                    player.AddScore(1);
                }
            }
        }

        public override void Tick()
        {
            HandleMoveAndAttack();

            if (m_holdingSkill) m_skillHoldTicksGone++;

            foreach (Skill skill in m_skills)
            {
                skill.Tick();
            }
            if (GameObjectManager.GetBattle().GetTicksGone() > Character.INTRO_TICKS)
            {
                TickTimers();
            }
            TickTile();
            if (CharacterData.IsHero()) TickHeals();

            if (m_attackingTicks < 63) m_attackingTicks++;

            if (GameObjectManager.GetBattle().GetTicksGone() > INTRO_TICKS) TickAI();
        }

        private void TickTimers()
        {
            if (m_immunity != null)
            {
                if (m_immunity.Tick(1))
                {
                    m_immunity.Destruct();
                    m_immunity = null;
                }
            }

            if (m_poison != null) 
            {
                if (m_poison.Tick(this))
                {
                    m_poison.Destruct();
                    m_poison = null;
                }
            }
        }

        private void TickTile()
        {
            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();

            Tile tile = tileMap.GetTile(GetX(), GetY());
            if (tile.Data.HidesHero && !tile.IsDestructed())
            {
                DecrementFadeCounter();
            }
            else
            {
                IncrementFadeCounter();
            }

            int x = TileMap.LogicToTile(GetX());
            int y = TileMap.LogicToTile(GetY());
            if (GameObjectManager.GetBattle().GetTicksGone() - m_lastTileDamageTick > 20)
            {
                if (GameObjectManager.GetBattle().IsTileOnPoisonArea(x, y))
                {
                    m_lastTileDamageTick = GameObjectManager.GetBattle().GetTicksGone();
                    CauseDamage(null, 1000);
                }
            }
        }

        private void StopMovement()
        {
            this.m_isMoving = false;
        }

        private int m_meleeAttackEndTick = -1;
        private Character m_meleeAttackTarget;
        private int m_meleeAttackDamage;

        private void StartMeleeAttack(Character target, int ticks, int damage)
        {
            this.m_meleeAttackTarget = target;
            this.m_attackingTicks = 0;
            this.m_meleeAttackEndTick = GameObjectManager.GetBattle().GetTicksGone() + ticks;
            this.m_meleeAttackDamage = damage;
            this.m_state = 3;
        }

        private Character ShamanPetTarget;
        private void TickAI()
        {
            if (m_isBot)
            {
                TickBot();
                return;
            }

            if (CharacterData.IsHero()) return;

            if (CharacterData.Name == "ShamanPet")
            {
                m_ticksSinceBotEnemyCheck++;

                if (m_ticksSinceBotEnemyCheck > 20)
                {
                    this.ShamanPetTarget = GetClosestEnemy();
                }

                if (this.ShamanPetTarget == null) return;

                if (this.ShamanPetTarget.GetPosition().GetDistance(this.Position) <= 300)
                {
                    this.StopMovement();
                    if (this.m_meleeAttackEndTick < this.GameObjectManager.GetBattle().GetTicksGone())
                    {
                        this.StartMeleeAttack(this.ShamanPetTarget, 10, GetAbsoluteDamage(CharacterData.AutoAttackDamage));
                    }
                }
                else
                {
                    this.MoveTo(this.ShamanPetTarget.GetX(), this.ShamanPetTarget.GetY());
                }
            }

            if (CharacterData.AutoAttackProjectile != null && CharacterData.AutoAttackSpeedMs > 0 && CharacterData.AutoAttackDamage > 0)
            {
                if (GameObjectManager.GetBattle().GetTicksGone() - m_lastAIAttackTick < CharacterData.AutoAttackSpeedMs / 50) return;
                foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                {
                    if (gameObject.GetObjectType() != 1) continue;
                    if (gameObject.GetIndex() / 16 == GetIndex() / 16) continue;
                    if (Position.GetDistance(gameObject.GetPosition()) > 100 * CharacterData.AutoAttackRange) continue;

                    ProjectileData projectileData = DataTables.Get(DataType.Projectile).GetData<ProjectileData>(CharacterData.AutoAttackProjectile);

                    Projectile projectile = new Projectile(6, projectileData.GetInstanceId());
                    projectile.SetPosition(GetX(), GetY(), 200);
                    int angle = LogicMath.GetAngle(gameObject.GetX() - GetX(), gameObject.GetY() - GetY());
                    projectile.ShootProjectile(angle, this, CharacterData.AutoAttackDamage, CharacterData.AutoAttackRange / 2, false);
                    projectile.SetTargetPosition(gameObject.GetX(), gameObject.GetY());

                    GameObjectManager.AddGameObject(projectile);
                    m_lastAIAttackTick = GameObjectManager.GetBattle().GetTicksGone();

                    m_state = 3;
                    m_attackingTicks = 0;
                    m_angle = angle;
                    break;
                }
            }
        }

        private void TickBot()
        {
            m_ticksSinceBotEnemyCheck++;

            if (m_ticksSinceBotEnemyCheck > 60 || m_closestEnemy == null)
            {
                m_ticksSinceBotEnemyCheck = 0;
                Character closestEnemy = GetClosestEnemy();

                if (closestEnemy == null) return;

                m_closestEnemy = closestEnemy;
                m_closestEnemyPosition = closestEnemy.GetPosition();
            }

            if (m_closestEnemy == null) return;

            if (m_ticksSinceBotEnemyCheck % 40 == 0) MoveTo(m_closestEnemyPosition.X, m_closestEnemyPosition.Y);

            if (GameObjectManager.GetBattle().GetTicksGone() - m_lastAIAttackTick <= 20) return;
            Skill weapon = GetWeaponSkill();
            LogicVector2 enemyPosition = m_closestEnemy.GetPosition();
            if (Position.GetDistance(enemyPosition) >= WeaponSkillData.CastingRange * 80) return;
            if (!weapon.HasEnoughCharge()) return;
            m_lastAIAttackTick = GameObjectManager.GetBattle().GetTicksGone();

            int deltaX = enemyPosition.X - Position.X;
            int deltaY = enemyPosition.Y - Position.Y;

            ActivateSkill(false, deltaX, deltaY);
        }

        public Character GetClosestEnemy()
        {
            Character closestEnemy = null;
            int distance = 99999999;

            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 1) continue; // not a character, ignore.

                Character enemy = (Character)gameObject;
                if (enemy == null) continue; // invalid object
                if (enemy.GetIndex() / 16 == GetIndex() / 16) continue; // teammate, ignore.

                int distanceToEnemy = Position.GetDistance(enemy.GetPosition());
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            return closestEnemy;
        }

        private void TickHeals()
        {
            if (m_hitpoints >= m_maxHitpoints) return;

            int ticksGone = GameObjectManager.GetBattle().GetTicksGone();
            if (ticksGone - m_tickWhenHealthRegenBlocked < 60) // 3 seconds
                return;
            if (ticksGone - m_lastSelfHealTick < 20) // 1 second
                return;

            m_lastSelfHealTick = ticksGone;

            int heal = 13 * m_maxHitpoints / 100;
            CauseDamage(this, -heal, false);

            BattlePlayer player = GameObjectManager.GetBattle().GetPlayerWithObject(GetGlobalID());
            if (player != null)
            {
                if (heal > 0)
                {
                    player.Healed(heal);
                }
            }
        }

        public void CauseDamage(Character damageDealer, int damage, bool shouldShow = true)
        {
            try
            {
                if (m_hitpoints <= 0) return;
                if (m_immunity != null)
                {
                    int damageDiff = ((int)(((float)m_immunity.GetImmunityPercentage() / 100) * (float)damage));
                    damage -= damageDiff;
                }

                m_hitpoints -= damage;
                m_hitpoints = LogicMath.Max(m_hitpoints, 0);
                m_hitpoints = LogicMath.Min(m_hitpoints, m_maxHitpoints);

                if (damage > 0) BlockHealthRegen();

                BattleMode battle = GameObjectManager.GetBattle();
                if (shouldShow) m_damageIndicator.Add(damage);

                if (CharacterData.IsHero())
                {
                    if (damageDealer != null && damageDealer != this)
                    {
                        BattlePlayer enemy = battle.GetPlayerWithObject(damageDealer.GetGlobalID());
                        if (enemy != null)
                        {
                            if (damage > 0)
                            {
                                enemy.AddUltiCharge(damageDealer.CharacterData.UltiChargeMul * (damage / 80));
                                enemy.DamageDealed(damage);
                            }
                        }
                    }

                    if (m_hitpoints <= 0)
                    {
                        BattlePlayer player = battle.GetPlayerWithObject(GetGlobalID());

                        if (player != null)
                        {
                            battle.PlayerDied(player);
                        }

                        if (damageDealer != null)
                        {
                            BattlePlayer enemy = battle.GetPlayerWithObject(damageDealer.GetGlobalID());
                            if (GameObjectManager.GetBattle().GetGameModeVariation() == 3)
                            {
                                if (enemy != null)
                                {
                                    damageDealer.AddItemsCollected(1);
                                    enemy.AddScore(m_itemCount + 1);
                                }
                            }

                            if (enemy != null)
                            {
                                int bountyStars = GameObjectManager.GetBattle().GetGameModeVariation() == 3 ? m_itemCount + 1 : 0;
                                enemy.KilledPlayer(GetIndex() % 16, bountyStars);
                            }
                        }

                        if (GameObjectManager.GetBattle().GetGameModeVariation() == 6)
                        {
                            ItemData data = DataTables.Get(18).GetData<ItemData>("BattleRoyaleBuff");
                            Item item = new Item(18, data.GetInstanceId());
                            item.SetPosition(GetX(), GetY(), 0);
                            item.SetAngle(GameObjectManager.GetBattle().GetRandomInt(0, 360));
                            GameObjectManager.AddGameObject(item);
                        }

                        if (GameObjectManager.GetBattle().GetGameModeVariation() == 0)
                        {
                            ItemData data = DataTables.Get(18).GetData<ItemData>("Point");
                            for (int i = 0; i < m_itemCount; i++)
                            {
                                Item item = new Item(18, data.GetInstanceId());
                                item.SetPosition(GetX(), GetY(), 0);
                                item.SetAngle(GameObjectManager.GetBattle().GetRandomInt(0, 360));
                                GameObjectManager.AddGameObject(item);
                            }
                        }
                    }
                }

                if (m_hitpoints <= 0)
                {
                    if (CharacterData.Name == "LootBox")
                    {
                        ItemData data = DataTables.Get(18).GetData<ItemData>("BattleRoyaleBuff");
                        Item item = new Item(18, data.GetInstanceId());
                        item.SetPosition(GetX(), GetY(), 0);
                        item.SetAngle(GameObjectManager.GetBattle().GetRandomInt(0, 360));
                        GameObjectManager.AddGameObject(item);
                    }
                }
            }
            catch (Exception) { }
        }

        public void AddItemsCollected(int a)
        {
            m_itemCount += a;
            if (GameObjectManager.GetBattle().GetGameModeVariation() == 3)
            {
                m_itemCount = LogicMath.Min(6, m_itemCount);
            }
        }

        public void ResetItemsCollected()
        {
            m_itemCount = 0;
        }

        public void HoldSkillStarted()
        {
            if (!m_holdingSkill)
            {
                m_holdingSkill = true;
                m_skillHoldTicksGone = 0;
            }
        }

        public void SkillReleased()
        {
            m_holdingSkill = false;
        }

        public Skill GetWeaponSkill()
        {
            return m_skills.Count > 0 ? m_skills[0] : null;
        }

        public Skill GetUltimateSkill()
        {
            return m_skills.Count > 1 ? m_skills[1] : null;
        }

        public void InterruptAllSkills()
        {
            foreach (Skill skill in m_skills)
            {
                skill.Interrupt();
            }
        }

        public void BlockHealthRegen()
        {
            m_tickWhenHealthRegenBlocked = GameObjectManager.GetBattle().GetTicksGone();
        }

        public void ActivateSkill(bool isUlti, int x, int y)
        {
            m_state = 3;

            Skill skill = isUlti ? GetUltimateSkill() : GetWeaponSkill();
            if (skill == null) return;
            if (skill.IsActive) return;
            if (skill.SkillData.BehaviorType == "Charge") return;

            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();
            m_angle = LogicMath.GetAngle(x, y);
            skill.Activate(this, x, y, tileMap);
            m_attackingTicks = 0;

            if (skill.SkillData.ChargeType == 3)
            {
                LogicVector2 Destination = new LogicVector2(GetX() + x, GetY() + y);
                JumpChargeDestination = Destination;
                int distance = Position.GetDistance(Destination);
                ChargeTime = distance / 50;
            }

            if (!string.IsNullOrEmpty(skill.SkillData.AreaEffectObject))
            {
                AreaEffectData effectData = DataTables.Get(17).GetData<AreaEffectData>(skill.SkillData.AreaEffectObject);
                AreaEffect effect = new AreaEffect(17, effectData.GetInstanceId());
                effect.SetPosition(GetX(), GetY(), 0);
                effect.SetSource(this);
                effect.SetIndex(GetIndex());
                effect.SetDamage(skill.SkillData.Damage);
                GameObjectManager.AddGameObject(effect);
            }
        }

        private LogicVector2 JumpChargeDestination;
        private int ChargeTime;

        // TODO: refactor
        private int GetBulletAngle(int n, int spread, int numBullets)
        {
            if (spread != 0)
            {
                int d = (-spread / 2) / 2;
                for (int i = 0; i < n; i++)
                {
                    d += (spread / 2) / numBullets;
                }
                return d;
            }
            else
            {
                int d = (-spread / 2) / 2;
                for (int i = 0; i < n; i++)
                {
                    d += 4;
                }
                return d;
            }
        }

        private int SpreadIndex = 0;

        private void Attack(int x, int y, int range, ProjectileData projectileData, int damage, int spread, int bulletsPerShot, Skill skill)
        {
            int originAngle = LogicMath.GetAngle(x, y);
            SetForcedVisible();

            if (m_holdingSkill)
            {
                if (m_skillHoldTicksGone > 14) bulletsPerShot = 1;
                else if (m_skillHoldTicksGone > 5) bulletsPerShot = 3;
            }

            for (int i = 0; i < bulletsPerShot; i++)
            {
                Projectile projectile = new Projectile(6, projectileData.GetInstanceId());
                projectile.MaxRange = skill.SkillData.CastingRange;

                int newRange = range / 2;

                if (m_holdingSkill)
                    newRange += skill.GetSkillRangeAddFromHold(m_skillHoldTicksGone);

                int a = LogicMath.Min(m_skillHoldTicksGone, MAX_SKILL_HOLD_TICKS) / 3;

                projectile.SetTargetPosition(GetX() + x, GetY() + y);

                if (!skill.IsRapidSpreadPattern)
                {
                    projectile.ShootProjectile(originAngle + GetBulletAngle(i, spread, bulletsPerShot) / (a != 0 ? a : 1), this, GetAbsoluteDamage(damage), newRange+1, skill == GetUltimateSkill());
                }
                else
                {
                    projectile.ShootProjectile(originAngle + skill.ATTACK_PATTERN_TABLE[SpreadIndex] / (a != 0 ? a : 1), this, GetAbsoluteDamage(damage), newRange+1, skill == GetUltimateSkill());
                    SpreadIndex++;
                    if (SpreadIndex >= skill.ATTACK_PATTERN_TABLE.Length) SpreadIndex = 0;
                }

                if (skill.SkillData.SummonedCharacter != null)
                {
                    CharacterData summonedCharacter = DataTables.Get(DataType.Character).GetData<CharacterData>(skill.SkillData.SummonedCharacter);
                    if (summonedCharacter != null)
                    {
                        projectile.SetSummonedCharacter(summonedCharacter);
                    }
                }

                GameObjectManager.AddGameObject(projectile);
            }
            m_holdingSkill = false;
        }

        public override bool ShouldDestruct()
        {
            return m_hitpoints <= 0;
        }

        public bool IsChargeActive()
        {
            return m_activeChargeType >= 0;
        }

        public int GetHitpointPercentage()
        {
            return (int)(((float)this.m_hitpoints / (float)this.m_maxHitpoints) * 100f);
        }

        public bool HasActiveSkill()
        {
            if (m_skills.Count == 0) return false;
            if (m_skills.Count == 1) return m_skills[0].IsActive;
            else return m_skills[0].IsActive || m_skills[1].IsActive;
        }

        private void HandleMoveAndAttack()
        {
            if (!HasActiveSkill())
                m_attackingTicks = 63;

            if (m_isStunned)
            {
                m_ticksGoneSinceStunned++;
                if (m_ticksGoneSinceStunned > 40)
                {
                    m_isStunned = false;
                }
                return;
            }

            foreach (GameObject obj in GameObjectManager.GetGameObjects())
            {
                if (Position.GetDistance(obj.GetPosition()) <= 200 && obj.GetIndex() / 16 != GetIndex() / 16)
                {
                    obj.SetForcedVisible();
                }

                if (CharacterData.IsHero())
                {
                    if (obj.GetObjectType() == 4)
                    {
                        Item item = (Item)obj;
                        if (Position.GetDistance(item.GetPosition()) < 350 && item.CanBePickedUp())
                        {
                            item.PickUp(this);
                        }
                    }
                }
            }

            if (this.m_meleeAttackEndTick == this.GameObjectManager.GetBattle().GetTicksGone())
            {
                if (this.m_meleeAttackTarget != null)
                    this.m_meleeAttackTarget.CauseDamage(null, this.m_meleeAttackDamage);
            }

            // Handle Attack
            foreach (Skill skill in m_skills)
            {
                if (!skill.IsActive) continue;

                if (skill.SkillData.BehaviorType == "Attack")
                {
                    if (!skill.ShouldAttackThisTick()) continue;

                    ProjectileData projectileData = DataTables.Get(DataType.Projectile).GetData<ProjectileData>(skill.SkillData.Projectile);
                    int damage = skill.SkillData.Damage;
                    int spread = skill.SkillData.Spread;
                    int bulletsPerShot = skill.SkillData.NumBulletsInOneAttack;

                    this.Attack(skill.X, skill.Y, skill.SkillData.CastingRange, projectileData, damage, spread, bulletsPerShot, skill);
                }
                else if (skill.SkillData.BehaviorType == "Charge")
                {
                    if (GamePlayUtil.IsJumpCharge(skill.SkillData.ChargeType))
                    {
                        // не придумали
                        return;
                    }

                    m_activeChargeType = skill.SkillData.ChargeType;

                    int dx = LogicMath.Cos(m_angle) / 100;
                    int dy = LogicMath.Sin(m_angle) / 100;

                    dx *= skill.SkillData.ChargeSpeed / 80;
                    dy *= skill.SkillData.ChargeSpeed / 80;

                    if (!GameObjectManager.GetBattle().IsInPlayArea(Position.X + dx, Position.Y + dy))
                    {
                        skill.Interrupt();
                        m_activeChargeType = -1;
                        return;
                    }

                    Tile tile = GameObjectManager.GetBattle().GetTileMap().GetTile(Position.X + dx, Position.Y + dy);

                    if (tile == null)
                    {
                        skill.Interrupt();
                        m_activeChargeType = -1;
                        return;
                    }

                    if (tile.Data.BlocksMovement && !tile.IsDestructed())
                    {
                        if (tile.Data.IsDestructible)
                        {
                            tile.Destruct();
                        }
                        else
                        {
                            skill.Interrupt();
                            m_activeChargeType = -1;
                            return;
                        }
                    }

                    Position.X += dx;
                    Position.Y += dy;

                    if (skill.ShouldEndThisTick) m_activeChargeType = -1;
                }
                else
                {
                    Debugger.Warning("Unknown skill type: " + skill.SkillData.BehaviorType);
                }
            }

            // Handle Move
            if (m_isMoving && !IsChargeActive())
            {
                if (Position.GetDistance(m_movementDestination) != 0)
                {

                    int angle = m_angle;
                    int initialDestX = m_movementDestination.X;
                    int initialDestY = m_movementDestination.Y;
                    bool isBot = this.m_isBot || !CharacterData.IsHero();
                    if (isBot)
                    {
                        while (CheckObstacle(15))
                        {
                            m_movementDestination.X = initialDestX;
                            m_movementDestination.Y = initialDestY;

                            m_movementDestination.X = Position.X;
                            m_movementDestination.Y = Position.Y;

                            angle += 2;

                            m_movementDestination.X += LogicMath.Cos(angle);
                            m_movementDestination.Y += LogicMath.Sin(angle);
                        }
                    }
                    else
                    {
                        if (CheckObstacle(1))
                            this.StopMovement();
                    }
                    m_angle = LogicMath.NormalizeAngle360(angle);

                    int movingSpeed = CharacterData.Speed / 20;

                    int deltaX;
                    int deltaY;

                    if (m_movementDestination.X - Position.X != 0)
                    {
                        if (m_movementDestination.X - Position.X > 0) deltaX = LogicMath.Min(movingSpeed, m_movementDestination.X - Position.X);
                        else deltaX = LogicMath.Max(-movingSpeed, m_movementDestination.X - Position.X);

                        Position.X += deltaX;
                    }
                    if (m_movementDestination.Y - Position.Y != 0)
                    {
                        if (m_movementDestination.Y - Position.Y > 0) deltaY = LogicMath.Min(movingSpeed, m_movementDestination.Y - Position.Y);
                        else deltaY = LogicMath.Max(-movingSpeed, m_movementDestination.Y - Position.Y);

                        Position.Y += deltaY;
                    }
                }

                m_isMoving = Position.GetDistance(m_movementDestination) != 0;
                if (!m_isMoving)
                {
                    m_state = 4;
                }
            }
        }

        private bool CheckObstacle(int nextTiles)
        {
            int movingSpeed = CharacterData.Speed / 20;
            int deltaX;
            int deltaY;

            int newX = this.Position.X;
            int newY = this.Position.Y;

            for (int i = 0; i < nextTiles; i++)
            {
                if (m_movementDestination.X - Position.X > 0) deltaX = LogicMath.Min(movingSpeed, m_movementDestination.X - Position.X);
                else deltaX = LogicMath.Max(-movingSpeed, m_movementDestination.X - Position.X);

                if (m_movementDestination.Y - Position.Y > 0) deltaY = LogicMath.Min(movingSpeed, m_movementDestination.Y - Position.Y);
                else deltaY = LogicMath.Max(-movingSpeed, m_movementDestination.Y - Position.Y);

                newX += deltaX;
                newY += deltaY;

                if (!GameObjectManager.GetBattle().IsInPlayArea(newX, newY)) return true;

                Tile nextTile = GameObjectManager.GetBattle().GetTileMap().GetTile(newX, newY);
                if (nextTile == null) return true;
                if (nextTile.Data.BlocksMovement && !nextTile.IsDestructed()) return true;
            }

            return false;
        }

        public void UltiEnabled()
        {
            m_usingUltiCurrently = true;
        }

        public void UltiDisabled()
        {
            m_usingUltiCurrently = false;
        }

        public override bool IsAlive()
        {
            return m_hitpoints > 0;
        }

        public override int GetRadius()
        {
            return CharacterData.CollisionRadius;
        }

        public void SetHeroLevel(int level)
        {
            m_heroLevel = level;
            m_maxHitpoints = CharacterData.Hitpoints + ((int)(((float)5 / 100) * (float)CharacterData.Hitpoints)) * level;
            m_hitpoints = m_maxHitpoints;
            m_damageMultiplier = level;
        }

        public int GetHeroLevel()
        {
            return m_heroLevel;
        }

        public int GetNormalWeaponDamage()
        {
            return WeaponSkillData.Damage + ((int)(((float)5 / 100) * (float)WeaponSkillData.Damage)) * (m_heroLevel + m_damageMultiplier);
        }

        public int GetAbsoluteDamage(int damage)
        {
            return damage + ((int)(((float)5 / 100) * (float)damage)) * (m_heroLevel + m_damageMultiplier);
        }

        public void MoveTo(int x, int y)
        {
            if (!GameObjectManager.GetBattle().IsInPlayArea(x, y)) return;
            if (IsChargeActive()) return;

            m_isMoving = true;
            if (m_attackingTicks >= 63) m_state = 1;
            m_movementDestination = new LogicVector2(x, y);

            LogicVector2 delta = m_movementDestination.Clone();
            delta.Substract(Position);

            if (!((delta.X < 150 && delta.X > -150) && (delta.Y < 150 && delta.Y > -150)))
            {
                m_angle = LogicMath.GetAngle(delta.X, delta.Y);
            }
        }

        public override void Encode(BitStream bitStream, bool isOwnObject, int visionTeam)
        {
            isOwnObject = isOwnObject && CharacterData.IsHero();
            base.Encode(bitStream, isOwnObject, visionTeam);
            bitStream.WritePositiveInt(visionTeam == this.GetIndex() / 16 ? 10 : GetFadeCounter(), 4);

            if (CharacterData.HasAutoAttack() || CharacterData.Speed != 0 || CharacterData.Type == "Minion_Building_charges_ulti")
            {
                if (isOwnObject)
                {
                    bitStream.WriteBoolean(false); // 0xa1aff8
                    bitStream.WriteBoolean(false);
                    {
                       // bitStream.WritePositiveInt(Angle, 9);
                       // bitStream.WritePositiveInt(Angle, 9);
                    }
                }
                else
                {
                    bitStream.WritePositiveIntMax511(m_angle);
                    bitStream.WritePositiveIntMax511(m_angle);
                }
                bitStream.WritePositiveIntMax7(m_state); // State
                bitStream.WriteBoolean(false); // Coctail used
                bitStream.WriteInt(m_attackingTicks, 6); // Animation Playing

                bitStream.WriteBoolean(false); // дёргает и не rotate
                bitStream.WriteBoolean(false); // Stun
                bitStream.WriteBoolean(false); // unk
                bitStream.WriteBoolean(false); // star power indicator
            }
            else
            {
                bitStream.WritePositiveIntMax7(m_state);
                if (CharacterData.Type == "Train")
                {
                    bitStream.WritePositiveIntMax511(m_angle);
                    bitStream.WritePositiveIntMax511(m_angle);
                }
                else if (CharacterData.AreaEffect != null)
                {
                    bitStream.WritePositiveIntMax511(m_angle);
                }
            }

            bitStream.WritePositiveVIntMax255OftenZero(0); // 0xa1b0d8
            bitStream.WritePositiveVIntMax255OftenZero(0); // 0xa1b0e4
            bitStream.WriteBoolean(false); // Speed up (Arrows up)
            bitStream.WriteBoolean(false); // Slow down

            if (m_poison != null)
            {
                bitStream.WritePositiveInt(m_poison.GetPoisonType(), 2);
                {
                    bitStream.WriteBoolean(m_poison.HasSlowDownEffect());
                }
            }
            else
            {
                bitStream.WritePositiveInt(0, 2);
            }

            if (CharacterData.HasVeryMuchHitPoints() || GameObjectManager.GetBattle().GetGameModeVariation() == 6)
            {
                bitStream.WritePositiveVIntMax65535(m_hitpoints);
                bitStream.WritePositiveVIntMax65535(m_maxHitpoints);
            }
            else
            {
                bitStream.WritePositiveInt(m_hitpoints, 13);
                bitStream.WritePositiveInt(m_maxHitpoints, 13);
            }

            if (CharacterData.IsHero())
            {
                bitStream.WritePositiveVIntMax255OftenZero(m_itemCount);
                bitStream.WritePositiveVIntMax255OftenZero(0);

                bitStream.WriteBoolean(false); // big brawler
                bitStream.WriteBoolean(true);
                {
                    bitStream.WriteBoolean(false);
                    bitStream.WriteBoolean(m_immunity != null); // immunity
                    bitStream.WriteBoolean(false); // когда ходишь дёргает и ещё не rotate
                    bitStream.WriteBoolean(false); // bull rage
                    bitStream.WriteBoolean(m_usingUltiCurrently); // using ulti
                    bitStream.WriteBoolean(false); // ulti activated???? 2
                    bitStream.WriteBoolean(false); // gold shield (unknown?)
                    bitStream.WriteBoolean(false); // unknown?
                }

                if (isOwnObject) bitStream.WriteBoolean(false);

                if (isOwnObject) bitStream.WritePositiveInt(0, 4);

                if (IsChargeActive())
                {
                    bitStream.WritePositiveInt(0, 8);
                    bitStream.WritePositiveInt(0, 4);
                }
            }

            bitStream.WritePositiveInt(0, 2);
            bitStream.WriteBoolean(false); // not fully visible
            bitStream.WritePositiveInt(0, 9);
            if (isOwnObject)
            {
                bitStream.WriteBoolean(false);
                bitStream.WriteBoolean(false);
            }
            bitStream.WritePositiveInt(m_damageIndicator.Count, 5);
            for (int i = 0; i < m_damageIndicator.Count; i++)
            {
                bitStream.WriteInt(m_damageIndicator[i], 15);
            }

            for (int i = 0; i < m_skills.Count; i++)
            {
                m_skills[i].Encode(bitStream);
            }
        }

        public override int GetObjectType()
        {
            return 1;
        }
    }
}
