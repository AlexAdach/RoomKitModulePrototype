using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public delegate void CodecResponseParseHandler(string response);
    public class CommandModule : BaseModule
    {
        //private List<LogicModule> _subscribers = new List<LogicModule>();
        public event EventHandler<InterModuleEventArgs> CommandModuleMessageSent = delegate { };
        private ICodecCommunication _codec;

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
            
        }
        public void Initialize(string id)
        {
            ModuleID = id;
            dispatcher.RegisterModule(this);
            _codec = new SSH(User, Password, Host);
            _codec.CodecResponseParseCallback = ResponseRouter;
            _codec.Connect();
        }

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
                Debug.Log("Comm Base - Logged In!");
                _codec.SendCommand("xPreferences outputmode json");
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
            }
        }

        /// <summary>
        /// This method handles all messages received from child modules.
        /// </summary>
        public void LogicModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Debug.Log("Command Module received message from logic module.", DebugAlertLevel.Debug);
            if (args.Message is XAPICommandDTO msg)
                _codec.SendCommand(msg);
        }

        private void CodecConnectionStatusChanged()
        {
            var status = new CodecCommStatusDTO();

            status.CodecConnected = codecConnected;
            status.CodecLoggedIn = codecLoggedIn;
            Debug.Log($"Codec Status {status.CodecConnected} {status.CodecLoggedIn}");
            CommandModuleMessageSent.Invoke(this, new InterModuleEventArgs(status));
        }
    }
}
