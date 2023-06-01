using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public sealed class AudioMicrophonesMute : CiscoStateFull<string[]>
    {
        public AudioMicrophonesMute(SendCommandToCodecDelegate callback)
        {
            _path = new string[] { "Audio", "Microphones" };
            _states = new string[] { "On", "Off" };
            _stateArgument = "Mute";
            SendCommandToCodecCallback = callback;
            IsRegisterFeedback = true;
        }

        public void MuteOn() { SendCommand(new XAPIxCommand(_path, "Mute")); }

        public void MuteOff() { SendCommand(new XAPIxCommand(_path, "Unmute")); }

        public void ToggleMute() { SendCommand(new XAPIxCommand(_path, "ToggleMute")); }

    }
}
