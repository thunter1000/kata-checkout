# Introduction

Supermarket pricing can be complex because of the different types of offers we offer to our customers. For example 2 for 1.

# Task

Implement a checkout that can total a customers shop. Below is a table of the products on sale, price and discount (if any).

| SKU | Price (£) | Discount |
| --- | --- | --- |
| `A` | 3.99 | - |
| `B` | 5.99 | 10% off |
| `C` | 11.22 | Buy one get one free |
| `D` | 2.99 | 3 for 2 |

Use it to calculate the cost of the following shop.

```
CCBBDCACBAABACACDBDBCADBBDCDBCDB
```

# Extension

The supermarket has started to implement deals depending on other items in the customers basket. Alter your implementation to account for this.

| SKU | Price (£) | Discount | Condition |
| --- | --- | --- | --- |
| `A` | 3.99 | - |
| `B` | 5.99 | 10% off |
| `C` | 11.22 | Buy one get one free |
| `D` | 2.99 | 3 for 2 |
| `E` | 6.99 | 20 % off when purchased with product `A` |

> **Example**
>
> `AEE` = 3.99 + 6.99 + 6.99 * 0.8 = 16.57
>
> `AAEE` = 3.99 * 2 + 6.99 * 0.8 * 2 = 19.16

What is the total of this customers shop?

```
ADADADDBEBECBBBDBECBDECDDBEDEDCA
```