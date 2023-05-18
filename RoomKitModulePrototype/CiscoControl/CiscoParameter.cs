using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoParameter
    {
        private string _id;
        private string[] _values;
        public string ID { get { return _id; } }
        public string[] Values { get { return _values; } }

        public CiscoParameter(string id, string[] values)
        {
            _id = id;
            _values = values;
        }

        public string GetParameter(int value)
        {
            return ID + ": " + "\"" + Values[value] + "\"";
        }
    }
}
