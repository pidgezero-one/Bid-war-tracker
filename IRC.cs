using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using System.Timers;
using System.Windows;

namespace WpfApp2
{
    internal struct IRCConfig
    {
        public bool joined;
        public string server;
        public int port;
        public string nick;
        public string name;
        public string channel;
        public string password;

        public string getChannel()
        {
            return channel;
        }
    }


    internal class IRCBot : IDisposable
    {
        private TcpClient IRCConnection = null;
        private IRCConfig config;
        private NetworkStream ns = null;
        private StreamReader sr = null;
        private StreamWriter sw = null;

        public IRCBot(IRCConfig config)
        {
            this.config = config;
        }

        public void log(String s)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            List<string> l = new List<string>();
            l.Add(unixTimestamp + " " + s);
            File.AppendAllLines("output/logs.txt", l);
        }

        public void Connect()
        {
            try
            {
                IRCConnection = new TcpClient(config.server, config.port);
            }
            catch
            {
                Console.WriteLine("Connection Error");
                throw;
            }

            try
            {
                ns = IRCConnection.GetStream();
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);
                //sendData("USER", config.nick + " 0 * " + config.name);
                sendData("PASS", config.password);
                sendData("NICK", config.nick);
            }
            catch
            {
                Console.WriteLine("Communication error");
                throw;
            }
        }

        public void sendData(string cmd, string param)
        {
            if (param == null)
            {
                sw.WriteLine(cmd);
                sw.Flush();
                System.Diagnostics.Debug.WriteLine(cmd);
            }
            else
            {
                sw.WriteLine(cmd + " " + param);
                sw.Flush();
                System.Diagnostics.Debug.WriteLine(cmd + " " + param);
            }
        }

        

        public void IRCWork(int uid, List<string> bidOptions)
        {
            string[] ex;
            string data;
            System.Diagnostics.Debug.WriteLine("IRC open");
            using (sr)
            {

                while ((data = sr.ReadLine()) != null)
                //string[] readText = File.ReadAllLines("logs2.txt");
                //foreach (string data in readText)
                {
                    List<string> lines = new List<string>();
                    lines.Add((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + " [IRC]  " + data);
                    File.AppendAllLines("output/logs.txt", lines);
                    char[] charSeparator = new char[] { ' ' };
                    try
                    {
                        ex = data.Split(charSeparator); //Split the data into 5 parts
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        if (!config.joined) //if we are not yet in the assigned channel
                        {
                            if (ex[1] == "001") //Normally one of the last things to be sent (usually follows motd)
                            {
                                sendData("CAP REQ :twitch.tv/tags twitch.tv/commands", null);
                                sendData("JOIN", config.channel); //join assigned channel
                                config.joined = true;
                            }
                        }

                        if (ex[0] == "PING")  //respond to pings
                        {
                            sendData("PONG", ex[1]);
                        }

                        if (ex[1] == "311")  //respond to whois
                        {
                            Console.WriteLine(String.Join(", ", ex));
                        }

                        int target = 0;
                        if (ex[0].Substring(0,1) == "@") {
                            target = 2;
                        }
                        else
                        {
                            target = 1;
                        }
                        
                        //user wants to include a bid war option if they forgot it previously
                        if (ex[target] == "PRIVMSG" && ex[target+1].ToLower() == config.getChannel().ToLower())
                        {
                            string donator = "";
                            string message = "";
                            char[] nameRetrieverSeparator = new char[] { '!' };
                            string[] nameData = ex[target - 1].Split(nameRetrieverSeparator, 2);
                            donator = nameData[0].Replace(":", "");
                            //New subscriber
                            if (donator == "twitchnotify" && data.ToLower().Trim().Contains("subscribed"))
                            {
                                string name = ex[target + 2].Replace(":", "");
                                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                                {
                                    db.Open();
                                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                                    insertSQL.Parameters.AddWithValue("@Id1", name + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User1", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount1", 500);
                                    insertSQL.Parameters.AddWithValue("@Currency1", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator1", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                                    insertSQL.Parameters.AddWithValue("@Message1", "");
                                    insertSQL.Parameters.AddWithValue("@Id", name + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount", 500);
                                    insertSQL.Parameters.AddWithValue("@Currency", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                                    insertSQL.Parameters.AddWithValue("@Message", "");
                                    try
                                    {
                                        int ret = insertSQL.ExecuteNonQuery();
                                        List<string> lines2 = new List<string>();
                                        lines2.Add(unixTimestamp + " [SUB]  New Sub Added: " + name);
                                        File.AppendAllLines("output/logs.txt", lines2);
                                        db.Close();
                                    }
                                    catch (SqliteException exe) { 
                                        db.Close();
                                    }
                                    File.WriteAllText("output/latest.txt", "Latest Esports Sponsorship: " + name + " (new sub)");
                                }
                            }
                            //CHECK FOR BITS FIRST!
                            //If not a new subscriber, check and see if it's an attempt to correct a donation message
                            else
                            {
                                //explode param 0 and check for bits property
                                Boolean bits = false;
                                int numBits = 0;
                                if (target == 2)
                                {
                                    char[] metadataSeparator = new char[] { ';' };
                                    string[] metadata = ex[0].Split(metadataSeparator);
                                    foreach (var datum in metadata)
                                    {
                                        if (datum.Contains("bits="))
                                        {
                                            char[] bitExtractor = new char[] { '=' };
                                            string[] extractedBits = datum.Split(bitExtractor);
                                            if (Int32.TryParse(extractedBits[1], out numBits))
                                                bits = true;
                                        }
                                    }
                                }
                                //If this is a cheer, save it as such
                                if (bits)
                                {
                                    String id = "";
                                    char[] metadataSeparator = new char[] { ';' };
                                    string[] metadata = ex[0].Split(metadataSeparator);
                                    foreach (var datum in metadata)
                                    {
                                        if (datum.IndexOf("id=") == 0)
                                        {
                                            char[] idExtractor = new char[] { '=' };
                                            string[] extractedId = datum.Split(idExtractor);
                                            id = extractedId[1];
                                        }
                                    }
                                    log(id);
                                    string msg = "";
                                    for (int x = target + 2; x < ex.Length; x++)
                                    {
                                        string input;
                                        if (x == target + 2)
                                        {
                                            input = ex[x].Replace(":", "");

                                        }
                                        else
                                        {
                                            input = ex[x];
                                        }
                                        msg += input + " ";
                                    }
                                    using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                                    {
                                        db.Open();
                                        SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                                        insertSQL.Parameters.AddWithValue("@Id1", id);
                                        insertSQL.Parameters.AddWithValue("@User1", uid);
                                        insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                                        insertSQL.Parameters.AddWithValue("@Amount1", numBits);
                                        insertSQL.Parameters.AddWithValue("@Currency1", "BIT");
                                        insertSQL.Parameters.AddWithValue("@Donator1", donator);
                                        insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                                        insertSQL.Parameters.AddWithValue("@Message1", msg);
                                        insertSQL.Parameters.AddWithValue("@Id", id);
                                        insertSQL.Parameters.AddWithValue("@User", uid);
                                        insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                                        insertSQL.Parameters.AddWithValue("@Amount", numBits);
                                        insertSQL.Parameters.AddWithValue("@Currency", "BIT");
                                        insertSQL.Parameters.AddWithValue("@Donator", donator);
                                        insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                                        insertSQL.Parameters.AddWithValue("@Message", msg);
                                        try
                                        {
                                            int ret = insertSQL.ExecuteNonQuery();
                                            List<string> lines6 = new List<string>();
                                            lines6.Add(unixTimestamp + " [BIT]  cheer" + numBits + " from " + donator);
                                            File.AppendAllLines("output/logs.txt", lines6);
                                            db.Close();
                                        }
                                        catch (SqliteException exe)
                                        {
                                            int ret = insertSQL.ExecuteNonQuery();
                                            List<string> lines6 = new List<string>();
                                            lines6.Add(unixTimestamp + " [BIT] error saving cheer" + numBits + " from " + donator);
                                            File.AppendAllLines("output/logs.txt", lines6);
                                            db.Close();
                                        }
                                        File.WriteAllText("output/latest.txt", "Latest Esports Sponsorship: " + donator + " (" + numBits + " bits)");
                                    }
                                }
                                //otherwise, check if it is an update attempt
                                else
                                {

                                    Boolean attemptUpdate = false;
                                    for (int x = target + 2; x < ex.Length; x++)
                                    {

                                        string input;
                                        if (x == target + 2)
                                        {
                                            input = ex[x].Replace(":", "");

                                        }
                                        else
                                        {
                                            input = ex[x];
                                        }
                                        foreach (var option in bidOptions)
                                        {
                                            if (input.ToLower().Contains(option.ToLower().Trim()))
                                            {
                                                message += input + " ";
                                                attemptUpdate = true;
                                            }
                                        }
                                    }
                                    System.Timers.Timer doDonationUpdate = new System.Timers.Timer();

                                    //if so, attempt to update any donation made in the last 60 seconds by this user, but execute it 10 seconds in the future
                                    if (attemptUpdate)
                                    {
                                        doDonationUpdate.Enabled = true;
                                        doDonationUpdate.Elapsed += new ElapsedEventHandler(doUpdate);
                                        doDonationUpdate.Interval = 11000;
                                    }
                                    else
                                    {
                                        doDonationUpdate.Enabled = false;
                                        doDonationUpdate.Stop();

                                    }
                                    void doUpdate(object source, ElapsedEventArgs ev)
                                    {
                                        doDonationUpdate.Enabled = false;
                                        doDonationUpdate.Stop();
                                        Int32 newUnixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                                        List<string> lines3 = new List<string>();
                                        lines3.Add(newUnixTimestamp + " [META] Attempting to update any unmatched donation made since " + (newUnixTimestamp - 71).ToString() + " made by " + donator);
                                        File.AppendAllLines("output/logs.txt", lines3);

                                        //if DB is already open from a cheer event or something else, just try again when closed
                                        try
                                        {
                                            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                                            {
                                                db.Open();
                                                SqliteCommand fetchDonations = new SqliteCommand("SELECT * FROM allDonations7 WHERE User = @User AND LOWER(Donator) = @Donator AND Created_At >= @1MinAgo ORDER BY Created_At DESC", db);
                                                fetchDonations.Parameters.AddWithValue("@User", uid);
                                                fetchDonations.Parameters.AddWithValue("@Donator", donator.ToLower());
                                                fetchDonations.Parameters.AddWithValue("@1MinAgo", (newUnixTimestamp - 71));
                                                Boolean foundDonationToEdit = false;
                                                using (SqliteDataReader query = fetchDonations.ExecuteReader())
                                                {
                                                    while (query.Read())
                                                    {
                                                        Boolean foundOptionInDonation = false;
                                                        foreach (var option in bidOptions) //only update one donation
                                                        {
                                                            if (query.GetString(7).Contains(option.ToLower().Trim()))
                                                            {
                                                                foundOptionInDonation = true;
                                                            }
                                                        }
                                                        if (!foundOptionInDonation && !foundDonationToEdit)
                                                        {
                                                            foundDonationToEdit = true;
                                                            string newMessage = query.GetString(7) + " " + message;
                                                            SqliteCommand updateDonation = new SqliteCommand("UPDATE allDonations7 SET Message = @NewMessage WHERE Id = @EditableId", db);
                                                            updateDonation.Parameters.AddWithValue("@NewMessage", newMessage);
                                                            updateDonation.Parameters.AddWithValue("@EditableId", query.GetString(0));
                                                            updateDonation.ExecuteNonQuery();
                                                            List<string> lines4 = new List<string>();
                                                            lines4.Add(unixTimestamp + " [META] Updated " + query.GetString(0));
                                                            File.AppendAllLines("output/logs.txt", lines4);
                                                        }
                                                    }
                                                    if (!foundDonationToEdit)
                                                    {
                                                        List<string> lines5 = new List<string>();
                                                        lines5.Add(unixTimestamp + " [META] Did not find a donation from " + donator + " to update since " + (newUnixTimestamp - 71).ToString());
                                                        File.AppendAllLines("output/logs.txt", lines5);
                                                    }
                                                    db.Close();
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            doUpdate(source, ev);
                                        }
                                    }
                                }
                            }
                        }


                        //resubs
                        if (ex[target] == "USERNOTICE" && ex[target + 1].ToLower() == config.getChannel().ToLower())
                        {
                            string message = "";

                            //resubs
                            bool messageFound = false;
                            for (int x = target + 2; x < ex.Length; x++)
                            {
                                messageFound = true;
                                string input;
                                if (x == target + 2)
                                {
                                    input = ex[x].Replace(":", "");

                                }
                                else
                                {
                                    input = ex[x];
                                }
                                message += input + " ";
                            }

                            char[] nameInfoSeparator = new char[] { ';' };
                            string[] nameInfo = ex[0].Split(nameInfoSeparator);

                            string display_name_param = nameInfo[2];
                            char[] nameParamSeparator = new char[] { '=' };
                            string[] nameChunks = display_name_param.Split(nameParamSeparator);
                            string name = nameChunks[1];
                            int subAmount = 500;
                            string outputSubAmount = "$4.99";
                            if (data.ToLower().Contains("\\ssubscribed"))
                            {
                                foreach (string substr in nameInfo)
                                {
                                    if (substr.IndexOf("system-msg=") == 0)
                                    {
                                        string processSubstr = substr.Replace("system-msg=", "");
                                        message = processSubstr.Replace("\\s", " ");
                                        string[] messageSeparator = new string[] { "\\s" };
                                        string[] messageChunks = processSubstr.Split(messageSeparator, StringSplitOptions.None);
                                        for (int i = 0; i < messageChunks.Length; i++)
                                        {
                                            if (messageChunks[i].IndexOf("$") == 0)
                                            {
                                                if (messageChunks[i] == "$4.99")
                                                {
                                                    subAmount = 500;
                                                    outputSubAmount = "$4.99";
                                                }
                                                else if (messageChunks[i] == "$9.99")
                                                {
                                                    subAmount = 1000;
                                                    outputSubAmount = "$9.99";
                                                }
                                                else if (messageChunks[i] == "$24.99")
                                                {
                                                    subAmount = 2500;
                                                    outputSubAmount = "$24.99";
                                                }
                                            }
                                            else if (messageChunks[i] == "Twitch" && messageChunks[i + 1].IndexOf("Prime") == 0)
                                            {
                                                subAmount = 500;
                                                outputSubAmount = "Twitch Prime";
                                            }
                                        }
                                    }
                                }
                            }
                            string user_id_param = nameInfo[12];
                            string[] userIdChunks = display_name_param.Split(nameParamSeparator);
                            string userid = userIdChunks[1];

                            if (data.ToLower().Contains("msg-id=resub"))
                            {
                                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                                {
                                    db.Open();
                                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                                    insertSQL.Parameters.AddWithValue("@Id1", userid + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User1", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount1", subAmount);
                                    insertSQL.Parameters.AddWithValue("@Currency1", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator1", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                                    insertSQL.Parameters.AddWithValue("@Message1", message);
                                    insertSQL.Parameters.AddWithValue("@Id", userid + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount", subAmount);
                                    insertSQL.Parameters.AddWithValue("@Currency", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                                    insertSQL.Parameters.AddWithValue("@Message", message);
                                    try
                                    {
                                        int ret = insertSQL.ExecuteNonQuery();
                                        List<string> lines6 = new List<string>();
                                        lines6.Add(unixTimestamp + " [SUB]  Resub: " + name + ", message: " + message);
                                        File.AppendAllLines("output/logs.txt", lines6);
                                        db.Close();
                                    }
                                    catch (SqliteException exe)
                                    {
                                        db.Close();
                                    }
                                    File.WriteAllText("output/latest.txt", "Latest Esports Sponsorship: " + name + " (" + outputSubAmount + " resub)");
                                }
                            }
                            else if (data.ToLower().Contains("msg-id=sub"))
                            {
                                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                                {
                                    db.Open();
                                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                                    insertSQL.Parameters.AddWithValue("@Id1", userid + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User1", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount1", subAmount);
                                    insertSQL.Parameters.AddWithValue("@Currency1", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator1", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                                    insertSQL.Parameters.AddWithValue("@Message1", message);
                                    insertSQL.Parameters.AddWithValue("@Id", userid + "_" + unixTimestamp.ToString());
                                    insertSQL.Parameters.AddWithValue("@User", uid);
                                    insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                                    insertSQL.Parameters.AddWithValue("@Amount", subAmount);
                                    insertSQL.Parameters.AddWithValue("@Currency", "SUB");
                                    insertSQL.Parameters.AddWithValue("@Donator", name);
                                    insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                                    insertSQL.Parameters.AddWithValue("@Message", message);
                                    try
                                    {
                                        int ret = insertSQL.ExecuteNonQuery();
                                        List<string> lines6 = new List<string>();
                                        lines6.Add(unixTimestamp + " [SUB]  Sub: " + name);
                                        File.AppendAllLines("output/logs.txt", lines6);
                                        db.Close();
                                    }
                                    catch (SqliteException exe)
                                    {
                                        db.Close();
                                    }
                                    File.WriteAllText("output/latest.txt", "Latest Esports Sponsorship: " + name + " (" + outputSubAmount + " sub)");
                                }

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error with chat operation. Please send a screenshot of this to pidge: " + e.Message);
                    }
                }
            }

            //need to work out sub and resub logic
        }

        public void Dispose()
        {
            if (sr != null)
                sr.Close();
            if (sw != null)
                sw.Close();
            if (ns != null)
                ns.Close();
            if (IRCConnection != null)
                IRCConnection.Close();
        }
    }
}
