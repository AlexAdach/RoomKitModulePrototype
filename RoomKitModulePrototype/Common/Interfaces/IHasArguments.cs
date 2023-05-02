using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace RoomKitModulePrototype
{
    interface IHasArguments
    {
        StringCollection CommandArgs { get; }
    }
}
