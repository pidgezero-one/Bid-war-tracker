using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public class AuthCodeContainer
    {
        private string access_token;
        private string token_type;
        private string refresh_token;

        public AuthCodeContainer (string access_token, string token_type, string refresh_token) {
            this.access_token = access_token;
            this.token_type = token_type;
            this.refresh_token = refresh_token;
        }
        public string getAccessToken()
        {
            return this.access_token;
        }
    }
    public class TwitchAuthCodeContainer
    {
        private string access_token;
        private string[] scope;
        private string refresh_token;

        public TwitchAuthCodeContainer(string access_token, string refresh_token, string[] scope)
        {
            this.access_token = access_token;
            this.refresh_token = refresh_token;
            this.scope = scope;
        }
        public string getAccessToken()
        {
            return this.access_token;
        }
        public string[] getScope()
        {
            return this.scope;
        }
    }
    public partial class Window1 : Window
    {
        private string type;
        public Window1()
        {
            InitializeComponent();
            this.Title = type + " access token";
        }
        public Window1(string type)
        {
            this.type = type;
            InitializeComponent();
            this.Title = type + " access token";
        }
        public void GetMyAuthToken(object sender, RoutedEventArgs e)
        {
            GetMyAuthTokenAsync();
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        public async Task GetMyAuthTokenAsync()
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string> { };
                string url = "";
                if (type == "streamlabs")
                {
                    values = new Dictionary<string, string>
                    {
                       { "grant_type", "authorization_code" },
                       { "client_id", "YOUR_CLIENT_ID" },
                       { "client_secret", "YOUR_CLIENT_SECRET" },
                       { "redirect_uri", "http://pidgezero.one/appredirect.html" },
                       { "code", AuthCode.Text }
                    };
                    url = "https://www.twitchalerts.com/api/v1.0/token";

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    var resp = JsonConvert.DeserializeObject<AuthCodeContainer>(responseString);
                    AuthTokenOutput.Text = resp.getAccessToken();
                }
                else if (type == "twitch")
                {
                    values = new Dictionary<string, string>
                    {
                       { "client_id", "YOUR_CLIENT_ID" },
                       { "client_secret", "YOUR_CLIENT_SECRET" },
                       { "grant_type", "authorization_code" },
                       { "redirect_uri", "http://pidgezero.one/appredirect.html" },
                       { "code", AuthCode.Text }
                    };
                    url = "https://api.twitch.tv/kraken/oauth2/token";

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseString);
                    var resp = JsonConvert.DeserializeObject<TwitchAuthCodeContainer>(responseString);
                    AuthTokenOutput.Text = resp.getAccessToken();
                }
            }
        }
    }
}
