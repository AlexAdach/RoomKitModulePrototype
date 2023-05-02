using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public interface ICoreModule
    {
        public void CodecResponseRecieved(BaseDTO response);
    }
}
