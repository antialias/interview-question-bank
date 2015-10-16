# Upgrading to OS X El Capitan (10.11)

With El Capitan (10.11), Apple added a new security feature called SIP (System
Integrity Protection) to help defend against userland attacks.  Even root can't
modify files in protected folders.  Unfortunately, PHP's dynamic libraries 
don't play nice with this feature.  By default, they reside in a protected 
location (/usr/lib/), so a work-around is necessary.

If you get errors about a missing `pcntl` module when running `composer
install`, this document is for you.

1. Create the directory `/usr/local/lib/php/extensions`.
2. Copy all of the existing dynamic extensions out of their existing location, `/usr/lib/php/extensions/no-debug-non-zts-20121212/` to the new directory.
3. Download the included [`pcntl.so-el-capitan`](./pcntl.so-el-capitan) and move it into the newly-created `/usr/local/lib/php/extensions/`, alongside the copied extensions.  Rename it to `pcntl.so`.
4. Copy `/etc/php.ini.default` to `/etc/php.ini`
5. Edit (as root) the new `php.ini` to add the following two lines:
```
extension_dir=/usr/local/lib/php/extensions
extension=pcntl.so
```
6. Restart the apache service by running `sudo apachectl restart`
7. Verify that PCNTL is installed by running `php -m | grep pcntl`.  If a line containing `pcntl` is output, everything is installed correctly.
