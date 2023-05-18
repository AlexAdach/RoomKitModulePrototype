using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public interface ICiscoStatus
    {
        public bool ShouldRegisterFeedback { get; set; }
        public void GetState();

        public void RegisterFeedback();

        //public void CheckEventResult(XAPIEventResponse xapiEvent);
        public void CheckStatusResponse(XAPIStatusResponse xapiStatus);

    }
}
