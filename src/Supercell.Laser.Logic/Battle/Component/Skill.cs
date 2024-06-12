namespace Supercell.Laser.Logic.Battle.Component
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public class Skill
    {
        public readonly int SkillId;
        public SkillData SkillData => DataTables.Get(DataType.Skill).GetDataByGlobalId<SkillData>(SkillId);

        public readonly bool IsUltiSkill;

        private readonly int MaxCharge;
        private int Charge;

        public int X { get; private set; }
        public int Y { get; private set; }

        public int ActiveTime { get; private set; }
        private int TicksActive;

        public bool IsRapidSpreadPattern 
        {   
            get
            {
                return SkillData.AttackPattern == 6;
            }
        }

        public readonly int[] ATTACK_PATTERN_TABLE;

        public Skill(int globalId, bool isUltiSkill)
        {
            SkillId = globalId;

            int maxChargeCount = SkillData.MaxCharge;
            MaxCharge = LogicMath.Max(1000, 1000 * maxChargeCount);
            Charge = MaxCharge;

            TicksActive = -1;

            if (SkillData.AttackPattern == 6)
            {
                ATTACK_PATTERN_TABLE = new int[4]; // столик

                int p = 0;
                int sp = -(SkillData.Spread / 6) * 2;
                for (int i = 0; i < 4; i++)
                {
                    if (i >= 2)
                    {
                        if (i == 2)
                        {
                            ATTACK_PATTERN_TABLE[i] = sp;
                        }
                        else
                        {
                            ATTACK_PATTERN_TABLE[i] = -sp;
                        }
                    }
                    else
                    {
                        ATTACK_PATTERN_TABLE[i] = p;
                        p += SkillData.Spread / 6;
                    }
                }
            }
            else
            {
                ATTACK_PATTERN_TABLE = new int[0];
            }
        }

        public bool IsActive
        {
            get
            {
                return TicksActive >= 0 && TicksActive < ActiveTime+1;
            }
        }

        public bool ShouldEndThisTick
        {
            get
            {
                return TicksActive >= ActiveTime;
            }
        }

        public bool HasEnoughCharge()
        {
            return Charge >= 1000;
        }

        public void Interrupt()
        {
            TicksActive = -1;
        }

        public void Tick()
        {
            if (Charge < MaxCharge && !IsActive && SkillData.MaxCharge != 0)
            {
                Charge += 1000 / (SkillData.RechargeTime / 50);
                Charge = LogicMath.Min(MaxCharge, Charge);
            }

            if (IsActive)
            {
                TicksActive++;
            }
        }

        public void Activate(Character character, int x, int y, TileMap tileMap)
        {
            if (Charge < 1000 && SkillData.MaxCharge != 0) return;

            Charge -= 1000;

            if (IsUltiSkill)
            {
                character.InterruptAllSkills();
            }

            X = x;
            Y = y;

            TicksActive = 1;
            if (SkillData.BehaviorType != "Charge")
            {
                ActiveTime = (SkillData.ActiveTime / 50) - 1;
            }
            else
            {
                ActiveTime = (SkillData.CastingRange / 2);
            }

            character.BlockHealthRegen();
        }

        public bool ShouldAttackThisTick()
        {
            if (TicksActive == 0)
            {
                return SkillData.ExecuteFirstAttackImmediately;
            }
            return (TicksActive % (SkillData.MsBetweenAttacks / 50)) == 0;
        }

        public void Encode(BitStream bitStream)
        {
            bitStream.WritePositiveVIntMax255OftenZero(TicksActive >= 0 && IsActive ? TicksActive+1 : 0); // 0x15a048
            bitStream.WriteBoolean(false); // 0x15a05c
            bitStream.WritePositiveVIntMax255OftenZero(0); // 0x15a068
            if (SkillData.MaxCharge >= 1)
            {
                bitStream.WritePositiveIntMax4095(Charge); // 0x15a09c
            }

            if (SkillData.SkillCanChange)
            {
                int instanceId = SkillData.GetInstanceId();
                bitStream.WritePositiveIntMax255(instanceId);
            }
        }

        public int GetSkillRangeAddFromHold(int ticks)
        {
            if (SkillData.AttackPattern != 13)
                return 0;

            return LogicMath.Min(ticks / 2, Character.MAX_SKILL_HOLD_TICKS / 2);
        }

        public int GetMaxCharge()
        {
            return MaxCharge;
        }

        public bool HasButton()
        {
            return true;
        }

        public bool IsOkToReduceCooldown()
        {
            return true;
        }

        public int MaxCooldown()
        {
            return SkillData.Cooldown;
        }
    }
}
