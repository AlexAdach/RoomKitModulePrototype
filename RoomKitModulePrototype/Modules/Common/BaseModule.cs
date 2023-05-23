using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RoomKitModulePrototype
{
    public class BaseModule
    {
        protected static readonly Dispatcher dispatcher = new Dispatcher();
        public string ModuleID { get; set; }
        public string ModuleType { get; protected set; }
        public BaseModule() {  }
        public virtual void Initialize(string id)
        {
            ModuleID = id;
            //
        }
        protected virtual void SendCommandToCodec(XAPIBaseCommand command) {  }

    }
}
