using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    [Flags]
    public enum DebugAlertLevel
    {
        None = 0,
        Error = 1,
        Debug = 2,
        DebugCode = 4,
        DebugComms = 8,
    }
}
