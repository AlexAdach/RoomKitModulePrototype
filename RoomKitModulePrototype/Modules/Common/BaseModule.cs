using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RoomKitModulePrototype
{
    public class BaseModule : Subscriber<BaseModule>, INotifyPropertyChanged
    {
        private static readonly Publisher<BaseModule> _publisher
            = new Publisher<BaseModule>();
        public string ModuleType { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public BaseModule()
        {
            _publisher.Subscribe(this);
        }


        protected override void OnMessageRecieved(object sender, PublisherEventArgs<BaseModule> args)
        {
            if (ID.Equals(args.NotificationSender.ID) && args.NotificationSender.CoreModuleId != CoreModuleId)
                return;

            string debug = $"{ModuleType} recieved notification from {args.NotificationSender.ModuleType}";
            Debug.Log(debug);
        }

        protected virtual void SendCommandToCodec(XAPICommandDTO command)
        {
            var args = new PublisherEventArgs<BaseModule>(this) { Message = command };
            _publisher.Notify(args);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }






        /*        public void SendGlobalMessage(string message)
        {
            var args = new PublisherEventArgs<BaseModule>(this) { Message = message };
            _publisher.Notify(args);
        }*/


    }
}
