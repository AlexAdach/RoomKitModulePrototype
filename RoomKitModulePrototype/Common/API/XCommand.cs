using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XCommand : XAPICommandBase, IHasArguments
    {
        private const string _command = "xCommand";
        private readonly string[] _path;
        private StringCollection _commandArgs;
        public StringCollection CommandArgs { get => _commandArgs; }


        public override XAPICommandDTO GetCommand(string arg)
        {
            if (_commandArgs.Contains(arg))
            {
                var i = _commandArgs.IndexOf(arg);
                var cmd = string.Join(" ", _command, _path, _commandArgs[i]);
                return new XAPICommandDTO { Command = _command, Path = _path, Argument = _commandArgs[i], CommandString = cmd };
            }
            else
            {
                return null;
            }
        }        
        public XCommand(string[] path, StringCollection commandArgs)
        {
            _path = path;
            _commandArgs = new StringCollection();
            _commandArgs = commandArgs;
        }

        public XCommand(string[] path, string[] commandArgs)
        {
            _path = path;
            _commandArgs = new StringCollection();
            _commandArgs.AddRange(commandArgs);
        }
    }
}
