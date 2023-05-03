using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public class PropertyModule : LogicModule
    {
        public List<CiscoProperty> ModuleProperties = new List<CiscoProperty>();

        public PropertyModule()
        {
            ModuleType = "Property Module";
        }

        #region PropertyCommands
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
        public void SetPropertyValue(int i, string arg)
        {
            if(i >= 0 && i <= ModuleProperties.Count - 1)
            {
                var cmd = ModuleProperties[i].SetState(arg);
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
                if (cmd != null)
                    SendCommandToCodec(cmd);
            }
        }
        public void GetPropertyValue(int i)
        {
            if (i >= 0 && i <= ModuleProperties.Count - 1)
            {
                var cmd = ModuleProperties[i].GetState();
                if (cmd != null)
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
        public void SetFeedback(int i)
        {
            if (i >= 0 && i <= ModuleProperties.Count - 1)
            {
                var cmd = ModuleProperties[i].GetState();
                if (cmd != null)
                    SendCommandToCodec(cmd);
            }
        }
        /// <summary>
        /// Simpl+ Module populates the module properties list by calling this method.
        /// </summary>
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
        #endregion PropertyCommands

        protected override void ModulePropertiesBoot()
        {
            base.ModulePropertiesBoot();
            foreach(var prop in ModuleProperties)
            {
                if (prop.Feedback)
                {
                    SendCommandToCodec(prop.FeedbackRegister());
                    SendCommandToCodec(prop.GetState());
                }
            }
        }


        public override void FromCommandModuleMessageReceived(object sender, InterModuleEventArgs args)
        {
            base.FromCommandModuleMessageReceived(sender, args);

            if(args.Message is XAPICommandResponse commandResponse)
            {
                foreach (var prop in ModuleProperties)
                {
                    prop.CheckCommandResult(commandResponse);
                }
            }
            
        }
    }
}
