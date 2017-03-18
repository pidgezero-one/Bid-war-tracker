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

    public class Contestant
    {
        private string text;
        private double total;

        public Contestant(string text, double total)
        {
            this.text = text;
            this.total = total;
        }

        public void setText(string val)
        {
            this.text = val;
        }
        public void setAmount(double val)
        {
            this.total = val;
        }
        public void addAmount(double val)
        {
            this.total += val;
        }
        public string getText()
        {
            return this.text;
        }
        public double getTotal()
        {
            return this.total;
        }
    }

    public class MoneyConversion
    {
        private string date;
        private MoneyRates rates;

        public MoneyConversion(string date, MoneyRates rates)
        {
            this.date = date;
            this.rates = rates;
        }

        public MoneyRates getMoneyRates()
        {
            return this.rates;
        }
        public string getDate()
        {
            return this.date;
        }
    }
    public class MoneyRates
    {
        public double AUD;
        public double BRL;
        public double CAD;
        public double CHF;
        public double CZK;
        public double DKK;
        public double EUR;
        public double GBP;
        public double HKD;
        public double HUF;
        public double ILS;
        public double JPY;
        public double MXN;
        public double MYR;
        public double NOK;
        public double NZD;
        public double PHP;
        public double PLN;
        public double RUB;
        public double SEK;
        public double SGD;
        public double THB;
        public double TRY;
        public double TWD;
        public double USD;

        public MoneyRates(double AUD = double.NaN, double BRL = double.NaN, double CAD = double.NaN, double CHF = double.NaN, double CZK = double.NaN, double DKK = double.NaN, double EUR = double.NaN, double GBP = double.NaN, double HKD = double.NaN, double HUF = double.NaN, double ILS = double.NaN, double JPY = double.NaN, double MXN = double.NaN, double MYR = double.NaN, double NOK = double.NaN, double NZD = double.NaN, double PHP = double.NaN, double PLN = double.NaN, double RUB = double.NaN, double SEK = double.NaN, double SGD = double.NaN, double THB = double.NaN, double TRY = double.NaN, double TWD = double.NaN, double USD = double.NaN)
        {
            this.AUD = AUD;
            this.BRL = BRL;
            this.CAD = CAD;
            this.CHF = CHF;
            this.CZK = CZK;
            this.DKK = DKK;
            this.EUR = EUR;
            this.GBP = GBP;
            this.HKD = HKD;
            this.HUF = HUF;
            this.ILS = ILS;
            this.JPY = JPY;
            this.MXN = MXN;
            this.MYR = MYR;
            this.NOK = NOK;
            this.NZD = NZD;
            this.PHP = PHP;
            this.PLN = PLN;
            this.RUB = RUB;
            this.SEK = SEK;
            this.SGD = SGD;
            this.THB = THB;
            this.TRY = TRY;
            this.TWD = TWD;
            this.USD = USD;
        }
        public string getSelectedCurrency()
        {
            if (this.AUD != double.NaN && this.AUD != 0) return "AUD";
            else if (this.BRL != double.NaN && this.BRL != 0) return "BRL";
            else if (this.CAD != double.NaN && this.CAD != 0) return "CAD";
            else if (this.CHF != double.NaN && this.CHF != 0) return "CHF";
            else if (this.CZK != double.NaN && this.CZK != 0) return "CZK";
            else if (this.DKK != double.NaN && this.DKK != 0) return "DKK";
            else if (this.EUR != double.NaN && this.EUR != 0) return "EUR";
            else if (this.GBP != double.NaN && this.GBP != 0) return "GBP";
            else if (this.HKD != double.NaN && this.HKD != 0) return "HKD";
            else if (this.HUF != double.NaN && this.HUF != 0) return "HUF";
            else if (this.ILS != double.NaN && this.ILS != 0) return "ILS";
            else if (this.JPY != double.NaN && this.JPY != 0) return "JPY";
            else if (this.MXN != double.NaN && this.MXN != 0) return "MXN";
            else if (this.MYR != double.NaN && this.MYR != 0) return "MYR";
            else if (this.NOK != double.NaN && this.NOK != 0) return "NOK";
            else if (this.NZD != double.NaN && this.NZD != 0) return "NZD";
            else if (this.PHP != double.NaN && this.PHP != 0) return "PHP";
            else if (this.PLN != double.NaN && this.PLN != 0) return "PLN";
            else if (this.RUB != double.NaN && this.RUB != 0) return "RUB";
            else if (this.SEK != double.NaN && this.SEK != 0) return "SEK";
            else if (this.SGD != double.NaN && this.SGD != 0) return "SGD";
            else if (this.THB != double.NaN && this.THB != 0) return "THB";
            else if (this.TRY != double.NaN && this.TRY != 0) return "TRY";
            else if (this.TWD != double.NaN && this.TWD != 0) return "TWD";
            else return null;
        }
        public double getRate()
        {
            if (this.AUD != double.NaN && this.AUD != 0) return this.AUD;
            else if (this.BRL != double.NaN && this.BRL != 0) return this.BRL;
            else if (this.CAD != double.NaN && this.CAD != 0) return this.CAD;
            else if (this.CHF != double.NaN && this.CHF != 0) return this.CHF;
            else if (this.CZK != double.NaN && this.CZK != 0) return this.CZK;
            else if (this.DKK != double.NaN && this.DKK != 0) return this.DKK;
            else if (this.EUR != double.NaN && this.EUR != 0) return this.EUR;
            else if (this.GBP != double.NaN && this.GBP != 0) return this.GBP;
            else if (this.HKD != double.NaN && this.HKD != 0) return this.HKD;
            else if (this.HUF != double.NaN && this.HUF != 0) return this.HUF;
            else if (this.ILS != double.NaN && this.ILS != 0) return this.ILS;
            else if (this.JPY != double.NaN && this.JPY != 0) return this.JPY;
            else if (this.MXN != double.NaN && this.MXN != 0) return this.MXN;
            else if (this.MYR != double.NaN && this.MYR != 0) return this.MYR;
            else if (this.NOK != double.NaN && this.NOK != 0) return this.NOK;
            else if (this.NZD != double.NaN && this.NZD != 0) return this.NZD;
            else if (this.PHP != double.NaN && this.PHP != 0) return this.PHP;
            else if (this.PLN != double.NaN && this.PLN != 0) return this.PLN;
            else if (this.RUB != double.NaN && this.RUB != 0) return this.RUB;
            else if (this.SEK != double.NaN && this.SEK != 0) return this.SEK;
            else if (this.SGD != double.NaN && this.SGD != 0) return this.SGD;
            else if (this.THB != double.NaN && this.THB != 0) return this.THB;
            else if (this.TRY != double.NaN && this.TRY != 0) return this.TRY;
            else if (this.TWD != double.NaN && this.TWD != 0) return this.TWD;
            else if (this.USD != double.NaN && this.USD != 0) return this.USD;
            else return double.NaN;
        }
        public double getUSD()
        {
            return this.USD;
        }
    }

    public class Bid {
        private string donation_id;
        private int created_at;
        private double amount;
        private string currency;
        private string name;
        private string email;
        private string message;


        public Bid(string donation_id, int created_at, double amount, string currency, string name, string email, string message)
        {
            this.donation_id = donation_id;
            this.created_at = created_at;
            this.amount = amount;
            this.currency = currency;
            this.name = name;
            this.email = email;
            this.message = message;
        }
        public string getId()
        {
            return this.donation_id;
        }
        public int getDate()
        {
            return this.created_at;
        }
        public double getAmount()
        {
            return this.amount;
        }
        public string getCurrency()
        {
            return this.currency;
        }
        public string getDonator()
        {
            return this.name;
        }
        public string getEmail()
        {
            return this.email;
        }
        public string getMessage()
        {
            return this.message;
        }
    }

    public class Donations
    {
        private List<Bid> data;

        public Donations(List<Bid> data)
        {
            this.data = data;
        }

        public List<Bid> getDonations()
        {
            return this.data;
        }
    }

    public class Notifications
    {
        private Boolean email;
        private Boolean push;
        public Notifications(Boolean email, Boolean push)
        {
            this.email = email;
            this.push = push;
        }
    }

    public class TwitchUser
    {
        private int _id;
        private string bio;
        private string created_at;
        private string display_name;
        private string email;
        private Boolean email_verified;
        private string logo;
        private string name;
        private Notifications notifications;
        private Boolean partnered;
        private Boolean twitter_connected;
        private string type;
        private string updated_at;

        public TwitchUser(int _id, string bio, string created_at, string email, Boolean email_verified, string logo, string name, Notifications notifications, Boolean partnered, Boolean twitter_connected, string type, string updated_at)
        {
            this._id = _id;
            this.bio = bio;
            this.created_at = created_at;
            this.email = email;
            this.email_verified = email_verified;
            this.logo = logo;
            this.name = name;
            this.notifications = notifications;
            this.partnered = partnered;
            this.twitter_connected = twitter_connected;
            this.type = type;
            this.updated_at = updated_at;
        }

        public int getId()
        {
            return this._id;
        }

    }

    public class User
    {
        private Twitch twitch;

        public User(Twitch twitch)
        {
            this.twitch = twitch;
        }
        public Twitch GetTwitch()
        {
            return this.twitch;
        }
    }

    public class Twitch
    {
        private int id;
        private string display_name;
        private string name;

        public Twitch(int id, string display_name, string name)
        {
            this.id = id;
            this.display_name = display_name;
            this.name = name;
        }
        public int GetId()
        {
            return this.id;
        }
    }

    public class SocketMessage
    {
        private string type;
        public SocketMessage(string type)
        {
            this.type = type;
        }
        public string getType()
        {
            return this.type;
        }
    }

    public class CheerMessageContainer
    {
        private string type;
        private CheerMessageData data;
        public CheerMessageContainer(string type, CheerMessageData data)
        {
            this.type = type;
            this.data = data;
        }
        public string getType()
        {
            return this.type;
        }
        public CheerMessageData getData()
        {
            return this.data;
        }
    }

    public class CheerMessageData
    {
        private string topic;
        private string message;
        public CheerMessageData(string topic, string message)
        {
            this.topic = topic;
            this.message = message;
        }
        public string getTopic()
        {
            return this.topic;
        }
        public string getMessage()
        {
            return this.message;
        }
    }

    public class CheerContainer
    {
        private Cheer data;
        private string version;
        private string message_type;
        private string message_id;
        public CheerContainer (Cheer data, string version, string message_type, string message_id)
        {
            this.data = data;
            this.version = version;
            this.message_type = message_type;
            this.message_id = message_id;
        }
        public Cheer getCheer()
        {
            return this.data;
        }
        public string getId()
        {
            return this.message_id;
        }
    }


    public class Cheer
    {
        private string user_name;
        private string channel_name;
        private int user_id;
        private int channel_id;
        private string time;
        private string chat_message;
        private int bits_used;
        private int total_bits_used;
        private string context;
        private BadgeEntitlement badge_entitlement;

        public Cheer(string user_name, string channel_name, int user_id, int channel_id, string time, string chat_message, int bits_used, int total_bits_used, string context, BadgeEntitlement badge_entitlement, string message_type, string message_id)
        {
            this.user_name = user_name;
            this.channel_name = channel_name;
            this.user_id = user_id;
            this.channel_id = channel_id;
            this.time = time;
            this.chat_message = chat_message;
            this.bits_used = bits_used;
            this.total_bits_used = total_bits_used;
            this.context = context;
            this.badge_entitlement = badge_entitlement;
        }
        public string getMessage()
        {
            return this.chat_message;
        }
        public string getUser()
        {
            return this.user_name;
        }
        public string getTime()
        {
            return this.time;
        }

        public int getAmount()
        {
            return this.bits_used;
        }
    }

    public class BadgeEntitlement
    {
        int new_version;
        int previous_version;

        public BadgeEntitlement(int new_version, int previous_version)
        {
            this.new_version = new_version;
            this.previous_version = previous_version;
        }
    }
    
    public partial class Form2 : Window
    {
        public Form2()
        {
        }
    }
    
    public partial class MainWindow : Window
    {
        string loaded = "";
        bool changed = false;
        Window2 window2 = new Window2();
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
                String tableCommand = "CREATE TABLE IF NOT EXISTS allDonations7 (Id STRING PRIMARY KEY, User INTEGER NOT NULL, Created_At INTEGER NOT NULL, Amount DOUBLE NOT NULL, Currency NVARCHAR(2048) NOT NULL, Donator NVARCHAR(2048), Donator_Email NVARCHAR(2048), Message NVARCHAR(2048))";
                String tableCommand2 = "CREATE TABLE IF NOT EXISTS lastFile2 (Id INTEGER PRIMARY KEY AUTOINCREMENT, Filename NVARCHAR(2048))";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                SqliteCommand createTable2 = new SqliteCommand(tableCommand2, db);
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
        public void GetToken(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://streamlabs.com/api/v1.0/authorize?client_id=YOUR_CLIENT_ID&redirect_uri=http://pidgezero.one/appredirect.html&response_type=code&scope=donations.read");

            Window1 w = new Window1("streamlabs");
            w.Show();
        }
        public void GetTwitchToken(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.twitch.tv/kraken/oauth2/authorize?client_id=YOUR_CLIENT_ID&redirect_uri=http://pidgezero.one/appredirect.html&scope=channel_read&response_type=code");

            Window1 w = new Window1("twitch");
            w.Show();
        }

        public void saveCheer(CheerContainer cheer, int uid)
        {
            try
            {
                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                {
                    db.Open();
                    var cheerObject = cheer.getCheer();

                    DateTime oDate = Convert.ToDateTime(cheerObject.getTime());
                    long epochTicks = new DateTime(1970, 1, 1).Ticks;
                    long unixTime = ((oDate.Ticks - epochTicks) / TimeSpan.TicksPerSecond);

                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, null, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = null AND Message = @Message)", db);
                    insertSQL.Parameters.AddWithValue("@Id1", cheer.getId());
                    insertSQL.Parameters.AddWithValue("@User1", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTime);
                    insertSQL.Parameters.AddWithValue("@Amount1", cheerObject.getAmount());
                    insertSQL.Parameters.AddWithValue("@Currency1", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator1", cheerObject.getUser());
                    insertSQL.Parameters.AddWithValue("@Message1", cheerObject.getMessage());
                    insertSQL.Parameters.AddWithValue("@Id", cheer.getId());
                    insertSQL.Parameters.AddWithValue("@User", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At", unixTime);
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
                    }
                    catch (SqliteException ex)
                    {
                        var tblock = new TextBlock();
                        tblock.Text = "Error saving donation " + cheerObject.getAmount() + " from " + cheerObject.getUser() + " (" + cheerObject.getMessage() + "): " + ex.Message;
                        tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                        window2.Logs.Children.Add(tblock);
                        throw new Exception(ex.Message);
                    }
                    db.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

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
                        //then convert to points, 1 US cent = 1 point or 1 bit = 1 point
                        //make SQL queries to insert if not previously existing
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
                        try
                        {
                            insertSQL.ExecuteNonQuery();
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
                        }
                        catch (SqliteException ex)
                        {
                            var tblock = new TextBlock();
                            tblock.Text = "Error saving donation $" + bid.getAmount() + " from " + bid.getDonator() + " (" + bid.getMessage() + "): " + ex.Message;
                            tblock.TextWrapping = System.Windows.TextWrapping.Wrap;
                            window2.Logs.Children.Add(tblock);
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

        public void UpdateDonations(int uid)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
            {
                db.Open();
                List<String> entries = new List<string>();
                //use SQL to get most recent donation
                SqliteCommand insertSQL = new SqliteCommand("SELECT Id FROM allDonations7 WHERE User = @User AND Currency != @Bits ORDER BY Created_At DESC LIMIT 1", db);
                insertSQL.Parameters.AddWithValue("@User", uid);
                insertSQL.Parameters.AddWithValue("@Bits", "BIT");
                SqliteDataReader query;
                try
                {
                    query = insertSQL.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    throw new Exception(error.Message);
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
                string q = "SELECT * FROM allDonations7 WHERE User = @User AND Created_At >= @since";
                if (DonationCutoff2.SelectedDate != null)
                {
                    var dto = DonationCutoff2.SelectedDate.Value.Date - new DateTime(1970, 1, 1, 0, 0, 0);
                    until = dto.TotalSeconds;
                    q += " AND Created_At <= @until";
                }
                fetchDonations(uid, earliest, ""); //replace 0 with whatever you get from DB
                SqliteCommand insertSQL2 = new SqliteCommand(q, db);
                insertSQL2.Parameters.AddWithValue("@User", uid);
                insertSQL2.Parameters.AddWithValue("@since", (Int64)since + 21600);
                if (until != 0)
                {
                    insertSQL2.Parameters.AddWithValue("@until", (Int64)until + 21600);
                }
                SqliteDataReader query2;
                try
                {
                    query2 = insertSQL2.ExecuteReader();
                }
                catch (SqliteException error)
                {
                    throw new Exception(error.Message);
                }
                List<Bid> donations = new List<Bid>();
                var lines = 0;
                while (query2.Read())
                {
                    lines++;
                    try { 
                        Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), query2.GetString(6), query2.GetString(7));
                        donations.Add(thisBid);
                    }
                    catch(Exception e)
                    {
                        Bid thisBid = new Bid(query2.GetString(0), query2.GetInt32(2), query2.GetDouble(3), query2.GetString(4), query2.GetString(5), null, query2.GetString(7));
                        donations.Add(thisBid);
                    }
                }
                db.Close();
                //then get all donations from DB
                List<Contestant> contestants = new List<Contestant>();
                foreach (object child in TextBoxes.Children)
                {
                    var textbox = (TextBox)child;
                    if (textbox.Text != "")
                        contestants.Add(new Contestant(textbox.Text, 0));
                }
                foreach (var bid in donations)
                {
                    foreach (var contestant in contestants)
                    {
                        if (bid.getMessage().ToLower().Trim().Contains(contestant.getText().ToLower().Trim()))
                        {
                            //convert to points
                            if (bid.getCurrency() == "BIT")
                            {
                                contestant.addAmount(Math.Floor(bid.getAmount()));
                            }
                            else
                            {
                                contestant.addAmount(Math.Floor(100 * bid.getAmount()));
                            }
                        }
                    }
                }
                foreach (var contestant in contestants)
                {
                    str = contestant.getText() + ": " + contestant.getTotal();
                    File.WriteAllText("output/" + contestant.getText() + ".txt", str);
                }
            }
        }

        public void LaunchWindow(object sender, RoutedEventArgs e)
        {
            DateTime oDate = Convert.ToDateTime("2017-02-09T13:23:58.168Z");
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
            request2.Headers["Client-ID"] = "YOUR_CLIENT_ID";
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
                WebResponse resp2 = request2.GetResponse();
                Stream dataStream2 = resp2.GetResponseStream();
                StreamReader reader2 = new StreamReader(dataStream2);
                string responseFromServer2 = reader2.ReadToEnd();
                dataStream2.Close();
                string json2 = responseFromServer2;
                TwitchUser twitchuser = JsonConvert.DeserializeObject<TwitchUser>(json2);
                void updateWindow(object source, ElapsedEventArgs ev)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        UpdateDonations(user.GetTwitch().GetId());
                    });

                }
                //Make logger
                //append textblocks to scrollable stackpanel
                UpdateDonations(user.GetTwitch().GetId());
                Timer myTimer = new System.Timers.Timer();
                myTimer.Elapsed += new ElapsedEventHandler(updateWindow);
                myTimer.Interval = 10000;
                myTimer.Enabled = true;
                WebSocket websocket = new WebSocket("wss://pubsub-edge.twitch.tv");
                websocket.Opened += new EventHandler(websocket_Opened);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Open();
                openListener();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var listen = false;
                Timer pingTimer = new System.Timers.Timer();
                pingTimer.Elapsed += new ElapsedEventHandler(sendPing);
                pingTimer.Interval = 240000;
                //pingTimer.Interval = 10000;
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
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Could not connect to StreamLabs. Try re-entering your access token. The server may also be down.");
            }
        }
    }
}
