namespace Supercell.Laser.Titan.DataStream
{
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Linq;
    using System.Text;

    public class ByteStream : ChecksumEncoder
    {
        private int BitOffset;

        private byte[] Buffer;
        private int Length;
        private int Offset;

        public ByteStream(int capacity)
        {
            this.Buffer = new byte[capacity];
        }

        public ByteStream(byte[] buffer, int length)
        {
            this.Length = length;
            this.Buffer = buffer;
        }

        public int GetLength()
        {
            if (this.Offset < this.Length)
            {
                return this.Length;
            }

            return this.Offset;
        }

        public int GetOffset()
        {
            return this.Offset;
        }

        public bool IsAtEnd()
        {
            return this.Offset >= this.Length;
        }

        public void Clear(int capacity)
        {
            this.Buffer = new byte[capacity];
            this.Offset = 0;
        }

        public byte[] GetByteArray()
        {
            return this.Buffer;
        }

        public bool ReadBoolean()
        {
            if (this.BitOffset == 0)
            {
                ++this.Offset;
            }

            bool value = (this.Buffer[this.Offset - 1] & (1 << this.BitOffset)) != 0;
            this.BitOffset = (this.BitOffset + 1) & 7;
            return value;
        }

        public byte ReadByte()
        {
            this.BitOffset = 0;
            return this.Buffer[this.Offset++];
        }

        public short ReadShort()
        {
            this.BitOffset = 0;

            return (short)((this.Buffer[this.Offset++] << 8) |
                            this.Buffer[this.Offset++]);
        }

        public int ReadInt()
        {
            this.BitOffset = 0;

            return (this.Buffer[this.Offset++] << 24) |
                   (this.Buffer[this.Offset++] << 16) |
                   (this.Buffer[this.Offset++] << 8) |
                   this.Buffer[this.Offset++];
        }

        public int ReadVInt()
        {
            this.BitOffset = 0;
            int value = 0;
            byte byteValue = this.Buffer[this.Offset++];

            if ((byteValue & 0x40) != 0)
            {
                value |= byteValue & 0x3F;

                if ((byteValue & 0x80) != 0)
                {
                    value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 6;

                    if ((byteValue & 0x80) != 0)
                    {
                        value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 13;

                        if ((byteValue & 0x80) != 0)
                        {
                            value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 20;

                            if ((byteValue & 0x80) != 0)
                            {
                                value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 27;
                                return (int)(value | 0x80000000);
                            }

                            return (int)(value | 0xF8000000);
                        }

                        return (int)(value | 0xFFF00000);
                    }

                    return (int)(value | 0xFFFFE000);
                }

                return (int)(value | 0xFFFFFFC0);
            }

            value |= byteValue & 0x3F;

            if ((byteValue & 0x80) != 0)
            {
                value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 6;

                if ((byteValue & 0x80) != 0)
                {
                    value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 13;

                    if ((byteValue & 0x80) != 0)
                    {
                        value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 20;

                        if ((byteValue & 0x80) != 0)
                        {
                            value |= ((byteValue = this.Buffer[this.Offset++]) & 0x7F) << 27;
                        }
                    }
                }
            }

            return value;
        }

        public long ReadLong()
        {
            byte[] buff = ReadBytes(8, 8);
            return BitConverter.ToInt64(buff.Reverse().ToArray(), 0);
        }

        public long ReadVLong()
        {
            int high = ReadVInt();
            int low = ReadVInt();

            return ((long)high << 32) | (uint)low;
        }

        public int ReadBytesLength()
        {
            this.BitOffset = 0;
            return (this.Buffer[this.Offset++] << 24) |
                   (this.Buffer[this.Offset++] << 16) |
                   (this.Buffer[this.Offset++] << 8) |
                   this.Buffer[this.Offset++];
        }

        public byte[] ReadBytes(int length, int maxCapacity)
        {
            this.BitOffset = 0;

            if (length <= -1)
            {
                return null;
            }

            if (length <= maxCapacity)
            {
                byte[] array = new byte[length];
                System.Buffer.BlockCopy(this.Buffer, this.Offset, array, 0, length);
                this.Offset += length;
                return array;
            }

            return null;
        }

        public string ReadString(int maxCapacity = 9000000)
        {
            int length = this.ReadBytesLength();

            if (length <= -1)
            {
                return null;
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(this.Buffer, this.Offset, length);
                    this.Offset += length;
                    return value;
                }

                return null;
            }
        }

        public string ReadStringReference(int maxCapacity)
        {
            int length = this.ReadBytesLength();

            if (length <= -1)
            {
                return string.Empty;
            }
            else
            {
                if (length <= maxCapacity)
                {
                    string value = Encoding.UTF8.GetString(this.Buffer, this.Offset, length);
                    this.Offset += length;
                    return value;
                }
            }

            return string.Empty;
        }

        public override bool WriteBoolean(bool value)
        {
            if (this.BitOffset == 0)
            {
                this.EnsureCapacity(1);
                this.Buffer[this.Offset++] = 0;
            }

            if (value)
            {
                this.Buffer[this.Offset - 1] |= (byte)(1 << this.BitOffset);
            }

            this.BitOffset = (this.BitOffset + 1) & 7;
            return value;
        }

        public override void WriteByte(byte value)
        {
            this.EnsureCapacity(1);

            this.BitOffset = 0;

            this.Buffer[this.Offset++] = value;
        }

