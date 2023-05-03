using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public sealed class CoreModule : BaseModule, ICoreModule
    {
        public List<CiscoProperty> ModuleProperties;

        private ICodecCommunication _codec;

        private string _user = "Tritech";
        private string _password = "20!9GolfR";
        private string _host = "192.168.0.113";



        public CoreModule()
        {
            ModuleType = "Core Module";
            CoreModuleId = "Module1";

            _codec = new SSH(_user, _password, _host, this);
            _codec.Connect();


            ModuleProperties = new List<CiscoProperty>();
        }

        void ICoreModule.CodecResponseRecieved(XAPICommandResponse response)
        {
            foreach(var prop in ModuleProperties)
            {
                prop.CheckCommandResult(response);
            }

        }

        public void SetPropertyValue(string key, string arg)
        {
            var prop = RetrievePropertyFromList(key);

            if (prop != null)
            {
                var cmd = prop.SetState(arg);
                if (cmd != null)
                    SendCommandToCodec(cmd);
            }
        }

        public void GetPropertyValue(string key)
        {
            var prop = RetrievePropertyFromList(key);

            if (prop != null)
            {
                var cmd = prop.GetState();
                if(cmd != null)
                    SendCommandToCodec(cmd);
            }
        }

        public void SetFeedback(string key)
        {
            var prop = RetrievePropertyFromList(key);

            if (prop != null)
            {
                var cmd = prop.FeedbackRegister();
                if (cmd != null)
                    SendCommandToCodec(cmd);
            }
        }
        

        protected override void SendCommandToCodec(XAPICommandDTO command)
        {
            _codec.SendCommand(command);
        }

        protected override void OnMessageRecieved(object sender, PublisherEventArgs<BaseModule> args)
        {
            base.OnMessageRecieved(sender, args);

            if(args.Message is XAPICommandDTO command)
            {
                _codec.SendCommand(command);
            }
        }

        public void AddProperty(string[] path, string key, string[] args, bool feedback = false)
        {
            var prop = new CiscoProperty(path, key, args, this, feedback);
            ModuleProperties.Add(prop);
        }


        private CiscoProperty RetrievePropertyFromList(string key)
        {
            if (ModuleProperties.Any(i => i.StatusArg == key))
                return ModuleProperties.Where(x => x.StatusArg == key).First();
            else
            {
                Debug.Log("No Property Found!");
                return null;
            }
        }

    }
}
