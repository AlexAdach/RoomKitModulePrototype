using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPICommandDTO : BaseDTO
    {
        public string Command { get; set; }
        public string[] Path { get; set; }
        public string Argument { get; set; }
        public string CommandString { get; set; }
    }
}
