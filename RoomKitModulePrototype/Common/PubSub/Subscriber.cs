using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public abstract class Subscriber<T> : IMessageReceived<T>
    {
        protected Guid ID { get; private set; }
        public string CoreModuleId { get; protected set; }

        protected Subscriber()
        {
            ID = Guid.NewGuid();
        }

        void IMessageReceived<T>.OnMessageRecieved(object sender, PublisherEventArgs<T> args)
        {
            OnMessageRecieved(sender, args);
        }

        protected abstract void OnMessageRecieved(object sender, PublisherEventArgs<T> args);

    }
}
