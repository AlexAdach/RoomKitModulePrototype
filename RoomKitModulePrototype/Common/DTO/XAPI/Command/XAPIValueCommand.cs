using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIValueCommand : XAPIBaseCommand
    {
        private XAPICommandType _type;
        private string[] _path;
        private string _argument;
        string[] _parameters;

        public XAPIValueCommand(XAPICommandType type, string[] path, string argument, string[] parameters = null)
        {
            _type = type;
            _path = path;
            _argument = argument;
            _parameters = parameters;
        }

        public override string CommandString()
        {
            //Concatenate path and separate with space. 
            var path = string.Join(" ", _path);
            //Get the string version of the command type.
            var prefix = Extensions.CommandTypeToString(_type);
            //Create the full command string.
            var fullstring = string.Join(" ", prefix, path, _argument);

            if(_parameters != null)
            {
                var allParameters = string.Join(" ", _parameters);
                return string.Join(" ", fullstring, allParameters);
            }
            else
            {
                return fullstring;
            }
        }
    }
}
