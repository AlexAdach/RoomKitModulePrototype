using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoProperty
    {
        private string[] _path; //Heirarchical Path of property
        private StringCollection _propertyArgs; //Arguments for property State
        private StringCollection _setResultArgs;
        private StringCollection _eventStatusArgs;
        private string _statusArg; //Parent state string (for xStatus, xEvent)

        private bool _feedback;

        private string _currentStateString;

        public string[] Path { get { return _path; } }
        public string StatusArg { get { return _statusArg; } }
        public bool Feedback { get { return _feedback; } }

        public StringCollection PropertyArgs { get { return _propertyArgs; } }

        public CiscoProperty(string[] path, string statusArg, string[] args, bool feedback = false)
        {
            _path = path;
            _statusArg = statusArg;
            _propertyArgs = new StringCollection();
            _setResultArgs = new StringCollection();
            _eventStatusArgs = new StringCollection();
            _propertyArgs.AddRange(args);
            _feedback = feedback;
            
            foreach(var propArg in _propertyArgs)
            {
                var child = _path[_path.Length - 1];
                var result = child + propArg + "Result";
                var status = child + propArg + "Status";
                _setResultArgs.Add(result);
                _eventStatusArgs.Add(status);
            }


        }

        public XAPICommand GetState()
        {
            return new XAPICommand(XAPICommandType.XStatus, _path, _statusArg);
        }

        public XAPICommand SetState(string arg)
        {
            if (_propertyArgs.Contains(arg))
            {
                var i = _propertyArgs.IndexOf(arg);
                return new XAPICommand(XAPICommandType.XCommand, _path, _propertyArgs[i]);
            }
            else
            {
                return null;
            }
        }

        public XAPICommand SetState(int arg)
        {
            if (arg <= _propertyArgs.Count && arg > 0)
            {
                var i = arg;
                return new XAPICommand(XAPICommandType.XCommand, _path, _propertyArgs[i]);
            }
            else
            {
                return null;
            }
        }

        public XAPICommand FeedbackRegister()
        {
            return new XAPICommand(XAPICommandType.XFeedbackRegister, _path, _statusArg);
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

        public void CheckEventResult(XAPIEventResponse resp)
        {
            if (_eventStatusArgs.Contains(resp.EventName))
            {
                
                //_currentStateString = _propertyArgs[i];
                Debug.Log($"Property Event Recorded! {resp.EventName} - {resp.EventValue}", DebugAlertLevel.Debug);
            }
        }
    }
}
