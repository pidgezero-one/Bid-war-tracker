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
    public partial class Window4 : Window
    {
        int uid = 0;
        public Window4(List<string> options, int uid)
        {
            this.uid = uid;
            InitializeComponent();
            ManualDonationOption2.ItemsSource = options;
            


            this.Title = "Enter manual donation";
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9-]+");
            return !regex.IsMatch(text);
        }

        public void SaveDonation(object sender, RoutedEventArgs e)
        {
            //Only accept if fields aren't blank and points amount is an integer
            if (ManualDonationOption2.Text != null && ManualDonationOption2.Text.Trim() != "" && ManualDonationAmount.Text != null && ManualDonationAmount.Text.Trim() != "" && IsTextAllowed(ManualDonationAmount.Text))
            {
                //fix timestamps... not working properly
                //crashing occasionally
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                Int32 time;
                if (DonationCutoff.SelectedDate != null)
                {
                    var dto = (DonationCutoff.SelectedDate.Value.Date.Subtract(new DateTime(1970, 1, 1)));
                    time = (Int32)dto.TotalSeconds;
                }
                else
                {
                    time = unixTimestamp;
                }
                using (SqliteConnection db = new SqliteConnection("Filename=donations.db"))
                {
                    db.Open();
                    Console.WriteLine("manual_" + time.ToString() + " " + uid.ToString() + " " + ManualDonationAmount.Text + " " + ManualDonationOption2.Text);
                    SqliteCommand insertSQL = new SqliteCommand("INSERT INTO allDonations7 (Id, User, Created_At, Amount, Currency, Donator, Donator_Email, Message) SELECT @Id1, @User1, @Created_At1, @Amount1, @Currency1, @Donator1, @Donator_Email1, @Message1 WHERE NOT EXISTS(SELECT 1 FROM allDonations7 WHERE Id = @Id AND User = @User AND Created_At = @Created_At AND Amount = @Amount AND Currency = @Currency AND Donator = @Donator AND Donator_Email = @Donator_Email AND Message = @Message)", db);
                    insertSQL.Parameters.AddWithValue("@Id1", "manual_" + time.ToString());
                    insertSQL.Parameters.AddWithValue("@User1", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At1", time);
                    insertSQL.Parameters.AddWithValue("@Amount1", Int32.Parse(ManualDonationAmount.Text));
                    insertSQL.Parameters.AddWithValue("@Currency1", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator1", "");
                    insertSQL.Parameters.AddWithValue("@Donator_Email1", "");
                    insertSQL.Parameters.AddWithValue("@Message1", ManualDonationOption2.Text);
                    insertSQL.Parameters.AddWithValue("@Id", "manual_" + time.ToString());
                    insertSQL.Parameters.AddWithValue("@User", uid);
                    insertSQL.Parameters.AddWithValue("@Created_At", time);
                    insertSQL.Parameters.AddWithValue("@Amount", Int32.Parse(ManualDonationAmount.Text));
                    insertSQL.Parameters.AddWithValue("@Currency", "BIT");
                    insertSQL.Parameters.AddWithValue("@Donator", "");
                    insertSQL.Parameters.AddWithValue("@Donator_Email", "");
                    insertSQL.Parameters.AddWithValue("@Message", ManualDonationOption2.Text);
                    try
                    {
                        insertSQL.ExecuteNonQuery();
                        List<string> lines = new List<string>();
                        lines.Add(unixTimestamp + " [META] Manual donation added at " + time + ": " + ManualDonationOption2.Text + " (" + ManualDonationAmount.Text + ")");
                        File.AppendAllLines("output/logs.txt", lines);
                        db.Close();
                        this.Close();
                    }
                    catch (SqliteException exe)
                    {
                        MessageBox.Show("Something went wrong with adding the manual donation. Please send a screenshot of this to pidge: " + exe.Message);
                        db.Close();
                        this.Close();
                    }
                }
            }
        }
    }
}
