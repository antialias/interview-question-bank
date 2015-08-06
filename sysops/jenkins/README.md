[jenkins-url]: http://jenkins.1stdibs.com/

# Sys Ops

## Deploying to a fruit server through jenkins

Create an account in [jenkins][jenkins-url] and email sysops@1stdibs.com for user permissions or log in if you alread have an account.

On the main page, you'll find a job called `1stdibs.com Deploy ANY FRUIT SERVER`. Once in the job page, click on `Build with Parameters` link on the left of the page. Fill the form as follows:

  - SERVER_NAME: This becomes [fruit].intranet.1stdibs.com

  - ENVIRONMENT: Which back-end environment you would like to point to.

  - GITHUB_ACCOUNT: The account where the branch you want to build lives.

  - BRANCH_NAME: The branch you want to build.

Finally, you'll be able to check on the status of the build by clicking on the build job. Keep an eye out for any errors that may occur during the build by going to the `console output` link on the left. You will also receive an email with the final status of the build.

TODO: Carter to add ALL sysops docs
