﻿using System;
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
        }

        public void SocketTasks(uint now)
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

            if (!this.stack.ExtractDataFromSocket(this.sock))
                this.socket_health++;
            else
                this.socket_health = 0;

            if (this.LoggedIn) // last update packet received?
            {
                if ((this.timestamp + 240) < now)
                    this.socket_health = 10;
            }
            else // loitering?
            {
                if ((this.timestamp + 15) < now)
                    this.socket_health = 10;
            }
        }

        public void Disconnect()
        {
            try { this.sock.Disconnect(false); }
            catch { }
            try { this.sock.Shutdown(SocketShutdown.Both); }
            catch { }
            try { this.sock.Close(); }
            catch { }

            this.socket_health = 10;
            this.stack.Dispose();
        }

        public bool Expired
        {
            get { return this.socket_health > 5; }
        }

    }
}
