using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class LogicModule : BaseModule
    {
        public event EventHandler<InterModuleEventArgs> ToCommsModuleMessageSent = delegate { };

        public delegate void SendCommandToCodecDelegate(XAPICommand command);

        protected bool _codecConnected;
        protected bool _codecLoggedIn;

        private bool codecConnected
        {
            get
            { return _codecConnected; }
            set
            {
                if (_codecConnected != value)
                {
                    _codecConnected = value;
                    if (value == false)
                    {
                        ModulePropertiesReset();
                    }
                }
            }
        }
        private bool codecLoggedIn
        {
            get
            { return _codecLoggedIn; }
            set
            {
                if (_codecLoggedIn != value)
                {
                    _codecLoggedIn = value;
                    if (value == true)
                    {
                        ModulePropertiesBoot();
                    }
                }
            }
        }

        public virtual void Initialize(string id)
        {
            ModuleID = id;
            dispatcher.SetCommsSubscriptions(this);
        }

        protected virtual void SendCommandToCodec(XAPICommand command)
        {
            ToCommsModuleMessageSent?.Invoke(this, new InterModuleEventArgs(command));
        }

        protected virtual void ModulePropertiesReset() 
        {
            Debug.Log($"{ModuleType} resetting properties.", DebugAlertLevel.DebugCode);
        }

        protected virtual void ModulePropertiesBoot() 
        {
            Debug.Log($"{ModuleType} properties boot.", DebugAlertLevel.DebugCode);
        }

        public virtual void FromCommandModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            Debug.Log($"Logic Module received message from Command Module.", DebugAlertLevel.DebugCode);

            if(args.Message is CodecCommStatusDTO codecStatus)
            {
                //Debug.Log($"Codec Status {codecStatus.CodecConnected} {codecStatus.CodecLoggedIn}");
                codecConnected = codecStatus.CodecConnected;
                codecLoggedIn = codecStatus.CodecLoggedIn;
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
