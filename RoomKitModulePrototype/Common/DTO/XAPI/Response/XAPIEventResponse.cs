using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIEventResponse : XAPIBaseResponse
    {
        public string EventName { get; set; }
        public string EventValue { get; set; }
    }
}
