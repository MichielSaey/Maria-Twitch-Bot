using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Bot_0._0._3
{
    class TextControoller
    {
        public string tochat;
        public string sendinguser;
        public string preamble;
        public string msg;
        public string commandVar;
        private string[] message;

        public TextControoller(string completeMsg)
        {
            if (completeMsg != null)
            {
                if (completeMsg != null)
                {

                    message = completeMsg.Split(':');
                    string[] preamble = message[1].Split(' ');
                    string tochat;

                    try
                    {
                        this.preamble = preamble[1];
                    }
                    catch (System.IndexOutOfRangeException e)
                    {
                        System.Console.WriteLine(e.Message);
                    }


                    if (this.preamble == "PRIVMSG")
                    {
                        string[] sendingUser = preamble[0].Split('!');
                        tochat = sendingUser[0] + ": " + message[2];
                        sendinguser = sendingUser[0];
                        this.tochat = tochat;
                        msg = message[2];

                    }

                }
            }

        }

        public void getThirdVar()
        {
            commandVar = message[2];
            if (commandVar.Length >= 7)
            {
                commandVar = commandVar.Substring(6);
                Console.WriteLine(commandVar);
            }
        }
    }
}
