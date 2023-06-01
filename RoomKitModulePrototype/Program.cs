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

/*            Thread CallModule2 = new Thread(Module2);
            CallModule2.Start();*/


            EssentialsModule essentialsModule = new EssentialsModule();
            essentialsModule.Initialize("Module1");
            

            while (true)
            {
                Console.ReadLine();
                essentialsModule.StandbyState.StandbyOn();
                Console.ReadLine();
                essentialsModule.StandbyState.StandbyOff();

            }

        }


        public static void Module1()
        {
            CommandModule commandModule1 = new CommandModule();
            commandModule1.Initialize("Module1");
            //commandModule1.Codec.InitializeSSH("Tritech", "20!9GolfR", "192.168.0.113");

    }

        public static void Module2()
        {


 
        }

    }
}
