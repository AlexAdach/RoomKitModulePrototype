using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RoomKitModulePrototype
{
    public delegate void CodecResponseParseHandler(string response);
    public class CommandModule : BaseModule
    {
        public event EventHandler<InterModuleEventArgs> CommandModuleMessageSent = delegate { };
        private ICodecCommunication _codec;
        private System.Timers.Timer _pollTimer;

        #region CodecStatus
        private bool _codecConnected;
        private bool _codecLoggedIn;
        private bool codecConnected
        {
            get
            { return _codecConnected; }
            set
            {
                if (_codecConnected != value)
                {
                    _codecConnected = value;
                    if (value == false)
                    {
                        CodecConnectionStatusChanged();
                    }
                }
            }
        }
        private bool codecLoggedIn
        {
            get
            { return _codecLoggedIn; }
            set
            {
                if (_codecLoggedIn != value)
                {
                    _codecLoggedIn = value;
                    if (value == true)
                    {
                        OnCodecLoggedIn();
                    }
                }
            }
        }
        #endregion CodecStatus

        #region Credentials
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        #endregion Credentials
        public CommandModule()
        {
            _pollTimer = new System.Timers.Timer(10000);
            _pollTimer.Elapsed += OnCodecHeartBeat;
            _pollTimer.AutoReset = true;

        }
        public void Initialize(string id)
        {
            ModuleID = id;
            dispatcher.RegisterModule(this);
            _codec = new SSH(User, Password, Host);
            _codec.CodecResponseParseCallback = ResponseDataHandler;
            _codec.Connect();
        }

        #region Poll&HeartBeat
        private void OnCodecLoggedIn()
        {
            Debug.Log($"Command Module {ModuleID} - Codec Logged In!", DebugAlertLevel.Debug);

            Tx(ReturnPeripheralConnectCommand());
            _pollTimer.Enabled = true;
            CodecConnectionStatusChanged();
        }
        private void OnCodecHeartBeat(object source, ElapsedEventArgs e)
        {
            string macAddress = "01:02:03:03:05:06";

            var parameters = new Dictionary<string, string>()
            {
                {"ID", macAddress },
            };
            var heartbeat = new XAPIStateCommand(XAPICommandType.XCommand, new string[] { "Peripherals" }, "HeartBeat", parameters);
            //_codec.SendCommand(heartbeat);

        }
        private XAPIStateCommand ReturnPeripheralConnectCommand()
        {
            string hardwareInfo = "1";
            string macAddress = "01:02:03:03:05:06";
            string deviceName = "Crestron";
            string deviceIP = "192.168.0.143";
            string serial = "123456789";
            string softwareInfo = ".NET 4.7.2 Console App";
            string type = "ControlSystem";

            //var cmdstring = $"xCommand Peripherals Connect HardwareInfo: {hardwareInfo} ID: {macAddress} Name: {deviceName} NetworkAddress: {deviceIP} SerialNumber: {serial} SoftwareInfo: {softwareInfo} Type: {type}";

            var parameters = new Dictionary<string, string>()
            {
                { "HardwareInfo", hardwareInfo },
                {"ID", macAddress },
                {"Name", deviceName },
                {"NetworkAddress", deviceIP },
                {"SerialNumber", serial },
                {"SoftwareInfo", softwareInfo },
                {"Type", type }
            };

            return new XAPIStateCommand(XAPICommandType.XCommand, new string[] { "Peripherals" }, "Connect", parameters);

        }
        #endregion Poll&HeartBeat

        private void Tx(string cmd) { _codec.CommandQueue.Add(cmd); }
        private void Tx(XAPIBaseCommand cmd) { _codec.CommandQueue.Add(cmd.CommandString()); }
        private void ResponseDataHandler(string responseData)
        {
            //Thread.CurrentThread.DebugThreadID("ParseThread");

            Debug.Log("<<<<RAW Response Start>>>");
            Debug.Log(responseData);
            Debug.Log("<<<<RAW Response End>>>\n");


            codecConnected = true;
            if (!codecLoggedIn)
            {
                ParseNonJson(responseData);
            }
            else if (codecLoggedIn)
            {
                var separatedJsons = responseData.SeparateJSON();
                
                foreach (var json in separatedJsons)
                {
                    var trimmed = json.TrimStart();
                    if (trimmed.ValidateJSON())
                    {
                        var responseObject = JObject.Parse(trimmed, new JsonLoadSettings { LineInfoHandling = 0 }).GenerateResponseObject();
                        CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(responseObject));
                    }
                    else
                    {
                        ParseNonJson(json);
                        break;
                    }
                }
            }
            _codec.CommandLock.Set();
            var cmdnum = _codec.CommandQueue.Count;
            Debug.Log(cmdnum.ToString());

        }
        private void ParseNonJson(string response)
        {
            //Debug.Log($"String Response: {response}");

            if(response.Contains("Login successful"))
                Tx("xPreferences outputmode json");

            if (response.Contains("xPreferences outputmode json"))
                codecLoggedIn = true;

        }



        #region Events
        /// <summary>
        /// This method handles all messages received from child modules.
        /// </summary>
        public void LogicModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Debug.Log("Command Module received message from logic module.", DebugAlertLevel.DebugCode);
            if (args.Message is XAPIBaseCommand msg)
                Tx(msg);
        }

        private void CodecConnectionStatusChanged()
        {
            var status = new CodecCommStatusDTO();

            status.CodecConnected = codecConnected;
            status.CodecLoggedIn = codecLoggedIn;
            //Debug.Log($"Codec Status {status.CodecConnected} {status.CodecLoggedIn}");
            CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(status));
        }
        #endregion Events


    }
}
