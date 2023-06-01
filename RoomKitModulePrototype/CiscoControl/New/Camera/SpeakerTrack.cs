using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class SpeakerTrack : CiscoStateFull<string[]>
    {

        public SpeakerTrack(SendCommandToCodecDelegate callback)
        {
            _path = new string[] { "Cameras", "SpeakerTrack" };
            _states = new string[] { "Active", "Inactive" };
            _stateArgument = "Status";
            SendCommandToCodecCallback = callback;
            IsRegisterFeedback = true;
        }
        public void SpeakerTrackOn() { SendCommand(new XAPIxCommand(_path, "Activate")); }
        public void SpeakerTrackOff() { SendCommand(new XAPIxCommand(_path, "Deactivate")); }

    }
}

