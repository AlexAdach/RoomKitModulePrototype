using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public static class Extensions
    {
        public static bool ValidateJSON(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Debug.Log(ex.Message, DebugAlertLevel.Error);
                return false;
            }
        }
    }
}
