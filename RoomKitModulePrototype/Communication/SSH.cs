using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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

        ManualResetEvent mre = new ManualResetEvent(false);
        //TaskCompletionSource 

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


            client.HostKeyReceived += (sender, e) =>
            {
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
                stream.DataReceived += OnStreamDataReceived;
            }
            catch (Exception e)
            {
                Debug.Log($"Error connecting SSH - {e.Message}");
            }
        }

        public override void SendString(string cmd)
        {
            WriteToStreamAsync(cmd);
        }
        public override void SendCommand(XAPIBaseCommand cmd)
        {
            WriteToStreamAsync(cmd.CommandString());
        }

        
        private async void WriteToStreamAsync(string cmdstring)
        {
            /*            Debug.Log($"Command Sent: {cmd}", DebugAlertLevel.DebugComms);
                        cmd += "\n";
                        stream?.Write(cmd);
                        mre.WaitOne();*/

            cmdstring += "\n";
            Debug.Log(cmdstring);
            using (var shellStream = client.CreateShellStream("custom", 80, 24, 800, 600, 1024))
            {
                var outputBuilder = new StringBuilder();

                // Send initial command
                shellStream.WriteLine(cmdstring);

                // Register a callback to handle the continuous output asynchronously
                var outputWaitHandle = new AutoResetEvent(false);
                var outputReader = new StreamReader(shellStream);
                var outputTask = Task.Run(async () =>
                {
                    while (true)
                    {
                        string line = await outputReader.ReadLineAsync();
                        if (line == null)
                            break;
                        outputBuilder.AppendLine(line);

                        // Process the output as needed
                        Console.WriteLine(line);
                    }

                    outputWaitHandle.Set();
                });

                // Wait for the output reading to complete
                await Task.Run(() => outputWaitHandle.WaitOne());

                // Perform any additional cleanup or actions

                outputReader.Close();
            }

        }

        private void OnStreamDataReceived(object sender, ShellDataEventArgs args)
        {
            var receivedData = stream.Read();
            var handler = CodecResponseParseCallback;

            Debug.Log("<<<<RAW Response Start>>>");
            Debug.Log(receivedData.AddCharacterOnLineFeed());
            Debug.Log("<<<<RAW Response End>>>");

            if (!string.IsNullOrWhiteSpace(receivedData)) 
                handler.Invoke(receivedData);

            //mre.Set();
        }



        private void OnCodecConnectionChanged(object sender, EventArgs e)
        {




        }
    }
}
