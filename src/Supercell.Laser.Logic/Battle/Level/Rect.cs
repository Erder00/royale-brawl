namespace Supercell.Laser.Logic.Battle.Level
{
    public class Rect
    {
        private readonly int StartX;
        private readonly int StartY;
        private readonly int EndX;
        private readonly int EndY;

        public Rect(int startX, int startY, int endX, int endY)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.EndX = endX;
            this.EndY = endY;
        }

        public void Destruct()
        {
            // Destruct.
        }

        public int GetStartX()
        {
            return this.StartX;
        }

        public int GetStartY()
        {
            return this.StartY;
        }

        public int GetEndX()
        {
            return this.EndX;
        }

        public int GetEndY()
        {
            return this.EndY;
        }

        public bool IsInside(int x, int y)
        {
            if (this.StartX <= x)
            {
                if (this.StartY <= y)
                {
                    return this.EndX >= x && this.EndY >= y;
                }
            }

            return false;
        }

        public bool IsInside(Rect rect)
        {
            if (this.StartX <= rect.StartX)
            {
                if (this.StartY <= rect.StartY)
                {
                    return this.EndX > rect.EndX && this.EndY > rect.EndY;
                }
            }

            return false;
        }
    }
}
