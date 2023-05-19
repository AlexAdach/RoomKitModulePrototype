using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;

namespace RoomKitModulePrototype
{
    public class SSH : ICodecCommunication
    {
        private CiscoSSHClient client;
        private ShellStream stream;

        public CodecResponseParseHandler CodecResponseParseCallback { get; set; }
        public BlockingCollection<string> CommandQueue { get; } = new BlockingCollection<string>();
        public AutoResetEvent CommandLock { get; } = new AutoResetEvent(false);

        public SSH(string user, string password, string host) : base()
        {
            Task.Run(() => ProcessCommands());
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
            client = new CiscoSSHClient(connectionInfo);
            client.HostKeyReceived += (sender, e) => { e.CanTrust = true; };
            client.OnCodecConnectionChanged += OnCodecConnectionChanged;
        }
        private void ProcessCommands()
        {
            foreach (string cmd in CommandQueue.GetConsumingEnumerable())
            {
                CommandLock.WaitOne(TimeSpan.FromSeconds(5));
                Thread.CurrentThread.DebugThreadID("Process Commands Thread: ");
                Debug.Log($"Command sent: {cmd}");
                var cmddel = cmd + "\n";
                stream.Write(cmddel);
                
            }
        }
        public void Connect()
        {
            try
            {
                client.Connect();
                stream = client.CreateShellStream("Default", 0, 0, 0, 0, 1024);
                stream.DataReceived += OnStreamDataReceived;
            }
            catch (Exception e)
            {
                Debug.Log($"Error connecting SSH - {e.Message}");
            }
        }
        private void OnStreamDataReceived(object sender, ShellDataEventArgs args)
        {
            var receivedData = stream.Read().RemoveBlankLines();
            var handler = CodecResponseParseCallback;
                handler.Invoke(receivedData);
        }
        private void OnCodecConnectionChanged(object sender, EventArgs e)
        {




        }
    }
}
