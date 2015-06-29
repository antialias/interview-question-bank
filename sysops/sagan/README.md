[sagan-repo]: https://github.com/1stdibs/sagan
[sagan-tool]: http://sagan.intranet.1stdibs.com/
[servers]: http://sagan.intranet.1stdibs.com/version/ 
[culinary-fruits]: https://en.wikipedia.org/wiki/List_of_culinary_fruits

# [Sagan][sagan-repo]

## TL;DR

- [Sagan Tool][sagan-tool]
- [Current Servers][servers]

## Sagan Tool

The [sagan tool][sagan-tool] lets you spin up a [(culinary) fruit][culinary-fruits] server at will. It is a two-step process that requires you to setup your server then confirm your server configs/request via email.

Fill out the following fields from the [sagan tool][sagan-tool]:

  1. `Server Name`
    1. This becomes [fruit].intranet.1stdibs.com
    2. The list is dynamically generated depending on which servers are currently in rotation
  2. `Server Mode`: `development` or `qa` mode
    1. These are the service versions the front-end consumes
    2. This also determines how front-end assets are built. In `qa` mode it does the production-level build process  (concatenating, uglifying, hashing, etc). In `development` mode assets are not run through those processes.
    3. *NOTE*: Pointing perishable fruit servers at team server services is currently not supported, but is a planned feature
  3. `1stdibs.com Branch`, `Admin v1 Branch`, `Admin v2 Branch` (well, steps 3, 4, 5 technically)
    1. The left dropdown is the GitHub user account where the branch lives with
    2. The right dropdown is the branch to use
    3. Whichever repo you do not need built, you can leave the default settings (usually `1stdibs` / `master`)
  4. `Email`: your email address
  5. Click the `Invent The Universe` button
  6. In your confirmation email, you will receive something like the following:
  
Subject: `Please confirm your fruit server request`

Copy:
```text
Hello,
Please confirm your request is correct, and then click the link below:

Server Name: araza.intranet.1stdibs.coerver 
Server Mode: qa
Branch to build for 1stdibs.com (buyer-facing site): hellatan/feature-branch
Branch to build for Admin v2: 1stdibs/master
Branch to build for Admin v1: 1stdibs/master

The process of building your server will not begin until you click the confirmation link below

If this looks correct, please click here:
http://sagan.intranet.1stdibs.com/confirm?id=1234565667
```

You must click on the last link in order to "create the universe" and get your fruit server created.

Once this is done, you should get an email that looks like this:

Subject: `Your fruit server is now ready`

Copy:
```text
Hello, Your fruit server is now ready for use. You can access it via the link below:
https://araza.intranet.1stdibs.com

https://adminv2.araza.intranet.1stdibs.com

When you are done with your server, please click the below link to shut it down and put the name back into circulation:
http://sagan.intranet.1stdibs.com/terminate?id=1234565667

Please note that if you do not explicitly shut down your server, it will shut down automatically after 72 hours.
```

Like the last line of that email, keep in mind that these servers will be destroyed after 72 hours and free to used by any other developer. If you need this server after that, you will have to create it again from scratch.
