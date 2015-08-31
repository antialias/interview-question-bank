# Git branching model

Our branching model is based on [A successful Git branching model](http://nvie.com/posts/a-successful-git-branching-model/).

The bleeding-edge branch is `develop`. When starting a feature branch or a bugfix branch that is targeted to a future release, generally you branch from `develop`. This branch should always have features for a future release (as opposed to the current release, which would go to the `release` branch).

The `release` branch is created for our weekly deploys. This branch is the version of the codebase that is under QA - the release candidate. Once QA is complete, it is merged into `master`, then `master` is regression-tested and ultimately released.

The `master` branch is meant to always be ready to be deployed to production. All other branches should be a progression from `master`. We break this paradigm somewhat when we merge `release` into `master` for regression tests. `master` is also the branch you branch from when creating a hotfix (an unplanned release) branch.


