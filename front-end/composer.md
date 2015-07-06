[composer]: https://getcomposer.org/
[satis]: http://getcomposer.org/doc/articles/handling-private-packages-with-satis.md
[dibs-satis]: http://staging.1stdibs.com/satis/
[dibs-satis-readme]: https://github.com/1stdibs/satis/blob/master/README.md
[dibs-satis-json]: https://github.com/1stdibs/satis/blob/master/satis.json
[dibs-satis-jenkins]: http://staging.1stdibs.com/jenkins/job/satis_update/

# Composer

[Composer][composer] handles some dependencies we have in the 1stdibs.com/dibs, 1stdibs-admin-v1/dibs and the 1stdibs-admin-v2 codebases.
This will only pertain to 1stdibs.com once Admin V2 is fully ported over to node.js.

To install in the /dibs directory: 

```bash
$ curl -s https://getcomposer.org/installer | php
```

You'll probably want it installed globally:

```bash
$ sudo mv composer.phar /usr/local/bin/composer
```

Once Composer is installed you'll need to install or update the project's dependencies.
If installed globally:

```bash
$ composer install
```

or, if installed in the project folder (to update): 

```bash
$ php composer.phar install
```

Once you do this, you'll have a dibs/vendor folder. This folder is ignored by Git.

Please **do not** commit the vendor folder or composer.phar. They are and should be .gitignored.

Also, please **do not** remove or ignore the **composer.json** or **composer.lock** files. They list and lock the dependencies and are used by Composer. If you add more Composer-handled dependencies, you'll update the json file.

## composer.lock file & Dibslibs

When updating Dibslibs you should push your changes to Dibslibs master. Repos should be pegged to dev-master of Dibslibs

**Then** you should run (on the branch you wish to update the library):

```bash
composer update
```

This will update Diblibs to the latest version and generate a new .lock file.

**Then** you need to commit and push the new lock file to whatever branch you want the updated Dibslibs.

When installing dependencies you should always:

```bash
composer install
```

NOT update.

This way we can have different commits of Dibslibs on different branches, in different projects, etc. Remember, only update when you want to actually update. Update also updates the .lock file.

## composer.lock file & other dependencies

The process is the same for updating or adding other dependencies except that you'll just update the version or add the dependency in the composer.json file of the branch where you want the updates. 

## Satis & Composer

We are using [satis][satis] to manage a [1stdibs Private Composer Repository][dibs-satis] for internal use (as opposed to using [Packagist](http://packagist.org/)).  There is a repo for the satis.json file that configures this repository. It lives here.  If a package is ever added to a repository, then it needs to be added there with it's dependencies. See the [readme][dibs-satis-readme] for more info. 

These are the steps to add a dependency to any 1stdibs project:

1. make sure you're using the 1stdibs.com repository in your composer.json file (see the [readme][dibs-satis-readme])
2. add the dependency (and it's dependencies) to [satis.json][dibs-satis-json]
3. build [satis_update on Jenkins][dibs-satis-jenkins[ to update the [1stdibs Private Composer Repository][dibs-satis]
4. add the dependency to your project's composer.json file
5. run `composer update` in your project
6. commit the composer.json and composer.lock file changes to your project

## General Troubleshooting

Errors that you may encounter when trying to install composer files:

```bash
[UnexpectedValueException] 
The checksum verification of the file failed (downloaded from http://staging.1stdibs.com/broker/repositories/dibs_repo/dists/twig_twig.zip)
```

Just delete all cache* folders in the .composer folder that should be in your user (~) folder.

```bash
user:~/projects/dibslibs (master)
$ composer install --dev
Loading composer repositories with package information
Installing dependencies (including require-dev) from lock file
Warning: The lock file is not up to date with the latest changes in composer.json. You may be getting outdated dependencies. Run update to update them.
Your requirements could not be resolved to an installable set of packages.
```

Just delete your composer.lock file and run composer install / composer install --dev command again and a new lock file should be generated with the latest composer.json settings.
