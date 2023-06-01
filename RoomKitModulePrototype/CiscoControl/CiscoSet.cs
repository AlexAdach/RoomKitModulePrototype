using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoSet : BaseControl
    {
        private XAPICommandPrefixEnum _prefix;
        public CiscoSet(XAPICommandPrefixEnum prefix = XAPICommandPrefixEnum.XCommand)
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
