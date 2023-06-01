using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIxCommand : XAPIBaseCommand
    {
        public XAPIxCommand(string[] path, string argument, Dictionary<string, string> parameters = null)
        {
            _type = XAPICommandPrefixEnum.XCommand;
            _path = path;
            _argument = argument;
            _parameters = parameters;
        }
    }
}
