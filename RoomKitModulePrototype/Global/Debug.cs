using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RoomKitModulePrototype
{
    public static class Debug
    {

        private static Mutex mutex = new Mutex();

        public static void Log(string s)
        {
            PrintToConsole(s);
        }

        public static void Log(string s, DebugAlertLevel level)
        {
            DebugAlertLevel alertLevel = DebugAlertLevel.Error | DebugAlertLevel.Debug | DebugAlertLevel.DebugComms;

            if(FlagsHelper.IsSet(alertLevel, level))
            PrintToConsole(s);
        }

        private static void PrintToConsole(string s)
        {
            mutex.WaitOne();
            try
            {
                Console.WriteLine(s);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public static void DebugThreadID(this Thread thread, string info = "")
        {
            PrintToConsole($"{info} Thread ID: {thread.ManagedThreadId}");
        }

        public static string ShowCarriageReturn(this string input)
        {
            var stringBuilder = new StringBuilder();

            foreach (char c in input)
            {
                stringBuilder.Append(c);

                if (c == '\r')
                {
                    stringBuilder.Append("CR");
                }
            }

            return stringBuilder.ToString();
        }
        public static string ShowLineFeed(this string input)
        {
            var stringBuilder = new StringBuilder();

            foreach (char c in input)
            {
                stringBuilder.Append(c);

                if (c == '\n')
                {
                    stringBuilder.Append("LF");
                }
            }

            return stringBuilder.ToString();
        }

        public static string ShowCRLF(this string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            return string.Join($"&\r\n", lines);
        }

        public static string RemoveCRLF(this string input)
        {
            string output = input.Replace("CRLF", "");

            return output;
        }
        public static string AddCharacterOnLineFeed(this string input)
        {
            string[] lines = input.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains('\r'))
                {
                    lines[i] = "%" + lines[i];
                }
            }

            return string.Join("\n", lines);
        }


    }



}


