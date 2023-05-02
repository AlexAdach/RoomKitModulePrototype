﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CommunicationBase
    {
        public virtual bool Connected { get; protected set; }
        public bool LoggedIn { get; protected set; }

        protected ICoreModule _core;

        public CommunicationBase(ICoreModule core)
        {
            _core = core;
        }
        public virtual void Connect() { }
        public virtual void SendCommand(string cmd) { }

        public virtual void SendCommand(XAPICommandDTO cmd) { }

        protected void ResponseRouter(string responseString)
        {
            if (responseString.Contains("Login successful") && !LoggedIn)
            {
                LoggedIn = true;
                Debug.Log("Comm Base - Logged In!");
                SendCommand("xPreferences outputmode json");
            }
            else if (LoggedIn && Extensions.ValidateJSON(responseString))
            {
                JObject responseJSON = JObject.Parse(responseString);

                if (responseJSON["CommandResponse"] != null)
                {
                    var cmdRsp = (JObject)responseJSON["CommandResponse"];

                    var name = cmdRsp.DescendantsAndSelf() // Loop through tokens in or under the root container, in document order. 
                        .OfType<JProperty>()             // For those which are properties
                        .Select(p => p.Name)             // Select the name
                        .FirstOrDefault();               // And take the first.


                    Debug.Log("This is a command response");
                    //Debug.Log(cmdRsp.ToString());
                    Debug.Log(name);

                }
            }


        }




    }
}
