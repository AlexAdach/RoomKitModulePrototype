using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RoomKitModulePrototype
{
    public class CiscoStateControl : BaseControl, ICiscoStateControl
    {
        public bool ShouldRegisterFeedback { get; set; }
        public string StateArgument { get; set; }
        public string[] States { get; set; }
        public string[] FeedbackStates { get; set; }
        public int CurrentStateIndex { get; private set; }
        public string CurrentStateString { get; private set; }


        public void SetState(ushort state)
        {
            if (States[state] != null)
            {
                var cmd = new XAPICommand(XAPICommandType.XCommand, Path, States[state]);
                SendCommandToCodecHandler.Invoke(cmd);
            }
        }

        void ICiscoStateControl.GetState()
        {
            var cmd = new XAPICommand(XAPICommandType.XStatus, Path, StateArgument);
            SendCommandToCodecHandler.Invoke(cmd);
        }
        void ICiscoStateControl.RegisterFeedback()
        {
            var cmd = new XAPICommand(XAPICommandType.XFeedbackRegister, Path, StateArgument);
            SendCommandToCodecHandler.Invoke(cmd);
        }

        void ICiscoStateControl.CheckEventResult(XAPIEventResponse xapiEvent)
        {

        }

        void ICiscoStateControl.CheckStatusResponse(XAPIStatusResponse xapiStatus)
        {
            if(xapiStatus.Path.SequenceEqual(Path) && xapiStatus.StateArgument == StateArgument )
            {
                if(FeedbackStates.Contains(xapiStatus.Value))
                {
                    Debug.Log($"{string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument} {xapiStatus.Value}");
                    var i = Array.IndexOf(FeedbackStates, xapiStatus.Value);
                    CommitCurrentState(xapiStatus.Value, i);
                }
                else
                {
                    Debug.Log($"Value: {xapiStatus.Value} is not a known value for: {string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument}");
                }
            }
        }

        private void CommitCurrentState(string state, int istate)
        {
            CurrentStateString = state;
            CurrentStateIndex = istate;
        }
    }
}
