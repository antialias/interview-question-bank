# General F/E Troubeshooting

## Problem: `Error: EMFILE, too many open files`
This can occur while running `gulp` locally.

**Solution**: Set your `ulimit` in your `.bash_profile` or `.bashrc` file:

```bash
ulimit -S -n 2048
```

Make sure to `source ~/.bash_profile` or `source ~/.bashrc` after setting it

## Problem: Bunsen is not updating after doing an install
This can occur after someone has updated bunsen and you're trying to install npm modules  it on the release branch.

**Solution**: Run `npm run cleaninstall`. This will clear out any npm caches you may have locally. Since the release branch creates a `npm-shrinkwrap.json` file pointing to the hash of bunsen to load, a regular `npm install` may not have the latest bunsen updates. 

## Problem: Develop builds keep on failing
Though there could be a handful of reasons this is happening (like the issue above), these are some of the more common ones:

- there is a `npm-shrinkwrap.json` file still committed in develop
- the build can't resolve a bunsen or dibslibs dependency

**Solution**: 

- Delete the `npm-shrinkwrap.json` file from the develop branch
- Make sure the release branch for bunsen and/or dibslibs has been merged back into their respective master branches (yes, *master*)

## Problem: Local admin login is not working
This can occur if you started your **node** app in `QA` mode, and the devbox is in `Dev` mode or vise versa.

**Solution**: Ensure that the **node** app and vm are in the same mode:

```bash
$ cd ~/projects/1stdibs.com
$ vagrant ssh -c 'get-env'
// says QA or Dev
```
check phpStorm or your command line to see what NODE_ENV is set to.

## Problem: Local style/template changes aren't working on 1stdibs.com
If your devbox is in `QA` mode, your templates could be cached 

**Solution**: Delete the following directory locally: `1stdibs.com/dibs/application/views/compiled` and refresh your browser.

## Problem: Your code can't resolve a file when doing a Jenkins build
OS X's file system (HFS+) is case-insensitive (ie. "foo.js" is the same as "Foo.js") while CentOS's isn't.


**Solution**: Your code could be doing `require('foo')` but the file name is actually `Foo`. Make sure the file casing matches up properly.

