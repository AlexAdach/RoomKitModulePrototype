﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIStateCommand : XAPIBaseCommand
    {
        private XAPICommandPrefixEnum _type;
        private string[] _path;
        private string _argument;
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
        
        //public XAPICommandPrefix Type { get { return _type; } }


        public XAPIStateCommand(XAPICommandPrefixEnum type, string[] path, string argument, Dictionary<string, string> parameters = null)
        {
            _type = type;
            _path = path;
            _argument = argument;
            _parameters = parameters;
        }
        public override string CommandString()
        {
            if (_type != XAPICommandPrefixEnum.XFeedbackRegister)
            {
                //Concatenate path and separate with space. 
                var path = string.Join(" ", _path);
                //Get the string version of the command type.
                var prefix = Extensions.CommandTypeToString(_type);
                //Create the full command string.
                var fullstring = string.Join(" ", prefix, path, _argument);

                if (_parameters != null)
                {
                    string parameterString = "";
                    foreach (var parameter in _parameters)
                    {
                        parameterString += parameter.Key + ": " + "\"" + parameter.Value + "\"" + " ";
                    }
                    return fullstring + " " + parameterString;
                }
                else
                {
                    return fullstring;
                }
            }
            else
            {
                var pathslash = "Status/" + string.Join("/", _path);
                var prefix = Extensions.CommandTypeToString(_type);
                var cmd = prefix + " " + pathslash + "/" + _argument;
                return cmd;
            }
        }

        public string GetExpectedResponse()
        {
            switch (_type)
            {
                case XAPICommandPrefixEnum.XCommand:
                    return "xCommand";
                case XAPICommandPrefixEnum.XConfiguration:
                    return "xConfiguration";
                case XAPICommandPrefixEnum.XFeedbackRegister:
                    return "xFeedback register";
                case XAPICommandPrefixEnum.XStatus:
                    return "xStatus";
                case XAPICommandPrefixEnum.XPreferences:
                    return "xPreferences";
                default:
                    return "";
            }

        }
    }
}
