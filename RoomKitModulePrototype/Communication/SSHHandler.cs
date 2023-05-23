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
    public class SSHHandler : ICodecCommunication
    {
        #region Fields

        private CiscoSSHClient client;
        private ShellStream stream;

        #endregion Fields
        #region Properties
        public BlockingCollection<string> CommandQueue { get; } = new BlockingCollection<string>();
        public AutoResetEvent CommandLock { get; } = new AutoResetEvent(false);
        public CodecResponseParseHandler CodecResponseParseCallback { get; set; }
        public event EventHandler<CodecConnectionStatusEventArgs> CodecConnectionChanged;
        #endregion Properties
        public SSHHandler(string user, string password, string host) : base()
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
            client.SSHStatusChanged += OnCodecConnectionChanged;
        }
        #region Public Methods
        public void Connect()
        {
            try
            {
                client.Connect();
                stream = client.CreateShellStream("Default", 0, 0, 0, 0, 4096);
                stream.DataReceived += OnStreamDataReceived;
            }
            catch (Exception e)
            {
                Debug.Log($"Error connecting SSH - {e.Message}");
            }
        }
        public void Disconnect()
        {

        }
        #endregion Public Methods
        private void ProcessCommands()
        {
            foreach (string cmd in CommandQueue.GetConsumingEnumerable())
            {
                CommandLock.WaitOne(TimeSpan.FromSeconds(5));
                //Thread.CurrentThread.DebugThreadID("Process Commands Thread: ");
                Debug.Log($"Command sent: {cmd}");
                var cmddel = cmd + "\n";
                stream.Write(cmddel);
                Thread.Sleep(1000);
            }
        }
        private void OnStreamDataReceived(object sender, ShellDataEventArgs args)
        {

            string result = Encoding.UTF8.GetString(args.Data);
            string resultString = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            if (true)
            {
                var handler = CodecResponseParseCallback;
                handler.Invoke(resultString);
            }
        }
        private void OnCodecConnectionChanged(object sender, EventArgs e)
        {
            var handler = CodecConnectionChanged;
            handler?.Invoke(this, new CodecConnectionStatusEventArgs(client.IsConnected));
        }
    }
}
