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
        public bool LoggedIn { get; private set; }

        private Socket sock;
        private uint timestamp;
        private uint socket_health = 0;
        private ushort vroom = 0;
        private String name = String.Empty;

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
                        AresTCPPacketProcessor.Evaluate(this, msg, new AresTCPPacketReader(buf));
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
            this.socket_health = 10;
            this.stack.Dispose();

            if (this.LoggedIn)
            {
                this.LoggedIn = false;
                ServerEvents.OnPart(this);
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

        public void PopulateCredentials(AresTCPPacketReader packet)
        {
            
        }

    }
}
