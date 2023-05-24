using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoSet : BaseControl
    {
        private XAPICommandPrefix _prefix;
        public CiscoSet(XAPICommandPrefix prefix = XAPICommandPrefix.XCommand)
        {
            _prefix = prefix;
        }
        public string[] States { get; set; }
        public void SetState(ushort state)
        {
            if (States[state] != null)
            {
                var cmd = new XAPIStateCommand(_prefix, Path, States[state]);
                SendCommandToCodecHandler.Invoke(cmd);
            }
        }

    }
}
