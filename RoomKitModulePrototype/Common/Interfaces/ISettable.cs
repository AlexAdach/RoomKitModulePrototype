using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    interface ISettable<T> : IStatus
    {
        public string GetSetValueString(T arg);
        public void SetStatus(T args);

    }
}
