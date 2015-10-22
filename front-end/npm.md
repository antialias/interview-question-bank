# npm

## What is npm?
It's basically the package manager for javascript. You can search for packages and learn more about npm here: [https://www.npmjs.com/](https://www.npmjs.com/)

## Standard npm procedures
### Initialize a new project
<code>npm init</code>
### Install a package
Required by your project:

<code>npm install --save <package-name></code>

Required for development:

<code>npm install --save-dev <package-name></code>

The --save(-dev) flags will tell npm to add the package to the dependencies in package.json when installed

### Remove Package
<code>npm uninstall --save(-dev) <package-name></code>

### Clean Install

Do this if you think some packages may be broken but you don't know which one(s) (Note: This only works on .com and adminv2):

<code>npm run cleaninstall</code>

## npm gotchas
- This is more of an issue with gulp/webpack on macs, but make sure that if you include the following in your <code>~/.bash_profile</code>:
	
		ulimit -S -n 2048
		
	(This raises the limit on the number of open files a process can have at the same time)


### NPM gotcha - duplicate modules when working locally

Our current setup uses bunsen as an npm module. Bunsen is kind of a catch-all repository for shared static content, like SCSS and JS. 

NPM 2 is smart enough not to install duplicate modules when you run an npm install for your project. However, when developing locally you will sometimes need to do an npm install in your top level project (eg. admin v2 or 1stdibs.com) -- and also npm install from within bunsen. This will cause npm to install some duplicate modules that are listed as dependencies of both v2/.com and bunsen. Notable duplicates between bunsen and v2/.com include : 

- reku
- dibs-jquery-shim
- dibs-backbone

#### example problem : 

If you have 2 different versions of reku and you are running the v2 node app, all requests coming from the node app will use the v2 version of reku, while all such requests using a clump will use the bunsen version. Some of the clump reqeusts will not complete and, even worse, the logging installed will not surface any information on why the requests are failing (becaue the logging is installed on the v2 version on reku).

Supposedly this issue will be resolved when we upgrade to npm 3. In the mean time, delete the bunsen versions of duplicate node modules causing problems. 