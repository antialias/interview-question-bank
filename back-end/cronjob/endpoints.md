#Endpoints

Each service has it's own endpoint for cron jobs so following endpoints need to be prefixed with the service.

###Read all cron jobs:
Returns a list of all the cron jobs configured

/2/cronJob.json
```json
{
	"apiType": "serviceListResponse",
	"httpCode": 200,
	"message": "OK",
	"result": [
		{
			"apiType": "cronJob",
			"key": "trendingDownload",
			"cron": "0 0 2 * * ?",
			"poolSize": 1,
			"timeoutMillis": 300000,
			"springProfileRegex": ".*",
			"leaderEnabled": "Y",
			"leaderRootNodeName": "/service/identity/recommendations",
			"semaphoreEnabled": "N"
		}
	]
}
```

###Read a single cron job:
Returns the configuration of the requested cron job

/2/cronJob/{cronJobKey}.json
```json
{
	"apiType": "serviceResponse",
	"httpCode": 200,
	"message": "OK",
	"result": {
		"apiType": "cronJob",
		"key": "trendingDownload",
		"cron": "0 0 2 * * ?",
		"poolSize": 1,
		"timeoutMillis": 300000,
		"springProfileRegex": ".*",
		"leaderEnabled": "Y",
		"leaderRootNodeName": "/service/identity/recommendations",
		"semaphoreEnabled": "N"
	}
}
```

###Run a cron job:
Manually kick off the cron job

If the job is zookeeper leader controlled this endpoint respects the bypassLeaderElection query parameter

/2/cronJob/{cronJobKey}/run?mode=FOREGROUND|BACKGROUND

###Create a cron job:
/2/cronJob (POST)
Body:
```json
{
    "apiType" : "cronJob",
    "key" : "trendingDownload",
    "cron" : "0 0 2 * * ?",
    "springProfileRegex" : ".*",
    "poolSize" : 1,
    "timeoutMillis" : 300000,
    "enabled" : "Y",
    "leaderEnabled" : "N",
    "semaphoreEnabled" : "N"
}
```
###Update a cron job
/2/cronJob (PUT)

Body is same as create