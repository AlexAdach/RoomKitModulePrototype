using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RoomKitModulePrototype
{
    
    public class CommandModule : BaseModule
    {
        public event EventHandler<InterModuleEventArgs> CommandModuleMessageSent = delegate { };
        public CodecCommunicationManager Codec;

        private CommandModuleControls commandControls;
        public CommandModule()
        {
            commandControls = new CommandModuleControls(SendCommandToCodec);
            Codec = new CodecCommunicationManager(new CodecCommunicationManager.CodecCommManagerCallback(OnCommunicationManagerMessageReceived));
        }
        public override void Initialize(string id)
        {
            base.Initialize(id);
            dispatcher.RegisterModule(this);
            Codec.InitializeSSH("Tritech", "20!9GolfR", "192.168.0.113");
        }
        protected override void SendCommandToCodec(XAPIBaseCommand command)
        {
            Codec.CodecTx(command);
        }
        public void FromLogicModules(object sender, InterModuleEventArgs args)
        {
            if (args.Message is XAPIBaseCommand msg)
                Codec.CodecTx(msg);
        }
        private void ToLogicModules(BaseDTO msg)
        {
            var handler = CommandModuleMessageSent;
            handler.Invoke(this, new InterModuleEventArgs(msg));
        }
        private void OnCommunicationManagerMessageReceived(BaseDTO msg)
        {
            
            if (msg is CodecCommunicationStatusDTO status)
            {
                OnCodecStatusChanged(status);
            }
            ToLogicModules(msg);

            
        }
        private void OnCodecStatusChanged(CodecCommunicationStatusDTO status)
        {
            if (status.CodecConnected && status.CodecLoggedIn && !status.ConnectionConfigured)
            {
                //commandControls.EchoMode.SetState(0);
                commandControls.OutputMode.SetState(0);
                commandControls.PeripheralConnect.SetValue(new int[,] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 } });
            }
        }
    }
}
