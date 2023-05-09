﻿using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace RoomKitModulePrototype
{
    //public delegate void CodecResponseReceivedHandler(object sender, ShellDataEventArgs args);
    
    public class SSH : CommunicationBase
    {
        private string _user;
        private string _password;
        private string _host;

        private CiscoSSHClient client;
        private ShellStream stream;

        public SSH(string user, string password, string host) : base()
        {
            _user = user;
            _password = password;
            _host = host;

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

            client = new CiscoSSHClient(connectionInfo);


            client.HostKeyReceived += (sender, e) => {
                e.CanTrust = true;
            };

            client.OnCodecConnectionChanged += OnCodecConnectionChanged;
        }

        public override void Connect()
        {
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
            Debug.Log(cmd, DebugAlertLevel.Debug);
            stream?.Write(cmd);
        }

        public override void SendCommand(XAPICommand cmd)
        {
            var command = cmd.CommandString();
            command += "\n";
            Debug.Log(command, DebugAlertLevel.Debug);
            stream?.Write(command);
        }

        private void OnDataRecieved(object sender, ShellDataEventArgs args)
        {            
            var resp = stream.Read();

            var handler = CodecResponseParseCallback;

            Debug.Log(resp, DebugAlertLevel.DebugComms);
            handler.Invoke(resp);
        }

        private void OnCodecConnectionChanged(object sender, EventArgs e)
        {




        }
    }
}
