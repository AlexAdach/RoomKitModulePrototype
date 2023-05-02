using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace RoomKitModulePrototype
{
    public class SSH : CommunicationBase, ICodecCommunication
    {
        public override bool Connected { get 
            {
                if (client != null)
                    return client.IsConnected;
                else
                    return false;
            } 
        }

        private string _user;
        private string _password;
        private string _host;

        private SshClient client;
        private ShellStream stream;


        public SSH(string user, string password, string host, ICoreModule core) : base(core)
        {
            _user = user;
            _password = password;
            _host = host;
        }

        public override void Connect()
        {
            //ConnectionInfo connection = new ConnectionInfo("192.168.0.113", 22, "Tritech", "Tritech1!");
            var kauth = new KeyboardInteractiveAuthenticationMethod(_user);

            kauth.AuthenticationPrompt += delegate (object sender, AuthenticationPromptEventArgs e)
            {
                foreach (var prompt in e.Prompts)
                {
                    if (prompt.Request.ToLowerInvariant().StartsWith("password"))
                    {
                        prompt.Response = _password;
                    }
                }
            };

            var connectionInfo =
             new ConnectionInfo(
                 _host,
                 22,
                 _user,
                 kauth);

            client = new SshClient(connectionInfo);

            client.HostKeyReceived += (sender, e) => {
                e.CanTrust = true;
            };

            try
            {
                client.Connect();
                stream = client.CreateShellStream("Default", 0, 0, 0, 0, 1024);
                stream.DataReceived += OnDataRecieved;
            }
            catch (Exception e)
            {
                Debug.Log($"Error connecting SSH - {e.Message}");
            }    
        }

        public override void SendCommand(string cmd)
        {
            cmd += "\n";
            stream?.Write(cmd);
        }

        public override void SendCommand(XAPICommandDTO req)
        {
            var cmd = req.CommandString;
            cmd += "\n";
            stream?.Write(cmd);
        }

        private void OnDataRecieved(object sender, ShellDataEventArgs args)
        {            
            var resp = stream.Read();
            base.ResponseRouter(resp);
            Debug.Log(resp);
        }
    }
}
