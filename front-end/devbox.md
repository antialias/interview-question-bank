# devbox

Your devbox is a linux virtual machine that runs on your mac. It is controled by [vagrant](https://www.vagrantup.com/). Make sure you have version 1.6.3 installed on your mac.

To start up your devbox, go to ~/projects/1stdibs.com/ and run `vagrant up`. This will download the VM image, install it, and start it up.

## Accessing devbox
The devbox will assume you have 1stdibs.com, admin-v2, admin-v1, and the static repo cloned into the standard locations. You can then access the sites by using these automatically proxied urls:

https://devbox.intranet.1stdibs.com
https://adminv2.devbox.intranet.1stdibs.com

## Devbox Environments
By default, your devbox will use the `dev` database and service environments. Your devbox can be set to the different service environments by SSH'ing into the devbox (see below) and running the `set-env` command

Set to the money team server environment
`set env money` 

Set to the qa environment
`set env qa`

## Vagrant Commands
All vagrant commands must be run from ~/projects/1stdibs.com/

### start up devbox
`vagrant up`

### shut off devbox
`vagrant halt`

### ssh into devbox
`vagrant ssh`
