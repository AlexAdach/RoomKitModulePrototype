using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIStatusResponse : XAPIBaseResponse
    {
        public string StateArgument { get; set; }
        public string Value { get; set; }
        public string[] Path { get; set; }

    }
}
