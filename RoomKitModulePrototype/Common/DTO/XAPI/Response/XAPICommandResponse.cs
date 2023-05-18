using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPICommandResponse : XAPIBaseResponse
    {
        public string CommandResponse { get; set; }
        public bool Success { get; set; }

    }
}
