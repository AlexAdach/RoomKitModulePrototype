using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public abstract class CiscoBase
    {
        public delegate void SendCommandToCodecDelegate(XAPIBaseCommand command);
        protected abstract SendCommandToCodecDelegate SendCommandToCodecCallback { get; set; }
        protected string[] _path;
        protected abstract void SendCommand(XAPIBaseCommand cmd);
    }
}
