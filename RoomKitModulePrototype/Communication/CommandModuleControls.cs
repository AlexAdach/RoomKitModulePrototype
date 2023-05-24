using System;
using System.Collections.Generic;
using System.Text;

namespace RoomKitModulePrototype
{
    public class CommandModuleControls
    {
        public CiscoValue PeripheralConnect;
        public CiscoSet EchoMode;
        public CiscoSet OutputMode;
        
        public CommandModuleControls(BaseControl.SendCommandToCodecDelegate callback)
        {
            PeripheralConnect = new CiscoValue()
            {
                Path = new string[] { "Peripherals" },
                StatusArgument = null,
                FeedbackStates = null,
                ShouldRegisterFeedback = false,
                SetValueArgument = "Connect",
                ValueParameters = new CiscoParameter[]
                {
                    new CiscoParameter("HardwareInfo", new string[] { "1" }),
                    new CiscoParameter("ID", new string[] { "01:02:03:03:05:06" }),
                    new CiscoParameter("Name", new string[] { "Crestron" }),
                    new CiscoParameter("NetworkAddress", new string[] { "192.168.0.143" }),
                    new CiscoParameter("SerialNumber", new string[] { "123456789" }),
                    new CiscoParameter("SoftwareInfo", new string[] { ".NET 4.7.2 Console App" }),
                    new CiscoParameter("Type", new string[] { "ControlSystem" }),
                },
                SendCommandToCodecHandler = callback
            };

            EchoMode = new CiscoSet(XAPICommandPrefix.Root)
            {
                Path = new string[] { "Echo" },
                States = new string[] {"On", "Off"},
                SendCommandToCodecHandler = callback
            };

            OutputMode = new CiscoSet(XAPICommandPrefix.XPreferences)
            {
                Path = new string[] { "outputmode" },
                States = new string[] { "json" },
                SendCommandToCodecHandler = callback
            };

        }




    }
}
