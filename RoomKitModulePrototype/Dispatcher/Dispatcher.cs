using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoomKitModulePrototype
{
    public class Dispatcher
    {
        private readonly Mutex _dispatcherLock= new Mutex();
        private LinkedList<CommandModule> CodecCommsList = new LinkedList<CommandModule>();

        public event EventHandler<ModuleRegistrationEventArgs<CommandModule>> OnNewCommsModuleRegistered = delegate { };
        public void RegisterModule(CommandModule module)
        {
            try
            {
                _dispatcherLock.WaitOne();
                if(!CodecCommsList.Contains(module))
                {
                    CodecCommsList.AddFirst(module);
                    Debug.Log($"Dispatcher - New Command Module Registered ID = {module.ModuleID}", DebugAlertLevel.Debug);
                    NewCommandModuleRegistered(new ModuleRegistrationEventArgs<CommandModule>(module));
                    
                }
                else
                {
                    Debug.Log($"Dispatcher - A Comms Module already exists with and ID of {module.ModuleID}", DebugAlertLevel.Error);
                }
            }
            finally
            {
                _dispatcherLock.ReleaseMutex();
            }
        }

        public void SetCommsSubscriptions(LogicModule module)
        {
            try
            {
                _dispatcherLock.WaitOne();
                if(CodecCommsList.Any(i => i.ModuleID == module.ModuleID))
                {
                    var comms = CodecCommsList.Where(x => x.ModuleID == module.ModuleID).First();
                    comms.CommandModuleMessageSent += module.FromCommandModuleMessageReceived;
                    module.ToCommsModuleMessageSent += comms.LogicModuleMessageReceived;
                    Debug.Log($"Dispatcher - Module: {module.ModuleType} successfully registered to Command Module: {comms.ModuleID}", DebugAlertLevel.Debug);
                }
                else
                {
                    OnNewCommsModuleRegistered += module.OnNewCommandModuleRegistered;
                }    
            }
            catch (Exception e)
            {
                Debug.Log($"Error setting event subscriptions - {e.Message}", DebugAlertLevel.Error);
            }
            finally
            {
                _dispatcherLock.ReleaseMutex();
            }


        }
        protected virtual void NewCommandModuleRegistered(ModuleRegistrationEventArgs<CommandModule> e)
        {
            OnNewCommsModuleRegistered?.Invoke(this, e);
        }


    }
}
