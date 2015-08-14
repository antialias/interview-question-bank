[shared-lib]:https://github.com/1stdibs/shared-lib
[shared-workflow]:https://github.com/1stdibs/shared-workflow
[shared-service]:https://github.com/1stdibs/shared-service
[available actions]:https://github.com/1stdibs/shared-lib/tree/master/src/main/java/com/dibs/lib/action
[shared-service actions]:https://github.com/1stdibs/shared-service/tree/master/src/main/java/com/dibs/service/v2/action
[action]:https://github.com/1stdibs/shared-service/blob/master/src/main/java/com/dibs/service/v2/action/Action.java

#Implementation Details

###Overview

Most of the core components of available actions paradigm live in [shared-lib] under the [com.dibs.lib.action][available actions] package. However the models, which determine the API for actions, live in [shared-service] under the [com.dibs.service.v2.action][shared-service actions] package.

![available_actions](https://cloud.githubusercontent.com/assets/4480376/9283874/2966493a-42a5-11e5-8741-dde6851dedee.jpg)

###Action Driven
Models in [shared-service] can now be marked as ActionDriven. This givens them a property called "actions" of type List<Action>.

```java
package com.dibs.service.v2.action;

import java.util.List;

/**
 * 
 * @author corey
 */
public interface ActionDriven
{
	public List<Action> getActions();
	public void setActions(List<Action> actions);
}
```

###Action
The basic API for an action is defined through the [Action interface][action]:

```java
@XmlJavaTypeAdapter(AnyTypeAdapter.class)
@XmlSeeAlso({CustomAction.class, WorkflowTransitionAction.class})
public interface Action extends TypedServiceData
{
	public String getCode();
	public void setCode(String code);
	
	public ActionType getType();
	public void setType(ActionType type);
	
	public Set<AuthorizedRole> getAuthorizedRoles();
	public void setAuthorizedRoles(Set<AuthorizedRole> authorizedRoles);
}
```

###AvailableActionsManager
At a very high level, there is an interface called AvailableActionsManager, which exposes an interface where an object can be passed in, and a set of action objects is returned. 

```java
/**
 * A interface for classes responsible for taking in a document and returning a set of actions
 * 
 * @see AvailableActionsProducer
 * 
 * @author corey
 */
public interface AvailableActionsManager
{
	public <D> Set<Action> computeAvailableActions(D document);
	
}
```
The default implementation of this interface does so by holding onto a set of other AvailableActionsManagers. When computeAvailableActions is called, it delegates to all available actions managers currently configured and aggregates the results. The the class also supports a scanner, which is currently scanning based on spring application context. Meaning if other mangers are implemented, they will automatically be included as long as they are in the application context.

####Custom Actions
Another implementation of this interface is the manager meant to handle custom actions (not related to workflow status transitions). It does this by scanning for instances of the ActionProcessor interface, and building up a mapping of class to Set of ActionProcessor. Then when a computeAvailableActions method call comes in, it pulls processors out of the map and invokes them all, build up available actions that way.

#####Action Processor

```java
package com.dibs.lib.action;

import com.dibs.service.v2.action.Action;

/**
 * A processor responsible for taking in a document and determining if a particular action is currently available.
 * 
 * @author corey
 *
 * @param <D> - The type of document that this processor is intended for.
 */
public interface ActionProcessor<D>
{
	public Action process(D document);
	public Class<D> supports();
}
```

####Workflow Actions
Another implementation of AvailableActionsManager has been added to [shared-workflow]. This implementation is responsible for 
building up actions that correspond to state transitions. It does this by wiring in workflow, checking if the given document is supported by workflow, then asking workflow to return what transitions are currently available, based on the workflow design. It then takes each transition and builds up a corresponding action.


