using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPICommand : BaseDTO
    {
        private XAPICommandType _type;
        private string[] _path;
        private string _command;
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();

        public XAPICommand(XAPICommandType type, string[] path, string command, Dictionary<string, string> parameters = null)
        {
            _type = type;
            _path = path;
            _command = command;
            _parameters = parameters;
        }
        public string CommandString()
        {
            if (_type != XAPICommandType.XFeedbackRegister)
            {
                //Concatenate path and separate with space. 
                var path = string.Join(" ", _path);
                //Get the string version of the command type.
                var prefix = Extensions.CommandTypeToString(_type);
                //Create the full command string.
                var fullstring = string.Join(" ", prefix, path, _command);

                if (_parameters != null)
                {
                    string parameterString = "";
                    foreach (var parameter in _parameters)
                    {
                        parameterString += parameter.Key + ": " +  "\"" + parameter.Value + "\"" + " ";
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
                var cmd = prefix + " " + pathslash + "/" + _command;
                return cmd;
            }
        }
    }
}
