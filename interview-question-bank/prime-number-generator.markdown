# write a function that prints all the prime numbers from 1 to N

It is recommended that the interviewee solve this challenge themself before presenting it to a candidate.

## recommended context
* phone screen
* easy interview

## free info
- A prime number is any natural number greater than 1 that has no positive divisors other than 1 and itself.

# solutions

Most people go for the brute force solution, which is perfectly acceptable in the context of a phone screen; knowledge of a sieve is more in the realm of trivia as far as front-end development should be concerned. That said, if the candidate does correctly implement a sieve, they should be looked upon favorably.

## brute force implementation
```js
function (N) {
    var isPrime;
    var i;
    var j;
    for(i=2; i <= N; ++i) {
        isPrime = true;
        for(j = 2; j < i; ++j) {
            if (0 === i % j) {
                isPrime = false;
                break; // for performance only
            }
        }
        if (isPrime) {
            console.log(i);
        }
    }
}
```

### common mistakes
Many candidates incorrectly assume that if `i % j !== 0` for any `j`, then `i` is prime.


### optimizations
* Keep a hash of primes found so far and iterate `j` along those instead of every number from `2` to `i-1`
* `j < i` can be replaced by `j < sqrti` where `sqrti = Math.floor(Math.sqrt(i))`
 * Bonus points if they can explain why

### follow-up questions
* What is the running time (in big-O) of your algorithm?

### advice to help them along
* Prime numbers do not need to be stored, just printed
* Explain clearly what a prime number is
* Break the problem into two parts:
 1. Iterate over all numbers from 1 to N
 1. Find which of those numbers are prime


### if they bomb out
recommendations for further study:
* [Project Euler problems](https://projecteuler.net/)

## sieve implementation
A sieve is a completely different approach to this problem that runs _much_ faster. The easiest to implement prime number sieve is the sieve of Eratosthenes. Candidates seldom code this option and chances are high that they are very well prepared if they implement it correctly.

### reference implementation
```js
function (N) {
    var composite = []; // in other languages, this array may need to zeroed to N before it can be used
    var i;
    var j;
    for(i = 2; i < N; ++i) {
        if (composite[i]) {
            continue;
        }
        console.log(i);
        for(j = i * 2; j < N; j += i) {
            composite[j] = true;
        }
    }
}
````

### common mistakes
`j` starts at `i` instead of `i * 2`

### follow-up questions
* what is the running time (in big-O) of your algorithm?

### more information
[Wikipedia](https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes)
