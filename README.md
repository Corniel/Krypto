# Krypto

Krypto is a card game designed by Daniel Yovich in 1963. It is a mathematical
game that promotes proficiency with basic arithmetic operations. 

## Rules of Krypto
The Krypto deck consists of 54 cards: three each of numbers 1-10, two each of
11-19, one each of 20-25. Six cards are dealt:
a common objective card at the top and five other cards below. Each player must
use all five of the cards' numbers exactly once, using any combination of
arithmetic operations (addition, subtraction, multiplication, and division), to
form the objective card's number. The first player to come up with a correct
formula is the winner.

## Example Solution
```
23 = 3 ? 4 ? 11 ? 13 ? 20

23 = (13 - 11) + (4 - 3) + 20
23 = 13 + (20 / ((11 - 3) / 4))
```

## Solver
Finding a solution itself is not that difficult (for a computer). Not even
finding it quickly. The challenge is to return only unique and readable
solutions. So when are solutions not different?

That: `4 = 1 + 1 + 2` equals `4 = 1 + 2 + 1`, is not such a debate, but
that: `4 = 2 + 2` equals `4 = 2 * 2`) can be, or that: `2 = 4 - 2` equals `2 = 4 / 2`.

The rule of thumb I used was, that if a pair of two numbers had the same
outcome, independent of the used operator, they are equal, so:
* `2 + 2` equals `2 * 2`
* `4 - 2` equals `4 / 2`
* `n * 1` equals `n / 1`
* `0 * n` equals `0 / n when n != 0`
