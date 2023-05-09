using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace RoomKitModulePrototype
{
    public delegate void CodecResponseParseHandler(string response);
    public class CommandModule : BaseModule
    {
        public event EventHandler<InterModuleEventArgs> CommandModuleMessageSent = delegate { };
        private ICodecCommunication _codec;

        private Timer _pollTimer;

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
                        CodecConnectionStatusChanged();
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
            _pollTimer = new Timer(10000);
            _pollTimer.Elapsed += OnCodecHeartBeat;
            _pollTimer.AutoReset = true;

        }
        public void Initialize(string id)
        {
            ModuleID = id;
            dispatcher.RegisterModule(this);
            _codec = new SSH(User, Password, Host);
            _codec.CodecResponseParseCallback = ResponseRouter;
            _codec.Connect();
        }

        #region Poll&HeartBeat
        private void OnCodecLoggedIn()
        {
            _codec.SendCommand("xPreferences outputmode json");
            _codec.SendCommand(ReturnPeripheralConnectCommand());
            Debug.Log($"Command Module {ModuleID} - Codec Logged In!", DebugAlertLevel.Debug);
            _pollTimer.Enabled = true;
        }
        private void OnCodecHeartBeat(object source, ElapsedEventArgs e)
        {
            string macAddress = "01:02:03:03:05:06";

            var parameters = new Dictionary<string, string>()
            {
                {"ID", macAddress },
            };
            var heartbeat = new XAPICommand(XAPICommandType.XCommand, new string[] { "Peripherals" }, "HeartBeat", parameters);
            _codec.SendCommand(heartbeat);

        }
        private XAPICommand ReturnPeripheralConnectCommand()
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

            return new XAPICommand(XAPICommandType.XCommand, new string[] { "Peripherals" }, "Connect", parameters);

        }
        #endregion Poll&HeartBeat

        /// <summary>
        /// This method handles parsing the responses from the codec. Creating relevant DTOs for the types of responses, and sending them over 
        /// to the child modules.
        /// </summary>
        /// <param name="responseString"></param>
        private void ResponseRouter(string responseString)
        {
            codecConnected = true;
            if (responseString.Contains("Login successful") && !codecLoggedIn)
            {
                 //*******CHANGE THIS
                codecLoggedIn = true;
            }
            else if (codecLoggedIn && Extensions.ValidateJSON(responseString))
            {
                JObject responseJSON = JObject.Parse(responseString, new JsonLoadSettings { LineInfoHandling = 0 });

                if (responseJSON["CommandResponse"] != null)
                {
                    var cmdRsp = (JObject)responseJSON["CommandResponse"];

                    var name = cmdRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                        .OfType<JProperty>()             // For those which are properties
                        .Select(p => p.Name)             // Select the name
                        .FirstOrDefault();               // And take the first.


                    Debug.Log("This is a command response");
                    var result = new XAPICommandResponse();
                    result.CommandResponse = name;
                    CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(result));

                }
                else if(responseJSON["Event"]!= null)
                {
                    Debug.Log($"Command Module {ModuleID} - Event Received from Codec.", DebugAlertLevel.Debug);

                    var eventRsp = (JObject)responseJSON["Event"].First.First;

                    var eStatusName = eventRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                        .OfType<JProperty>()             // For those which are properties
                        .Select(p => p.Name)             // Select the name
                        .FirstOrDefault();               // And take the first.

                    JProperty eStatusValue = eventRsp.Descendants()
                        .OfType<JProperty>()
                        .FirstOrDefault(p => p.Name == "Value");

                    if (eStatusValue != null)
                    {
                        var eventResponse = new XAPIEventResponse();
                        eventResponse.EventName = eStatusName;
                        eventResponse.EventValue = eStatusValue.Value.Value<string>();

                        CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(eventResponse));
                    }
                    
                    


                    
                }
            }
        }

        /// <summary>
        /// This method handles all messages received from child modules.
        /// </summary>
        public void LogicModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Debug.Log("Command Module received message from logic module.", DebugAlertLevel.Debug);
            if (args.Message is XAPICommand msg)
                _codec.SendCommand(msg);
        }

        private void CodecConnectionStatusChanged()
        {
            var status = new CodecCommStatusDTO();

            status.CodecConnected = codecConnected;
            status.CodecLoggedIn = codecLoggedIn;
            //Debug.Log($"Codec Status {status.CodecConnected} {status.CodecLoggedIn}");
            CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(status));
        }


    }
}
