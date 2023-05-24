using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;


namespace RoomKitModulePrototype
{
    public class SshClientModified : SshClient
    {
        public EventHandler<CodecConnectionStatusEventArgs> SSHStatusChanged;
        public SshClientModified(string host, int port, string username, string password) : base(host, port, username, password) { }
        public SshClientModified(ConnectionInfo connectionInfo) : base(connectionInfo) { }
        protected override void OnConnected()
        {
            base.OnConnected();
            SSHStatusChanged.Invoke(this, new CodecConnectionStatusEventArgs(true, "Connected" ));
        }
        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            SSHStatusChanged.Invoke(this, new CodecConnectionStatusEventArgs(false, "Disconnected"));
        }
        protected override void OnConnecting()
        {
            base.OnConnecting();
            SSHStatusChanged.Invoke(this, new CodecConnectionStatusEventArgs(false, "Connecting"));
        }
        protected override void OnDisconnecting()
        {
            base.OnDisconnecting();
            SSHStatusChanged.Invoke(this, new CodecConnectionStatusEventArgs(false, "Disconnecting"));
        }

    }
}
