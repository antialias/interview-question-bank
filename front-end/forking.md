# forking

Any pull request that isn't coming from your fork will not be merged. 

## What's the forking? What does this mean? 

### Forking, Aye?
You need to fork all FE repos: 1stdibs.com, 1stdibs-admin-v2, 1stdibs-admin-v1, bunsen and dibslibs. You will then make pull requests against 1stdibs's account from your forks (instead of, making pull requests from topic branches against 1stdibs repos).

### Create & clone the fork
How do I fork a repo? It's easy! Read this: https://help.github.com/articles/fork-a-repo/

Once the fork is created, you need to clone it to your machine. To do this, you set the URL for the remote called origin (which should be now set to 1stdibs's account).

For example, do this in your 1stdibs.com clone (use the ssh URL from github):
```
$ cd ~/projects/1stdibs.com
$ git remote set-url origin git@github.com:[my-account]/1stdibs.com.git
```

To verify:
```
$ git remote -v
origin git@github.com:[my-account]/1stdibs.com.git (fetch)
origin git@github.com:[my-account]/1stdibs.com.git (push)
```

Now, when you push, pull or fetch from origin you'll be getting the code from your fork. Your fork is now origin.

How do you stay in sync with all the changes happening in 1stdibs's account? I'm glad you asked...

### upstream vs. origin
Once you've cloned your five forks, you need to set a new remote. Call this remote "upstream."

It's as easy as this (using 1stdibs.com as an example):
```
$ git remote add upstream https://github.com/1stdibs/1stdibs.com.git
```

To verify:
```
$ git remote -v
origin git@github.com:[my-account]/1stdibs.com.git (fetch)
origin git@github.com:[my-account]/1stdibs.com.git (push)
upstream https://github.com/1stdibs/1stdibs.com.git (fetch)
upstream https://github.com/1stdibs/1stdibs.com.git (push)
```

There are github instructions too: https://help.github.com/articles/configuring-a-remote-for-a-fork/ 

Now, upstream is the 1stdibs account and origin is your fork on your own account.

To sync changes from the mainline, instead of pulling and merging develop or master from origin, you'll pull from upstream (aka the 1stdibs account). 

e.g. if on master...
```
$ git pull --rebase upstream master
```

Or,
```
$ git fetch --all
$ git merge upstream/master
```

Github has more instructions here: https://help.github.com/articles/syncing-a-fork/
