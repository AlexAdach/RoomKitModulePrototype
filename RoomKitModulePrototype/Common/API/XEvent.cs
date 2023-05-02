using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    class XEvent : XAPICommandBase
    {
        private const string _command = "xFeedback Register";
        private readonly string[] _path;
        private readonly string _arg;

        public override XAPICommandDTO GetCommand(string arg)
        {
            var cmd = string.Join("/", _command, _path, _arg);
            return new XAPICommandDTO { Command = _command, Path = _path, Argument = _arg, CommandString = cmd };
        }

        public XEvent(string[] path, string arg)
        {
            _path = path;
            _arg = arg;
        }

    }
}
