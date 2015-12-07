#package extraction guide

There has been discussion of using a scaffolding system so we can stamp out new packages with ease and uniformity, but so far, we've been doing everything by hand. Here are the packages that I know have been extracted from bunsen:

* https://github.com/1stdibs/serverVars
* https://github.com/1stdibs/dibs-endpoints
* https://github.com/1stdibs/copy-text
* https://github.com/antialias/newsy

## some patterns to follow:
* Use a `.npmrc` to point to the dibs npm registry. [For example](https://github.com/1stdibs/dibs-endpoints/blob/master/.npmrc).
* When appropriate, host the code under the 1stdibs repository.
 * if the new package depends on any packages that are private to 1stdibs, then that new package should be private.
* If the module doesn't require the DOM, consider porting the specs over to mocha + sinon spies
* Move any comment-based documentation into the readme
* Remove the code (including specfile) from the original repo (usually bunsen)
* Shim the original repo to point at the new package and console.warn to notify unaware devs that they should be using the extracted pacakge. [For example](https://github.com/1stdibs/bunsen/blob/master/libraries/endpoints.js#L2).

## recomendations:
* Use [`git-filter-branch`](https://git-scm.com/docs/git-filter-branch) to preserve the commit history of modules as they are migrated to the new package.
