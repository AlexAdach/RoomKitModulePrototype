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
        private ICodecCommunication _codec;
        private System.Timers.Timer _pollTimer;
        public event EventHandler<InterModuleEventArgs> CommandModuleMessageSent = delegate { };

        CiscoValue PeripheralConnect;

        #region CodecStatus
        private bool _codecConnected;
        private bool _codecLoggedIn;
        private bool _codecJSONSet;
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
                        BroadcastCodecStatus();
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

            PeripheralConnect = new CiscoValue()
            {
                Path = new string[] { "Peripherals" },
                StatusArgument = null,
                FeedbackStates = null,
                ShouldRegisterFeedback = false,
                SetValueArgument = "Connect",
                ValueParameters = new CiscoParameter[]
                {
                    new CiscoParameter("HardwareInfo", new string[] { "1" }),
                    new CiscoParameter("ID", new string[] { "01:02:03:03:05:06" }),
                    new CiscoParameter("Name", new string[] { "Crestron" }),
                    new CiscoParameter("NetworkAddress", new string[] { "192.168.0.143" }),
                    new CiscoParameter("SerialNumber", new string[] { "123456789" }),
                    new CiscoParameter("SoftwareInfo", new string[] { ".NET 4.7.2 Console App" }),
                    new CiscoParameter("Type", new string[] { "ControlSystem" }),
                },
                SendCommandToCodecHandler = SendCommandToCodec

            };

        }
        public override void Initialize(string id)
        {
            base.Initialize(id);
            dispatcher.RegisterModule(this);
            _codec = new SSHHandler(User, Password, Host);
            _codec.CodecResponseParseCallback = ParseCodecResponse;
            _codec.CodecConnectionChanged += OnCodecConnectionChanged;
            _codec.Connect();
        }

        #region Poll&HeartBeat
        private void OnCodecLoggedIn()
        {
            Debug.Log($"Command Module {ModuleID} - Codec Logged In!", DebugAlertLevel.Debug);

            PeripheralConnect.SetValue(new int[,] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 } });
            _pollTimer.Enabled = true;
            BroadcastCodecStatus();
        }
        private void OnCodecHeartBeat(object source, ElapsedEventArgs e)
        {
            string macAddress = "01:02:03:03:05:06";
            var parameters = new Dictionary<string, string>()
            {
                {"ID", macAddress },
            };
            var heartbeat = new XAPIStateCommand(XAPICommandType.XCommand, new string[] { "Peripherals" }, "HeartBeat", parameters);
        }
        #endregion Poll&HeartBeat
        private void Tx(string cmd) { _codec.CommandQueue.Add(cmd); }
        private void Tx(XAPIBaseCommand cmd) { _codec.CommandQueue.Add(cmd.CommandString()); }
        private void ParseCodecResponse(string responseData)
        {
            Debug.Log("<<<<RAW Response Start>>>");
            Debug.Log(responseData);
            Debug.Log("<<<<RAW Response End>>>");

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

        }
        private void ParseNonJson(string response)
        {
            //Debug.Log($"String Response: {response}");

            if (response.Contains("Login successful"))
            {
                Tx("Echo Off");
                Tx("xPreferences outputmode json");
                codecLoggedIn = true;
            }
            //if (response.Contains("xPreferences outputmode json"))
                

        }
        protected override void SendCommandToCodec(XAPIBaseCommand command)
        {
            Tx(command);
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
        private void OnCodecConnectionChanged(object sender, CodecConnectionStatusEventArgs args)
        {
            codecConnected = args.CodecConnected;
        }
        private void BroadcastCodecStatus()
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
