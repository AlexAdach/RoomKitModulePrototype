using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public interface IMessageReceived<T>
    {
        void OnMessageRecieved(object sender, PublisherEventArgs<T> args);

    }
}
