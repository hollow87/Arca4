using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace arca4
{
    class Arca
    {
        private Thread server_thread;
        private TcpListener listener;
        private bool terminate;

        public void Start()
        {
            this.server_thread = new Thread(new ThreadStart(this.ServerThread));
            this.server_thread.Start();
        }

        public void Stop()
        {
            this.terminate = true;

            if (this.listener != null)
                this.listener.Stop();
        }

        private void ServerThread()
        {
            this.terminate = false;

            UserPool.Init();

            this.listener = new TcpListener(new IPEndPoint(IPAddress.Any, 22454));
            this.listener.Start();

            while (true)
            {
                if (this.terminate)
                    return;

                uint time = UnixTime;
                this.CheckNewUsers(time);
                this.ServiceCurrentUsers(time);

                Thread.Sleep(25);
            }
        }

        private void CheckNewUsers(uint time)
        {
            while (this.listener.Pending())
                UserPool.Users.Add(new UserObject(this.listener.AcceptSocket(), time));
        }

        private void ServiceCurrentUsers(uint time)
        {
            UserPool.Users.ForEach(x => x.SocketTasks(time));
            UserPool.Users.FindAll(x => x.Expired).ForEach(x => x.Disconnect());
            UserPool.Users.RemoveAll(x => x.Expired);
        }

        private static uint UnixTime
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return (uint)ts.TotalSeconds;
            }
        }

    }
}
