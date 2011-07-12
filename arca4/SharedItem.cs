using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class SharedItem
    {
        public Mime Mime { get; set; }
        public uint Size { get; set; }
        public byte[] Data { get; set; }
        public String FileName { get; set; }
        public String Title { get; set; }
        public String SearchWords { get; set; }

        public SharedItem(AresTCPPacketReader packet)
        {
            this.Mime = (Mime)packet.ReadByte();
            this.Size = packet.ReadUInt32();
            ushort len = packet.ReadUInt16();
            this.SearchWords = Encoding.UTF8.GetString(packet.ReadBytes(len)).ToUpper();
            this.Data = packet.ReadBytes();
            this.FileName = null;
            this.Title = null;

            packet = new AresTCPPacketReader(this.Data);
            packet.SkipBytes(18); // 16 guid + 2 detail len

            switch (this.Mime)
            {
                case Mime.ARES_MIME_MP3:
                    packet.SkipBytes(4);
                    break;

                case Mime.ARES_MIME_VIDEO:
                    packet.SkipBytes(6);
                    break;

                case Mime.ARES_MIME_IMAGE:
                    packet.SkipBytes(5);
                    break;
            }

            while (packet.Remaining > 2)
            {
                byte size = packet.ReadByte();
                byte type = packet.ReadByte();

                if (packet.Remaining < size)
                    break;

                byte[] data = packet.ReadBytes(size);
                
                switch (type)
                {
                    case 1:
                        this.Title = Encoding.UTF8.GetString(data);
                        break;

                    case 15:
                        this.FileName = Encoding.UTF8.GetString(data);
                        break;
                }
            }

            if (this.FileName != null)
                if (this.Title == null)
                    this.Title = this.FileName;
        }

        public bool CanScript
        {
            get
            {
                switch (this.Mime) // don't filter audio files
                {
                    case Mime.ARES_MIME_AUDIOOTHER1:
                    case Mime.ARES_MIME_AUDIOOTHER2:
                    case Mime.ARES_MIME_MP3:
                        return false;
                }

                return !String.IsNullOrEmpty(this.FileName);
            }
        }
    }
}
