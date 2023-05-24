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
                //JToken.Parse(s);
                return true;
            }
            catch
            {
                //Debug.Log(ex.Message, DebugAlertLevel.Error);
                return false;
            }
        }

        public static bool JSONNotEmpty(this string s)
        {
            try
            {
                var total = JObject.Parse(s).Count;

                if (total > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static string CommandTypeToString(XAPICommandPrefix type)
        {
            switch (type)
            {
                case XAPICommandPrefix.XCommand:
                    return "xCommand";
                case XAPICommandPrefix.XConfiguration:
                    return "xConfiguration";
                case XAPICommandPrefix.XFeedbackRegister:
                    return "xFeedback register";
                case XAPICommandPrefix.XStatus:
                    return "xStatus";
                case XAPICommandPrefix.XPreferences:
                    return "xPreferences";
                default:
                    return "";
            }
        }
    }
}
