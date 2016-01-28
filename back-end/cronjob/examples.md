#CronJob Configuration

Each service needs to configure their own CronJobManager and CronJobDao. If your service isn't using CronJobManager yet there is a some initial steps to set it up.

First create a cron job table in the database. Database table definition:
```sql
CREATE TABLE `cron_job` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `key` varchar(64) DEFAULT NULL,
  `spring_profile_regex` varchar(64) DEFAULT NULL,
  `time_out_millis` int(11) DEFAULT NULL,
  `pool_size` int(11) DEFAULT NULL,
  `cron` varchar(11) DEFAULT NULL,
  `leader_enabled` int(1) DEFAULT NULL,
  `leader_root_node` varchar(128) DEFAULT NULL,
  `semaphore_enabled` int(1) DEFAULT NULL,
  `semaphore_path` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
```

Next create a spring configuration for the cron job and add the following:
```xml
	<bean id="cronJobManager" class="com.dibs.lib.concurrent.cronjob.CronJobManager">
		<property name="name" value="identityCronJobManager"/>
	</bean>
	
	<bean id="cronJobDao" class="com.dibs.lib.concurrent.cronjob.DbCronJobDao">
		<property name="databaseSchema" value="identity"/>
		<property name="databaseTable" value="cron_job"/>
	</bean>
```

Note you'll need to update the databaseSchema and databaseTable properties to point to the table you defined.

After you add the database table and CronJobManager configuration you can start creating cron jobs.

Add the @CronEnabledJob annotation to a method like:
```java
	@CronEnabledJob(key = "trendingDownload")
	public void downloadTrends() {
		LOG.info("Downloading trends.");

		String trendTableName = createTrendTable();

		Map<String, Object> parameters = new HashMap<>();
		parameters.put("tableName", trendTableName);

		trendingItemsDownload.downloadData(parameters);

		trendingCategoriesDownload.downloadData(parameters);

		trendingAttributesDownload.downloadData(parameters);

		trendingDao.createIndices(trendTableName);

		updateTrendView(trendTableName);

		deleteLastTrendTable();
	}
```

Now start up the service and create the cron job configuration through the create cron job endpoint.
```json
{
    "apiType" : "cronJob",
    "key" : "trendingDownload",
    "springProfileRegex" : ".*",
    "cron" : "0 0 2 * * ?",
    "poolSize" : 1,
    "timeoutMillis" : 300000,
    "enabled" : "Y",
    "leaderEnabled" : "N",
    "semaphoreEnabled" : "N"
}
```

#Zookeeper leader controlled cron jobs
If the cron job is leader controlled there is some additional configuration needed.

Create the leader election support bean in spring xml. Make sure to change bean id, rootNodeName and hostNameSuffix for the approriate service
```xml
	<bean id="recommendationsLeaderElectionSupport" class="com.dibs.lib.zookeeper.LeaderElectionSupport">
		<property name="rootNodeName" value="/service/identity/recommendations" />
		<property name="hostNameSuffix" value="_identity${env:zookeeper.host.suffix}" />
		<property name="initialDelay" value="5000" />
		<property name="serverHost" value="${env:zookeeper.host}" />
		<property name="serverTimeout" value="${env:zookeeper.sessionTimeout}" />
		<property name="standaloneMode" value="${env:zookeeper.standaloneMode}" />
	</bean>
```

Additionally in env.properties you need to add the root node name to the zookeeper.leaderElection.rootNodes property
```
zookeeper.leaderElection.rootNodes=/service/inventory/itemAds,/service/identity/recommendations
```