        public override void WriteShort(short value)
        {
            this.EnsureCapacity(2);

            this.BitOffset = 0;

            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }

        public override void WriteInt(int value)
        {
            this.EnsureCapacity(4);

            this.BitOffset = 0;

            this.Buffer[this.Offset++] = (byte)(value >> 24);
            this.Buffer[this.Offset++] = (byte)(value >> 16);
            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }

        public override void WriteVInt(int value)
        {
            this.EnsureCapacity(5);
            this.BitOffset = 0;

            if (value >= 0)
            {
                if (value >= 64)
                {
                    if (value >= 0x2000)
                    {
                        if (value >= 0x100000)
                        {
                            if (value >= 0x8000000)
                            {
                                this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 13) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 20) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)((value >> 27) & 0xF);
                            }
                            else
                            {
                                this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 13) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)((value >> 20) & 0x7F);
                            }
                        }
                        else
                        {
                            this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0x80);
                            this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                            this.Buffer[this.Offset++] = (byte)((value >> 13) & 0x7F);
                        }
                    }
                    else
                    {
                        this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0x80);
                        this.Buffer[this.Offset++] = (byte)((value >> 6) & 0x7F);
                    }
                }
                else
                {
                    this.Buffer[this.Offset++] = (byte)(value & 0x3F);
                }
            }
            else
            {
                if (value <= -0x40)
                {
                    if (value <= -0x2000)
                    {
                        if (value <= -0x100000)
                        {
                            if (value <= -0x8000000)
                            {
                                this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0xC0);
                                this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 13) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 20) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)((value >> 27) & 0xF);
                            }
                            else
                            {
                                this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0xC0);
                                this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)(((value >> 13) & 0x7F) | 0x80);
                                this.Buffer[this.Offset++] = (byte)((value >> 20) & 0x7F);
                            }
                        }
                        else
                        {
                            this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0xC0);
                            this.Buffer[this.Offset++] = (byte)(((value >> 6) & 0x7F) | 0x80);
                            this.Buffer[this.Offset++] = (byte)((value >> 13) & 0x7F);
                        }
                    }
                    else
                    {
                        this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0xC0);
                        this.Buffer[this.Offset++] = (byte)((value >> 6) & 0x7F);
                    }
                }
                else
                {
                    this.Buffer[this.Offset++] = (byte)((value & 0x3F) | 0x40);
                }
            }
        }

        public void WriteIntToByteArray(int value)
        {
            this.EnsureCapacity(4);
            this.BitOffset = 0;

            this.Buffer[this.Offset++] = (byte)(value >> 24);
            this.Buffer[this.Offset++] = (byte)(value >> 16);
            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }

        public void WriteLong(long value)
        {
            this.WriteIntToByteArray((int)(value >> 32));
            this.WriteIntToByteArray((int)value);
        }

        public override void WriteBytes(byte[] value, int length)
        {
            if (value == null)
            {
                this.WriteIntToByteArray(-1);
            }
            else
            {
                this.EnsureCapacity(length + 4);
                this.WriteIntToByteArray(length);

                System.Buffer.BlockCopy(value, 0, this.Buffer, this.Offset, length);

                this.Offset += length;
            }
        }

        public void WriteBytesWithoutLength(byte[] value, int length)
        {
            if (value != null)
            {
                this.EnsureCapacity(length);
                System.Buffer.BlockCopy(value, 0, this.Buffer, this.Offset, length);
                this.Offset += length;
            }
        }

        public override void WriteString(string value)
        {
            if (value == null)
            {
                this.WriteIntToByteArray(-1);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                int length = bytes.Length;

                if (length <= 900000)
                {
                    this.EnsureCapacity(length + 4);
                    this.WriteIntToByteArray(length);

                    System.Buffer.BlockCopy(bytes, 0, this.Buffer, this.Offset, length);

                    this.Offset += length;
                }
                else
                {
                    this.WriteIntToByteArray(-1);
                }
            }
        }

        public override void WriteStringReference(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            int length = bytes.Length;

            if (length <= 900000)
            {
                this.EnsureCapacity(length + 4);
                this.WriteIntToByteArray(length);

                System.Buffer.BlockCopy(bytes, 0, this.Buffer, this.Offset, length);

                this.Offset += length;
            }
            else
            {
                this.WriteIntToByteArray(-1);
            }
        }

        public void SetByteArray(byte[] buffer, int length)
        {
            this.Offset = 0;
            this.BitOffset = 0;
            this.Buffer = buffer;
            this.Length = length;
        }

        public void ResetOffset()
        {
            this.Offset = 0;
            this.BitOffset = 0;
        }

        public void SetOffset(int offset)
        {
            this.Offset = offset;
            this.BitOffset = 0;
        }

        public byte[] RemoveByteArray()
        {
            byte[] byteArray = this.Buffer;
            this.Buffer = null;
            return byteArray;
        }

        public void EnsureCapacity(int capacity)
        {
            int bufferLength = this.Buffer.Length;

            if (this.Offset + capacity > bufferLength)
            {
                byte[] tmpBuffer = new byte[this.Buffer.Length + capacity + 100];
                System.Buffer.BlockCopy(this.Buffer, 0, tmpBuffer, 0, bufferLength);
                this.Buffer = tmpBuffer;
            }
        }

        public void Destruct()
        {
            this.Buffer = null;
            this.BitOffset = 0;
            this.Length = 0;
            this.Offset = 0;
        }
    }
}
