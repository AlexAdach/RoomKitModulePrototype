using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIBaseCommand : BaseDTO
    {
        protected XAPICommandPrefixEnum _type;
        protected string[] _path;
        protected string _argument;
        protected Dictionary<string, string> _parameters;
        public virtual string CommandString()
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
    }
}
