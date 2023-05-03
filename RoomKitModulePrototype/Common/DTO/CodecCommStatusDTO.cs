using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CodecCommStatusDTO : BaseDTO
    {
        public bool CodecConnected { get; set; }
        public bool CodecLoggedIn { get; set; }
    }
}
