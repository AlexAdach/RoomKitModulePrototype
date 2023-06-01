using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIxFeedbackRegister : XAPIBaseCommand
    {
        public XAPIxFeedbackRegister(string[] path, string argument)
        {
            _type = XAPICommandPrefixEnum.XFeedbackRegister;
            _path = path;
            _argument = argument;
        }

        public override string CommandString()
        {
            var pathslash = "Status/" + string.Join("/", _path);
            var prefix = Extensions.CommandTypeToString(_type);
            var cmd = prefix + " " + pathslash + "/" + _argument;
            return cmd;
        }
    }
}
