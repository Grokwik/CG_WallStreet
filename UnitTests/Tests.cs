using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Project
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void BasicRegistration()
        {
            Solution.RegisterOrder("AAA BUY 1 10.00");
            Solution.RegisterOrder("AAA SELL 1 10.00");

            Check.That(Solution.BuyBook.Count).Equals(1);
            Check.That(Solution.SellBook.Count).Equals(1);

            Order O1 = Solution.BuyBook[0];
            Check.That(O1.Price).Equals((float)10);
            Check.That(O1.Qty).Equals(1);
            Check.That(O1.Symbol).Equals("AAA");

            Order O2 = Solution.SellBook[0];
            Check.That(O1.Price).Equals((float)10);
            Check.That(O1.Qty).Equals(1);
            Check.That(O1.Symbol).Equals("AAA");
        }

        [TestMethod]
        public void OrderDisplay()
        {
            Solution.RegisterOrder("AAA BUY 1 10.00");
            Order O1 = Solution.BuyBook[0];

            Check.That(O1.Display()).Equals("AAA 1 10.00");
        }

        [TestMethod]
        public void SimpleTrade()
        {
            Solution.RegisterOrder("AAA BUY 1 10.00");
            Solution.RegisterOrder("AAA SELL 1 10.00");
        }

        [TestMethod]
        public void TwoSymbols()
        {
            Solution.RegisterOrder("AAA BUY 1 10.00");
            Solution.RegisterOrder("BBB SELL 1 10.00");
            Solution.RegisterOrder("BBB BUY 1 10.00");
        }

        [TestMethod]
        public void NoTrade()
        {
            Solution.RegisterOrder("ABC BUY 1 9.00");
            Solution.RegisterOrder("ABC SELL 1 10.0");
        }

        [TestMethod]
        public void MultipleTrades()
        {
            Solution.RegisterOrder("ABC BUY 10 23.50");
            Solution.RegisterOrder("BBC BUY 10 13.50");
            Solution.RegisterOrder("ABC SELL 8 23.50");
            Solution.RegisterOrder("ABC SELL 5 23.50");
            Solution.RegisterOrder("BBC BUY 8 13.50");
            Solution.RegisterOrder("BBC BUY 12 13.50");
            Solution.RegisterOrder("ABC BUY 10 23.50");
            Solution.RegisterOrder("ABC SELL 10 23.45");
            Solution.RegisterOrder("ABC BUY 3 23.45");
            Solution.RegisterOrder("BBC BUY 10 13.50");
            Solution.RegisterOrder("BBC SELL 100 13.45");
        }
    }
}