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
        public static List<Order> BuyBook;
        public static List<Order> SellBook;
        public static List<Order> ExecBook;
        private static string SymbolToErase;

        public static void RegisterOrder(string input)
        {
            if (BuyBook == null)
                BuyBook = new List<Order>();
            if (SellBook == null)
                SellBook = new List<Order>();

            string[] inputs = input.Split(' ');
            string symbol = inputs[0];
            string verb = inputs[1];
            int qty = int.Parse(inputs[2]);
            float price = float.Parse(inputs[3].Replace('.', ','));

            if (verb.Equals("BUY"))
                BuyBook.Add(new Order(symbol, qty, price));
            else
                SellBook.Add(new Order(symbol, qty, price));
        }

        public static void SortBooks()
        {
            SellBook.Sort(delegate (Order x, Order y)
            {
                if (x.Symbol == y.Symbol)
                {
                    if (x.Price == y.Price)
                    {
                        if (x.Qty == y.Qty)
                            return 0;
                        return y.Qty.CompareTo(x.Qty);
                    }
                    return x.Price.CompareTo(y.Price);
                }
                return x.Symbol.CompareTo(y.Symbol);
            });

            BuyBook.Sort(delegate (Order x, Order y)
            {
                if (x.Symbol == y.Symbol)
                {
                    if (x.Price == y.Price)
                    {
                        if (x.Qty == y.Qty)
                            return 0;
                        return y.Qty.CompareTo(x.Qty);
                    }
                    return x.Price.CompareTo(y.Price);
                }
                return x.Symbol.CompareTo(y.Symbol);
            });
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
            SortBooks();
            while (SellBook.Count != 0 && BuyBook.Count != 0)
            {
                var STop = SellBook[0];
                SellBook.RemoveAt(0);

                var BTop = BuyBook[0];
                BuyBook.RemoveAt(0);

                if (STop.Symbol == BTop.Symbol)
                {
                    if ((STop.Price == BTop.Price)
                     && (STop.Qty == BTop.Qty))
                    {
                        ExecBook.Add(new Order(STop.Symbol, STop.Qty, STop.Price));
                        BuyBook.Remove(BTop);
                        SellBook.Remove(STop);
                    }
                }
                else
                {
                    if (STop.Symbol.CompareTo(BTop.Symbol) == -1)
                        SymbolToErase = STop.Symbol;
                    else
                        SymbolToErase = BTop.Symbol;

                    BuyBook.RemoveAll(delegate (Order o)
                    {
                        return (o.Symbol == Solution.SymbolToErase);
                    });
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