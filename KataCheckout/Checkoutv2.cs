using System;
using System.Collections.Generic;
using System.Linq;

namespace KataCheckout
{

    public class Checkoutv2 : ICheckout
    {
        public static readonly Func<Func<int, double>, Func<IDictionary<string, int>, string, double>> CONVERT
            = func
                => (basket, sku)
                    => func(basket.TryGetValue(sku, out var quan) ? quan : 0);

        public static readonly Func<double, string, int, Func<IDictionary<string, int>, string, double>> OFFER_PERC_WITH =
            (price, withSku, perc) =>
            {
                return (bas, sku) =>
                {
                    ;
                    var countWithSku = bas.TryGetValue(withSku, out var cw) ? cw : 0;
                    var count = bas.TryGetValue(sku, out var c) ? c : 0;

                    var fullPrice = count - countWithSku;

                    return Checkout.NO_OFFER(price)(fullPrice) + Checkout.OFFER_PERC_DISCOUNT(price, perc)(count - fullPrice);
                };
            };

        private readonly IDictionary<string, Func<IDictionary<string, int>, string, double>> _rules;
        private IDictionary<string, int> basket = new Dictionary<string, int>();

        public Checkoutv2(IDictionary<string, Func<IDictionary<string, int>, string, double>> rules)
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
                .Select(sku => _rules[sku.Key](basket, sku.Key))
                .Sum();
        }
    }
}