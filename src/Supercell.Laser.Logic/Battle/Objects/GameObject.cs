namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class GameObject
    {
        protected int DataId;
        protected GameObjectManager GameObjectManager;

        private int Index;
        private int FadeCounter;
        private int ObjectGlobalId;

        protected LogicVector2 Position;
        protected int Z;
        
        public GameObject(int classId, int instanceId)
        {
            DataId = GlobalId.CreateGlobalId(classId, instanceId);

            Position = new LogicVector2();
            Z = 0;

            FadeCounter = 10;
        }

        public virtual void Tick()
        {
            ;
        }

        public virtual void Encode(BitStream bitStream, bool isOwnObject, int visionTeam)
        {
            bitStream.WritePositiveVInt(Position.X, 4);
            bitStream.WritePositiveVInt(Position.Y, 4);
            bitStream.WritePositiveVInt(Index, 3);
            bitStream.WritePositiveVInt(Z, 4);
        }

        public virtual void PreTick()
        {
            ;
        }

        public void SetForcedVisible()
        {
            FadeCounter = 10;
        }

        public void SetForcedInvisible()
        {
            FadeCounter = 0;
        }

        public void IncrementFadeCounter()
        {
            if (FadeCounter < 10) FadeCounter++;
        }

        public void DecrementFadeCounter()
        {
            if (FadeCounter > 0) FadeCounter--;
        }

        public int GetFadeCounter()
        {
            return FadeCounter;
        }

        public void SetPosition(int x, int y, int z)
        {
            Position.Set(x, y);
            Z = z;
        }

        public LogicVector2 GetPosition()
        {
            return Position.Clone();
        }

        public BattlePlayer GetPlayer()
        {
            BattleMode battle = GameObjectManager.GetBattle();
            return battle.GetPlayer(ObjectGlobalId);
        }

        public int GetGlobalID()
        {
            return ObjectGlobalId;
        }

        public int GetDataId()
        {
            return DataId;
        }

        public void AttachGameObjectManager(GameObjectManager gameObjectManager, int globalId)
        {
            GameObjectManager = gameObjectManager;
            ObjectGlobalId = globalId;
        }

        public virtual bool ShouldDestruct()
        {
            return false;
        }

        public virtual void OnDestruct()
        {
            ;
        }

        public int GetX()
        {
            return Position.X;
        }

        public int GetY()
        {
            return Position.Y;
        }

        public int GetZ()
        {
            return Z;
        }

        public void SetIndex(int i)
        {
            Index = i;
        }

        public int GetIndex()
        {
            return Index;
        }

        public virtual bool IsAlive()
        {
            return true;
        }

        public virtual int GetRadius()
        {
            return 100;
        }

        public virtual int GetSize()
        {
            return 100;
        }

        public virtual int GetObjectType()
        {
            return -1;
        }
    }
}
