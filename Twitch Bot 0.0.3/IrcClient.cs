using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Bot_0._0._3
{
    class IrcClient
    {
        private static byte[] data;
        private TcpClient client;
        private StreamReader inputStream;
        private StreamWriter outputstream;

        private string user;
        private string channel;

        public IrcClient(string ip, int port, string user, string pass)
        {

            client = new TcpClient(ip, port);
            inputStream = new StreamReader(client.GetStream());
            outputstream = new StreamWriter(client.GetStream());
            this.user = user;

            outputstream.Flush();

            // login string aanmaken en om zetten naarbyte
            string loginstring = "PASS " + pass + "\r\nNICK " + user + "\r\n";

            // login string versturen
            outputstream.WriteLine(loginstring);

            outputstream.Flush();

            Console.WriteLine("Sent login.\r\n");
            Console.WriteLine("USER " + user);
            Console.WriteLine("PASS " + pass);
            Console.WriteLine("IP " + ip);
            Console.WriteLine("PORT " + port + "\r\n");

            // Buffer om bytes op te slaan
            data = new Byte[512];

            // opvang string aanmaken
            String responseData = String.Empty;

            //respons terug krijgen
            Console.WriteLine("RECEIVED WELCOME: \r\n\r\n{0}", ReadMessage());

        }

        public void JoinChannel(string channel)
        {
            this.channel = channel;

            // joinstring aanmaken omzetten naar bytes en verzenden
            string joinstring = "JOIN " + "#" + channel + "\r\n";
            outputstream.WriteLine(joinstring);

            outputstream.Flush();

            Console.WriteLine("SENT CHANNEL JOIN REQUEST\r\n");
            Console.WriteLine("JOINED CHANNEL #" + channel);

            //aan de server zeggen dat we aanwezig zijn
            string announcestring = channel + "!" + channel + "@" + channel + ".tmi.twitch.tv PRIVMSG " + channel + " BOT ENABLED\r\n";
            outputstream.WriteLine(announcestring);

            outputstream.Flush();

            //twerkt
            Console.WriteLine("TWITCH CHAT HAS BEGUN.\r\n");

        }

        public string ReadMessage()
        {
            try
            {
                string message = inputStream.ReadLine();
                Console.WriteLine(message);
                return message;

            }
            catch (Exception ex)
            {
                return "Error receiving message: " + ex.Message;
            }
        }

        public void sendPublicmsg(string msg)
        {
            try
            {
                sendIrcMsg(":" + user + "!" + user + "@" + user + ".tmi.twitch.tv PRIVMSG #" + channel + " :" + msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void sendIrcMsg(string msg)
        {
            try
            {
                outputstream.WriteLine(msg);
                outputstream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void pingpong()
        {
            try
            {
                sendIrcMsg("PONG :tmi.twitch.tv");
                Console.WriteLine("Ping? Pong!");
            }
            catch (Exception e)
            {
                Console.WriteLine("OH SHIT SOMETHING WENT WRONG\r\n", e);
            }
        }


    }
}
