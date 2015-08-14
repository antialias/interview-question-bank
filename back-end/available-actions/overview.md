# Available Actions

###Overview
Available actions is a new pattern for telling a client (whether it be FE, mobile, or another BE service) what actions 
the service supports that are currently valid to perform.

On the backend, a paradigm has been setup where certain models can be marked as "Action Driven" models. Once a model is marked
as such, it will automatically get a property called "actions" which renders in a consistent way through our API. 

###Sample JSON of an Item Object With Actions

```json
{
  "apiType" : "item",
  "actions" : [ {
      "apiType" : "WorkflowTransitionAction",
      "code" : "DELETED",
      "type" : "WORKFLOW_TRANSITION"
    }, {
      "apiType" : "WorkflowTransitionAction",
      "code" : "INCOMPLETE",
      "type" : "WORKFLOW_TRANSITION"
    }, {
      "apiType" : "WorkflowTransitionAction",
      "code" : "UNAVAILABLE",
      "type" : "WORKFLOW_TRANSITION"
    }, {
      "apiType" : "customAction",
      "code" : "ATTACH_LISTINGS",
      "authorizedRoles" : [ {
        "apiType" : "adminRole",
        "name" : "ADMIN",
        "permissions" : [ "MASQUERADE_AS_DEALER_PERMISSION", "ITEM_ACCESS_PERMISSION" ]
      }, {
        "apiType" : "userRole",
        "name" : "SELLER",
        "id" : "f_9883"
      } ],
      "type" : "CUSTOM_ACTION"
    } ]
}
```

As the example above shows, actions can be workflow related actions (status transitions), or other actions not directly 
related to status transitions. In the example above (taken from the publishing tools branch), we want to show that it is only
valid to attach listings to an item when the item is in a certain state. To achieve this, the backend writes the logic for when
this action should be available in an "action processor". Once this is done, when an item is loaded, the action will be either 
present or absent based on whether or not it is currently valid to take that action.

###Authorized Roles
In Addition, authorized roles are also given per action. These tell the client who is able to perform this action. There are two 
main types of authorized role objects. The first is an admin role, which means that the user is either authorized or not based
on permissions. The second is a user role, meaning the user is authorized or not based on a specific user id.

