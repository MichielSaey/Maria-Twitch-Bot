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
    class commands
    {

        private DbController dbcontroller = new DbController();

        public string respons;
        public string returnvar;


        public Boolean modlistChecker(string sendingUser)
        {
            dbcontroller.exeQuery("SELECT Users.[username], Users.[mod] FROM Users WHERE Users.[mod] = TRUE AND Users.[username] = '" + sendingUser + "' ;");
            if (dbcontroller.DBDT.Rows.Count != 0)
            {
                if (dbcontroller.DBDT.Rows[0].Field<string>(0) == sendingUser)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void ModCommandChecker(string command)
        {
            dbcontroller.exeQuery("SELECT Commands.[Command], Commands.[respons], Commands.[returnVar], Commands.[modCommand] FROM Commands WHERE Commands.[modCommand] = TRUE ");

            if (dbcontroller.DBDT.Rows.Count != 0)
            {
                foreach (DataRow row in dbcontroller.DBDT.Rows)
                {
                    if (command.Contains(row.Field<string>(0)))
                    {
                        respons = row.Field<string>(1);
                        returnvar = row.Field<string>(2);
                    }
                }
            }
        }

        public List<string> getCommandList()
        {
            dbcontroller.exeQuery("SELECT Commands.[Command] FROM Commands ");

            List<string> CommandList = new List<string>();

            foreach (DataRow row in dbcontroller.DBDT.Rows)
            {
                CommandList.Add(row.Field<string>(0));
            }

            return CommandList;
        }
    }
}
