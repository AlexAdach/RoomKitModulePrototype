using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public interface IState
    {
        public bool IsRegisterFeedback { get; }
        public void GetState();
        public void CheckStatusResponse(XAPIStatusResponse xapiStatus);
        public void RegisterFeedback();

    }
}
