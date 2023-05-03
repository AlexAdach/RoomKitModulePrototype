using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class InterModuleEventArgs : EventArgs
    {
        private BaseDTO _message;
        public BaseDTO Message { get { return _message; } }
        public InterModuleEventArgs(BaseDTO message)
        {
            _message = message;
        }
    }
}
