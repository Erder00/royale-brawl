namespace Supercell.Laser.Titan.DataStream
{
    public class BitStream
    {
        private byte[] buffer; // this.add(4);
        private int offset; // this.add(2);
        private int length; // this.add(1);
        private int bitOffset;

        public BitStream(int length)
        {
            this.buffer = new byte[length];
            this.length = length;
        }

        public BitStream(byte[] buffer, int length)
        {
            this.buffer = buffer;
            this.length = length;
        }

        public int GetLength()
        {
            if (this.length > this.offset)
                return this.length;

            return this.offset;
        }

        public byte ReadBit()
        {
            if (this.offset > this.buffer.Length)
                return 0;

            byte value = (byte)((this.buffer[this.offset] >> this.bitOffset) & 1);
            this.bitOffset++;
            if (this.bitOffset == 8)
            {
                this.bitOffset = 0;
                this.offset += 1;
            }
            return value;
        }

        public byte[] ReadBytes(int length)
        {
            List<byte> data = new List<byte>();
            for (int i = 0; i < length;)
            {
                byte value = 0;
                for (int p = 0; p < 8 && i < length; p++, i++)
                {
                    value |= (byte)(this.ReadBit() << p);
                }
                data.Add(value);
            }

            return data.ToArray();
        }

        public int ReadPositiveInt(int bitsCount)
        {
            byte[] bytes = this.ReadBytes(bitsCount);
            if (bytes.Length == 1)
            {
                return bytes[0];
            }
            if (bytes.Length == 2)
            {
                return BitConverter.ToUInt16(bytes);
            }
            if (bytes.Length == 3)
            {
                return bytes[0] << 16 | bytes[1] << 8 | bytes[2];
            }
            if (bytes.Length == 4)
            {
                return BitConverter.ToInt32(bytes);
            }
            else
            {
                return -1;
            }
        }

        public int ReadInt(int bits)
        {
            int v2 = 2 * this.ReadPositiveInt(1) - 1;
            return v2 * this.ReadPositiveInt(bits);
        }

        public int ReadIntMax32767()
        {
            return this.ReadInt(15);
        }

        public int ReadPositiveVIntMax255()
        {
            int v2 = this.ReadPositiveInt(3);
            return this.ReadPositiveInt(v2);
        }

        public int ReadPositiveIntMax15()
        {
            return this.ReadPositiveInt(4);
        }

        public int ReadPositiveIntMax32767()
        {
            return this.ReadPositiveInt(15);
        }

        public bool ReadBoolean()
        {
            return this.ReadPositiveInt(1) == 1;
        }

        public void WriteBit(byte data)
        {
            if (this.bitOffset == 0)
            {
                this.offset += 1;
                this.WriteByte((byte)0xFF);
            }
            int value = this.buffer[this.offset - 1];
            value &= ~(1 << this.bitOffset);
            value |= (data << this.bitOffset);
            this.buffer[this.offset - 1] = (byte)value;
            this.bitOffset = (this.bitOffset + 1) % 8;
        }

        public void WriteBits(byte[] bits, long count)
        {
            int position = 0;
            for (long i = 0; i < count;)
            {
                byte value;
                for (int p = 0; p < 8 && i < count; i++, p++)
                {
                    value = (byte)((bits[position] >> p) & 1);
                    this.WriteBit(value);
                }
                position++;
            }
        }

        public void WritePositiveInt(int value, int bits)
        {
            this.WriteBits(BitConverter.GetBytes(value), bits);
        }

        public void WritePositiveIntMax1(int value)
        {
            this.WritePositiveInt(value, 1);
        }

        public void WritePositiveIntMax3(int value)
        {
            this.WritePositiveInt(value, 2);
        }

        public void WritePositiveIntMax7(int value)
        {
            this.WritePositiveInt(value, 3);
        }

        public void WritePositiveIntMax15(int value)
        {
            this.WritePositiveInt(value, 4);
        }

        public void WritePositiveIntMax31(int value)
        {
            this.WritePositiveInt(value, 5);
        }

        public void WritePositiveIntMax63(int value)
        {
            this.WritePositiveInt(value, 6);
        }

        public void WritePositiveIntMax127(int value)
        {
            this.WritePositiveInt(value, 7);
        }

        public void WritePositiveIntMax255(int value)
        {
            this.WritePositiveInt(value, 8);
        }

        public void WritePositiveIntMax511(int value)
        {
            this.WritePositiveInt(value, 9);
        }

        public void WritePositiveIntMax1023(int value)
        {
            this.WritePositiveInt(value, 10);
        }

        public void WritePositiveIntMax2047(int value)
        {
            this.WritePositiveInt(value, 11);
        }

        public void WritePositiveIntMax4095(int value)
        {
            this.WritePositiveInt(value, 12);
        }

        public void WritePositiveIntMax8191(int value)
        {
            this.WritePositiveInt(value, 13);
        }

        public void WritePositiveIntMax16383(int value)
        {
            this.WritePositiveInt(value, 14);
        }

        public void WritePositiveIntMax32767(int value)
        {
            this.WritePositiveInt(value, 15);
        }

        public void WritePositiveIntMax65535(int value)
        {
            this.WritePositiveInt(value, 16);
        }

        public void WritePositiveIntMax131071(int value)
        {
            this.WritePositiveInt(value, 17);
        }

        public void WritePositiveIntMax262143(int value)
        {
            this.WritePositiveInt(value, 18);
        }

        public void WritePositiveIntMax524287(int value)
        {
            this.WritePositiveInt(value, 19);
        }

        public void WritePositiveIntMax1048575(int value)
        {
            this.WritePositiveInt(value, 20);
        }

        public void WritePositiveIntMax2097151(int value)
        {
            this.WritePositiveInt(value, 21);
        }

        public void WritePositiveIntMax4194303(int value)
        {
            this.WritePositiveInt(value, 22);
        }

        public bool WriteBoolean(bool value)
        {
            if (value)
                this.WritePositiveInt(1, 1);
            else
                this.WritePositiveInt(0, 1);

            return value;
        }

        public void WriteInt(int value, int bits)
        {
            int val = value;
            if (val <= -1)
            {
                this.WritePositiveInt(0, 1);
                val = -value;
            }

            else if (val >= 0)
            {
                this.WritePositiveInt(1, 1);
                val = value;
            }
            this.WritePositiveInt(val, bits);
        }

        public void WriteIntMax1(int value)
        {
            this.WriteInt(value, 1);
        }

        public void WriteIntMax3(int value)
        {
            this.WriteInt(value, 2);
        }

        public void WriteIntMax7(int value)
        {
            this.WriteInt(value, 3);
        }

        public void WriteIntMax15(int value)
        {
            this.WriteInt(value, 4);
        }

        public void WriteIntMax31(int value)
        {
            this.WriteInt(value, 5);
        }

        public void WriteIntMax63(int value)
        {
            this.WriteInt(value, 6);
        }

        public void WriteIntMax127(int value)
        {
            this.WriteInt(value, 7);
        }

        public void WriteIntMax2047(int value)
        {
            this.WriteInt(value, 11);
        }

        public void WriteIntMax16383(int value)
        {
            this.WriteInt(value, 14);
        }

        public void WriteIntMax65535(int value)
        {
            this.WriteInt(value, 16);
        }
        public void WritePositiveVInt(int value, int bits)
        {
            int v3 = 1;
            int v7 = value;
            if (v7 != 0)
            {
                if (v7 < 1)
                {
                    v3 = 0;
                }
                else
                {
                    int v8 = v7;
                    v3 = 0;
                    do
                    {
                        v3 += 1;
                        v8 >>= 1;
                    }
                    while (v8 != 0);
                }
            }
            this.WritePositiveInt(v3 - 1, bits);
            this.WritePositiveInt(v7, v3);
        }

        public void WritePositiveVIntMax255OftenZero(int value)
        {
            if (value == 0)
            {
                this.WritePositiveInt(1, 1);
                return;
            }

            this.WritePositiveInt(0, 1);
            this.WritePositiveVInt(value, 3);
        }

        public void WritePositiveVIntMax65535OftenZero(int value)
        {
            if (value == 0)
            {
                this.WritePositiveInt(1, 1);
                return;
            }

            this.WritePositiveInt(0, 1);
            this.WritePositiveVInt(value, 4);
        }

        public void WritePositiveVIntMax65535(int value)
        {
            this.WritePositiveVInt(value, 4);
        }

        public void WritePositiveVIntMax255(int value)
        {
            this.WritePositiveVInt(value, 3);
        }

        public void WriteByte(byte value)
        {
            this.EnsureCapacity(1);
            this.bitOffset = 0;
            this.buffer[this.offset - 1] = value;
        }

        public void EnsureCapacity(int capacity)
        {
            int bufferLength = this.buffer.Length;

            if (this.offset + capacity > bufferLength)
            {
                byte[] tmpBuffer = new byte[this.buffer.Length + capacity];
                Array.Copy(this.buffer, 0, tmpBuffer, 0, bufferLength);
                this.buffer = tmpBuffer;
            }
        }

        public byte[] GetByteArray()
        {
            return this.buffer;
        }

        public void WritePositiveVIntOftenZero(int v1, int v2)
        {
            if (v1 == 0)
            {
                this.WritePositiveInt(1, 1);
                return;
            }

            this.WritePositiveInt(0, 1);
            this.WritePositiveVInt(v1, v2);
        }
    }
}
