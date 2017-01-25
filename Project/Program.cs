using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Order
    {
        public string Verb;
        public string Symbol;
        public int Qty;
        public float Price;

        public Order(string symbol, int qty, float price, string verb)
        {
            Symbol = symbol;
            Qty = qty;
            Price = price;
            Verb = verb;
        }

        public string Display()
        {
            var strPrice = String.Format("{0:0.00}", Price);
            strPrice = strPrice.Replace(',', '.');
            var output = String.Format("{0} {1} {2}", Symbol, Qty, strPrice);
            return output;
        }
    }

    public class Solution
    {
        public static List<string> Symbols;
        public static List<Order> BuyBook;
        public static List<Order> SellBook;
        public static List<Order> ExecBook;

        public static void RegisterOrder(string input)
        {
            if (BuyBook == null)    BuyBook = new List<Order>();
            if (SellBook == null)   SellBook = new List<Order>();
            if (Symbols == null)    Symbols = new List<string>();

            string[] inputs = input.Split(' ');
            string symbol = inputs[0];
            string verb = inputs[1];
            int qty = int.Parse(inputs[2]);
            float price = float.Parse(inputs[3].Replace('.', ','));

            if (verb.Equals("BUY"))
                BuyBook.Add(new Order(symbol, qty, price, "BUY"));
            else
                SellBook.Add(new Order(symbol, qty, price, "SELL"));

            if (!Symbols.Contains(symbol))
                Symbols.Add(symbol);
        }

        public static void EchoBooks()
        {
            Console.Error.WriteLine("BUY BOOK");
            foreach (var o in BuyBook)
                Console.Error.WriteLine("{0}", o.Display());

            Console.Error.WriteLine("SELL BOOK");
            foreach (var o in SellBook)
                Console.Error.WriteLine("{0}", o.Display());
        }

        public static Order ExecuteOrder(Order buyO, Order sellO, float price)
        {
            var tradedQty = Math.Min(buyO.Qty, sellO.Qty);
            ExecBook.Add(new Order(sellO.Symbol, tradedQty, price, "EXEC"));
            var remainingQty = sellO.Qty - buyO.Qty;
            if (remainingQty < 0)
            {
                buyO.Qty = -remainingQty;
                return buyO;
            }
            else if (remainingQty > 0)
            {
                sellO.Qty = remainingQty;
                return sellO;
            }
            return null;
        }

        public static void Execute()
        {
            ExecBook = new List<Order>();
            foreach (var s in Symbols)
            {
                var cBuyOrders = from o in BuyBook
                                 where o.Symbol == s
                                 orderby o.Price ascending
                                 select o;
                var cSellOrders = from o in SellBook
                                  where o.Symbol == s
                                  orderby o.Price descending
                                  select o;
                var BO = cBuyOrders.FirstOrDefault();
                BuyBook.Remove(BO);
                var SO = cSellOrders.FirstOrDefault();
                SellBook.Remove(SO);
                float lastPrice = -1;
                Order remainingOrder = null;
                while (BO != null && SO != null)
                {
                    if (BO.Price == SO.Price)
                    {
                        lastPrice = BO.Price;
                        remainingOrder = ExecuteOrder(BO, SO, BO.Price);
                    }
                    else if (BO.Price > SO.Price
                         && lastPrice != -1)
                    {
                        remainingOrder = ExecuteOrder(BO, SO, lastPrice);
                    }
                    else
                        break;
                    if (remainingOrder != null)
                    {
                        if (remainingOrder.Verb == "BUY")
                        {
                            BO = remainingOrder;
                            SO = cSellOrders.FirstOrDefault();
                        }
                        else
                        {
                            SO = remainingOrder;
                            BO = cBuyOrders.FirstOrDefault();
                        }
                    }
                    else
                    {
                        SO = cSellOrders.FirstOrDefault();
                        BO = cBuyOrders.FirstOrDefault();
                    }
                    remainingOrder = null;
                }
            }
        }

        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            for (int i = 0; i < N; i++)
            {
                RegisterOrder(Console.ReadLine());
            }

            Execute();

            if (ExecBook.Count == 0)
                Console.WriteLine("NO TRADE");
            else
            {
                foreach (var e in ExecBook)
                {
                    e.Display();
                }
            }
        }
    }
}