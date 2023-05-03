using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class ButtonModule : BaseModule
    {
        public ButtonModule()
        {
            ModuleType = "ButtonModule";
            CoreModuleId = "Module1";
        }

        protected override void OnMessageRecieved(object sender, PublisherEventArgs<BaseModule> args)
        {
            base.OnMessageRecieved(sender, args);

            if (args.Message is XAPICommandDTO)
                return;
        }
    }
}
