using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CodecCommunicationStatusDTO : BaseDTO
    {
        public bool CodecConnected { get; set; }
        public bool CodecLoggedIn { get; set; }
        public bool ConnectionConfigured { get; set; }
    }
}
