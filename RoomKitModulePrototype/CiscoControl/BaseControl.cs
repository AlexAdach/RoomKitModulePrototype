using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class BaseControl
    {
        public delegate void SendCommandToCodecDelegate(XAPIBaseCommand command);
        public string[] Path { get; set; }
        public SendCommandToCodecDelegate SendCommandToCodecHandler { get; set; }
        protected virtual void OnControlStateChanged() { } 
        
    }
}
