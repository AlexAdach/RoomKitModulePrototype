using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class ModuleRegistrationEventArgs<T> : EventArgs
    {
        private T _module;
        public T Module { get { return _module; } }

        public ModuleRegistrationEventArgs(T module)
        {
            _module = module;
        }
    }
}
