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


            coreModule.AddProperty(new string[] { "Audio", "Microphones" }, "Mute", new string[] { "Mute", "UnMute" }, true);
            coreModule.AddProperty(new string[] { "Standby" }, "State", new string[] { "Activate", "Deactivate", "Halfwake" }, true);



            while (true)
            {
                Console.ReadLine();
                for (var i = 0; i < coreModule.ModuleProperties.Count; i++)
                {
                    var prop = coreModule.ModuleProperties[i];

                    string line = i + " - " + String.Join(" ",prop.Path) + " " + prop.StatusArg;
                    Debug.Log(line);
                }

                //Select a property to affect
                var propSelect = Console.ReadLine();
                var ipropSelect = Int32.Parse(propSelect);

                Console.WriteLine("0 - GetStatus");
                Console.WriteLine("1 - SetStatus");
                Console.WriteLine("2 - SetFeedback");

                //Select a command to send
                var cmd = Console.ReadLine();
                
                if(cmd == "0")
                {
                    coreModule.GetPropertyValue(coreModule.ModuleProperties[ipropSelect].StatusArg);
                }
                else if (cmd == "1")
                {
                    foreach(var arg in coreModule.ModuleProperties[ipropSelect].PropertyArgs)
                    {
                        Console.WriteLine(arg);
                    }
                    var argSelect = Console.ReadLine();
                    coreModule.SetPropertyValue(coreModule.ModuleProperties[ipropSelect].StatusArg, argSelect);
                }
                else if(cmd == "2")
                {
                    coreModule.SetFeedback(coreModule.ModuleProperties[ipropSelect].StatusArg);

                }


            }


            

            





        }

        
    }
}
