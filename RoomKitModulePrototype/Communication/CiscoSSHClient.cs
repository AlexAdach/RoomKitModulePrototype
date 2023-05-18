using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;


namespace RoomKitModulePrototype
{
    class CiscoSSHClient : SshClient
    {
        public EventHandler OnCodecConnectionChanged;

        public CiscoSSHClient(string host, int port, string username, string password) : base(host, port, username, password) { }
        public CiscoSSHClient(ConnectionInfo connectionInfo) : base(connectionInfo) { }
        protected override void OnConnected()
        {
            base.OnConnected();
            OnCodecConnectionChanged.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            OnCodecConnectionChanged.Invoke(this, EventArgs.Empty);
        }
        protected override void OnConnecting()
        {
            base.OnConnecting();
            OnCodecConnectionChanged.Invoke(this, EventArgs.Empty);
        }
        protected override void OnDisconnecting()
        {
            base.OnDisconnecting();
            OnCodecConnectionChanged.Invoke(this, EventArgs.Empty);
        }

    }
}
