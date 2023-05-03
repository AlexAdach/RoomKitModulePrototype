using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CodecCommunicationEventArgs<T> : EventArgs
    {
        private readonly T _notificationSender;
        public T NotificationSender { get { return _notificationSender; } }

        public bool IsConnected { get; set; }
        public bool IsLoggedIn { get; set; }

        public CodecCommunicationEventArgs(T notificationSender)
        {
            _notificationSender = notificationSender;
        }
    }
}
