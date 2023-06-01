using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class XAPIxStatus : XAPIBaseCommand
    {
        public XAPIxStatus(string[] path, string argument)
        {
            _type = XAPICommandPrefixEnum.XStatus;
            _path = path;
            _argument = argument;
        }
    }
}
