using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public class ComboBoxPairs
        {
            public string _Key { get; set; }
            public string _Value { get; set; }
            public string _OriginalMessage { get; set; }

            public ComboBoxPairs(string _key, string _value, string _orig)
            {
                _Key = _key;
                _Value = _value;
                _OriginalMessage = _orig;
            }
        }

        int uid = 0;
        public Window3(List<string> options, List<Bid> donationHistory, int uid)
        {
            this.uid = uid;
            InitializeComponent();
            ManualDonationOption.ItemsSource = options;

            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();

            foreach(Bid donation in donationHistory)
            {
                string msg = "";
                if (donation.getCurrency() != "SUB")
                {
                    if (donation.getCurrency() == "BIT")
                    {
                        msg += donation.getAmount().ToString() + " bits";
                    }
                    else
                    {
                        var amt = Math.Round(donation.getAmount(), 2);
                        msg += "$" + String.Format("{0:0.00}", amt);
                    }
                }
                else
                {
                    msg += "Sub";
                }
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(donation.getDate()).ToLocalTime();
                msg += " from " + donation.getDonator() + " at " + dtDateTime.ToString("yyyy-MM-dd HH:mm:ss") + ": " + donation.getMessage();
                if (msg.Length > 150)
                {
                    msg = msg.Substring(0, 150) + "...";
                }
                cbp.Add(new ComboBoxPairs(msg, donation.getId(), donation.getMessage()));
            }

            ChosenDonation.DisplayMemberPath = "_Key";
            ChosenDonation.SelectedValuePath = "_Value";
            ChosenDonation.ItemsSource = cbp;


            this.Title = "Update unmatched donation";
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9-]+");
            return !regex.IsMatch(text);
        }

        public void SaveDonation(object sender, RoutedEventArgs e)
        {
            //Only accept if fields aren't blank and points amount is an integer
            ComboBoxPairs cbp = (ComboBoxPairs)ChosenDonation.SelectedItem;
            if (ManualDonationOption.Text.Trim() != "" && cbp._Value != ""/*  && ManualDonationAmount.Text.Trim() != ""&& IsTextAllowed(ManualDonationAmount.Text)*/)
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                {
                    db.Open();
                    SqliteCommand ud = new SqliteCommand("UPDATE allDonations7 SET Message = @NewMessage WHERE Id = @Id", db);
                    ud.Parameters.AddWithValue("@NewMessage", cbp._OriginalMessage + " " + ManualDonationOption.Text);
                    ud.Parameters.AddWithValue("@Id", cbp._Value);
                    try
                    {
                        ud.ExecuteNonQuery();
                        List<string> lines = new List<string>();
                        lines.Add(unixTimestamp + " [META] Manually updated donation " + cbp._Value + ": " + cbp._OriginalMessage + " " + ManualDonationOption.Text);
                        File.AppendAllLines("output/logs.txt", lines);
                        db.Close();
                        this.Close();
                    }
                    catch (SqliteException exe)
                    {
                        MessageBox.Show("Something went wrong with updating the donation. Please send a screenshot of this to pidge: " + exe.Message);
                        db.Close();
                        this.Close();
                    }
                    /*Console.WriteLine("manual_" + unixTimestamp.ToString() + " " + uid.ToString() + " " + ManualDonationAmount.Text + " " + ManualDonationOption.Text);
                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                    insertSQL.Parameters.AddWithValue("@Id1", "manual_" + unixTimestamp.ToString());
                    insertSQL.Parameters.AddWithValue("@User1", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At1", unixTimestamp);
                    insertSQL.Parameters.AddWithValue("@Amount1", Int32.Parse(ManualDonationAmount.Text));
                    insertSQL.Parameters.AddWithValue("@Currency1", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator1", "");
                    insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                    insertSQL.Parameters.AddWithValue("@Message1", ManualDonationOption.Text);
                    insertSQL.Parameters.AddWithValue("@Id", "manual_" + unixTimestamp.ToString());
                    insertSQL.Parameters.AddWithValue("@User", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At", unixTimestamp);
                    insertSQL.Parameters.AddWithValue("@Amount", Int32.Parse(ManualDonationAmount.Text));
                    insertSQL.Parameters.AddWithValue("@Currency", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator", "");
                    insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                    insertSQL.Parameters.AddWithValue("@Message", ManualDonationOption.Text);
                    try
                    {
                        insertSQL.ExecuteNonQuery();
                        db.Close();
                        this.Close();
                    }
                    catch (SqliteException exe)
                    {
                        db.Close();
                        this.Close();
                    }*/
                }
            }
        }
    }
}
