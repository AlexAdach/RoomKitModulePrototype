using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class ConfigEchoMode : CiscoStateLess
    {

        public ConfigEchoMode(SendCommandToCodecDelegate callback)
        {
            SendCommandToCodecCallback = callback;
        }

        public void EchoModeOn() { }

    }
}
