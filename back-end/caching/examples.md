[cache-factory](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/factory/CacheFactory.java)
[cache-helper](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/impl/CacheHelper.java)
[cache-interface](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/Cache.java)
[memcacheUtil](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/memcache/MemcacheUtil.java)

#Caching Examples


###Basics
####Cache Factory
[cache-factory] is intended to help you construct new caches easily if you opt to construct them programmatically. It also handles certain things for you automatically, like registering your newly created cache so that it is visible/available to the [cache-manager](https://github.com/1stdibs/necrodibsicon/blob/master/back-end/caching/cache-manager.md). To use the [cache-factory], simply autowire it where needed and call getCache (example below).

```java
public class CacheFactoryExample
{
  private Cache<Long, Stuff> stuffCache;
  
  @Autowired
  private CacheFactory cacheFactory;
  
  @PostConstruct
  public void init()
  {
    CacheConfig config = new CacheConfig().setName("exampleCache");
    
    stuffCache = cacheFactory.getCache(CacheType.STATIC_MAP, config);
  }
}
```

####CacheHelper
[cache-helper] is a convenience class, which makes it easy to use the [cache-factory] in XML. Under the hood, it wraps a cache, has a config and setter methods that put variables into the config. Then on post construct, it uses the [cache-factory] to instatiate the internal cache (it then delegates to all this internal cache for all the methods overrided from the [cache-interface]. This is the preferred method for instantiating a new cache using XML, since it auto-registers and reaps the benefits of the configuration code in [cache-factory].

```xml
<bean class="com.dibs.util.cache.impl.CacheHelper" >
	<property name="name" value="publicationGroupDaoCache" />
	<property name="type" value="STATIC_MAP" />
</bean>
```

###Memcache-d Caching
For full details on our memcached implementation, see [memcacheCache](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/impl/MemcacheCache.java). In general, memcache cache uses [memcacheVersion](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/memcache/data/MemcacheVersion.java) objects and key templates to build up ids to use for caching objects in memcached. Then it uses the static methods in [memcacheUtil] behind the seens to actual perform memcache operations.

####Memcache Examples

#####Programmatic Config
```java
public class CacheFactoryExample
{
  private Cache<Long, Stuff> memcachedCache;
  
  @Autowired
  private CacheFactory cacheFactory;
  
  @PostConstruct
  public void init()
  {
    CacheConfig config = new CacheConfig().setName("exampleCache").setValueClass(Stuff.class);
    
    memcachedCache = cacheFactory.getCache(CacheType.MEMCACHE_D, config);
  }
}
```

#####XML Config
```xml
<bean class="com.dibs.util.cache.impl.CacheHelper" >
	<property name="name" value="publicationGroupDaoCache" />
	<property name="type" value="MEMCACHE_D" />
	<property name="valueClass" value="com.dibs.inventory.data.DomainPublicationGroup" />
</bean>
```

####Notes on Memcache Cache
* For memcache cache, value class is required, since it is needed when using [memcacheUtil].

###Static Map Cache Caching
StaticMapCache is an in-memory implementation that is backed by a simple java hashmap. It supports options like expire millis and clone (true/false- tells the cache to clone the object as it is coming in/out of the cache), however only the name is required (it defaults to indefinite caching and clone = FALSE).

####Static Map Cache Examples

#####Programmatic Config
```java
public class CacheFactoryExample
{
  private Cache<Long, Stuff> staticMapCache;
  
  @Autowired
  private CacheFactory cacheFactory;
  
  @PostConstruct
  public void init()
  {
    CacheConfig config = new CacheConfig().setName("exampleCache");
    
    staticMapCache = cacheFactory.getCache(CacheType.STATIC_MAP, config);
  }
}
```

#####XML Config
```xml
<bean class="com.dibs.util.cache.impl.CacheHelper" >
	<property name="name" value="publicationGroupDaoCache" />
	<property name="type" value="STATIC_MAP" />
</bean>
```

###Thread Local Util Caching
ThreadLocalUtil caching is a simple way to implement request level caching (only load this thing once per request unless specifically evicted).

####Thread Local Util Cache Examples
#####Programmatic Config
```java
public class CacheFactoryExample
{
  private Cache<Long, Stuff> threadLocalCache;
  
  @Autowired
  private CacheFactory cacheFactory;
  
  @PostConstruct
  public void init()
  {
    CacheConfig config = new CacheConfig().setName("exampleCache");
    
    threadLocalCache = cacheFactory.getCache(CacheType.THREAD_LOCAL_UTIL, config);
  }
}
```

#####XML Config
```xml
<bean class="com.dibs.util.cache.impl.CacheHelper" >
	<property name="name" value="publicationGroupDaoCache" />
	<property name="type" value="THREAD_LOCAL_UTIL" />
</bean>
```


