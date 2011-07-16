using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class SearchRequest
    {
        public ushort ID { get; set; }
        public Mime Mime { get; set; }
        public bool AllTypes { get; set; }
        public String SearchWord { get; set; }

        public SearchRequest(AresTCPPacketReader packet)
        {
            this.ID = packet.ReadUInt16();
            packet.SkipByte();
            this.Mime = (Mime)packet.ReadByte();
            this.AllTypes = (byte)this.Mime == 0 || (byte)this.Mime== 255;
            this.Mime = (byte)this.Mime == 8 ? (Mime)0 : this.Mime;
            ushort search_len = packet.ReadUInt16();
            byte[] tmp = packet.ReadBytes(search_len);
            this.SearchWord = Encoding.UTF8.GetString(tmp).ToUpper();
        }
    }
}
