using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Compression;

namespace Ares.IO
{
    class AresTCPDataStack : IDisposable
    {
        private List<byte> data = new List<byte>();

        public ProtoMessage Msg { get; private set; }

        public void Dispose()
        {
            this.data.Clear();
        }

        public bool ExtractDataFromSocket(Socket sock)
        {
            byte[] buf = new byte[8192];
            int len = 0;
            SocketError e = SocketError.Success;

            try
            {
                len = sock.Receive(buf, 0, sock.Available, SocketFlags.None, out e);
            }
            catch { }

            if (len > 0)
            {
                this.data.AddRange(buf.Take(len));
                return true;
            }

            return e == SocketError.WouldBlock;
        }

        public bool Available
        {
            get
            {
                if (this.data.Count < 3)
                    return false;

                ushort len = BitConverter.ToUInt16(this.data.ToArray(), 0);
                return this.data.Count >= (len + 3);
            }
        }

        public byte[] NextPacket
        {
            get
            {
                ushort len = BitConverter.ToUInt16(this.data.ToArray(), 0);
                this.Msg = (ProtoMessage)this.data[2];
                byte[] buf = this.data.GetRange(3, len).ToArray();
                this.data.RemoveRange(0, (len + 3));

                while (this.Msg == ProtoMessage.MSG_CHAT_CLIENTCOMPRESSED)
                {
                    buf = ZipLib.Decompress(buf);
                    this.data.InsertRange(0, buf);
                    len = BitConverter.ToUInt16(this.data.ToArray(), 0);
                    this.Msg = (ProtoMessage)this.data[2];
                    buf = this.data.GetRange(3, len).ToArray();
                    this.data.RemoveRange(0, (len + 3));
                }

                return buf;
            }
        }
    }
}
