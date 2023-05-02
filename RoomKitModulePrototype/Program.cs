using System;

namespace RoomKitModulePrototype
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            CoreModule coreModule = new CoreModule();
            ButtonModule buttonModule = new ButtonModule();

            /*            foreach(var prop in coreModule.ModuleProperties)
                        {
                            Debug.Log(prop.Name);
                        }*/

            coreModule.AddProperty(new string[] { "Audio", "Microphones" }, "Mute", new string[] { "Mute", "UnMute" });



            while (true)
            {
                Console.ReadLine();
                for (var i = 0; i < coreModule.ModuleProperties.Count; i++)
                {
                    var name = coreModule.ModuleProperties[i].Name;
                    string line = i + " - " + name;
                    Debug.Log(line);
                }

                var prop = Console.ReadLine();
                var iprop = Int32.Parse(prop);

                var property = coreModule.ModuleProperties[iprop];

                for (var i = 0; i < property.CommandList.Count; i++)
                {
                    var name = property.CommandList[i].GetType().ToString();
                    string line = i + " - " + name;
                    Debug.Log(line);
                }
                var cmd = Console.ReadLine();
                var icmd = Int32.Parse(cmd);

                var command = property.CommandList[icmd];

                if(command is IHasArguments newCommand)
                {
                    var argCommand = (IHasArguments)command;
                    
                    foreach(var arg in argCommand.CommandArgs)
                    {
                        Debug.Log(arg);
                    }
                    var sarg = Console.ReadLine();

                    coreModule.SendPropertyCommand(iprop, icmd, sarg);
                }
                else
                {
                    coreModule.SendPropertyCommand(iprop, icmd);
                }


                
                /*                var cmd = Console.ReadLine();
                                if (program.TestCommands(Int32.Parse(cmd), AudioMute)) 
                                comm.SendCommand(cmd);*/
            }


            

            





        }

        public bool TestCommands(int num, CiscoProperty prop)
        {
            switch (num)
            {
                case 1:
                    prop.GetStatus();
                return false;
                case 2:
                    prop.SetStatus("Mute");
                    return false;
                case 3:
                    prop.SetStatus("UnMute");
                    return false;
                default:
                    return true;

            }






        }
    }
}
