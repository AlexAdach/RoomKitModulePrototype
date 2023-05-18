using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public interface ICodecCommunication
    {
        //public bool Initialized { get; }

        public CodecResponseParseHandler CodecResponseParseCallback { get; set; }
        public void Connect();
        public void SendString(string cmd);
        public void SendCommand(XAPIBaseCommand cmd);

        //public event EventHandler<CodecCommunicationEventArgs> CodecCommStatusChanged;
    }
}
