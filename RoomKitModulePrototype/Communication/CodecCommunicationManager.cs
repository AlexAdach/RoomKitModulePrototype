using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoomKitModulePrototype
{
    public class CodecCommunicationManager
    {
        public delegate void CodecResponseParseHandler(string response);
        public delegate void CodecCommManagerCallback(BaseDTO data);

        private BlockingCollection<XAPIBaseCommand> commandQueue = new BlockingCollection<XAPIBaseCommand>();

        private CodecCommManagerCallback toCommandModule;

        private SSHClientWrapper _ssh;
        private bool _codecLoggedIn;
        private bool _codecConnected;
        private bool _connectionConfigured;

        public bool CodecLoggedIn { get { return _codecLoggedIn; } private set { _codecLoggedIn = value; NotifyStatusChange(); } }
        public bool CodecConnected { get { return _codecConnected; } private set { _codecConnected = value; NotifyStatusChange(); } }
        public bool ConnectionConfigured { get { return _connectionConfigured; } private set { _connectionConfigured = value; NotifyStatusChange(); } }
        //public bool CommManagerReady { get; private set; }

        public CodecCommunicationManager(CodecCommManagerCallback callback)
        {
            toCommandModule = callback;
            _ = Task.Run(() => ProcessCommands());
        }

        private async void ProcessCommands()
        {
            foreach (XAPIBaseCommand command in commandQueue.GetConsumingEnumerable())
            {
                Debug.Log($"Sending Command: {command.CommandString()}");
                bool sendTask = await Task.Run(() => _ssh.Send(command));
            }

        }
        public void CodecTx(XAPIBaseCommand command)
        {
            Debug.Log("Adding command to Queue");
            _ = Task.Run(() => commandQueue.Add(command));
        }

        public void CodecRx(string response)
        {
            ParseCodecResponse(response);
        }

        public void InitializeSSH(string user, string password, string host)
        {
            _ssh = new SSHClientWrapper(user, password, host, new CodecResponseParseHandler(CodecRx));
            _ssh.client.SSHStatusChanged += OnSSHConnectionStatusChanged;
            _ssh.ConnectSSH();
            //CommManagerReady = true;
        }
        private void ParseCodecResponse(string responseData)
        {


            Debug.Log($"<<<<RAW Response Start>>>\n{responseData}<<<<RAW Response End>>>\n");


            var separatedJsons = responseData.SeparateJSON();
            foreach (var json in separatedJsons)
            {
                var trimmed = json.TrimStart();
                if (trimmed.ValidateJSON())
                {
                    var responseObject = JObject.Parse(trimmed, new JsonLoadSettings { LineInfoHandling = 0 }).GenerateResponseObject();
                    toCommandModule.Invoke(responseObject);
                }
                else
                {
                    var response = json;

                    if (response.Contains("Login successful"))
                    {
                        CodecLoggedIn = true;
                    }
                    if (response.Contains("xPreferences outputmode json"))
                    {
                        ConnectionConfigured = true;
                    }
                }
            }
        }


        private void NotifyStatusChange()
        {
            var status = new CodecCommunicationStatusDTO();
            status.CodecConnected = CodecConnected;
            status.CodecLoggedIn = CodecLoggedIn;
            status.ConnectionConfigured = ConnectionConfigured;

            Debug.Log($"Codec Connected: {CodecConnected}");
            Debug.Log($"Codec Logged In: {CodecLoggedIn}");
            Debug.Log($"Codec Configured: {ConnectionConfigured} \n");

            toCommandModule.Invoke(status);
        }
        private void OnSSHConnectionStatusChanged(object sender, CodecConnectionStatusEventArgs args)
        {
            CodecConnected = args.CodecConnected;
            //Debug.Log($"SSH Connection Status: {args.CodecConnectionStatus}");
        }
    }
}
