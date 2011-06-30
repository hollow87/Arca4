using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Ares.IO
{
    class AresTCPPacketWriter
    {
        private List<byte> Data = new List<byte>();

        public int ByteCount
        {
            get { return this.Data.Count; }
        }

        public void WriteByte(byte b)
        {
            this.Data.Add(b);
        }

        public void WriteBytes(byte[] b)
        {
            this.Data.AddRange(b);
        }

        public void WriteGuid(Guid g)
        {
            this.Data.AddRange(g.ToByteArray());
        }

        public void WriteUInt16(ushort i)
        {
            this.Data.AddRange(BitConverter.GetBytes(i));
        }

        public void WriteUInt32(uint i)
        {
            this.Data.AddRange(BitConverter.GetBytes(i));
        }

        public void WriteUInt64(ulong i)
        {
            this.Data.AddRange(BitConverter.GetBytes(i));
        }

        public void WriteIP(String ip_string)
        {
            this.Data.AddRange(IPAddress.Parse(ip_string).GetAddressBytes());
        }

        public void WriteIP(byte[] ip_bytes)
        {
            this.Data.AddRange(new IPAddress(ip_bytes).GetAddressBytes());
        }

        public void WriteIP(IPAddress ip_object)
        {
            this.Data.AddRange(ip_object.GetAddressBytes());
        }

        public void WriteString(String text)
        {
            this.Data.AddRange(Encoding.UTF8.GetBytes(text));
            this.Data.Add(0);
        }

        public void WriteString(String text, bool null_terminated)
        {
            this.Data.AddRange(Encoding.UTF8.GetBytes(text.Replace("", "")));

            if (null_terminated)
                this.Data.Add(0);
        }

        public byte[] ToAresPacket(ProtoMessage packet_id)
        {
            List<byte> tmp = new List<byte>(this.Data.ToArray());
            tmp.Insert(0, (byte)packet_id);
            tmp.InsertRange(0, BitConverter.GetBytes((ushort)(tmp.Count - 1)));
            return tmp.ToArray();
        }
    }
}
