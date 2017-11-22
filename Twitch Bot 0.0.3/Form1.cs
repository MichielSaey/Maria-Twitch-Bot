using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Bot_0._0._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChannelTextBox.Text = "steven_mc19";
            PortTextBox.Text = "6667";
            ServerTextbox.Text = "irc.chat.twitch.tv";
            OathKeyTextbox.Text = "oauth:djteqbw4y8kgwky1vqs301xvliugos";
            UsernameTextBox.Text = "assistentmaria";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> modList = new List<string>();
            List<string> VipList = new List<string>();

            modList.Add("g0rtys");
            modList.Add("nightbot");
            modList.Add("steven_mc19");

            //modList.Add("skydiver555");

            VipList.Add("PirateOverLord");


            //intellingen
            int port = Int32.Parse(PortTextBox.Text);
            string ip = ServerTextbox.Text;
            //channel zonder caps
            string channel = ChannelTextBox.Text;
            string pass = OathKeyTextbox.Text;
            string user = UsernameTextBox.Text;

            // verbinding naar Irc client maken
            IrcClient irc = new IrcClient(ip, port, user, pass);
            irc.JoinChannel(channel);

            //nodige elementen declarre,
            commands cmd = new commands();
            Boolean startvote = false;
            TextControoller xtc;

            //constante loop die berichten in leest
            while (true)
            {

                if (startvote == true)
                {
                    Stopwatch klokje = new Stopwatch();
                    klokje.Start();
                    string msg;
                    TextControoller Speed;
                    List<string> userList = new List<string>();

                    int aantalA = 0;
                    int aantalB = 0;
                    int aantalX = 0;

                    while (klokje.Elapsed <= TimeSpan.FromSeconds(18))
                    {
                        msg = irc.ReadMessage();
                        Speed = new TextControoller(msg);


                        if (msg != "PING :tmi.twitch.tv")
                        {
                            if (Speed.msg.Contains("!cast"))
                            {

                                if (!userList.Contains(Speed.sendinguser))
                                {
                                    Speed.getThirdVar();
                                    switch (Speed.commandVar.ToUpper())
                                    {
                                        case "A":
                                            aantalA++;
                                            Console.WriteLine("A counted");
                                            userList.Add(Speed.sendinguser);
                                            Console.WriteLine("!cast registerd");
                                            break;
                                        case "B":
                                            aantalB++;
                                            Console.WriteLine("B counted");
                                            userList.Add(Speed.sendinguser);
                                            Console.WriteLine("!cast registerd");
                                            break;
                                        case "X":
                                            aantalX++;
                                            Console.WriteLine("C counted");
                                            userList.Add(Speed.sendinguser);
                                            Console.WriteLine("!cast registerd");
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            irc.pingpong();
                        }

                    }
                    int quickMath = Math.Max(aantalA, Math.Max(aantalB, aantalX));

                    if (quickMath == aantalA && quickMath == aantalB && quickMath == aantalX)
                    {
                        irc.sendPublicmsg("You guys voted all on different things? Why? Now we have a tie breaker. of " + quickMath + " votes");
                    }
                    else if (quickMath == aantalA && quickMath == aantalB)
                    {
                        irc.sendPublicmsg("Yep you guys voted a tie breaker again *sigh* with A and B" + quickMath + " votes");
                    }
                    else if (quickMath == aantalB && quickMath == aantalX)
                    {
                        irc.sendPublicmsg("For real tie breaker. Again. Between B and X with " + quickMath + " votes");
                    }
                    else if (quickMath == aantalA && quickMath == aantalX)
                    {
                        irc.sendPublicmsg("This is not making my job more fun. Tie breaker Between A and X of " + quickMath + " votes");
                    }
                    else if (quickMath == aantalA)
                    {
                        irc.sendPublicmsg("okay most of you voted A. this is why democracy doesn't work. " + quickMath + " of you idiots voted wrong");
                    }
                    else if (quickMath == aantalB)
                    {
                        irc.sendPublicmsg("So B won, whatever. With like " + quickMath + " votes or what ever");
                    }
                    else if (quickMath == aantalX)
                    {
                        irc.sendPublicmsg("Who of you idiots voted for X " + quickMath + " times.");
                    }
                    startvote = false;
                }
                // ingelezen string in de textController zetten
                string myCompleteMessage = irc.ReadMessage();

                xtc = new TextControoller(myCompleteMessage);

                switch (myCompleteMessage)
                {
                    // ping pong om in leven te blijven

                    case "PING :tmi.twitch.tv":
                        irc.pingpong();
                        break;
                    default:
                        try
                        {

                            // This means it's a message to the channel.  Yes, PRIVMSG is IRC for messaging a channel too
                            if (xtc.preamble == "PRIVMSG")
                            {
                                string command = xtc.msg; ;
                                if (modList.Contains(xtc.sendinguser))
                                {
                                    if (cmd.cammandChecker(command))
                                    {
                                        string say = cmd.commandsSelector(command);
                                        if (say == "startvote")
                                        {
                                            startvote = true;
                                            irc.sendPublicmsg("So yea,you idiots can vote for like 20 seconds. Using !cast with (A,B or X).");
                                        }
                                        else
                                        {
                                            irc.sendPublicmsg(say);
                                        }
                                    }
                                }
                                else if (VipList.Contains(xtc.sendinguser))
                                {
                                    if (cmd.vipCammandChecker(command))
                                    {
                                        string say = cmd.vipCommandsSelector(command);

                                        irc.sendPublicmsg(say);

                                    }
                                }
                            }
                        }

                        // This is a disgusting catch for something going wrong that keeps it all running.  I'm sorry.
                        catch (Exception f)
                        {
                            Console.WriteLine("OH well it aint AI\r\n", f);
                        }
                        break;
                }
            }
        }
    }
}
