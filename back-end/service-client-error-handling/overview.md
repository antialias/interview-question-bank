
# Service Client Error Handling

###Overview
Are you ever hitting your favorite backend endpoint when suddenly... disaster strikes? You might see a response like this:

```json
{
  "apiType" : "serviceResponse",
  "httpCode" : "500",
  "message" : "invalid content type"
}
```

And you can't help but ask yourself...

![chef](http://s-media-cache-ak0.pinimg.com/236x/07/52/8a/07528a1c87f9d53f2c510789ea9bf5f5.jpg)

Typically this error means something went horribly wrong when making a network call to another BE service (service down/borked). However, this is obviously not a very intuitive or helpful message and we could be doing way better.

###Enter Automatic Client Error Handling

In BE world, we create client classes for services if we plan on making BE to BE service calls. This is just a simple way of generically doing Http calls to some other part of BE. These clients now use an error handler for if either of the following happens:

  - No response or a response that BE can't parse is returned
  - A response is returned and parsed, but it's a 5** range http code

If one of those things happens, we build up an error that shows what call we were trying to make and what happened. Additionally, these errors stack. Meaning that if ecom calls inventory to load the item and inventory fails because it tries to load the shipment quotes from logistics and logistics is down, then the final response from ecom will say show both the failure in inventory and in logistics. This is useful because it makes it clear that logistics is actually causing the problem.

#Sample Error Response
```json
{
  "apiType" : "serviceResponse",
  "httpCode" : 500,
  "serviceResponseCode" : "EXTERNAL_SERVICE_CALL_FAILED",
  "message" : "GET - http://localhost:8080/inventory-3/2/item/f_8000?apiToken=inventory_1_ws_w1e95GT1787DV6Nv514g5y9u9M0t3kqN&fields=full,shipping",
  "error" : {
    "apiType" : "serviceErrorV2",
    "errorType" : "EXTERNAL_SERVICE_ERROR",
    "details" : [ {
      "apiType" : "errorDetail",
      "rule" : "Expected 2** response, INTERNAL_SERVER_ERROR - error response received.",
      "message" : "GET - http://localhost:8080/inventory-3/2/item/f_8000?apiToken=inventory_1_ws_w1e95GT1787DV6Nv514g5y9u9M0t3kqN&fields=full,shipping"
    }, {
      "apiType" : "errorDetail",
      "rule" : "Expected 2** response, received bad/no response, assuming service temporarily unavailable. Bork bork bork. http://s-media-cache-ak0.pinimg.com/236x/07/52/8a/07528a1c87f9d53f2c510789ea9bf5f5.jpg",
      "message" : "GET - http://dev.intranet.1stdibs.com/soa/lergersterrccchss-2/4/shipmentQuotes?anchors=ITEM-f_8000&apiToken=inventory_1_ws_w1e95GT1787DV6Nv514g5y9u9M0t3kqN&itemId=f_8000&type=INVENTORY_POSTING&ipAddress=127.0.0.1"
    } ]
  }
}
```

The above JSON is from a test in which a call to inventory fails because the logistics call failed. This was simulated by messing up the configured URL for logistics so that we get back an error. Note that in error.details we see two errors. The first is the initial call to inventory item endpoint, the second is the call that inventory tried to make to logistics.

###Summary
These new response errors should not be displayed to the end user, but instead are more intended for our own debugging purposes. And hopefully can prove to be useful going forward. 
