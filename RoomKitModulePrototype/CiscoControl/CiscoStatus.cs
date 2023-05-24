using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RoomKitModulePrototype
{
    public class CiscoStatus : BaseControl, ICiscoStatus
    {
        private string _currentStateString;
        public string StatusArgument { get; set; }
        public string[] FeedbackStates { get; set; }
        public bool ShouldRegisterFeedback { get; set; }
        public int CurrentStatusIndex { get { return Array.IndexOf(FeedbackStates, _currentStateString); } }
        public string CurrentStatusString
        {
            get { return _currentStateString; }
            private set { _currentStateString = value; 
                //NotifySimplPlusHandler.Invoke(CurrentStatusString, CurrentStatusIndex); 
            }
        }
        public NotifySimplPlusDelegate NotifySimplPlusHandler { get; set; }
        void ICiscoStatus.GetState()
        {
            var cmd = new XAPIStateCommand(XAPICommandPrefix.XStatus, Path, StatusArgument);
            SendCommandToCodecHandler.Invoke(cmd);
        }
        void ICiscoStatus.RegisterFeedback()
        {
            var cmd = new XAPIStateCommand(XAPICommandPrefix.XFeedbackRegister, Path, StatusArgument);
            SendCommandToCodecHandler.Invoke(cmd);
        }
        void ICiscoStatus.CheckStatusResponse(XAPIStatusResponse xapiStatus)
        {
            if (xapiStatus.Path.SequenceEqual(Path) && xapiStatus.StateArgument == StatusArgument)
            {
                if (FeedbackStates.Contains(xapiStatus.Value))
                {
                    Debug.Log($"{string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument} {xapiStatus.Value}");
                    var i = Array.IndexOf(FeedbackStates, xapiStatus.Value);
                    CurrentStatusString = xapiStatus.Value;
                }
                else
                {
                    Debug.Log($"Value: {xapiStatus.Value} is not a known value for: {string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument}");
                    CurrentStatusString = "";
                }
            }
        }
        protected override void OnControlStateChanged()
        {
            base.OnControlStateChanged();
        }
    }
}
