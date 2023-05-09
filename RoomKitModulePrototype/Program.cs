using System;
using System.Collections.Generic;
using System.Threading;

namespace RoomKitModulePrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            


            Thread CallModule1 = new Thread(Module1);
            CallModule1.Start();

            Thread CallModule2 = new Thread(Module2);
            CallModule2.Start();

            PropertyModule propertyModule2 = new PropertyModule();
            propertyModule2.Initialize("Module1");

            propertyModule2.AddProperty(new string[] { "Audio", "Microphones" }, "Mute", new string[] { "Mute", "UnMute" }, true);
            propertyModule2.AddProperty(new string[] { "Standby" }, "State", new string[] { "Activate", "Deactivate", "HalfWake" }, true);
            propertyModule2.AddProperty(new string[] { "Video", "Input" }, "MainVideoSource", new string[] { "SetMainVideoSource ConnectorId: 1", "SetMainVideoSource ConnectorId: 2" });
            propertyModule2.AddProperty(new string[] { "Conference", "Presentation", "LocalInstance" }, "SendingMode", new string[] { "LocalOnly", "LocalRemote", "Off" }, true);

               
            while (true)
            {
                var console = Console.ReadLine();
                Random random = new Random();
                int randomProp = random.Next(0, propertyModule2.ModuleProperties.Count);
                int setOrGet = random.Next(1, 3);
                if (setOrGet == 1)
                    propertyModule2.GetPropertyValue(randomProp);
                else
                {
                    int randomArg = random.Next(0, propertyModule2.ModuleProperties[randomProp].PropertyArgs.Count);
                    propertyModule2.SetPropertyValue(randomProp, randomArg);
                }
            }

        }


        public static void Module1()
        {
            CommandModule commandModule1 = new CommandModule();
            commandModule1.User = "Tritech";
            commandModule1.Password = "20!9GolfR";
            commandModule1.Host = "192.168.0.113";


            commandModule1.Initialize("Module1");


    }

        public static void Module2()
        {
            PropertyModule propertyModule1 = new PropertyModule();
            propertyModule1.Initialize("Module1");

            propertyModule1.AddProperty(new string[] { "Audio", "Microphones" }, "Mute", new string[] { "Mute", "UnMute" }, true);

 
        }

    }
}
