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
            Console.WriteLine(s);

        }
    }

    public enum DebugAlertLevel
    {
        None,
        Error,
        Debug,
        DebugCode,
        DebugComms,




    }
}


