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

        public static string CommandTypeToString(XAPICommandPrefixEnum type)
        {
            switch (type)
            {
                case XAPICommandPrefixEnum.XCommand:
                    return "xCommand";
                case XAPICommandPrefixEnum.XConfiguration:
                    return "xConfiguration";
                case XAPICommandPrefixEnum.XFeedbackRegister:
                    return "xFeedback register";
                case XAPICommandPrefixEnum.XStatus:
                    return "xStatus";
                case XAPICommandPrefixEnum.XPreferences:
                    return "xPreferences";
                default:
                    return "";
            }
        }
    }
}
