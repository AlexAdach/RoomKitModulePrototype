using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;


namespace RoomKitModulePrototype
{
    class CiscoSSHClient : SshClient
    {
        public EventHandler<EventArgs> SSHStatusChanged;
        public CiscoSSHClient(string host, int port, string username, string password) : base(host, port, username, password) { }
        public CiscoSSHClient(ConnectionInfo connectionInfo) : base(connectionInfo) { }
        protected override void OnConnected()
        {
            base.OnConnected();
            SSHStatusChanged.Invoke(this, EventArgs.Empty);
        }
        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            SSHStatusChanged.Invoke(this, EventArgs.Empty);
        }
        protected override void OnConnecting()
        {
            base.OnConnecting();
            SSHStatusChanged.Invoke(this, EventArgs.Empty);
        }
        protected override void OnDisconnecting()
        {
            base.OnDisconnecting();
            SSHStatusChanged.Invoke(this, EventArgs.Empty);
        }

    }
}
