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
                essentialsModule.CameraSelection.SetValue(new int[,] {{ 0, 0 }});
                Console.ReadLine();
                essentialsModule.CameraSelection.SetValue(new int[,] { { 0, 1 } });
                Console.ReadLine();
                essentialsModule.StandbyState.SetState(1);
                Console.ReadLine();
               
                essentialsModule.PrivacyMuteState.SetState(1);
                //Console.ReadLine();

                Console.WriteLine($"Standby - {essentialsModule.StandbyState.CurrentStatusIndex} - {essentialsModule.StandbyState.CurrentStatusString}");
                Console.WriteLine($"Microphones - {essentialsModule.PrivacyMuteState.CurrentStatusIndex} - {essentialsModule.PrivacyMuteState.CurrentStatusString}");

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


 
        }

    }
}
