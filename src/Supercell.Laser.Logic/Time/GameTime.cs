namespace Supercell.Laser.Logic.Time
{
    public class GameTime
    {
        private int Tick;

        public GameTime()
        {
            // GameTime.
        }

        public void Reset()
        {
            Tick = 0;
        }

        public void IncreaseTick()
        {
            Tick++;
        }

        public int GetTick()
        {
            return Tick;
        }
    }
}
