using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;

namespace Twitch_Bot_0._0._3
{
    public class DbController
    {
        // CREATE CONNECTION '

        private OleDbConnection DBCon = new OleDbConnection("provider=Microsoft.Ace.OLEDB.12.0;Data Source=twitchBot.accdb;");
        // PREPARE DB COMMAND '

        private OleDbCommand DBCmd;
        // DB DATA '
        public OleDbDataAdapter DBDA;

        public DataTable DBDT;
        // QUERY PARAMITERS '

        public List<OleDbParameter> @params = new List<OleDbParameter>();
        // GUERY STATISTICS '
        public Int32 recordcount;

        public string exeptions;
        public void exeQuery(string query)
        {
            // RESET STATISTICS '
            recordcount = 0;
            exeptions = "";

            try
            {
                // OPEN CONNECTION ' 
                DBCon.Open();

                // CREATE DB COMMAND '
                DBCmd = new OleDbCommand(query, DBCon);

                // LOAD PARAMS INTO DB COMMAND '
                @params.ForEach(p => DBCmd.Parameters.Add(p));

                // purge list '
                @params.Clear();

                // exe command + fill data table '
                DBDT = new DataTable();
                DBDA = new OleDbDataAdapter(DBCmd);
                recordcount = DBDA.Fill(DBDT);


            }
            catch (Exception ex)
            {
                exeptions = ex.Message;

            }

            // close db connection '

            if (DBCon.State == ConnectionState.Open)
            {
                DBCon.Close();
            }

        }

        // include query & command params '

        public void AadParam(string nama, object value)
        {
            OleDbParameter newParam = new OleDbParameter(nama, value);
            @params.Add(newParam);
        }



    }

}



