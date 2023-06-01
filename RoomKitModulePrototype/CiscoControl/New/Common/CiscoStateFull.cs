using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoStateFull<T> : CiscoBase, IState
    {
        protected T _states;
        protected string _stateArgument;
        protected string _currentStateString;
        protected string CurrentStateString { get { return _currentStateString; } private set { _currentStateString = value; } }
        public bool IsRegisterFeedback { get; protected set; }
        protected override SendCommandToCodecDelegate SendCommandToCodecCallback { get; set; }
        protected override void SendCommand(XAPIBaseCommand cmd)
        {
            if (SendCommandToCodecCallback != null)
                SendCommandToCodecCallback.Invoke(cmd);
        }
        void IState.GetState()
        {
            { SendCommand(new XAPIxStatus(_path, _stateArgument)); }
        }
        void IState.CheckStatusResponse(XAPIStatusResponse xapiStatus)
        {
            if (xapiStatus.Path.SequenceEqual(_path) && xapiStatus.StateArgument == _stateArgument)
            {
                if(_states is string[])
                {
                    string[] sStringArray = (string[])(object)_states;
                    if (sStringArray.Contains(xapiStatus.Value))
                    {
                        Debug.Log($"{string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument} {xapiStatus.Value}");
                        var i = Array.IndexOf(sStringArray, xapiStatus.Value);
                        CurrentStateString = xapiStatus.Value;
                    }
                    else
                    {
                        Debug.Log($"Value: {xapiStatus.Value} is not a known value for: {string.Join(" ", xapiStatus.Path)} {xapiStatus.StateArgument}");
                        CurrentStateString = "";
                    }

                }
                
            }
        }
        void IState.RegisterFeedback() { SendCommand(new XAPIxFeedbackRegister(_path, _stateArgument)); }
    }
}
