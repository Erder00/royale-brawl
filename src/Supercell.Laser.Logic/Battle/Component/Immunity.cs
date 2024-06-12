namespace Supercell.Laser.Logic.Battle.Component
{
    public class Immunity
    {
        private int m_ticks;
        private int m_immunityPercent;

        public Immunity(int ticks, int immunityPercent)
        {
            m_ticks = ticks;
            m_immunityPercent = immunityPercent;
        }

        public int GetImmunityPercentage()
        {
            return m_immunityPercent;
        }

        public bool Tick(int a2)
        {
            m_ticks -= a2;
            return m_ticks < 1;
        }

        public void Destruct()
        {
            m_ticks = 0;
        }
    }
}
