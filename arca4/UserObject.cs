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

        private Socket sock;
        private uint timestamp;
        private uint socket_health = 0;

        private AresTCPDataStack stack = new AresTCPDataStack();

        public UserObject(Socket socket, uint now)
        {
            this.sock = socket;
            this.timestamp = now;
            this.ExternalIP = ((IPEndPoint)this.sock.RemoteEndPoint).Address;
            UserPool.SetID(this);
        }

        public void SocketTasks(uint now)
        {
            if (!this.stack.ExtractDataFromSocket(this.sock))
                this.socket_health++;
            else
                this.socket_health = 0;
        }

        public bool Expired
        {
            get { return this.socket_health > 5; }
        }

    }
}
