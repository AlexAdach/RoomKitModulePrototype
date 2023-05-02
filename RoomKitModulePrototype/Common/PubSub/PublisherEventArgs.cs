using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class PublisherEventArgs<T> : EventArgs
    {
        private readonly T _notificationSender;

        public T NotificationSender { get { return _notificationSender; } }

        public BaseDTO Message { get; set; } 
        
        public PublisherEventArgs(T notificationSender)
        {
            _notificationSender = notificationSender;
        }

    }
}
