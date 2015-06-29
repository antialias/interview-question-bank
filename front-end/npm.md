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

Do this if you think some packages may be broken but you don't know which one(s):

<code>npm run clean && npm install</code>

## npm gotchas
- This is more of an issue with gulp/webpack on macs, but make sure that if you include the following in your <code>~/.bash_profile</code>:
	
		<code>ulimit -S -n 2048</code>
		
	(This raises the limit on the number of open files a process can have at the same time)