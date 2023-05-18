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
                        OnCodecLoggedIn();
                        CodecConnectionStatusChanged();

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
            _codec.CodecResponseParseCallback = ResponseDataHandler;
            _codec.Connect();
        }

        #region Poll&HeartBeat
        private void OnCodecLoggedIn()
        {
            _codec.SendString("xPreferences outputmode json");
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


        private void ResponseDataHandler(string responseData)
        {
            Debug.Log("ResponseDataHandler");
            codecConnected = true;
            if (!codecLoggedIn && responseData.Contains("Login successful"))
            {
                codecLoggedIn = true;
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
        }
        private void ParseNonJson(string response)
        {
            Debug.Log($"String Response: {response}");
        }
        private void ParseJson(string responseJSON)
        {
            codecConnected = true;

            if (responseJSON.Contains("Login successful") && !codecLoggedIn)
            {
                //*******CHANGE THIS
                codecLoggedIn = true;
            }
            else if (codecLoggedIn && Extensions.ValidateJSON(responseJSON))
            {
                JObject parsedJSON = JObject.Parse(responseJSON, new JsonLoadSettings { LineInfoHandling = 0 });

                if (parsedJSON["CommandResponse"] != null)
                {
                    Debug.Log($"Command Module {ModuleID} - Command Response Received from Codec.", DebugAlertLevel.DebugComms);
                    var cmdRsp = (JObject)parsedJSON["CommandResponse"];

                    var name = cmdRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                        .OfType<JProperty>()             // For those which are properties
                        .Select(p => p.Name)             // Select the name
                        .FirstOrDefault();               // And take the first.

                    var result = new XAPICommandResponse();
                    result.CommandResponse = name;
                    CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(result));

                }
                else if (parsedJSON["Event"] != null)
                {
                    Debug.Log($"Command Module {ModuleID} - Event Received from Codec.", DebugAlertLevel.DebugComms);

                    var eventRsp = (JObject)parsedJSON["Event"].First.First;

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
                else if (parsedJSON["Status"] != null)
                {

                    Debug.Log($"Command Module {ModuleID} - Status Received from Codec.", DebugAlertLevel.DebugComms);


                    // Parse JSON string
                    //JObject jsonObject = JObject.Parse(json);
                    var statusHeirachy = parsedJSON.Descendants().OfType<JProperty>();
                    statusHeirachy = statusHeirachy.Reverse();

                    string value = statusHeirachy.First().Value.Value<string>();

                    statusHeirachy = statusHeirachy.Skip(1);
                    string stateArgProperty = statusHeirachy.First().Name;
                    statusHeirachy = statusHeirachy.Skip(1);

                    List<string> pathlist = new List<string>();

                    while (statusHeirachy.Count() > 1)
                    {
                        pathlist.Add(statusHeirachy.First().Name);
                        statusHeirachy = statusHeirachy.Skip(1);
                    }

                    var statusResponse = new XAPIStatusResponse();

                    statusResponse.StateArgument = stateArgProperty;
                    statusResponse.Value = value;
                    pathlist.Reverse();
                    statusResponse.Path = pathlist.ToArray();

                    CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(statusResponse));

                }
            }
        }
        private List<string> SeparateJSON(string receivedData)
        {
            List<string> jsonList = new List<string>();
            StringBuilder buffer = new StringBuilder();

            int openingBrackets = 0;  // Count of opening brackets '{'
            int closingBrackets = 0;  // Count of closing brackets '}'

            // Loop through the received data
            foreach (char c in receivedData)
            {
                buffer.Append(c);
                if (c == '{')
                {
                    openingBrackets++;
                }
                else if (c == '}')
                {
                    closingBrackets++;

                    if (openingBrackets == closingBrackets)
                    {
                        // Complete JSON response
                        string json = buffer.ToString();
                        jsonList.Add(json);

                        // Clear the buffer and reset bracket counts for the next response
                        buffer.Clear();
                        openingBrackets = 0;
                        closingBrackets = 0;
                    }
                }
            }
            // Handle any remaining data in the buffer (in case a complete response is not received yet)
            if (buffer.Length > 0)
            {
                string json = buffer.ToString();
                if(!string.IsNullOrWhiteSpace(json))
                {
                    Debug.Log($"Buffer: {json}");
                    jsonList.Add(json);
                }
            }
            Debug.Log(jsonList.Count().ToString());
            return jsonList;
        }

        #region Events

        /// <summary>
        /// This method handles all messages received from child modules.
        /// </summary>
        public void LogicModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Debug.Log("Command Module received message from logic module.", DebugAlertLevel.DebugCode);
            if (args.Message is XAPIBaseCommand msg)
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
        #endregion Events


    }
}
