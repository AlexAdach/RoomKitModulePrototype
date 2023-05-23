using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public static class ResponseJanitor
    {
        public static List<string> SeparateJSON(this string data)
        {
            List<string> jsonList = new List<string>();
            StringBuilder buffer = new StringBuilder();

            int openingBrackets = 0;  // Count of opening brackets '{'
            int closingBrackets = 0;  // Count of closing brackets '}'

            // Loop through the received data
            foreach (char c in data)
            {
                buffer.Append(c);
                if (c == '{')
                {
                    openingBrackets++;
                }
                else if (c == '}')
                {
                    closingBrackets++;

                    if (openingBrackets == closingBrackets)
                    {
                        // Complete JSON response
                        string json = buffer.ToString();
                        jsonList.Add(json);

                        // Clear the buffer and reset bracket counts for the next response
                        buffer.Clear();
                        openingBrackets = 0;
                        closingBrackets = 0;
                    }
                }
            }
            // Handle any remaining data in the buffer (in case a complete response is not received yet)
            if (buffer.Length > 0)
            {
                string json = buffer.ToString();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    //Debug.Log($"Buffer: {json}");
                    jsonList.Add(json);
                }
            }
            //Debug.Log(jsonList.Count().ToString());
            return jsonList;
        }
    }
}
