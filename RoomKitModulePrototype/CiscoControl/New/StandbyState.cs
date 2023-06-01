using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class StandbyState : CiscoStateFull<string[]>
    {
        public StandbyState(SendCommandToCodecDelegate callback)
        { 
            _path = new string[] { "Standby" };
            _states = new string[] { "Standby", "Off", "Halfwake" };
            _stateArgument = "State";
            SendCommandToCodecCallback = callback;
            IsRegisterFeedback = true;
        }

        public void StandbyOn() { { SendCommand(new XAPIxCommand(_path, "Activate")); } }
        public void StandbyOff() { { SendCommand(new XAPIxCommand(_path, "Deactivate")); } }

    }
}
