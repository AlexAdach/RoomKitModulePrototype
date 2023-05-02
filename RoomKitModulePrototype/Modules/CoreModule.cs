using System;
using System.Collections.Generic;
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

        void ICoreModule.CodecResponseRecieved(BaseDTO response)
        {
            

        }

        public void SendPropertyCommand(int propIndex, int propCommand, string cmdArgs = "")
        {
            try
            {
                XAPICommandDTO cmd;
                if (cmdArgs != "")
                   cmd = ModuleProperties[propIndex].CommandList[propCommand].GetCommand(cmdArgs);
                else
                   cmd = ModuleProperties[propIndex].CommandList[propCommand].GetCommand();

                SendCommandToCodec(cmd);
            }
            catch (Exception)
            {
                Debug.Log("Property or command not found in module.", DebugAlertLevel.Error);
            }
        }
        

        public override void SendCommandToCodec(XAPICommandDTO command)
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

        public void AddProperty(string[] path, string key, string[] args)
        {
            var prop = new CiscoProperty(path, key, args, this);
            ModuleProperties.Add(prop);
        }


    }
}
