using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class LogicModule : BaseModule
    {
        public event EventHandler<InterModuleEventArgs> ToCommsModuleMessageSent = delegate { };

        public override void Initialize(string id)
        {
            base.Initialize(id);
            dispatcher.SetCommsSubscriptions(this);
        }

        protected override void SendCommandToCodec(XAPIBaseCommand command)
        {
            ToCommsModuleMessageSent?.Invoke(this, new InterModuleEventArgs(command));
        }

        protected virtual void ModulePropertiesReset() 
        {
            Debug.Log($"{ModuleType} resetting properties.", DebugAlertLevelEnum.DebugCode);
        }

        protected virtual void ModulePropertiesBoot() 
        {
            Debug.Log($"{ModuleType} properties boot.", DebugAlertLevelEnum.DebugCode);
        }

        public virtual void FromCommandModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            //Debug.Log($"{ModuleType} Message Received! {args.Message.GetType()}");
            if(args.Message is CodecCommunicationStatusDTO status)
            {
                Debug.Log($"{status.ConnectionConfigured}");
                if (status.ConnectionConfigured)
                    ModulePropertiesBoot();
            }
        }

        #region Dispatch
        /// <summary>
        /// Used during program initialization
        /// </summary>
        public virtual void OnNewCommandModuleRegistered(object sender, ModuleRegistrationEventArgs<CommandModule> e)
        {
            dispatcher.SetCommsSubscriptions(this);
        }
        #endregion Dispatch
    }
}
