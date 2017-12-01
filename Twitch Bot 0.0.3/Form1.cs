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

        private DbController dbcontroler = new DbController();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dbcontroler.exeQuery("SELECT * FROM settings");

            ChannelTextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(3);
            PortTextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(1);
            ServerTextbox.Text = dbcontroler.DBDT.Rows[0].Field<string>(2);
            OathKeyTextbox.Text = dbcontroler.DBDT.Rows[0].Field<string>(4);
            UsernameTextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(5);
            VotingTimerTextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(6);
            Voteoption1TextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(7);
            Voteoption2TextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(8);
            Voteoption3TextBox.Text = dbcontroler.DBDT.Rows[0].Field<string>(9);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            //intellingen
            int port = Int32.Parse(PortTextBox.Text);
            string ip = ServerTextbox.Text;
            //channel zonder caps
            string channel = ChannelTextBox.Text;
            string pass = OathKeyTextbox.Text;
            string user = UsernameTextBox.Text;
            //command settings
            double voteTimer = Double.Parse(VotingTimerTextBox.Text);
            string voteOption1 = Voteoption1TextBox.Text;
            string voteOption2 = Voteoption2TextBox.Text;
            string voteOption3 = Voteoption3TextBox.Text;

            // verbinding naar Irc client maken
            IrcClient irc = new IrcClient(ip, port, user, pass);
            irc.JoinChannel(channel);

            Boolean startvote = false;

            //constante loop die berichten in leest
            while (true)
            {
                //nodige elementen declarre,
                commands cmd = new commands();
                TextControoller xtc;

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

                    while (klokje.Elapsed <= TimeSpan.FromSeconds(voteTimer))
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

                                    if (Speed.commandVar.ToUpper() == voteOption1)
                                    {
                                        aantalA++;
                                        Console.WriteLine(voteOption1 + "counted");
                                        userList.Add(Speed.sendinguser);
                                        Console.WriteLine("!cast registerd");                                      
                                    }
                                    else if (Speed.commandVar.ToUpper() == voteOption2)
                                    {
                                        aantalB++;
                                        Console.WriteLine("B counted");
                                        userList.Add(Speed.sendinguser);
                                        Console.WriteLine("!cast registerd");                                     
                                    }
                                    else if (Speed.commandVar.ToUpper() == voteOption3)
                                    {
                                        aantalX++;
                                        Console.WriteLine("C counted");
                                        userList.Add(Speed.sendinguser);
                                        Console.WriteLine("!cast registerd");  
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
                        irc.sendPublicmsg("Yep you guys voted a tie breaker again *sigh* with " + voteOption1 + " and " + voteOption2 + " " + quickMath + " votes");
                    }
                    else if (quickMath == aantalB && quickMath == aantalX)
                    {
                        irc.sendPublicmsg("For real tie breaker. Again. Between " + voteOption2 + " and " + voteOption3 + " with " + quickMath + " votes");
                    }
                    else if (quickMath == aantalA && quickMath == aantalX)
                    {
                        irc.sendPublicmsg("This is not making my job more fun. Tie breaker Between " + voteOption1 + " and " + voteOption3 + " of " + quickMath + " votes");
                    }
                    else if (quickMath == aantalA)
                    {
                        irc.sendPublicmsg("okay most of you voted " + voteOption1 + ". this is why democracy doesn't work. " + quickMath + " of you idiots voted wrong");
                    }
                    else if (quickMath == aantalB)
                    {
                        irc.sendPublicmsg("So " + voteOption2 + " won, whatever. With like " + quickMath + " votes or what ever");
                    }
                    else if (quickMath == aantalX)
                    {
                        irc.sendPublicmsg("Who of you idiots voted for " + voteOption3 + " " + quickMath + " times.");
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
                                if (cmd.modlistChecker(xtc.sendinguser))
                                {
                                    cmd.ModCommandChecker(command);

                                    switch (cmd.returnvar)
                                    {
                                        case "startvote":
                                            irc.sendPublicmsg(cmd.respons + " (" + voteOption1 + " ," + voteOption2 + " ," + voteOption3 + ") for the next " + voteTimer + " seconds");
                                            startvote = true;
                                            Console.WriteLine("vote command starded");
                                            break;
                                        case "settime":
                                            xtc.getThirdVar();
                                            voteTimer = Int32.Parse(xtc.commandVar);
                                            Console.WriteLine("voteTime set to " + voteTimer);
                                            irc.sendPublicmsg("The vote time has been set to " + voteTimer);
                                            break;
                                        case "setoption":
                                            xtc.get3Options();
                                            voteOption1 = xtc.option1;
                                            voteOption2 = xtc.option2;
                                            voteOption3 = xtc.option3;

                                            Console.WriteLine("vote optoions have been set to" + voteOption1 + " " + voteOption2 + " " + voteOption3);
                                            irc.sendPublicmsg(cmd.respons + voteOption1 + " " + voteOption2 + " " + voteOption3);

                                            break;
                                        case "stop":
                                            irc.sendPublicmsg(cmd.respons);
                                            StopBotton();
                                            return;
                                        case "controlle":
                                            dbcontroler.exeQuery("UPDATE Users SET mod = TRUE WHERE username = 'skydiver555'");
                                            irc.sendPublicmsg(cmd.respons);
                                            break;
                                        case "stopcontrolle":
                                            dbcontroler.exeQuery("UPDATE Users SET mod = FALSE WHERE username = 'skydiver555'");
                                            irc.sendPublicmsg(cmd.respons);
                                            break;
                                        case "commandList":
                                            List<string> commandList = new List<string>();
                                            commandList = cmd.getCommandList();
                                            string text = "";
                                            foreach (string i in commandList)
                                            {
                                                text = text + i + " ";
                                            }
                                            irc.sendPublicmsg(text);
                                            break;
                                        default:
                                            irc.sendPublicmsg(cmd.respons);
                                            break;
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

        private void StopBotton()
        {
            Console.WriteLine("Maria is stopped");
            button1.Enabled = true;

        }
    }
}
