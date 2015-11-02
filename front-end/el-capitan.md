# Upgrading to OS X El Capitan (10.11)

## Missing `pcntl` module

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

## Unable to run `vagrant up`

If you run `vagrant up` and are faced with this error:

```shell
Bringing machine 'default' up with 'virtualbox' provider...
==> default: Clearing any previously set network interfaces...
There was an error while executing `VBoxManage`, a CLI used by Vagrant
for controlling VirtualBox. The command and stderr is shown below.

Command: ["hostonlyif", "create"]

Stderr: 0%...
Progress state: NS_ERROR_FAILURE
VBoxManage: error: Failed to create the host-only adapter
VBoxManage: error: VBoxNetAdpCtl: Error while adding new interface: failed to open /dev/vboxnetctl: No such file or directory
VBoxManage: error: Details: code NS_ERROR_FAILURE (0x80004005), component HostNetworkInterface, interface IHostNetworkInterface
VBoxManage: error: Context: "int handleCreate(HandlerArg*, int, int*)" at line 66 of file VBoxManageHostonly.cpp
```

Upgrade your VirtualBox. As of 11/2/15, the latest version is [5.0.8](https://www.virtualbox.org/wiki/Downloads)

After updating VirtualBox, and you see this error:

```shell
The provider 'virtualbox' that was requested to back the machine
'default' is reporting that it isn't usable on this system. The
reason is shown below:

Vagrant has detected that you have a version of VirtualBox installed
that is not supported. Please install one of the supported versions
listed below to use Vagrant:

4.0, 4.1, 4.2, 4.3
```

You will need to update vagrant. As of 11/2/15, the latest version is [1.7.4](https://www.vagrantup.com/downloads.html)

Once the two updates are done, `vagrant up` should be good to go. However, that still leave us...

## Unable to `vagrant ssh` / `vagrant provision`

Even after doing the two updates above and you cannot `vagrant ssh` (as in you're asked for a password), run the following:

```shell
ssh -i ~/.vagrant.d/insecure_private_key -p 2222 root@localhost
```

If you are trying to `provision`, your vagrant box, run the code above then, once inside the vagrant box, run the following:

```shell
puppet agent --no-daemonize --verbose --onetime
```

