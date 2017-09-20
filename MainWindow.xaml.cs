using Microsoft.Data.Sqlite;
using Microsoft.Data.Sqlite.Internal;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WebSocket4Net;

namespace WpfApp2
{
    
    public partial class MainWindow : Window
    {
        string loaded = "";
        bool changed = false;
        string my_username;
        int my_user_id;


        //launch status log
        Window2 window2 = new Window2();

        public void logMessage(string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                var tblock3 = new TextBlock();
                tblock3.Text = text;
                tblock3.TextWrapping = System.Windows.TextWrapping.Wrap;
                this.window2.Logs.Children.Add(tblock3);
            });
        }

        //settings window save/close functions
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Click_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (changed)
            {
                switch (promptSave())
                {
                    case MessageBoxResult.Yes:
                        if (loaded == "")
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                saveFile(saveFileDialog.FileName);
                                closeFile();
                            }
                        }
                        else
                        {
                            saveFile(loaded);
                            closeFile();
                        }
                        break;

                    case MessageBoxResult.No:
                        closeFile();
                        break;

                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                closeFile();
            }
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (loaded == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML file (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    saveFile(saveFileDialog.FileName);
                }

            }
            else
            {
                saveFile(loaded);
            }
        }
        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                saveFile(saveFileDialog.FileName);
            }
        }
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e) {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                if (changed)
                {
                    switch (promptSave())
                    {
                        case MessageBoxResult.Yes:
                            if (loaded == "")
                            {
                                SaveFileDialog saveFileDialog = new SaveFileDialog();
                                saveFileDialog.Filter = "XML file (*.xml)|*.xml";
                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    saveFile(saveFileDialog.FileName);
                                    openFile(openFileDialog.FileName);
                                }
                            }
                            else
                            {
                                saveFile(loaded);
                                openFile(openFileDialog.FileName);
                            }
                            break;

                        case MessageBoxResult.No:
                            openFile(openFileDialog.FileName);
                            break;

                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
                else
                {
                    openFile(openFileDialog.FileName);
                }
            }
        }
        private System.Windows.MessageBoxResult promptSave()
        {
            string sMessageBoxText;
            if (loaded != "")
            {
                sMessageBoxText = "Do you want to save your changes to " + loaded + "?";
            }
            else
            {
                sMessageBoxText = "Do you want to save this configuration?";
            }
            string sCaption = "Save";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return rsltMessageBox;
        }
        private void closeFile()
        {
            initializeBlankConfig();
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                SqliteCommand insertSQL = new SqliteCommand("INSERT INTO lastFile2 (Filename) VALUES (null)", db);
                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    throw new Exception(ex.Message);
                }
                db.Close();
            }
            loaded = "";
            this.Title = "Unsaved Bid War Config";
            changed = false;
        }
        private void openFile(string filename)
        {
            resetConfig();
            XmlDocument xd = new XmlDocument();
            xd.Load(filename);
            XmlNodeList at = xd.SelectNodes("/BidWarConfig/AccessToken");
            foreach (XmlNode atnode in at)
            {
                AccessToken.Password = atnode.InnerText;
            }
            XmlNodeList tt = xd.SelectNodes("/BidWarConfig/TwitchAccessToken");
            foreach (XmlNode ttnode in tt)
            {
                TwitchAccessToken.Password = ttnode.InnerText;
            }
            XmlNodeList fr = xd.SelectNodes("/BidWarConfig/From");
            foreach (XmlNode frnode in fr)
            {
                if (frnode.InnerText != "")
                    DonationCutoff.SelectedDate = DateTime.Parse(frnode.InnerText);
            }
            XmlNodeList un = xd.SelectNodes("/BidWarConfig/Until");
            foreach (XmlNode unnode in un)
            {
                if (unnode.InnerText != "")
                    DonationCutoff2.SelectedDate = DateTime.Parse(unnode.InnerText);
            }
            XmlNodeList options = xd.SelectNodes("/BidWarConfig/Options/Option");
            foreach (XmlNode opnode in options)
            {
                addTextbox(opnode.InnerText);
            }
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                SqliteCommand insertSQL = new SqliteCommand("INSERT INTO lastFile2 (Filename) VALUES (@filename)", db);
                insertSQL.Parameters.AddWithValue("@filename", filename);
                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    throw new Exception(ex.Message);
                }
                db.Close();
            }
            loaded = filename;
            this.Title = loaded;
            changed = false;
        }

        private void saveFile(string fileName)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("BidWarConfig");
            xmlWriter.WriteStartElement("AccessToken");
            xmlWriter.WriteString(AccessToken.Password);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("TwitchAccessToken");
            xmlWriter.WriteString(TwitchAccessToken.Password);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("From");
            if (DonationCutoff.SelectedDate != null)
                xmlWriter.WriteString(DonationCutoff.SelectedDate.Value.Date.ToString("yyyy-MM-dd"));
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Until");
            if (DonationCutoff2.SelectedDate != null)
                xmlWriter.WriteString(DonationCutoff2.SelectedDate.Value.Date.ToString("yyyy-MM-dd"));
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Options");
            foreach (object child in TextBoxes.Children)
            {
                xmlWriter.WriteStartElement("Option");
                var textbox = (TextBox)child;
                xmlWriter.WriteString(textbox.Text);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                SqliteCommand insertSQL = new SqliteCommand("INSERT INTO lastFile2 (Filename) VALUES (@filename)", db);
                insertSQL.Parameters.AddWithValue("@filename", fileName);
                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    throw new Exception(ex.Message);
                }
                db.Close();
            }
            loaded = fileName;
            this.Title = loaded;
            changed = false;
        }

        private void unsaved(object Sender, EventArgs e)
        {
            changed = true;
            if (this.Title == loaded)
            {
                this.Title += "*";
            }
        }

        public void UpdateExistingDonation(object sender, RoutedEventArgs e)
        {
            if (my_user_id == 0)
            {
                MessageBox.Show("Please connect to Twitch using the 'Launch' button first to add a manual donation.");
            }
            else
            {
                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                {
                    List<string> bidwarOptions = new List<string>();
                    foreach (object child in TextBoxes.Children)
                    {
                        var textbox = (TextBox)child;
                        if (textbox.Text != "")
                            bidwarOptions.Add(textbox.Text);
                    }
                    db.Open();
                    string q = "SELECT * FROM allDonations7 WHERE User = @User ORDER BY Created_At DESC";
                    SqliteCommand insertSQL2 = new SqliteCommand(q, db);
                    insertSQL2.Parameters.AddWithValue("@User", my_user_id);
                    SqliteDataReader query2;
                    try
                    {
                        query2 = insertSQL2.ExecuteReader();
                    }
                    catch (SqliteException error)
                    {
                        throw new Exception(error.Message);
                        System.Diagnostics.Debug.WriteLine(error.Message);
                    }
                    List<Bid> donationHistory = new List<Bid>();
                    while (query2.Read())
                    {
                        string message = query2.GetString(7);
                        Boolean found = false; //only attribute this donation to the first found search term in the donation message
                        foreach (var msg in bidwarOptions)
                        {
                            if (message.ToLower().Trim().Contains(msg.ToLower().Trim()) && !found)
                            {
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            try
                            {
                                Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), query2.GetString(6), query2.GetString(7));
                                donationHistory.Add(thisBid);

                            }
                            catch (Exception eee)
                            {
                                Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), null, query2.GetString(7));
                                donationHistory.Add(thisBid);
                            }
                        }
                    }
                    db.Close();
                    Window3 window3 = new Window3(bidwarOptions, donationHistory, my_user_id);
                    window3.Show();
                }
            }
        }

        public void AddManualDonation(object sender, RoutedEventArgs e)
        {
            if (my_user_id == 0)
            {
                MessageBox.Show("Please connect to Twitch using the 'Launch' button first to add a manual donation.");
            }
            else
            {
                List<string> bidwarOptions = new List<string>();
                foreach (object child in TextBoxes.Children)
                {
                    var textbox = (TextBox)child;
                    if (textbox.Text != "")
                        bidwarOptions.Add(textbox.Text);
                }
                Window4 window4 = new Window4(bidwarOptions, my_user_id);
                window4.Show();
            }
        }




        public int numOptions = 1;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Unsaved Bid War Config";
            //make function for config handlers
            //load and display config, default to 1 textbox if no config selected
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                //initialize DB if not exists
                String tableCommand = "CREATE TABLE IF NOT EXISTS allDonations7 (Id STRING PRIMARY KEY, User INTEGER NOT NULL, Created_At INTEGER NOT NULL, Amount DOUBLE NOT NULL, Currency NVARCHAR(2048) NOT NULL, Donator NVARCHAR(2048), Donator_Email NVARCHAR(2048), Message NVARCHAR(2048))";
                String tableCommand2 = "CREATE TABLE IF NOT EXISTS lastFile2 (Id INTEGER PRIMARY KEY AUTOINCREMENT, Filename NVARCHAR(2048))";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);
                //retrieve last opened file and open it by default
                SqliteCommand selectSQL = new SqliteCommand("SELECT Filename FROM lastFile2 ORDER BY Id DESC LIMIT 1", db);
                SqliteDataReader query = null;
                try
                {
                    createTable.ExecuteReader();
                    createTable2.ExecuteReader();
                    query = selectSQL.ExecuteReader();
                }
                catch (SqliteException e)
                {
                    throw new Exception(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                string fname = null;
                if (query != null)
                {
                    while (query.Read())
                    {
                        try {
                            fname = query.GetString(0);
                        }
                        catch (Exception err)
                        {
                            fname = null;
                        }
                        
                    }
                }
                db.Close();
                if (fname == null)
                {
                    initializeBlankConfig();
                }
                else
                {
                    openFile(fname);
                }
            }
        }


        //what to do to configure options dialog if no file loaded or file closed
        public void initializeBlankConfig()
        {
            resetConfig();
            addTextbox();
        }
        public void resetConfig()
        {
            RemoveButtons.Children.Clear();
            TextBoxes.Children.Clear();
            DonationCutoff.SelectedDate = null;
            DonationCutoff2.SelectedDate = null;
            AccessToken.Password = null;
        }

        //functions for options dialog controls
        public void addTextbox(string def = null)
        {
            TextBox txt = new TextBox();
            txt.Margin = new Thickness(10);
            txt.TextChanged += new TextChangedEventHandler(unsaved);
            if (def != null)
            {
                txt.Text = def;
            }
            TextBoxes.Children.Add(txt);
            Button rm = new Button();
            rm.Margin = new Thickness(10);
            rm.Content = "Remove";
            rm.Click += DeleteRow;
            if (TextBoxes.Children.Count == 1)
            {
                rm.IsEnabled = false;
            }
            else
            {
                RemoveButtons.Children[0].IsEnabled = true;
            }
            RemoveButtons.Children.Add(rm);

        }
        public void AddRow(object sender, RoutedEventArgs e)
        {
            addTextbox();
            changed = true;
            if (this.Title == loaded)
            {
                this.Title += "*";
            }
        }
        public void DeleteRow(object sender, RoutedEventArgs e)
        {
            if (TextBoxes.Children.Count > 1)
            {
                var button = (Button)sender;
                int index = RemoveButtons.Children.IndexOf(button);
                RemoveButtons.Children.RemoveAt(index);
                TextBoxes.Children.RemoveAt(index);
            }
            if (TextBoxes.Children.Count == 1)
            {
                RemoveButtons.Children[0].IsEnabled = false;
            }
            changed = true;
            if (this.Title == loaded)
            {
                this.Title += "*";
            }
        }


        //launch window to direct user to get their token
        public void GetToken(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://streamlabs.com/api/v1.0/authorize?client_id=oT3hU90De8ZVbyWmVd1xQpghuwB4UXG843wuI5Nb&redirect_uri=http://pidgezero.one/appredirect.html&response_type=code&scope=donations.read");

            Window1 w = new Window1("streamlabs");
            w.Show();
        }
        public void GetTwitchToken(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.twitch.tv/kraken/oauth2/authorize?client_id=9tztppkgkayamw5135n4uo216fwvoy&redirect_uri=http://pidgezero.one/appredirect.html&scope=channel_read chat_login&response_type=code");

            Window1 w = new Window1("twitch");
            w.Show();
        }


        //saving cheer -- will only work when application is live since there is no REST API for cheer history
        //this doesnt seem to be firing at all? no log message
        public void saveCheer(CheerContainer cheer, int uid)
        {
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                {
                    db.Open();
                    var cheerObject = cheer.getCheer();
                    

                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, null, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = null AND Message = @Message)", db);
                    insertSQL.Parameters.AddWithValue("@Id1", cheer.getId());
                    insertSQL.Parameters.AddWithValue("@User1", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                    insertSQL.Parameters.AddWithValue("@Amount1", cheerObject.getAmount());
                    insertSQL.Parameters.AddWithValue("@Currency1", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator1", cheerObject.getUser());
                    insertSQL.Parameters.AddWithValue("@Message1", cheerObject.getMessage());
                    insertSQL.Parameters.AddWithValue("@Id", cheer.getId());
                    insertSQL.Parameters.AddWithValue("@User", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                    insertSQL.Parameters.AddWithValue("@Amount", cheerObject.getAmount());
                    insertSQL.Parameters.AddWithValue("@Currency", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator", cheerObject.getUser());
                    insertSQL.Parameters.AddWithValue("@Message", cheerObject.getMessage());
                    try
                    {
                        insertSQL.ExecuteNonQuery();
                        var tblock = new TextBlock();
                        tblock.Text = "Cheer: " + cheerObject.getAmount() + " from " + cheerObject.getUser() + " (" + cheerObject.getMessage() + ")";
                        tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock);
                        List<string> lines = new List<string>();
                        lines.Add(unixTimestamp + " [BITS] " + cheerObject.getAmount() + " from " + cheerObject.getUser() + " (" + cheerObject.getMessage() + ")");
                        File.AppendAllLines("output/logs.txt", lines);
                    }
                    catch (SqliteException ex)
                    {
                        var tblock = new TextBlock();
                        tblock.Text = "Error saving cheer " + cheerObject.getAmount() + " from " + cheerObject.getUser() + " (" + cheerObject.getMessage() + "): " + ex.Message;
                        tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock);
                        List<string> lines = new List<string>();
                        lines.Add(unixTimestamp + " [BITS] " + "Error saving cheer " + cheerObject.getAmount() + " from " + cheerObject.getUser() + " (" + cheerObject.getMessage() + "): " + ex.Message);
                        File.AppendAllLines("output/logs.txt", lines);
                    }
                    db.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

        //retrieve all new streamlabs donations, fires every 10 seconds
        //accepts parameters to specify a timeframe to reduce workload, based on the ID of the most recently saved donation
        public void fetchDonations(int uid, int earliest = -1, string latest = "")
        {
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                string url = "https://www.twitchalerts.com/api/v1.0/donations?limit=100&currency=USD&access_token=" + AccessToken.Password;
                HttpWebRequest request;
                if (earliest > 0)
                {
                    url += "&after=" + earliest.ToString();
                }
                if (latest != "")
                {
                    url += "&before=" + latest.ToString();
                }
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                try
                {
                    WebResponse resp = request.GetResponse();
                    Stream dataStream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    dataStream.Close();
                    string json = responseFromServer;
                    Donations donations = JsonConvert.DeserializeObject<Donations>(json);
                    List<Bid> allDonations7 = donations.getDonations();
                    foreach (var bid in allDonations7)
                    {
                        //convert to USD first if currency is not USD or bits
                        //  use http://fixer.io/ -- GET http://api.fixer.io/yyyy-mm-dd?symbols=USD,[CUR] where yyyy-mm-dd is date of donation
                        //  amount = amount * USD / [CUR]
                        /*if (bid.currency != "USD")
                        {
                            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(bid.getDate());
                            string url2 = "http://api.fixer.io/" + dt.ToString("yyyy-MM-dd") + "?symbols=USD," + bid.getCurrency();
                            HttpWebRequest request2;
                            request2 = (HttpWebRequest)WebRequest.Create(url2);
                            request2.Method = "GET";
                            WebResponse resp2 = request2.GetResponse();
                            Stream dataStream2 = resp2.GetResponseStream();
                            StreamReader reader2 = new StreamReader(dataStream2);
                            string responseFromServer2 = reader2.ReadToEnd();
                            dataStream2.Close();
                            string json2 = responseFromServer2;
                            MoneyConversion definition = JsonConvert.DeserializeObject<MoneyConversion>(json2);
                            Window window = new Window();
                            window.Content = bid.getAmount() * definition.getMoneyRates().getUSD() / definition.getMoneyRates().getRate();
                            window.Show();
                        }*/
                        SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                        insertSQL.Parameters.AddWithValue("@Id1", bid.getId());
                        insertSQL.Parameters.AddWithValue("@User1", uid);
                        insertSQL.Parameters.AddWithValue("@Created_At1", bid.getDate());
                        insertSQL.Parameters.AddWithValue("@Amount1", bid.getAmount());
                        insertSQL.Parameters.AddWithValue("@Currency1", bid.getCurrency());
                        insertSQL.Parameters.AddWithValue("@Donator1", bid.getDonator());
                        insertSQL.Parameters.AddWithValue("@Donator_Email1", bid.getEmail());
                        insertSQL.Parameters.AddWithValue("@Message1", bid.getMessage());
                        insertSQL.Parameters.AddWithValue("@Id", bid.getId());
                        insertSQL.Parameters.AddWithValue("@User", uid);
                        insertSQL.Parameters.AddWithValue("@Created_At", bid.getDate());
                        insertSQL.Parameters.AddWithValue("@Amount", bid.getAmount());
                        insertSQL.Parameters.AddWithValue("@Currency", bid.getCurrency());
                        insertSQL.Parameters.AddWithValue("@Donator", bid.getDonator());
                        insertSQL.Parameters.AddWithValue("@Donator_Email", bid.getEmail());
                        insertSQL.Parameters.AddWithValue("@Message", bid.getMessage());
                        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        try
                        {
                            int inserted = insertSQL.ExecuteNonQuery();
                            if (inserted == 1)
                            {
                                string matched = null;
                                foreach (object child in TextBoxes.Children)
                                {
                                    var textbox = (TextBox)child;
                                    if (bid.getMessage().ToLower().Trim().Contains(textbox.Text.ToLower().Trim()))
                                    {
                                        matched = textbox.Text;
                                    }
                                }
                                var tblock = new TextBlock();
                                tblock.Text = "Donation: $" + bid.getAmount() + " from " + bid.getDonator() + " (" + bid.getMessage() + ") -- " + (matched == null ? "did not match any fields" : "matched " + matched);
                                tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                                window2.Logs.Children.Add(tblock);
                                List<string> lines = new List<string>();
                                lines.Add(unixTimestamp + " [SLBS] New donation: " + bid.getAmount() + " from " + bid.getDonator() + " at " + bid.getDate() + " (" + bid.getMessage() + ") -- " + (matched == null ? "did not match any fields" : "matched " + matched));
                                File.AppendAllLines("output/logs.txt", lines);
                                File.WriteAllText("output/latest.txt", "Latest Esports Sponsorship: " + bid.getDonator() + " ($" + Math.Round(bid.getAmount(), 2) + ")");
                            }
                        }
                        catch (SqliteException ex)
                        {
                            var tblock = new TextBlock();
                            tblock.Text = "Error saving donation $" + bid.getAmount() + " from " + bid.getDonator() + " (" + bid.getMessage() + "): " + ex.Message;
                            tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                            window2.Logs.Children.Add(tblock);
                            List<string> lines = new List<string>();
                            lines.Add(unixTimestamp + " [SLBS] Error saving donation $" + bid.getAmount() + " from " + bid.getDonator() + " (" + bid.getMessage() + "): " + ex.Message);
                            File.AppendAllLines("output/logs.txt", lines);
                        }
                    }
                    if (allDonations7.Count == 100)
                    {
                        fetchDonations(uid, earliest, allDonations7[0].getId()); //is this most recent donation? check to be sure
                    }
                    else
                    {
                        db.Close();
                    }
                }
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                }
            }
        }


        //loop thru DB to refresh point counters
        public void UpdateDonations(int uid)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                List<String> entries = new List<string>();
                //use SQL to get most recent donation
                SqliteCommand insertSQL = new SqliteCommand("SELECT Id FROM allDonations7 WHERE User = @User AND Currency NOT LIKE @Bits AND Currency NOT LIKE @Sub ORDER BY Created_At DESC LIMIT 1", db);
                insertSQL.Parameters.AddWithValue("@User", uid);
                insertSQL.Parameters.AddWithValue("@Bits", "%BIT%");
                insertSQL.Parameters.AddWithValue("@Sub", "%SUB%");
                SqliteDataReader query;
                try
                {
                    query = insertSQL.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    throw new Exception(error.Message);
                    System.Diagnostics.Debug.WriteLine(error.Message);
                }
                int earliest = -1;
                string str = "";
                double since = 0;
                double until = 0;
                if (DonationCutoff.SelectedDate != null)
                {
                    var dto = DonationCutoff.SelectedDate.Value.Date - new DateTime(1970, 1, 1, 0, 0, 0);
                    since = dto.TotalSeconds;
                }
                while (query.Read())
                {
                    earliest = query.GetInt32(0);
                }
                //then check for new donations and save them
                fetchDonations(uid, earliest, "");
                //then fetch all donations in DB in chosen timeframe, loop through them to update points counters
                string q = "SELECT * FROM allDonations7 WHERE User = @User AND Created_At >= @since";
                //string q = "SELECT * FROM allDonations7 WHERE Created_At >= @since";
                if (DonationCutoff2.SelectedDate != null)
                {
                    var dto = DonationCutoff2.SelectedDate.Value.Date - new DateTime(1970, 1, 1, 0, 0, 0);
                    until = dto.TotalSeconds;
                    q += " AND Created_At <= @until";
                }
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                List<string> lines1 = new List<string>();
                lines1.Add(unixTimestamp + " [META] Refreshing totals from time " + ((Int64)since + 21600) + (until != 0 ? " to " + ((Int64)until + 21600) : ""));
                File.AppendAllLines("output/logs.txt", lines1);
                SqliteCommand insertSQL2 = new SqliteCommand(q, db);
                insertSQL2.Parameters.AddWithValue("@User", uid);
                insertSQL2.Parameters.AddWithValue("@since", ((Int64)since + 21600));
                if (until != 0)
                {
                    insertSQL2.Parameters.AddWithValue("@until", ((Int64)until + 21600));
                }
                List<Bid> donations = new List<Bid>();
                using (SqliteDataReader query2 = insertSQL2.ExecuteReader())
                {
                    var lines = 0;
                    while (query2.Read())
                    {
                        lines++;
                        try
                        {
                            Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), query2.GetString(6), query2.GetString(7));
                            donations.Add(thisBid);
                        }
                        catch (Exception e)
                        {
                            Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), null, query2.GetString(7));
                            donations.Add(thisBid);
                        }
                    }
                    db.Close();
                }
                List<Contestant> contestants = new List<Contestant>();
                foreach (object child in TextBoxes.Children)
                {
                    var textbox = (TextBox)child;
                    if (textbox.Text != "")
                        contestants.Add(new Contestant(textbox.Text, 0));
                }
                foreach (var bid in donations)
                {
                    Boolean found = false; //only attribute this donation to the first found search term in the donation message
                    foreach (var contestant in contestants)
                    {
                        if (bid.getMessage().ToLower().Trim().Contains(contestant.getText().ToLower().Trim()) && !found)
                        {
                            //convert to points
                            if (bid.getCurrency() == "BIT")
                            {
                                contestant.addAmount(Math.Floor(bid.getAmount()));
                            }
                            else if (bid.getCurrency() == "SUB")
                            {
                                contestant.addAmount(Math.Floor(bid.getAmount()));
                            }
                            else
                            {
                                contestant.addAmount(Math.Floor(100 * bid.getAmount()));
                            }
                            found = true;
                        }
                    }
                }
                foreach (var contestant in contestants)
                {
                    List<string> lines2 = new List<string>();
                    lines2.Add(unixTimestamp + " [META] " + contestant.getText() + ": " + contestant.getTotal());
                    File.AppendAllLines("output/logs.txt", lines2);
                    str = contestant.getText() + ": " + contestant.getTotal();
                    File.WriteAllText("output/" + contestant.getText() + ".txt", str);
                }
            }
        }

        //start listening for donations, open status window
        public void LaunchWindow(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("output"))
            {
                DirectoryInfo di = Directory.CreateDirectory("output");
            }
            string url = "https://www.twitchalerts.com/api/v1.0/user?access_token=" + AccessToken.Password;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            window2.Show();
            string url2 = "https://api.twitch.tv/kraken/channel";
            HttpWebRequest request2;
            request2 = (HttpWebRequest)WebRequest.Create(url2);
            request2.Method = "GET";
            request2.Headers["Client-ID"] = "9tztppkgkayamw5135n4uo216fwvoy";
            request2.Headers["Authorization"] = "OAuth " + TwitchAccessToken.Password;
            try
            {
                WebResponse resp = request.GetResponse();
                Stream dataStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                dataStream.Close();
                string json = responseFromServer;
                User user = JsonConvert.DeserializeObject<User>(json);
                my_username = user.GetTwitch().getName();
                my_user_id = user.GetTwitch().GetId();
                WebResponse resp2 = request2.GetResponse();
                Stream dataStream2 = resp2.GetResponseStream();
                StreamReader reader2 = new StreamReader(dataStream2);
                string responseFromServer2 = reader2.ReadToEnd();
                dataStream2.Close();
                string json2 = responseFromServer2;
                TwitchUser twitchuser = JsonConvert.DeserializeObject<TwitchUser>(json2);

                //set up timer for streamlabs donation checker
                void updateWindow(object source, ElapsedEventArgs ev)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        UpdateDonations(user.GetTwitch().GetId());
                    });

                }
                UpdateDonations(user.GetTwitch().GetId());
                System.Timers.Timer myTimer = new System.Timers.Timer();
                myTimer.Elapsed += new ElapsedEventHandler(updateWindow);
                myTimer.Interval = 10000;
                myTimer.Enabled = true;
                List<string> bidwarOptions = new List<string>();
                foreach (object child in TextBoxes.Children)
                {
                    var textbox = (TextBox)child;
                    if (textbox.Text != "")
                        bidwarOptions.Add(textbox.Text);
                }

                //set up IRC listener for subs, resubs, and corrections
                void doIRC()
                {
                    
                    IRCConfig conf = new IRCConfig();
                    conf.name = my_username;
                    conf.nick = my_username;
                    conf.port = 6667;
                    conf.channel = "#" + my_username;
                    conf.server = "irc.chat.twitch.tv";
                    conf.password = "oauth:" + TwitchAccessToken.Password;
                    conf.joined = false;


                    IRCBot bot = new IRCBot(conf);
                    using (bot)
                    {
                        System.Diagnostics.Debug.WriteLine("connecting");
                        bot.Connect();
                        bot.IRCWork(user.GetTwitch().GetId(), bidwarOptions);
                    }
                }
                Thread irc = new Thread(new ThreadStart(doIRC));
                irc.Start();

                //websocket listener for bits
                /*WebSocket websocket = new WebSocket("wss://pubsub-edge.twitch.tv");
                websocket.Opened += new EventHandler(websocket_Opened);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Open();
                openListener();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var listen = false;
                System.Timers.Timer pingTimer = new System.Timers.Timer();
                pingTimer.Elapsed += new ElapsedEventHandler(sendPing);
                pingTimer.Interval = 240000;
                pingTimer.Enabled = true;
                void websocket_Opened(object sender2, EventArgs e2)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        var tblock2 = new TextBlock();
                        tblock2.Text = "Established connection to bits service";
                        tblock2.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock2);
                    });
                }
                void websocket_Error(object sender2, SuperSocket.ClientEngine.ErrorEventArgs e2)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        var tblock3 = new TextBlock();
                        tblock3.Text = "Bits service error: " + e2.Exception.Message;
                        tblock3.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock3);
                    });
                }
                void websocket_Closed(object sender2, EventArgs e2)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        var tblock4 = new TextBlock();
                        tblock4.Text = "Closed bits service";
                        tblock4.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock4);
                    });
                }
                void websocket_MessageReceived(object sender2, MessageReceivedEventArgs e2)
                {
                    SocketMessage baseMessage = JsonConvert.DeserializeObject<SocketMessage>(e2.Message);
                    var output = "";
                    if (baseMessage.getType() == "PONG")
                    {
                        output = "pong";
                    }
                    else if (baseMessage.getType() == "RECONNECT")
                    {
                        websocket.Close();
                        websocket.Open();
                        output = "Reconnecting due to server restart";
                    }
                    if (baseMessage.getType() == "MESSAGE")
                    {
                        var container = JsonConvert.DeserializeObject<CheerMessageContainer>(e2.Message);
                        CheerMessageData data = container.getData();
                        try
                        {
                            if (data.getTopic() == "channel-bits-events-v1." + twitchuser.getId().ToString())
                            {
                                CheerContainer thisCheer = JsonConvert.DeserializeObject<CheerContainer>(data.getMessage());
                                saveCheer(thisCheer, user.GetTwitch().GetId());
                            }
                        }
                        catch(Exception eee)
                        {
                            output = e2.Message;
                        }
                    }
                    else
                    {
                        output = e2.Message;
                    }
                    Application.Current.Dispatcher.Invoke((Action)delegate {
                        var tblock5 = new TextBlock();
                        tblock5.Text = "Message from bits service: " + e2.Message;
                        tblock5.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock5);
                    });
                }
                void sendPing(object source, ElapsedEventArgs ev)
                {
                    var ping = new { type = "PING" };
                    websocket.Send(serializer.Serialize(ping));
                }
                async void openListener()
                {
                    await Task.Delay(TimeSpan.FromSeconds(7));
                    var ts = new string[1];
                    ts[0] = "channel-bits-events-v1." + twitchuser.getId().ToString();
                    var listener = new { type = "LISTEN", data = new { topics = ts, auth_token = TwitchAccessToken.Password } };
                    websocket.Send(serializer.Serialize(listener));
                }*/
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.Message);
                MessageBox.Show("Could not connect to StreamLabs. Try re-entering your access token. The server may also be down.");
            }
        }
    }
}
