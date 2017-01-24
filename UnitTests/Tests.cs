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
            /*
                ABC BUY 10 23.50    ABC SELL 8 23.50        8
                reste 2             ABC SELL 5 23.50        2
                ABC BUY 10 23.50    reste 3                 3
                reste 7             ABC SELL 10 23.45       7
                ABC BUY 3 23.45     reste 3                 3

                BBC BUY 10 13.50    BBC SELL 100 13.45      10
                BBC BUY 8 13.50                             8
                BBC BUY 12 13.50                            12
                BBC BUY 10 13.50                            10
            */
        }

        [TestMethod]
        public void BusyDay()
        {
            #region inputs
            Solution.RegisterOrder("AAA BUY 10 12.00");
            Solution.RegisterOrder("ABC BUY 10 11.50");
            Solution.RegisterOrder("DFE BUY 1 10.00");
            Solution.RegisterOrder("ABC BUY 8 11.25");
            Solution.RegisterOrder("DEF BUY 10 11.25");
            Solution.RegisterOrder("AAB BUY 8 11.00");
            Solution.RegisterOrder("ABC BUY 8 11.25");
            Solution.RegisterOrder("AAB BUY 9 11.50");
            Solution.RegisterOrder("AAB BUY 12 11.25");
            Solution.RegisterOrder("AAB BUY 12 11.00");
            Solution.RegisterOrder("AAB BUY 9 10.00");
            Solution.RegisterOrder("DEF BUY 1 11.00");
            Solution.RegisterOrder("AAA BUY 10 11.25");
            Solution.RegisterOrder("DCE BUY 1 11.75");
            Solution.RegisterOrder("DFE BUY 11 11.50");
            Solution.RegisterOrder("DEF BUY 1 11.50");
            Solution.RegisterOrder("ABC BUY 8 11.00");
            Solution.RegisterOrder("DCE BUY 12 11.50");
            Solution.RegisterOrder("DCE BUY 9 12.00");
            Solution.RegisterOrder("DCE BUY 9 11.75");
            Solution.RegisterOrder("AAA BUY 12 11.50");
            Solution.RegisterOrder("AAB BUY 12 11.75");
            Solution.RegisterOrder("ABC BUY 12 10.00");
            Solution.RegisterOrder("DEF BUY 11 12.00");
            Solution.RegisterOrder("DFE BUY 11 11.25");
            Solution.RegisterOrder("ABC BUY 12 12.00");
            Solution.RegisterOrder("DEF BUY 10 11.50");
            Solution.RegisterOrder("AAB BUY 9 11.50");
            Solution.RegisterOrder("DFE BUY 8 12.00");
            Solution.RegisterOrder("AAA BUY 11 11.75");
            Solution.RegisterOrder("DCE BUY 9 11.25");
            Solution.RegisterOrder("DCE BUY 12 11.25");
            Solution.RegisterOrder("DCE BUY 11 11.50");
            Solution.RegisterOrder("DFE BUY 8 11.75");
            Solution.RegisterOrder("DCE BUY 1 11.00");
            Solution.RegisterOrder("AAA BUY 8 10.00");
            Solution.RegisterOrder("DFE BUY 12 10.00");
            Solution.RegisterOrder("AAB BUY 8 10.00");
            Solution.RegisterOrder("AAA BUY 8 10.00");
            Solution.RegisterOrder("ABC BUY 1 11.00");
            Solution.RegisterOrder("AAA BUY 8 11.25");
            Solution.RegisterOrder("AAA BUY 8 11.75");
            Solution.RegisterOrder("DCE BUY 8 11.25");
            Solution.RegisterOrder("DFE BUY 8 12.00");
            Solution.RegisterOrder("DCE BUY 11 10.00");
            Solution.RegisterOrder("DEF BUY 10 11.25");
            Solution.RegisterOrder("DEF BUY 1 11.75");
            Solution.RegisterOrder("ABC BUY 9 11.00");
            Solution.RegisterOrder("DCE BUY 8 11.75");
            Solution.RegisterOrder("DEF BUY 8 11.25");
            Solution.RegisterOrder("DFE BUY 10 10.00");
            Solution.RegisterOrder("DFE BUY 10 10.00");
            Solution.RegisterOrder("ABC BUY 10 12.00");
            Solution.RegisterOrder("AAB BUY 8 11.00");
            Solution.RegisterOrder("DEF BUY 11 11.75");
            Solution.RegisterOrder("AAA BUY 10 11.25");
            Solution.RegisterOrder("ABC SELL 11 11.00");
            Solution.RegisterOrder("DFE SELL 10 11.25");
            Solution.RegisterOrder("AAB SELL 8 12.00");
            Solution.RegisterOrder("AAA SELL 10 11.00");
            Solution.RegisterOrder("AAB SELL 1 10.00");
            Solution.RegisterOrder("DFE SELL 11 11.00");
            Solution.RegisterOrder("DCE SELL 9 11.50");
            Solution.RegisterOrder("DFE SELL 1 12.00");
            Solution.RegisterOrder("DEF SELL 8 11.25");
            Solution.RegisterOrder("DCE SELL 1 11.25");
            Solution.RegisterOrder("DFE SELL 1 11.25");
            Solution.RegisterOrder("DEF SELL 9 12.00");
            Solution.RegisterOrder("ABC SELL 9 11.75");
            Solution.RegisterOrder("DCE SELL 12 11.50");
            Solution.RegisterOrder("DFE SELL 10 11.50");
            Solution.RegisterOrder("DEF SELL 12 11.75");
            Solution.RegisterOrder("DCE SELL 9 11.25");
            Solution.RegisterOrder("DFE SELL 8 11.00");
            Solution.RegisterOrder("AAB SELL 8 12.00");
            Solution.RegisterOrder("DFE SELL 10 12.00");
            Solution.RegisterOrder("DFE SELL 12 11.75");
            Solution.RegisterOrder("ABC SELL 10 11.25");
            Solution.RegisterOrder("ABC SELL 11 11.25");
            Solution.RegisterOrder("AAA SELL 9 11.50");
            Solution.RegisterOrder("DCE SELL 1 11.75");
            Solution.RegisterOrder("DEF SELL 11 11.25");
            Solution.RegisterOrder("AAB SELL 8 10.00");
            Solution.RegisterOrder("AAB SELL 1 11.25");
            Solution.RegisterOrder("ABC SELL 11 12.00");
            Solution.RegisterOrder("DFE SELL 12 11.25");
            Solution.RegisterOrder("DCE SELL 12 11.25");
            Solution.RegisterOrder("DEF SELL 11 11.50");
            Solution.RegisterOrder("DCE SELL 9 11.75");
            Solution.RegisterOrder("AAB SELL 11 12.00");
            Solution.RegisterOrder("AAA SELL 10 10.00");
            Solution.RegisterOrder("DEF SELL 1 11.00");
            Solution.RegisterOrder("AAB SELL 11 11.75");
            Solution.RegisterOrder("AAA SELL 9 10.00");
            Solution.RegisterOrder("ABC SELL 10 11.00");
            Solution.RegisterOrder("ABC SELL 8 11.50");
            Solution.RegisterOrder("AAA SELL 12 11.25");
            Solution.RegisterOrder("DFE SELL 9 12.00");
            Solution.RegisterOrder("DEF SELL 12 12.00");
            Solution.RegisterOrder("ABC SELL 11 12.00");
            #endregion
            
        }
    }
}