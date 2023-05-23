using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CodecConnectionStatusEventArgs : EventArgs
    {
        private bool _codecConnected;
        public bool CodecConnected { get { return _codecConnected; } }
        
        public CodecConnectionStatusEventArgs(bool codecConnected)
        {
            _codecConnected = codecConnected;
        }
    }
}
