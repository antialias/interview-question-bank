[examples]:https://github.com/1stdibs.com/necrodibsicon/blob/feature-cronjob-documentation/back-end/cronjob/examples.md
[endpoints]:https://github.com/1stdibs.com/necrodibsicon/blob/feature-cronjob-documentation/back-end/cronjob/endpoints.md

#CronJobs

Examples on how to use: [examples]
Endpoints: [endpoints]

CronJobManager is a framework for easy configuration and management of cron scheduled jobs. The framework registers cron jobs by a CronEnabledJob annotation on a method and a configuration in the database. Has support for scheduling jobs with leader election support, zookeeper semaphore support and reporting support. Endpoints are provided for basic CRUD operations on all cron job configurations, allowing you to enable/disable or change the scheduling of a job on the fly. It also provides an endpoint for manually running the job for testing purposes.

###CronJobManager

CronJobManager does the bulk of the work in configuring and setting up the jobs. On service startup it finds all CronEnabledJob annotated method and reads the configuration from the database. It sets up CronJobRunnables that schedule themselves to be run based on the cron provided. It also provides the support for CRUD operations on the cron jobs.

