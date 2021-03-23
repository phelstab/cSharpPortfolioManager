using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LambdaBeta
{
    class LambdaBeta
    {
        private String stock1, stock2, stock3;

        private double share1, share2, share3;

        private int wallet;

        public LambdaBeta(String stock1, String stock2, String stock3, double share1, double share2, double share3, int wallet)
        {

            this.stock1 = stock1;
            this.stock2 = stock2;
            this.stock3 = stock3;

            this.share1 = share1;
            this.share2 = share2;
            this.share3 = share3;

            this.wallet = wallet;
        }

        public double calcPortfolio()
        {

            return share1 + share2 + share3;
        }

        public JObject getSData(String pStockName)
        {
            string jsonStockData;
            try
            {

                using (WebClient web = new WebClient())
                {
                    jsonStockData = web.DownloadString("https://query1.finance.yahoo.com/v7/finance/quote?symbols=" + pStockName);
                    // safe data in var rawData
                    return JObject.Parse(jsonStockData);

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error connecting to yahoo finance" + ex);
                return null;
            }

        }

        public double getStockPrice(String pStockName) {

            double __price__ = (double)getSData(pStockName)["quoteResponse"]["result"][0]["regularMarketPrice"];
            return __price__;
        }

        public String getStockName(String pStockName)
        {

            string __name__ = (string)getSData(pStockName)["quoteResponse"]["result"][0]["longName"];
            return __name__;
        }

        public String getStockMarket(String pStockName)
        {

            string __name__ = (string)getSData(pStockName)["quoteResponse"]["result"][0]["quoteSourceName"];
            return __name__;
        }

        public double calculateShare(double pshare, double pprice) {

            return Math.Floor((wallet * pshare) / pprice);
        }

        public void calculatePortfolio() {

            string sname1 = getStockName(stock1);
            string sname2 = getStockName(stock2);
            string sname3 = getStockName(stock3);

            double sprice1 = getStockPrice(stock1);
            double sprice2 = getStockPrice(stock2);
            double sprice3 = getStockPrice(stock3);

            string smarket1 = getStockMarket(stock1);
            string smarket2 = getStockMarket(stock2);
            string smarket3 = getStockMarket(stock3);

            double sshare1 = calculateShare(share1, sprice1);
            double sshare2 = calculateShare(share2, sprice2);
            double sshare3 = calculateShare(share3, sprice3);

            double spendings = sshare1 * sprice1 + sshare2 * sprice2 + share3 * sprice3;

            Console.WriteLine("Stock Name 1: " + sname1 + " ### Current Price: " + sprice1 + " ### Market: " + smarket1);
            Console.WriteLine("Stock Name 2: " + sname2 + " ### Current Price: " + sprice2 + " ### Market: " + smarket2);
            Console.WriteLine("Stock Name 3: " + sname3 + " ### Current Price: " + sprice3 + " ### Market: " + smarket3);

            Console.WriteLine("########################");

            Console.WriteLine("Buy " + sname1 + " this amount: " + sshare1);
            Console.WriteLine("Buy " + sname2 + " this amount: " + sshare2);
            Console.WriteLine("Buy " + sname3 + " this amount: " + sshare3);

            Console.WriteLine("########################");

            Console.WriteLine("Outgoings: " + spendings);

            Console.WriteLine("########################");

            Console.WriteLine("Bank rest: " + (wallet - spendings));

        }


        static void Main(string[] args)
        {
            LambdaBeta portfolio1 = new LambdaBeta("XWD.To", "EEM", "IBCQ.F", 0.4, 0.4, 0.2, 2000);

            Console.WriteLine("____________________________");

            portfolio1.calculatePortfolio();

            Console.WriteLine("____________________________");

        }
    }
}
