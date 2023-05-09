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
    public static class FlagsHelper
    {
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }
    }

    [Flags]
    public enum DebugAlertLevel
    {
        None = 0,
        Error = 1,
        Debug = 2,
        DebugCode = 4,
        DebugComms = 8,
    }
}


