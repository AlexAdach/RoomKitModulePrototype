using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public abstract class XAPIBaseCommand : BaseDTO
    {
        public abstract string CommandString();
    }
}
