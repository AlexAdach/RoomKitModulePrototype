using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public static class Debug
    {

        public static void Log(string s)
        {
            Console.WriteLine(s);
        }

        public static void Log(string s, DebugAlertLevel level)
        {
            DebugAlertLevel alertLevel = DebugAlertLevel.Error | DebugAlertLevel.Debug | DebugAlertLevel.DebugComms;

            if(FlagsHelper.IsSet(alertLevel, level))
            Console.WriteLine(s);

        }
    }



}


