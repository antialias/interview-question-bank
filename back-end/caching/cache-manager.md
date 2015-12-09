

# Cache Manager

###Overview
Cache Manager is a basic service endpoint that allows you to see what caches a service is currently using as well as perform basic operations on them.

###Loading caches
Use the following endpoint to view caches being used in the service:

* http://qa/soa/inventory-3/2/cache.json?apiToken=engineering_w1e95GT1787DV6Nv514g5y9u9M0t3kqN

####Notes
* If you change inventory-3 to another application (identity, ecom, etc) it will show caches in that application. 
* The engineering is required, otherwise the service will throw errors at you

####Sample Response

```json

{
  "apiType" : "serviceListResponse",
  "httpCode" : 200,
  "message" : "OK",
  "result" : [ {
    "apiType" : "cacheDetails",
    "name" : "regionCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:02.811-05:00",
    "enabled" : true
  }, {
    "apiType" : "cacheDetails",
    "name" : "categoryKeyCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:06.430-05:00",
    "enabled" : true
  }, {
    "apiType" : "cacheDetails",
    "name" : "regionFilterCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:02.814-05:00",
    "enabled" : true
  }, {
    "apiType" : "cacheDetails",
    "name" : "categoryLegacyKeyCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:06.430-05:00",
    "enabled" : true
  }, {
    "apiType" : "cacheDetails",
    "name" : "categoryIdCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:06.430-05:00",
    "enabled" : true
  }, {
    "apiType" : "cacheDetails",
    "name" : "dimensionalWeightCache",
    "type" : "StaticMapCache",
    "lastEvicted" : "2015-12-08T12:31:02.790-05:00",
    "enabled" : true
  } ]
}
```

###Evicting

####Evict All
A GET request to the following endpoint will call the .evictAll() method on all registered caches:

* http://qa/soa/inventory-3/2/cache/evict.json?apiToken=engineering_w1e95GT1787DV6Nv514g5y9u9M0t3kqN

####Sample Response
```json
{
  "apiType" : "serviceResponse",
  "httpCode" : 200,
  "message" : "OK"
}
```

####More Granular Evicts
A GET request to the endpoint below supports java regular expression syntax ([java-regex](http://www.tutorialspoint.com/java/java_regular_expressions.htm)) and evicts all caches whose names match the
regular expression:

* http://qa/soa/inventory-3/2/cache/evict/_someRegularExpression_.json?apiToken=engineering_w1e95GT1787DV6Nv514g5y9u9M0t3kqN

###Disable/Enable
You can turn caches on/off using the following endpoints (both GET requests)

* http://qa/soa/inventory-3/2/cache/disable/_someCache_.json?apiToken=engineering_w1e95GT1787DV6Nv514g5y9u9M0t3kqN

* http://qa/soa/inventory-3/2/cache/enable/_someCache_.json?apiToken=engineering_w1e95GT1787DV6Nv514g5y9u9M0t3kqN

####Sample Response
```json
{
  "apiType" : "serviceResponse",
  "httpCode" : 200,
  "message" : "OK"
}
```

