# compact the zeros in an array by shifting all the non-zero elements to towards the beginning

## recommended context
* phone screen
* easy interview

## restrictions
* the array only contains numbers
* the operation must be performed in-place without using a second array
* only reference and assignment can be used. No Array.prototype methods are allowed
* the order of the non-zero elements must be preserved
* as long as the non-zero values are shifted to the left, we do not care about what is in the remainder of the array, i.e. if the array was `[0, 0, 1]` before compaction, it is okay for the compacted version to be `[1, 1, 1]`

## examples
```js
compact([0, 1, 0, 0, 2, 0, 3, 4, 0, 5, 0, 6])
> [1, 2, 3, 4, 5, 6, ...]
```

```js
compact([0,0,0,0,1])
> [1, ...]
```

```js
compact([1, 2, 1, 0, 2, 1, 2])
> [1, 2, 1, 2, 1, 2, ...]
```

## watch out for code that:
* doesn't remove consecutive zeros
* changes the order of the non-zero elements
* uses a temporary array

## reference implementation
```js
var compact = functionInPlace(a) {
    var i;
    var p = 0;
    for(i=0; i < a.length; ++i) {
        if (a[i] !== 0) {
            a[p] = a[i];
            ++p;
        }
    }
}
```
