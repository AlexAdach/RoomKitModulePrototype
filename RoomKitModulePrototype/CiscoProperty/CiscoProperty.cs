using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoProperty
    {
        //public readonly string FriendlyName;


        private string[] _path; //Heirarchical Path of property
        private StringCollection _propertyArgs; //Arguments for property State
        private string _key; //Parent state string (for xStatus, xEvent)
        private bool _feedback;

        private BaseModule _module;

        private string _currentStateString;




        public List<XAPICommandBase> CommandList;
        private ICodecCommunication _comms;

        public CiscoProperty(string[] path, string key, string[] args, BaseModule module, bool feedback = false)
        {
            _path = path;
            _key = key;
            _propertyArgs = new StringCollection();
            _propertyArgs.AddRange(args);
            _module = module;
            _feedback = feedback;
        }

        public void GetState()
        {
            var cmdString = string.Join(" ", "xStatus", _path, _key);
            var dto = new XAPICommandDTO { Command = "xStatus", Path = _path, Argument = _key, CommandString = cmdString };
        }

        public void SetState(string arg)
        {
            if (_propertyArgs.Contains(arg))
            {
                var i = _propertyArgs.IndexOf(arg);
                var cmd = string.Join(" ", "xCommand", _path, _propertyArgs[i]);
                new XAPICommandDTO { Command = "xCommand", Path = _path, Argument = _propertyArgs[i], CommandString = cmd };
            }
        }

        public void SetState(int arg)
        {
            if (arg <= _propertyArgs.Count && arg > 0)
            {
                var i = arg;
                var cmd = string.Join(" ", "xCommand", _path, _propertyArgs[i]);
                new XAPICommandDTO { Command = "xCommand", Path = _path, Argument = _propertyArgs[i], CommandString = cmd };
            }
        }

        public void SetFeedback()
        {          
            var pathslash = string.Join("/", "Event", _path);
            var cmd = "xFeedback Register " + pathslash + _key + "Status";
            new XAPICommandDTO { Command = "xFeedback Register", Path = _path, Argument = _key, CommandString = cmd };
        }



        /*       public void SetStatus(string arg)
               {
                   var cmd = RetrieveCommandFromList(typeof(XCommand));

                   if (cmd != null)
                       _comms.SendCommand(cmd.GetCommand(arg));
               }

               public void ToggleStatus()
               {
                   var cmd = RetrieveCommandFromList(typeof(XCommand));

                   if (cmd != null)
                       _comms.SendCommand(cmd.GetCommand());
               }

               public void GetStatus()
               {
                   var cmd = RetrieveCommandFromList(typeof(XStatus));

                   if (cmd != null)
                       _comms.SendCommand(cmd.GetCommand());
               }

               private XAPICommandBase RetrieveCommandFromList(Type cmdType)
               {
                   if (CommandList.Any(i => i.GetType() == cmdType))
                       return CommandList.Where(x => x.GetType() == cmdType).First();
                   else
                   {
                       Debug.Log("No Command Found!");
                       return null;
                   }

               }*/
    }
}
