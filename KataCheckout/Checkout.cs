using System;
using System.Collections.Generic;
using System.Linq;

namespace KataCheckout
{
    public interface ICheckout
    {
        void Scan(string sku);
        double Total();
    }
    
    public class Checkout : ICheckout
    {
        public static readonly Func<double, Func<int, double>> NO_OFFER
            = price
                => quan
                    => quan * price;

        public static readonly Func<double, int, Func<int, double>> OFFER_PERC_DISCOUNT 
            = (price, discount)
                => quan => NO_OFFER(price)(quan) * (100 - discount) * 0.01;

        public static readonly Func<double, int, Func<int, double>> OFFER_XFORXMINUS1 =
            (price, x)
                => quan
                    => NO_OFFER(price)(quan - quan / x);
        
        public static readonly Func<double, Func<int, double>> OFFER_3FOR2 =
            price =>
                OFFER_XFORXMINUS1(price, 3);
        
        public static readonly Func<double, Func<int, double>> OFFER_BOGOF =
            price =>
                OFFER_XFORXMINUS1(price, 2);

        private readonly IDictionary<string, Func<int, double>> _rules;
        private IDictionary<string, int> basket = new Dictionary<string, int>();

        public Checkout(IDictionary<string, Func<int, double>> rules)
        {
            _rules = rules;
        }

        public void Scan(string sku)
        {
            basket[sku] = basket.TryGetValue(sku, out var quantity) ? quantity + 1 : 1;
        }

        public double Total()
        {
            return basket
                .Select(sku => _rules[sku.Key](sku.Value))
                .Sum();
        }
    }
}