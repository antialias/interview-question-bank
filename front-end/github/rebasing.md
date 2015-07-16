[preserve-merges]: http://stackoverflow.com/questions/15915430/what-exactly-does-gits-rebase-preserve-merges-do-and-why

# Rebasing your branch

## More in-depth explanation

This is an [in-depth git presentation](/assets/pdfs/thomas-git.pdf) [Thomas](https://github.com/antialias/) gave which contains more information about rebasing (amongst other git features).

## What is rebasing?

_re_ – change  
_base_ – a structure on which something draws or depends

When you rebase, you are changing the parent commit of your branch. In order to accommodate this change, all the commits after the parent commit must be reconstructed.

## Why rebase? 

Whenever your branch is in conflict with the target branch, you need to get it up to date. A merge can handle that, sure, but here are two reasons why rebasing might be preferred over merging:

1. Merge commits are removed
2. History is much easier to read

## Standard rebasing

### Get the most up-to-date target branch

Checkout `develop` (or the target branch) and get it up to date: 

```bash
$ git pull upstream develop
```

If you happen to have local commits on a shared branch, you should use the `--rebase` flag:

```bash
$ git pull --rebase upstream <branch>
```

### Replay your changes on top of the target branch

Checkout your feature branch and rebase it on top of develop (or target branch): 

```bash
$ git checkout feature-branch
$ git rebase develop
```

Fix any conflicts as they come along.

When rebasing your branch on top of a new parent commit, all the commits from the part of your branch being rebased are played on top of the new parent. During this process, any commits from merges are replayed as well, forming a perfectly linear commit history, devoid of any merge commits (unless the [--preserve-merges][preserve-merges] flag is used.)

If you get a conflict, it's advisable to use `--conflict=diff3` so you can see your common ancestor of when your change split versus the current state of the file:

```bash
CONFLICT (content): Merge conflict in file.ext
$ git checkout --conflict=diff3 file.ext
```

To avoid needing to pass this flag all the time, you can set this in your `.gitconfig` file instead:

```text
[merge]
  conflictstyle = diff3
```

### Update your remote branch
 
Once you have resolved all of your conflicts, then you will have to do a force update on your branch since you rewrote the hashes:

```bash
 $ git push -f origin feature-branch
```

## Interactive rebasing

Sometimes a feature branch can result in many commits that may end up leading to confusion if someone is reading the commit history. Or you want to rearrange some history to read a little nicer. Or delete a commit. The list could go on. Regardless, this is where interactive rebasing (also known as squashing) comes in handy:

```bash
$ git rebase -i <base>
```

By going into interactive mode, you can remove, reorder, and even add commits. 

### Commands 

The following is a list of commands that are allowed during an interactive rebase:  

```vim
# Commands:
#  p, pick = use commit
#  r, reword = use commit, but edit the commit message
#  e, edit = use commit, but stop for amending
#  s, squash = use commit, but meld into previous commit
#  f, fixup = like "squash", but discard this commit's log message
#  x, exec = run command (the rest of the line) using shell
```

That list is always included while in an interactive rebase. 

### Squashing

You can also squash multiple commits into one:

```bash
pick aeb2b97 amazing bug fix
squash 6f0ab66 bs bug fix
squash 637e4fd meh, ok bug fix
```

After saving, git will start rebasing your commits and then bring you to a second screen:

```vim
# This is a combination of 3 commits.
# The first commit's message is:
amazing bug fix

# This is the 2nd commit message:

bs bug fix

# This is the 3rd commit message:

meh, ok bug fix
```

At this state, you can modify the commit messages as you wish. 

### Fixup

However, if you know that one commit message will suffice, you can use `fixup` (or `f`) instead to meld commits into the `pick`ed commit:

```bash
pick aeb2b97 amazing bug fix
fixup 6f0ab66 bs bug fix
fixup 637e4fd meh, ok bug fix
```

This will accomplish the same as using the `squash` command (indicated by `s`), but rather than letting you edit the message, it will end up using the `amazing bug fix` message for all three commits without going to a second screen.

Once you have finished your interactive rebasing, it's advisable to rebase these changes on top of your target branch.





