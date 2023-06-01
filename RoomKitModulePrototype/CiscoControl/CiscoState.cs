using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoState : CiscoStatus
    {
        public string[] States { get; set; }
        public void SetState(ushort state)
        {
            if (States[state] != null)
            {
                var cmd = new XAPIStateCommand(XAPICommandPrefixEnum.XCommand, Path, States[state]);
                SendCommandToCodecHandler.Invoke(cmd);
            }
        }
    }
}
