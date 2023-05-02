using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RoomKitModulePrototype
{
    public class Publisher<TPublisher>
    {
        private readonly Mutex _publisherLock = new Mutex();
        public readonly LinkedList<Subscriber<TPublisher>> _subscribers = new LinkedList<Subscriber<TPublisher>>();
        private event EventHandler<PublisherEventArgs<TPublisher>> NotificationEvent = delegate { };
        
        public void Subscribe<T>(T subscriber)
            where T : Subscriber<TPublisher>
        {
            try
            {
                _publisherLock.WaitOne();
                if(!_subscribers.Contains(subscriber))
                {
                    _subscribers.AddFirst(subscriber);
                    NotificationEvent += ((IMessageReceived<TPublisher>)subscriber).OnMessageRecieved;
                }
            }
            finally
            {
                _publisherLock.ReleaseMutex();
            }
        }

        public void Notify<T>(T args)
            where T : PublisherEventArgs<TPublisher>
        {
            try
            {
                _publisherLock.WaitOne();
                OnNotificationCallback(args);
            } finally
            {
                _publisherLock.ReleaseMutex();
            }
        }

        //Remember to replace with Crestron Invoke.
        private void OnNotificationCallback(PublisherEventArgs<TPublisher> e)
        {
            var handler = NotificationEvent;
            if (handler != null)
                handler.Invoke(this, e);
        }

    }
}
