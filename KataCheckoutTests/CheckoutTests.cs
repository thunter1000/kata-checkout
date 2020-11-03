using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using KataCheckout;
using Xunit;

namespace KataCheckoutTests
{
    public class CheckoutTests
    {
        [Fact]
        public void SingleItem()
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                {"A", Checkout.NO_OFFER(3.99)}
            });
            
            checkout.Scan("A");

            checkout.Total().Should().Be(3.99);
        }

        [Fact]
        public void MulitpleItems()
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                {"A", Checkout.NO_OFFER(3.99)}
            });
            
            checkout.Scan("A");
            checkout.Scan("A");

            checkout.Total().Should().Be(7.98);
        }

        [Fact]
        public void PercentDiscount()
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                {"A", Checkout.OFFER_PERC_DISCOUNT(10, 10)}
            });
            
            checkout.Scan("A");

            checkout.Total().Should().Be(9);
        }
        
        [Theory]
        [InlineData("A", 10)]
        [InlineData("AA", 20)]
        [InlineData("AAA", 20)]
        [InlineData("AAAA", 30)]
        [InlineData("AAAAAA", 40)]
        public void ThreeFor2(string skus, double expectedResult)
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                { "A", Checkout.OFFER_3FOR2(10) }
            });

            foreach (var sku in skus.ToCharArray())
            {
                checkout.Scan(sku.ToString());
            }

            checkout.Total().Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("A", 10)]
        [InlineData("AA", 10)]
        [InlineData("AAA", 20)]
        [InlineData("AAAA", 20)]
        public void BOGOF(string skus, double expectedResult)
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                { "A", Checkout.OFFER_BOGOF(10) }
            });

            foreach (var sku in skus.ToCharArray())
            {
                checkout.Scan(sku.ToString());
            }

            checkout.Total().Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("AAA", 30)]
        [InlineData("ACCC", 42)]
        [InlineData("DDCCC", 54)]
        public void MultiDeals(string skus, double expectedResult)
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                {"A", Checkout.NO_OFFER(10)},
                {"B", Checkout.OFFER_PERC_DISCOUNT(15, 10)},
                {"C", Checkout.OFFER_3FOR2(16)},
                {"D", Checkout.OFFER_BOGOF(22)}
            });

            foreach (var sku in skus.ToCharArray())
            {
                checkout.Scan(sku.ToString());
            }

            checkout.Total().Should().Be(expectedResult);
        }
    
        [Fact]
        public void FinalAns()
        {
            var checkout = new Checkout(new Dictionary<string, Func<int, double>>
            {
                {"A", Checkout.NO_OFFER(3.99)},
                {"B", Checkout.OFFER_PERC_DISCOUNT(5.99, 10)},
                {"C", Checkout.OFFER_BOGOF(11.22)},
                {"D", Checkout.OFFER_3FOR2(2.99)}
            });

            foreach (var sku in "CCBBDCACBAABACACDBDBCADBBDCDBCDB".ToCharArray())
            {
                checkout.Scan(sku.ToString());
            }

            checkout.Total().Should().Be(148.90000000000003);
        }
        
        [Fact]
        public void FinalAns2()
        {
            var checkout = new Checkoutv2(new Dictionary<string, Func<IDictionary<string, int>, string, double>>
            {
                {"A", Checkoutv2.CONVERT(Checkout.NO_OFFER(3.99))},
                {"B", Checkoutv2.CONVERT(Checkout.OFFER_PERC_DISCOUNT(5.99, 10))},
                {"C", Checkoutv2.CONVERT(Checkout.OFFER_BOGOF(11.22))},
                {"D", Checkoutv2.CONVERT(Checkout.OFFER_3FOR2(2.99))},
                {"E", Checkoutv2.OFFER_PERC_WITH(6.99, "A", 20)}
            });

            foreach (var sku in "ADADADDBEBECBBBDBECBDECDDBEDEDCA".ToCharArray())
            {
                checkout.Scan(sku.ToString());
            }

            checkout.Total().Should().Be(138.806);
        }
    }
}