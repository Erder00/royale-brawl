namespace Supercell.Laser.Titan.Math
{
    using Supercell.Laser.Titan.DataStream;

    public class LogicVector2
    {
        public int X;
        public int Y;

        public LogicVector2()
        {
        }

        public LogicVector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Destruct()
        {
            this.X = 0;
            this.Y = 0;
        }

        public void Add(LogicVector2 vector2)
        {
            this.X += vector2.X;
            this.Y += vector2.Y;
        }

        public LogicVector2 Clone()
        {
            return new LogicVector2(this.X, this.Y);
        }

        public int Dot(LogicVector2 vector2)
        {
            return this.X * vector2.X + this.Y * vector2.Y;
        }

        public int GetAngle()
        {
            return LogicMath.GetAngle(this.X, this.Y);
        }

        public int GetAngleBetween(int x, int y)
        {
            return LogicMath.GetAngleBetween(LogicMath.GetAngle(this.X, this.Y), LogicMath.GetAngle(x, y));
        }

        public int GetDistance(LogicVector2 vector2)
        {
            int x = this.X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = this.Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return LogicMath.Sqrt(distance);
        }

        public int GetDistanceSquared(LogicVector2 vector2)
        {
            int x = this.X - vector2.X;
            int distance = 0x7FFFFFFF;

            if ((uint)(x + 46340) <= 92680)
            {
                int y = this.Y - vector2.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        public int GetDistanceSquaredTo(int x, int y)
        {
            int distance = 0x7FFFFFFF;

            x -= this.X;

            if ((uint)(x + 46340) <= 92680)
            {
                y -= this.Y;

                if ((uint)(y + 46340) <= 92680)
                {
                    int distanceX = x * x;
                    int distanceY = y * y;

                    if ((uint)distanceY < (distanceX ^ 0x7FFFFFFFu))
                    {
                        distance = distanceX + distanceY;
                    }
                }
            }

            return distance;
        }

        public int GetLength()
        {
            int length = 0x7FFFFFFF;

            if ((uint)(46340 - this.X) <= 92680)
            {
                if ((uint)(46340 - this.Y) <= 92680)
                {
                    int lengthX = this.X * this.X;
                    int lengthY = this.Y * this.Y;

                    if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                    {
                        length = lengthX + lengthY;
                    }
                }
            }

            return LogicMath.Sqrt(length);
        }

        public int GetLengthSquared()
        {
            int length = 0x7FFFFFFF;

            if ((uint)(46340 - this.X) <= 92680)
            {
                if ((uint)(46340 - this.Y) <= 92680)
                {
                    int lengthX = this.X * this.X;
                    int lengthY = this.Y * this.Y;

                    if ((uint)lengthY < (lengthX ^ 0x7FFFFFFFu))
                    {
                        length = lengthX + lengthY;
                    }
                }
            }

            return length;
        }

        public bool IsEqual(LogicVector2 vector2)
        {
            return this.X == vector2.X && this.Y == vector2.Y;
        }

        public bool IsInArea(int minX, int minY, int maxX, int maxY)
        {
            if (this.X >= minX && this.Y >= minY)
                return this.X < minX + maxX && this.Y < maxY + minY;
            return false;
        }

        public void Multiply(LogicVector2 vector2)
        {
            this.X *= vector2.X;
            this.Y *= vector2.Y;
        }

        public int Normalize(int value)
        {
            int length = this.GetLength();

            if (length != 0)
            {
                this.X = this.X * value / length;
                this.Y = this.Y * value / length;
            }

            return length;
        }

        public void Rotate(int degrees)
        {
            int newX = LogicMath.GetRotatedX(this.X, this.Y, degrees);
            int newY = LogicMath.GetRotatedY(this.X, this.Y, degrees);

            this.X = newX;
            this.Y = newY;
        }

        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Substract(LogicVector2 vector2)
        {
            this.X -= vector2.X;
            this.Y -= vector2.Y;
        }

        public void Decode(ByteStream stream)
        {
            this.X = stream.ReadVInt();
            this.Y = stream.ReadVInt();
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(this.X);
            stream.WriteVInt(this.Y);
        }

        public override string ToString()
        {
            return "LogicVector2(" + this.X + "," + this.Y + ")";
        }
    }
}
