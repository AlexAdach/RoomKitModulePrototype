using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public static class CodecResponseParsing
    {
        public static XAPIBaseResponse GenerateResponseObject(this JObject parsedJSON)
        {
            if (parsedJSON["CommandResponse"] != null)
            {
                var cmdRsp = (JObject)parsedJSON["CommandResponse"];

                var name = cmdRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                    .OfType<JProperty>()             // For those which are properties
                    .Select(p => p.Name)             // Select the name
                    .FirstOrDefault();               // And take the first.

                var result = new XAPICommandResponse();
                result.CommandResponse = name;
                return result;
            }
            else if (parsedJSON["Event"] != null)
            {
                var eventRsp = (JObject)parsedJSON["Event"].First.First;

                var eStatusName = eventRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                    .OfType<JProperty>()             // For those which are properties
                    .Select(p => p.Name)             // Select the name
                    .FirstOrDefault();               // And take the first.

                JProperty eStatusValue = eventRsp.Descendants()
                    .OfType<JProperty>()
                    .FirstOrDefault(p => p.Name == "Value");

                if (eStatusValue != null)
                {
                    var eventResponse = new XAPIEventResponse();
                    eventResponse.EventName = eStatusName;
                    eventResponse.EventValue = eStatusValue.Value.Value<string>();
                    return eventResponse;
                }
                else
                {
                    return null;
                }
            }
            else if (parsedJSON["Status"] != null)
            {
                // Parse JSON string
                //JObject jsonObject = JObject.Parse(json);
                var statusHeirachy = parsedJSON.Descendants().OfType<JProperty>();
                statusHeirachy = statusHeirachy.Reverse();

                string value = statusHeirachy.First().Value.Value<string>();

                statusHeirachy = statusHeirachy.Skip(1);
                string stateArgProperty = statusHeirachy.First().Name;
                statusHeirachy = statusHeirachy.Skip(1);

                List<string> pathlist = new List<string>();

                while (statusHeirachy.Count() > 1)
                {
                    pathlist.Add(statusHeirachy.First().Name);
                    statusHeirachy = statusHeirachy.Skip(1);
                }

                var statusResponse = new XAPIStatusResponse();

                statusResponse.StateArgument = stateArgProperty;
                statusResponse.Value = value;
                pathlist.Reverse();
                statusResponse.Path = pathlist.ToArray();

                return statusResponse;
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<JToken> GetDescendantsAndSelf(this JToken token)
        {
            yield return token;

            if (token is JContainer container)
            {
                foreach (var child in container.Children())
                {
                    foreach (var descendant in GetDescendantsAndSelf(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}

