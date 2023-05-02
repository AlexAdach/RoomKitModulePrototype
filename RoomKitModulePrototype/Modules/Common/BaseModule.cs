using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class BaseModule : Subscriber<BaseModule>
    {
        private static readonly Publisher<BaseModule> _publisher
            = new Publisher<BaseModule>();
        public string ModuleType { get; protected set; }
        public BaseModule()
        {
            _publisher.Subscribe(this);
        }
/*        public void SendGlobalMessage(string message)
        {
            var args = new PublisherEventArgs<BaseModule>(this) { Message = message };
            _publisher.Notify(args);
        }*/

        protected override void OnMessageRecieved(object sender, PublisherEventArgs<BaseModule> args)
        {
            if (ID.Equals(args.NotificationSender.ID) && args.NotificationSender.CoreModuleId != CoreModuleId)
                return;

            string debug = $"{ModuleType} recieved notification from {args.NotificationSender.ModuleType}";
            Debug.Log(debug);
        }

        public virtual void SendCommandToCodec(XAPICommandDTO command)
        {
            var args = new PublisherEventArgs<BaseModule>(this) { Message = command };
            _publisher.Notify(args);
        }



        



    }
}
