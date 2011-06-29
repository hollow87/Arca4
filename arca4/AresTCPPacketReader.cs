﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Ares.IO
{
    class AresTCPPacketReader
    {
        private int Position = 0;
        private List<byte> Data = new List<byte>();

        public AresTCPPacketReader(byte[] bytes)
        {
            this.Data.Clear();
            this.Position = 0;
            this.Data.AddRange(bytes);
        }

        public int ByteCount
        {
            get { return this.Data.Count; }
        }

        public byte PeekNext
        {
            get { return this.Data[this.Position]; }
        }

        public int Remaining
        {
            get { return this.Data.Count - this.Position; }
        }

        public void SetPosition(int position)
        {
            this.Position = position;
        }

        public void SkipByte()
        {
            this.Position++;
        }

        public void SkipBytes(int count)
        {
            this.Position += count;
        }

        public byte ReadByte()
        {
            byte tmp = this.Data[this.Position];
            this.Position++;
            return tmp;
        }

        public byte[] ReadBytes(int count)
        {
            byte[] tmp = new byte[count];
            Array.Copy(this.Data.ToArray(), this.Position, tmp, 0, tmp.Length);
            this.Position += count;
            return tmp;
        }

        public byte[] ReadBytes()
        {
            byte[] tmp = new byte[this.Data.Count - this.Position];
            Array.Copy(this.Data.ToArray(), this.Position, tmp, 0, tmp.Length);
            this.Position += tmp.Length;
            return tmp;
        }

        public Guid ReadGuid()
        {
            byte[] tmp = new byte[16];
            Array.Copy(this.Data.ToArray(), this.Position, tmp, 0, tmp.Length);
            this.Position += 16;
            return new Guid(tmp);
        }

        public ushort ReadUInt16()
        {
            ushort tmp = BitConverter.ToUInt16(this.Data.ToArray(), this.Position);
            this.Position += 2;
            return tmp;
        }

        public uint ReadUInt32()
        {
            uint tmp = BitConverter.ToUInt32(this.Data.ToArray(), this.Position);
            this.Position += 4;
            return tmp;
        }

        public ulong ReadUInt64()
        {
            ulong tmp = BitConverter.ToUInt64(this.Data.ToArray(), this.Position);
            this.Position += 8;
            return tmp;
        }

        public IPAddress ReadIP()
        {
            byte[] tmp = new byte[4];
            Array.Copy(this.Data.ToArray(), this.Position, tmp, 0, tmp.Length);
            this.Position += 4;
            return new IPAddress(tmp);
        }

        public String ReadString()
        {
            int split = this.Data.IndexOf(0, this.Position);
            byte[] tmp = new byte[split > -1 ? (split - this.Position) : (this.Data.Count - this.Position)];
            Array.Copy(this.Data.ToArray(), this.Position, tmp, 0, tmp.Length);
            this.Position = split > -1 ? (split + 1) : this.Data.Count;
            String str = Encoding.UTF8.GetString(tmp);

            String[] bad_chars = new String[] // skiddy
            {
                "\r\n",
                "\r",
                "\n",
                "",
                "",
                "\x00cc\x00b8",
                "͋"
            };

            foreach (String c in bad_chars)
                str = Regex.Replace(str, Regex.Escape(c), "", RegexOptions.IgnoreCase);

            return str;
        }

        public byte[] ToByteArray()
        {
            return this.Data.ToArray();
        }
    }
}
