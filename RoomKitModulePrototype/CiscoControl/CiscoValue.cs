using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CiscoValue : CiscoState
    {
        public CiscoParameter[] ValueParameters { get; set; }
        public string SetValueArgument { get; set; }

        public void SetValue(int[,] keyValuePairs)
        {
            var ids = keyValuePairs.GetLength(0);

            List<string> parameterString = new List<string>();
            for (var id = 0; id < ids; id++)
            {
                int key = keyValuePairs[id, 0];
                int value = keyValuePairs[id, 1];
                //Check to see if the ID index exists
                if (key < ValueParameters.Length)
                {
                    //pass in -1 for the value if you don't want it sent.
                    if (value >= 0)
                    {
                        if (value < ValueParameters[key].Values.Length)
                        {
                            parameterString.Add(ValueParameters[key].GetParameter(value));
                        }
                        else
                        {
                            Debug.Log("Set Value Argument Error. Value for ID is outside array bounds.", DebugAlertLevelEnum.Error);
                        }
                    }
                }
                else
                {
                    Debug.Log("Set Value Argument Error. ID index is outside declared bounds", DebugAlertLevelEnum.Error);
                }
            }

            var cmd = new XAPIValueCommand(XAPICommandPrefixEnum.XCommand, Path, SetValueArgument, parameterString.ToArray());
            SendCommandToCodecHandler.Invoke(cmd);
        }

    }
}
