using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CodecConnectionStatusEventArgs : EventArgs
    {
        private bool _codecConnected;
        private string _codecConnectionStatus;
        public bool CodecConnected { get { return _codecConnected; } }
        public string CodecConnectionStatus { get { return _codecConnectionStatus; } }
        public CodecConnectionStatusEventArgs(bool codecConnected, string codecConnectionStatus)
        {
            _codecConnected = codecConnected;
            _codecConnectionStatus = codecConnectionStatus;
        }
    }
}
