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
                JToken.Parse(s, new JsonLoadSettings { LineInfoHandling = 0 });
                return true;
            }
            catch 
            {
                //Debug.Log(ex.Message, DebugAlertLevel.Error);
                return false;
            }
        }

        public static string CommandTypeToString(XAPICommandType type)
        {
            switch (type)
            {
                case XAPICommandType.XCommand:
                    return "xCommand";
                case XAPICommandType.XConfiguration:
                    return "xConfiguration";
                case XAPICommandType.XFeedbackRegister:
                    return "xFeedback Register";
                case XAPICommandType.XStatus:
                    return "xStatus";
                default:
                    return "";
            }
        }
    }
}
