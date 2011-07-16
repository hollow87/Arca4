using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ares.IO;

namespace arca4
{
    class BrowseRequest
    {
        public ushort ID { get; set; }
        public Mime Mime { get; set; }
        public UserObject Target { get; set; }
        public bool AllTypes { get; set; }

        public BrowseRequest(AresTCPPacketReader packet)
        {
            this.ID = packet.ReadUInt16();
            this.Mime = (Mime)packet.ReadByte();
            this.AllTypes = (byte)this.Mime == 0 || (byte)this.Mime == 255;
            this.Mime = (byte)this.Mime == 8 ? (Mime)0 : this.Mime;
            String name = packet.ReadString();
            this.Target = UserPool.Users.Find(x => x.LoggedIn && x.CanBrowse && x.Name == name);
        }
    }
}
