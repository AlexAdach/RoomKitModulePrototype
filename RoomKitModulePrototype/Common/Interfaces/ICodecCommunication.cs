using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace RoomKitModulePrototype
{
    public interface ICodecCommunication
    {
        //public bool Initialized { get; }
        public event EventHandler<CodecConnectionStatusEventArgs> CodecConnectionChanged;
        public CodecResponseParseHandler CodecResponseParseCallback { get; set; }
        public AutoResetEvent CommandLock { get; }
        public BlockingCollection<string> CommandQueue { get; }
        public void Connect();

        //public void SendString(string cmd);
        //public void SendCommand(XAPIBaseCommand cmd);

        //public event EventHandler<CodecCommunicationEventArgs> CodecCommStatusChanged;
    }
}
