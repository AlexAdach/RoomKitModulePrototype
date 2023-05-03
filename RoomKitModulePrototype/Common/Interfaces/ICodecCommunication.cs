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
        public void SendCommand(string cmd);
        public void SendCommand(XAPICommandDTO cmd);

        //public event EventHandler<CodecCommunicationEventArgs> CodecCommStatusChanged;
    }
}
