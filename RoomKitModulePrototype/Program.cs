using System;
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

            while (true)
            {
                Console.ReadLine();
                propertyModule2.GetPropertyValue("Mute");
                Console.ReadLine();
                propertyModule2.SetPropertyValue(0, "Mute");
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
