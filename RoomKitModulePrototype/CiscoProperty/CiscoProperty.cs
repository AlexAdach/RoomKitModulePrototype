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
        private StringCollection _setResultArgs;
        private string _statusArg; //Parent state string (for xStatus, xEvent)


        private bool _feedback;

        private BaseModule _module;

        private string _currentStateString;

        public string[] Path { get { return _path; } }
        public string StatusArg { get { return _statusArg; } }
        public bool Feedback { get { return _feedback; } }

        public StringCollection PropertyArgs { get { return _propertyArgs; } }

        //private ICodecCommunication _comms;

        public CiscoProperty(string[] path, string statusArg, string[] args, BaseModule module, bool feedback = false)
        {
            _path = path;
            _statusArg = statusArg;
            _propertyArgs = new StringCollection();
            _setResultArgs = new StringCollection();
            _propertyArgs.AddRange(args);
            _module = module;
            _feedback = feedback;
            
            foreach(var propArg in _propertyArgs)
            {
                var child = _path[_path.Length - 1];
                var result = child + propArg + "Result";
                _setResultArgs.Add(result);
            }
        }

        public XAPICommandDTO GetState()
        {
            var path = string.Join(" ", _path);
            var cmdString = string.Join(" ", "xStatus", path, _statusArg);
            return new XAPICommandDTO { Command = "xStatus", Path = _path, Argument = _statusArg, CommandString = cmdString };
        }

        public XAPICommandDTO SetState(string arg)
        {
            if (_propertyArgs.Contains(arg))
            {
                var i = _propertyArgs.IndexOf(arg);
                var path = string.Join(" ", _path);
                var cmd = string.Join(" ", "xCommand", path, _propertyArgs[i]);
                return new XAPICommandDTO { Command = "xCommand", Path = _path, Argument = _propertyArgs[i], CommandString = cmd };
            }
            else
            {
                return null;
            }
        }

        public XAPICommandDTO SetState(int arg)
        {
            if (arg <= _propertyArgs.Count && arg > 0)
            {
                var i = arg;
                var cmd = string.Join(" ", "xCommand", _path, _propertyArgs[i]);
                return new XAPICommandDTO { Command = "xCommand", Path = _path, Argument = _propertyArgs[i], CommandString = cmd };
            }
            else
            {
                return null;
            }
        }

        public XAPICommandDTO FeedbackRegister()
        {          
            var pathslash = "Event/" + string.Join("/", _path);

            var cmd = "xFeedback Register " + pathslash + _statusArg + "Status";
            return new XAPICommandDTO { Command = "xFeedback Register", Path = _path, Argument = _statusArg, CommandString = cmd };
        }

        public void CheckCommandResult(XAPICommandResponse resp)
        {
            if(_setResultArgs.Contains(resp.CommandResponse))
            {
                var i = _setResultArgs.IndexOf(resp.CommandResponse);
                _currentStateString = _propertyArgs[i];
                Debug.Log("Property State Changed!", DebugAlertLevel.Debug);
            }

        }
    }
}
