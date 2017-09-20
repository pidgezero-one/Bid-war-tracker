using System;
using System.Collections.Generic;

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

    public class Bid
    {
        public string donation_id;
        public int created_at;
        public double amount;
        public string currency;
        public string name;
        public string email;
        public string message;


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
        public string getName()
        {
            return this.name;
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
        public string getName()
        {
            return this.name;
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
        public CheerContainer(Cheer data, string version, string message_type, string message_id)
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
}