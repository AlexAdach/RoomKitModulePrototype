using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace RoomKitModulePrototype
{
    public class SSHClientWrapper
    {
        public SshClientModified client;
        private ShellStream stream;
        private bool _connect;
        private int _connectionAttemps;


        private CodecCommunicationManager.CodecResponseParseHandler CodecResponseParseCallback;
        public SSHClientWrapper(string user, string password, string host, CodecCommunicationManager.CodecResponseParseHandler responseCallback) : base()
        {
            CodecResponseParseCallback = responseCallback;

            var kauth = new KeyboardInteractiveAuthenticationMethod(user);

            kauth.AuthenticationPrompt += delegate (object sender, AuthenticationPromptEventArgs e)
            {
                foreach (var prompt in e.Prompts)
                {
                    if (prompt.Request.ToLowerInvariant().StartsWith("password"))
                    {
                        prompt.Response = password;
                    }
                }
            };

            var connectionInfo = new ConnectionInfo(host, 22, user, kauth);
            client = new SshClientModified(connectionInfo);
            client.HostKeyReceived += (sender, e) => { e.CanTrust = true; };
            //client.SSHStatusChanged += OnCodecConnectionChanged;

        }
        public Task<bool> Send(XAPIBaseCommand command)
        {
            try
            {

                stream.WriteLine(command.CommandString());
                /*                var state = new object();
                                var expectTask = stream.BeginExpect(TimeSpan.FromMilliseconds(100), null, state, new ExpectAction(command.CommandString(), output => {
                                    Thread.CurrentThread.DebugThreadID("ExpectCallback");
                                    Debug.Log(output); }));

                                stream.EndExpect(expectTask);
                                test = false;
                                return Task.FromResult(true);*/
                stream.Expect(TimeSpan.FromMilliseconds(100), new ExpectAction(command.CommandString(), output =>
                {
                    Thread.CurrentThread.DebugThreadID("ExpectCallback");
                    Debug.Log(output);
                }));
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to send command over SSH.");
                return Task.FromResult(false);
            }

        }
        public async void ConnectSSH()
        {
            _connect = true;
            _connectionAttemps = 0;
            while (_connect)
            {
                Debug.Log("Reconnect Loop Start");
                _connectionAttemps++;
                var success = await ConnectASync();
                if (success)
                {
                    Debug.Log("Creating Shell Stream");
                    stream = client.CreateShellStream("Default", 0, 0, 0, 0, 4096);
                    stream.DataReceived += OnStreamDataReceived;
                    break;
                }
                Thread.Sleep(100);
            }
            Debug.Log("Loop Exited");
        }

        private Task<bool> ConnectASync()
        {
            try
            {
                client.Connect();
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Debug.Log($"Error connecting SSH. Attempt: {_connectionAttemps} - {e.Message}");
            }
            return Task.FromResult(client.IsConnected);
        }
        public void DisconnectSSH()
        {
            _connect = false;
            try
            {
                stream.Dispose();
                client.Disconnect();
            }
            catch (Exception e)
            {
                Debug.Log($"Error disconnecting SSH - {e.Message}");
            }
        }
        private void OnStreamDataReceived(object sender, ShellDataEventArgs args)
        {
            string result = Encoding.UTF8.GetString(args.Data);
            //string resultString = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            var resultString = result;
            Thread.CurrentThread.DebugThreadID("On Stream Received");
            if (true)
            {
                var handler = CodecResponseParseCallback;
                handler.Invoke(resultString);
            }
        }
    }
}
