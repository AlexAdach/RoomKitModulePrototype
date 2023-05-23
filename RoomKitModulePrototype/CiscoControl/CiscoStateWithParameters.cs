using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoStateWithParameters : CiscoStatus
    {
        public string[] States { get; set; }
        public CiscoParameter[] StateParameters { get; set; }
        public void SetState(ushort state, int[,] keyValuePairs)
        {
            if (States[state] != null)
            {
                var ids = keyValuePairs.GetLength(0);
                List<string> parameterString = new List<string>();

                for (var id = 0; id < ids; id++)
                {
                    int key = keyValuePairs[id, 0];
                    int value = keyValuePairs[id, 1];

                    //Check to see if the ID index exists
                    if (key < StateParameters.Length)
                    {
                        //pass in -1 for the value if you don't want it sent.
                        if (value >= 0)
                        {
                            if (value < StateParameters[key].Values.Length)
                            {
                                parameterString.Add(StateParameters[key].GetParameter(value));
                            }
                            else
                            {
                                Debug.Log("Set Value Argument Error. Value for ID is outside array bounds.", DebugAlertLevel.Error);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Set Value Argument Error. ID index is outside declared bounds", DebugAlertLevel.Error);
                    }
                }

                var cmd = new XAPIValueCommand(XAPICommandType.XCommand, Path, States[state], parameterString.ToArray());
                SendCommandToCodecHandler.Invoke(cmd);
            }
        }
        
    }
}
