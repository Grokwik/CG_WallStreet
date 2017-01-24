using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Order
    {
        public string Symbol;
        public int Qty;
        public float Price;

        public Order(string symbol, int qty, float price)
        {
            Symbol = symbol;
            Qty = qty;
            Price = price;
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
            if (BuyBook == null) BuyBook = new List<Order>();
            if (SellBook == null) SellBook = new List<Order>();
            if (Symbols == null) Symbols = new List<string>();

            string[] inputs = input.Split(' ');
            string symbol = inputs[0];
            string verb = inputs[1];
            int qty = int.Parse(inputs[2]);
            //float price = float.Parse(inputs[3]);
            float price = float.Parse(inputs[3].Replace('.', ','));

            if (verb.Equals("BUY"))
                BuyBook.Add(new Order(symbol, qty, price));
            else
                SellBook.Add(new Order(symbol, qty, price));

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

        public static void Execute()
        {
            ExecBook = new List<Order>();
            foreach (var s in Symbols)
            {
                var cBuyOrders = from o in BuyBook
                                 where o.Symbol == s
                                 orderby o.Price descending
                                 select o;
                var cSellOrders = from o in SellBook
                                  where o.Symbol == s
                                  orderby o.Price descending
                                  select o;
                var BO = cBuyOrders.FirstOrDefault();
                BuyBook.Remove(BO);
                var SO = cSellOrders.FirstOrDefault();
                SellBook.Remove(SO);
                while (BO != null && SO != null)
                {
                    if (BO.Price < SO.Price)
                        break;

                    if (BO.Price > SO.Price)
                        break;

                    var tradedQty = Math.Min(BO.Qty, SO.Qty);
                    ExecBook.Add(new Order(s, tradedQty, BO.Price));
                    var remainingQty = SO.Qty - BO.Qty;
                    if (remainingQty < 0)
                    {
                        BO.Qty = -remainingQty;
                        SO = cSellOrders.FirstOrDefault();
                        SellBook.Remove(SO);
                    }
                    else if (remainingQty > 0)
                    {
                        SO.Qty = remainingQty;
                        BO = cBuyOrders.FirstOrDefault();
                        BuyBook.Remove(BO);
                    }
                    else
                    {
                        BO = cBuyOrders.FirstOrDefault();
                        SO = cSellOrders.FirstOrDefault();
                        BuyBook.Remove(BO);
                        SellBook.Remove(SO);
                    }
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