using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public abstract class XAPICommandBase
    {
        public abstract XAPICommandDTO GetCommand(string arg = "");

    }
}
