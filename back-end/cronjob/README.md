[examples]:https://github.com/1stdibs/necrodibsicon/blob/master/back-end/cronjob/examples.md
[endpoints]:https://github.com/1stdibs/necrodibsicon/blob/master/back-end/cronjob/endpoints.md

#CronJobs

Configuration and examples: [examples]

Endpoints: [endpoints]

CronJobManager is a framework for easy configuration and management of cron scheduled jobs. The framework registers cron jobs by a CronEnabledJob annotation on a method and a configuration in the database. Has support for scheduling jobs with leader election support, zookeeper semaphore support and reporting support. Endpoints are provided for basic CRUD operations on all cron job configurations, allowing you to enable/disable or change the scheduling of a job on the fly. It also provides an endpoint for manually running the job for testing purposes.

###CronJobManager

CronJobManager does the bulk of the work in configuring and setting up the jobs. On service startup it finds all CronEnabledJob annotated methods and reads the configuration from the database. It sets up CronJobRunnables that schedule themselves to be run based on the cron provided. It also provides the support for CRUD operations on the cron jobs.

###What's Supported on a CronJob
Each cron job can have the following properties:

key: Unique identifier for the cron job

spring_profile_regex: A regex that specifies what environments the cron job should run in. Eg .* for everything or production|stage for production and stage.

time_out_millis: How long the job can run before the job manager cancels it

pool_size: How many threads the thread pool will be created with

cron: When to run

leader_enabled: Y to enable using zookeeper leader election to only run the job on leader server

leader_root_node: If the above is Y this is required. The root node in zookeeper that will be used. See examples for more info

semaphore_enabled: Y to enable zookeeper semaphores to control where to run

semaphore_path: If above is Y this is required. The path in zookeeper where the semaphore will be created