

# Caching

###Overview
Shared-util now supports pluggable/easy to configure caching via various implementations of the Cache interface (found here: [cache-interface](https://github.com/1stdibs/shared-util/blob/master/src/main/java/com/dibs/util/cache/Cache.java)). 

Apart from making caching code more consistent across BE, this affords us several other benefits, such as being able to scan for all caches being used in a application (i.e inventory or ecom) and register them against a unique name. Having them all registered, means we can build a shared controller (every service gets this endpoint) that can do basic operations like display all caches in use, evict all from a specific cache, or even disable a cache if it is causing bugs.

#More Info
For more on the cache manager service - ...
For more detailed code example on setting up caching - ...

###Sample Code

```java
public class ExampleWithCache
{
  /*
   * Can be configured in XML or in post construct.
   */
  private Cache<Long, Item> itemCache;
  
  @Autowired
  private CacheFactory cacheFactory;
  
  /*
   *  Example config in post construct
   */
  @PostConstruct
  public void init()
  {
    //Build up a cache config
    CacheConfig config = new CacheConfig().setExpireMillis(1000L).setName("inventoryItemCache");
    
    //use factory class to build a cache of the desired type/configuration
    itemCache = cacheFactory.getCache(CacheType.STATIC_MAP, config);
  }
  
  public Item readItem(Long id)
  {
    Item item = itemCache.get(id);
    
    if(item == null)
    {
      //go get it the more expensive way
    }
    
    return item;
  }
  
  public void updateItem(Item item)
  {
    //do item update
    
    Long id = item.getId();
    
    //clearCache
    itemCache.evict(id);
  }
  
  public void clearItemCache()
  {
    itemCache.evictAll();
  }
}
```
