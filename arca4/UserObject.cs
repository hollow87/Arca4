using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Ares.IO;

namespace arca4
{
    class UserObject
    {
        public int ID { get; set; }
        public IPAddress ExternalIP { get; private set; }
        public bool LoggedIn { get; set; }
        public uint Cookie { get; private set; }
        public Guid Guid { get; private set; }
        public ushort FileCount { get; private set; }
        public ushort Port { get; private set; }
        public IPAddress NodeIP { get; private set; }
        public ushort NodePort { get; private set; }
        public String OrgName { get; private set; }
        public String Version { get; private set; }
        public IPAddress LocalIP { get; private set; }
        public bool CanBrowse { get; private set; }
        public byte CurrentUploads { get; private set; }
        public byte MaxUploads { get; private set; }
        public byte CurrentQueued { get; private set; }
        public byte Age { get; private set; }
        public byte Sex { get; private set; }
        public byte Country { get; private set; }
        public String Location { get; private set; }
        public List<String> Ignores { get; private set; }
        

        private Socket sock;
        private uint timestamp;
        private uint socket_health = 0;
        private ushort vroom = 0;
        private String name = String.Empty;
        private byte level = 0;
        private byte[] avatar = new byte[] { };
        private String personal_message = String.Empty;
        private AresTCPDataStack stack = new AresTCPDataStack();
        private List<byte[]> data_out = new List<byte[]>();

        public UserObject(Socket socket, uint now)
        {
            this.sock = socket;
            this.sock.Blocking = false;
            this.timestamp = now;
            this.ExternalIP = ((IPEndPoint)this.sock.RemoteEndPoint).Address;
            UserPool.SetID(this);
            this.LoggedIn = false;
            this.Expired = false;
            this.Cookie = now;

            while (UserPool.Users.Find(x => x.LoggedIn && x.Cookie == this.Cookie) != null)
                this.Cookie++;

            this.Ignores = new List<String>();
        }

        public void SendPacket(byte[] data)
        {
            this.data_out.Add(data);
        }

        public void SocketTasks(uint now)
        {
            this.SendPending();

            if (!this.stack.ExtractDataFromSocket(this.sock))
                this.socket_health++;
            else
                this.socket_health = 0;

            if (this.LoggedIn)
            {
                if ((this.timestamp + 240) < now)
                    this.Expired = true;
            }
            else
            {
                if ((this.timestamp + 15) < now)
                    this.Expired = true;
            }

            while (this.stack.Available)
            {
                try
                {
                    byte[] buf = this.stack.NextPacket;
                    ProtoMessage msg = this.stack.Msg;

                    if (!this.Expired)
                        AresTCPPacketProcessor.Evaluate(this, msg, new AresTCPPacketReader(buf), now);
                    else break;
                }
                catch { this.Expired = true; }
            }
        }

        private void SendPending()
        {
            while (this.data_out.Count > 0)
            {
                try
                {
                    this.sock.Send(this.data_out[0]);
                    this.data_out.RemoveAt(0);
                }
                catch { break; }
            }
        }

        public void Disconnect()
        {
            this.SendPending();
            this.TerminateSocket();
            this.Expired = true;
            this.stack.Dispose();

            if (this.LoggedIn)
            {
                this.LoggedIn = false;
                ServerEvents.OnPart(this);
                UserPool.BroadcastToVroom(this.Vroom, AresTCPPackets.Part(this));
            }
        }

        public void TerminateSocket()
        {
            try { this.sock.Disconnect(false); }
            catch { }
            try { this.sock.Shutdown(SocketShutdown.Both); }
            catch { }
            try { this.sock.Close(); }
            catch { }
        }

        public bool Expired
        {
            get { return this.socket_health > 5; }
            set { this.socket_health = value ? (uint)10 : (uint)0; }
        }

        public ushort Vroom
        {
            get { return this.vroom; }
            set
            {
                this.vroom = value;
            }
        }

        public String Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
            }
        }

        public byte Level
        {
            get { return this.level; }
            set
            {
                this.level = value;
            }
        }

        public byte[] Avatar
        {
            get { return this.avatar; }
            set
            {
                byte[] temp = value;

                if (temp.Length < 10)
                    temp = new byte[] { };

                if (!this.avatar.SequenceEqual<byte>(temp))
                {
                    this.avatar = temp;
                    UserPool.BroadcastToVroom(this.vroom, AresTCPPackets.Avatar(this));
                }
            }
        }

        public String PersonalMessage
        {
            get { return this.personal_message; }
            set
            {
                String temp = value;

                if (temp.Trim().Length == 0)
                    temp = String.Empty;

                if (temp != this.personal_message)
                {
                    this.personal_message = temp;
                    UserPool.BroadcastToVroom(this.vroom, AresTCPPackets.PersonalMessage(this));
                }
            }
        }

        public void PopulateCredentials(AresTCPPacketReader packet)
        {
            this.Guid = packet.ReadGuid();
            this.FileCount = packet.ReadUInt16();
            packet.SkipByte(); // not used
            this.Port = packet.ReadUInt16();
            this.NodeIP = packet.ReadIP();
            this.NodePort = packet.ReadUInt16();
            packet.SkipBytes(4); // line speed
            this.OrgName = UserPool.PrepareUserName(packet.ReadString(), this.Cookie);
            this.name = this.OrgName;
            this.Version = packet.ReadString();
            this.LocalIP = packet.ReadIP();
            packet.SkipBytes(4); // external ip
            this.CanBrowse = packet.ReadByte() >= 3;
            this.FileCount = this.CanBrowse ? this.FileCount : (ushort)0;
            this.CurrentUploads = packet.ReadByte();
            this.MaxUploads = packet.ReadByte();
            this.CurrentQueued = packet.ReadByte();
            this.Age = packet.ReadByte();
            this.Sex = packet.ReadByte();
            this.Country = packet.ReadByte();
            this.Location = packet.ReadString();
        }

        public void Pinged(uint now)
        {
            this.timestamp = now;
        }

    }
}
