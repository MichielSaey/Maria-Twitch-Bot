using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Bot_0._0._3
{
    class commands
    {
        private List<string> commandlist = new List<string>();
        private List<string> vipCommandlist = new List<string>();

        public commands()
        {
            commandlist.Add("!awake");
            commandlist.Add("!startvote");
            commandlist.Add("!tanks");

            vipCommandlist.Add("!pirate");
        }

        public Boolean cammandChecker(string msg)
        {
            if (commandlist.Contains(msg))
            {
                return true;
            }
            return false;
        }

        public Boolean vipCammandChecker(string msg)
        {
            if (vipCommandlist.Contains(msg))
            {
                return true;
            }
            return false;
        }

        public string commandsSelector(string msg)
        {
            string output = null;

            switch (msg)
            {

                case "!awake":
                    output = awake();
                    break;
                case "!startvote":
                    output = startvote();
                    break;
                case "!tanks":
                    output = tanksmaria();
                    break;
                case "!pirate":
                    output = pirate();
                    break;
            }
            return output;
        }

        public string vipCommandsSelector(string msg)
        {
            string output = null;

            switch (msg)
            {
                case "!pirate":
                    output = pirate();
                    break;
            }
            return output;
        }

        private string pirate()
        {
            Console.WriteLine("start vote commando actevatid vote command");
            string say = "Arr maties! I'm the pirate queen!!!";
            return say;
        }

        private string startvote()
        {
            Console.WriteLine("start vote commando actevatid vote command");
            string say = "startvote";
            return say;
        }

        private string awake()
        {
            Console.WriteLine("awake command started");
            string say = "yeah shut up i'm here";
            return say;
        }
        private string tanksmaria()
        {
            Console.WriteLine("awake command started");
            string say = "Tank you so much everybody.";
            return say;
        }
    }
}
