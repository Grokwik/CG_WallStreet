using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Project
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Init()
        {
            if (Solution.ExecBook != null)
                Solution.ExecBook.Clear();
            if (Solution.SellBook != null)
                Solution.SellBook.Clear();
            if (Solution.BuyBook != null)
                Solution.BuyBook.Clear();
        }

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
            Solution.Execute();
            Check.That(Solution.ExecBook[0].Display()).Equals("AAA 1 10.00");
            Check.That(Solution.ExecBook.Count).Equals(1);
        }

        [TestMethod]
        public void TwoSymbols()
        {
            Solution.RegisterOrder("AAA BUY 1 10.00");
            Solution.RegisterOrder("BBB SELL 1 10.00");
            Solution.RegisterOrder("BBB BUY 1 10.00");
            Solution.Execute();
            Check.That(Solution.ExecBook.Count).Equals(1);
            Check.That(Solution.ExecBook[0].Display()).Equals("BBB 1 10.00");
        }

        [TestMethod]
        public void NoTrade()
        {
            Solution.RegisterOrder("ABC BUY 1 9.00");
            Solution.RegisterOrder("ABC SELL 1 10.0");
            Solution.Execute();
            Check.That(Solution.ExecBook).IsEmpty();
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
            Solution.Execute();
            Check.That(Solution.ExecBook.Count).Equals(9);
            Check.That(Solution.ExecBook[0].Display()).Equals("ABC 8 23.50");
            Check.That(Solution.ExecBook[1].Display()).Equals("ABC 2 23.50");
            Check.That(Solution.ExecBook[2].Display()).Equals("ABC 3 23.50");
            Check.That(Solution.ExecBook[3].Display()).Equals("ABC 7 23.50");
            Check.That(Solution.ExecBook[4].Display()).Equals("ABC 3 23.45");
            Check.That(Solution.ExecBook[5].Display()).Equals("BBC 10 13.50");
            Check.That(Solution.ExecBook[6].Display()).Equals("BBC 8 13.50");
            Check.That(Solution.ExecBook[7].Display()).Equals("BBC 12 13.50");
            Check.That(Solution.ExecBook[8].Display()).Equals("BBC 10 13.50");
        }
    }
}