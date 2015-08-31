[bad-pr-image]: /assets/images/github/bad-pr.png
[good-pr-image]: /assets/images/github/good-pr.png

# Pull Requests on GitHub

This is the typical workflow for 1stdibs front-end code reviews/pull requests (aka PR):

1. open up against the proper [branch](./git-branching.md)
    - If it is a future feature, typically it's `develop`
    - a fix for the current release - `release` branch
    - a hotfix, or unscheduled release - `master` branch
2. add appropriate [labels](./labels.md)
3. add a [milestone](./milestones.md)
    - unless if the PR has the "milestone tbd" label, there should always be a milestone associated with a PR. This makes it easier for the repo maintainers to see what is landing per release
4. add an assignee
    - typically it will be the team lead or the developer that is most familiar with the updated functionality

Adding a useful description is helpful, but not required. However, if there are dependencies in other repos, please add that to the DESCRIPTION field.
Do not leave it in as a comment in the PR thread. It makes it much easier to see dependencies in the description rather than needing to scour through the comments.
The description is editable after the PR is created so you can add it in later if necessary. Something like this is suffice:

> depends on: https://github.com/1stdibs/bunsen/pull/785

Github will auto format and shorten the URL for you. For example, if the above dependency was added to a description in .com, it would format to this:

> depends on: 1stdibs/bunsen#785

Update anything as necessary. Code/features can slip so if your PR gets moved out to a further milestone, update the milestone.

Here is an example of a good PR description:

![good pr description][good-pr-image]

Notice in the description of that PR, there are two linked dependent PRs as well as letting another senior dev know about the PR since it was already assigned to one senior dev. Since GitHub only allows for one assignee, this is only way you can add other devs to PRs.
This is important to note - if you want to get the attention of any developer, make sure to mention the developer(s) you want to see the thread since some developers will only receive notifications when they are mentioned.

[Here is an example of where dependencies get lost within the comments and should have been added to the description instead][bad-pr-image] (This is a rather long screen grab so it's linked instead of being put inline). If you followed that link, you'll see how long a PR conversation can get and how easy it is to skip over dependent PRs, especially when there are references to other commits within the conversation.

