using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public abstract class CommunicationBase : ICodecCommunication
    {
        public CodecResponseParseHandler CodecResponseParseCallback { get; set; }
        public abstract void Connect();
        public abstract void SendCommand(string cmd);
        public abstract void SendCommand(XAPICommandDTO cmd);

        /*protected void ResponseRouter(string responseString)
        {
            if (responseString.Contains("Login successful") && !LoggedIn)
            {
                LoggedIn = true;
                Debug.Log("Comm Base - Logged In!");
                SendCommand("xPreferences outputmode json");
            }
            else if (LoggedIn && Extensions.ValidateJSON(responseString))
            {
                JObject responseJSON = JObject.Parse(responseString, new JsonLoadSettings { LineInfoHandling = 0 }) ;

                if (responseJSON["CommandResponse"] != null)
                {
                    var cmdRsp = (JObject)responseJSON["CommandResponse"];

                    var name = cmdRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                        .OfType<JProperty>()             // For those which are properties
                        .Select(p => p.Name)             // Select the name
                        .FirstOrDefault();               // And take the first.


                    Debug.Log("This is a command response");
                    var result = new XAPICommandResponse();
                    result.CommandResponse = name;
                    Debug.Log(name);

                }
            }*/

    }
}
