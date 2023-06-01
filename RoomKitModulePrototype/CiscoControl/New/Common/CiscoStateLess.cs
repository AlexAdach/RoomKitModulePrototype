using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoStateLess : CiscoBase
    {
        protected override SendCommandToCodecDelegate SendCommandToCodecCallback { get; set; }
        protected override void SendCommand(XAPIBaseCommand cmd)
        {
            if (SendCommandToCodecCallback != null)
                SendCommandToCodecCallback.Invoke(cmd);
        }
    }
}
