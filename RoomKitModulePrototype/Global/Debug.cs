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

        public static string ShowCarriageReturn(this string input)
        {
            var stringBuilder = new StringBuilder();

            foreach (char c in input)
            {
                stringBuilder.Append(c);

                if (c == '\r')
                {
                    stringBuilder.Append("#");
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
                    stringBuilder.Append("*");
                }
            }

            return stringBuilder.ToString();
        }

        public static string ShowCRLF(this string input)
        {
            string[] lines = input.Split(new[] { "\r\n" }, StringSplitOptions.None);
            return string.Join($"&\r\n", lines);
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


